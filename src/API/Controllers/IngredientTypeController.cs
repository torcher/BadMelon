using BadMelon.API.Helpers;
using BadMelon.Data.DTOs;
using BadMelon.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BadMelon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    [DTOValidationFilter]
    public class IngredientTypeController : ControllerBase
    {
        private readonly IIngredientTypeService _typeService;

        public IngredientTypeController(IIngredientTypeService typeService)
        {
            _typeService = typeService;
        }

        [HttpGet]
        public async Task<IEnumerable<IngredientType>> Get()
        {
            return await _typeService.GetIngredientTypes();
        }

        [HttpGet("{id}")]
        public async Task<IngredientType> Get(Guid id)
        {
            var ingredientType = await _typeService.GetIngredientType(id);
            return ingredientType;
        }

        [HttpPost]
        [Produces(typeof(IngredientType))]
        public async Task<IActionResult> Post(IngredientType ingredientType)
        {
            var newIngredientType = await _typeService.AddIngredientType(ingredientType);
            return Ok(newIngredientType);
        }
    }
}