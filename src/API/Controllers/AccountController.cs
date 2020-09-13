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
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Registration registration)
        {
            await _userService.Register(registration);
            return Ok();
        }

        [HttpGet("verify/{id}")]
        public async Task<IActionResult> Verify(Guid id)
        {
            await _userService.Verify(id);
            return Ok();
        }
    }
}