﻿using BadMelon.API.Helpers;
using BadMelon.Data.DTOs;
using BadMelon.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> PostLogout()
        {
            await _userService.Logout();
            return Ok();
        }
    }
}