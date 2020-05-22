using BadMelon.API.DTOs;
using BadMelon.API.Extensions;
using BadMelon.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BadMelon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
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
            if (ingredientType == null)
                HttpContext.SetResponseNotFound();
            return ingredientType;
        }

        [HttpPost]
        [Produces(typeof(IngredientType))]
        public async Task<IActionResult> Post(IngredientType ingredientType)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ConvertToDTO());
            var newIngredientType = await _typeService.AddIngredientType(ingredientType);
            return Ok(newIngredientType);
        }
    }
}