using BadMelon.Data.DTOs;
using BadMelon.Data.Entities;
using BadMelon.Data.Exceptions;
using BadMelon.Data.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BadMelon.Data.Services
{
    public class UserService : IUserService
    {
        private readonly BadMelonDataContext _db;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<IUserService> _logger;

        public UserService(
            BadMelonDataContext db,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IOptions<JwtSettings> jwtSettings,
            IEmailService emailService,
            IHttpContextAccessor httpContext,
            ILogger<IUserService> logger)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _jwtSettings = jwtSettings.Value;
            _httpContext = httpContext;
            _logger = logger;
        }

        public User GetLoggedInUser()
        {
            return (User)_httpContext.HttpContext.Items["User"];
        }

        public async Task<User> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<bool> IsLoggedIn()
        {
            return _httpContext.HttpContext.Items["User"] != null;
        }

        public async Task<LoginResponse> Login(Login login)
        {
            if (login.LoginMethod == LoginMethod.ACCOUNT)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(login.Username, login.Password, true, false);

                if (!signInResult.Succeeded)
                {
                    _logger.LogWarning($"User {login.Username} failed an authentication attempt.");
                    return new LoginResponse();
                }

                _logger.LogInformation($"User {login.Username} authenticated at {DateTime.UtcNow} UTC Time");
                var loggedInUser = await _userManager.FindByNameAsync(login.Username);
                return new LoginResponse(new Jwt(generateJwtToken(loggedInUser)));
            }
            else if (login.LoginMethod == LoginMethod.EMAIL)
            {
                var user = await _db.Users.SingleOrDefaultAsync(u => u.EmailConfirmed && (u.NormalizedUserName == login.Username.ToUpper() || u.NormalizedEmail == login.Username.ToUpper()));
                if (user == null)
                    return new LoginResponse();

                user.EmailVerificationCode = Guid.NewGuid();
                user.EmailVerificationCreated = DateTime.Now;
                await _db.SaveChangesAsync();

                await _emailService.SendEmail(user.Email, "BadMelon: Login link requested",
    $@"Hello {user.UserName},

You can login with the following URL for the next 10 minutes.

http://localhost:9000/api/auth/code/{user.EmailVerificationCode}

If you did not request this, someone is trying to hack you.

Thanks,
Bad Melon
");
                return new LoginResponse(true);
            }

            return new LoginResponse();
        }

        public async Task<LoginResponse> Login(Guid code)
        {
            var user = await _db.Users.SingleOrDefaultAsync(u => u.EmailVerificationCode == code);
            if (user == null || DateTime.Now - user.EmailVerificationCreated > new TimeSpan(0, 10, 0))
                return new LoginResponse();
            return new LoginResponse(new Jwt(generateJwtToken(user)));
        }

        public async Task Register(Registration registration)
        {
            var newUser = new User
            {
                UserName = registration.Username,
                Email = registration.EmailAddress,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(newUser);
            if (!result.Succeeded)
            {
                throw new ValidationException(result.Errors.ToDictionary(e => e.Code, e => e.Description));
            }

            var user = await _db.Users.SingleOrDefaultAsync(u => u.NormalizedUserName == registration.Username.ToUpper());
            if (!string.IsNullOrEmpty(registration.Password))
            {
                var passwordRx = new Regex(@"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_]).{6,})");
                if (registration.Password.Length < 22 && !passwordRx.IsMatch(registration.Password))
                    throw new ValidationException(new System.Collections.Generic.Dictionary<string, string> { { "WeakPassword", "Provided password does not meet the password requirements." } });

                var passwordResult = await _userManager.AddPasswordAsync(user, registration.Password);
                if (!passwordResult.Succeeded)
                {
                    throw new ValidationException(passwordResult.Errors.ToDictionary(e => e.Code, e => e.Description));
                }
            }

            user.EmailVerificationCode = Guid.NewGuid();
            user.EmailVerificationCreated = DateTime.Now;
            await _db.SaveChangesAsync();

            await _emailService.SendEmail(user.Email, "Bad Melon:Verify Email Address",
$@"Hello {user.UserName},

Please verify your email address by visiting the following link: {"http://localhost:9000/api/account/verify/" + user.EmailVerificationCode}

Thanks,
Bad Melon Admin");
        }

        public async Task Reset(PasswordReset reset)
        {
            var user = GetLoggedInUser();
            if (!user.IsPasswordSet)
            {
                var passwordResult = await _userManager.AddPasswordAsync(user, reset.NewPassword);
                if (!passwordResult.Succeeded)
                    throw new ValidationException(new Dictionary<string, string> { { "NewPassword", "Password invalid: " + passwordResult.Errors.FirstOrDefault()?.Description } });
                user.IsPasswordSet = true;
                await _db.SaveChangesAsync();
                return;
            }

            var updatePasswordResult = await _userManager.ChangePasswordAsync(user, reset.CurrentPassword, reset.NewPassword);
            if (!updatePasswordResult.Succeeded)
                throw new ValidationException(new Dictionary<string, string> { { "NewPassword", "Password invalid: " + updatePasswordResult.Errors.FirstOrDefault()?.Description } });
        }

        public async Task Verify(Guid verificationId)
        {
            var user = await _db.Users.SingleOrDefaultAsync(u => !u.EmailConfirmed && u.EmailVerificationCode == verificationId);
            if (user == null || user.EmailVerificationCreated - DateTime.Now > new TimeSpan(0, 30, 0))
                throw new EntityNotFoundException("Could not find user account");
            user.EmailConfirmed = true;
            await _db.SaveChangesAsync();
        }

        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(_jwtSettings.ExpiryInDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}