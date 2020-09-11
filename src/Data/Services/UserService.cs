using BadMelon.Data.DTOs;
using BadMelon.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BadMelon.Data.Services
{
    public class UserService : IUserService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger<IUserService> _logger;

        public UserService(SignInManager<User> signInManager, IHttpContextAccessor httpContext, ILogger<IUserService> logger)
        {
            _signInManager = signInManager;
            _httpContext = httpContext;
            _logger = logger;
        }

        public async Task<bool> IsLoggedIn()
        {
            if (_httpContext.HttpContext.User == null) return false;

            return _signInManager.IsSignedIn(_httpContext.HttpContext.User);
        }

        public async Task<bool> Login(Login login)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(login.Username, login.Password, true, false);

            if (signInResult.Succeeded)
                _logger.LogInformation($"User {login.Username} logged in at {DateTime.UtcNow} UTC Time");
            else
                _logger.LogWarning($"User {login.Username} failed a login attempt.");

            return signInResult.Succeeded;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}