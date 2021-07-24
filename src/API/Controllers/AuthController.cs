using BadMelon.API.Helpers;
using BadMelon.Data.DTOs;
using BadMelon.Data.Services;
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
            var result = await _userService.Login(login);
            if (result.IsSuccess)
                return Ok(result.jwt);
            return NotFound();
        }

        [HttpGet("code/{code}")]
        public async Task<IActionResult> PostLogin(Guid code)
        {
            var result = await _userService.Login(code);
            if (result.IsSuccess)
                return Ok(result.jwt);
            return NotFound();
        }

        [JwtAuthorizedFilter]
        [HttpPost("reset-password")]
        public async Task<IActionResult> Reset(PasswordReset reset)
        {
            await _userService.Reset(reset);
            return Ok();
        }
    }
}