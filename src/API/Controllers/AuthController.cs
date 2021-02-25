using BadMelon.API.Helpers;
using BadMelon.Data.DTOs;
using BadMelon.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BadMelon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [DTOValidationFilter]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> PostLogin(Login login)
        {
            if (await _userService.Login(login))
                return Ok();
            return NotFound();
        }

        [HttpGet("code/{code}")]
        public async Task<IActionResult> PostLogin(Guid code)
        {
            if (await _userService.Login(code))
                return Ok();
            return NotFound();
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> PostLogout()
        {
            await _userService.Logout();
            return Ok();
        }

        [Authorize]
        [HttpPost("reset-password")]
        public async Task<IActionResult> Reset(PasswordReset reset)
        {
            await _userService.Reset(reset);
            return Ok();
        }
    }
}