using BadMelon.Data.DTOs;
using BadMelon.Data.Entities;
using BadMelon.Data.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BadMelon.Data.Services
{
    public class UserService : IUserService
    {
        private readonly BadMelonDataContext _db;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<IUserService> _logger;

        public UserService(
            BadMelonDataContext db,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailService emailService,
            IHttpContextAccessor httpContext,
            ILogger<IUserService> logger)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _httpContext = httpContext;
            _logger = logger;
        }

        public async Task<User> GetLoggedInUser()
        {
            var user = await _userManager.GetUserAsync(_httpContext.HttpContext.User);
            if (user == null)
                throw new UnauthorizedException();
            return user;
        }

        public async Task<bool> IsLoggedIn()
        {
            if (_httpContext.HttpContext.User == null) return false;

            return _signInManager.IsSignedIn(_httpContext.HttpContext.User);
        }

        public async Task<bool> Login(Login login)
        {
            if (login.LoginMethod == LoginMethod.ACCOUNT)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(login.Username, login.Password, true, false);

                if (signInResult.Succeeded)
                    _logger.LogInformation($"User {login.Username} logged in at {DateTime.UtcNow} UTC Time");
                else
                    _logger.LogWarning($"User {login.Username} failed a login attempt.");

                return signInResult.Succeeded;
            }

            var user = await _db.Users.SingleOrDefaultAsync(u => u.EmailConfirmed && u.NormalizedUserName == login.Username.ToUpper() || u.NormalizedEmail == login.Username.ToUpper());
            if (user == null)
                return false;

            user.EmailVerificationCode = Guid.NewGuid();
            user.EmailVerificationCreate = DateTime.Now;
            await _db.SaveChangesAsync();

            await _emailService.SendEmail(user.Email, "BadMelon: Login link requested",
$@"Hello {user.UserName},

You can login with the following URL for the next 10 minutes.

http://localhost:9000/api/auth/code/{user.EmailVerificationCode}

If you did not request this, someone is trying to hack you.

Thanks,
Bad Melon
");
            return true;
        }

        public async Task<bool> Login(Guid code)
        {
            var user = await _db.Users.SingleOrDefaultAsync(u => u.EmailVerificationCode == code);
            if (user == null || DateTime.Now - user.EmailVerificationCreate > new TimeSpan(0, 10, 0))
                return false;
            await _signInManager.SignInAsync(user, true);
            return true;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
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
            user.EmailVerificationCreate = DateTime.Now;
            await _db.SaveChangesAsync();

            await _emailService.SendEmail(user.Email, "Bad Melon:Verify Email Address",
$@"Hello {user.UserName},

Please verify your email address by visiting the following link: {"http://localhost:9000/api/account/verify/" + user.EmailVerificationCode}

Thanks,
Bad Melon Admin");
        }

        public async Task Verify(Guid verificationId)
        {
            var user = await _db.Users.SingleOrDefaultAsync(u => !u.EmailConfirmed && u.EmailVerificationCode == verificationId);
            if (user == null || user.EmailVerificationCreate - DateTime.Now > new TimeSpan(0, 30, 0))
                throw new EntityNotFoundException("Could not find user account");
            user.EmailConfirmed = true;
            await _db.SaveChangesAsync();
        }
    }
}