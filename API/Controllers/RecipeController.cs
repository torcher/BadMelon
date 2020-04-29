using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BadMelon.API.Models;
using BadMelon.API.Services;
using Microsoft.AspNetCore.Mvc;


namespace BadMelon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipes;
        public RecipeController(IRecipeService recipes)
        {
            _recipes = recipes;
        }

        [HttpGet]
        public async Task<Recipe[]> Get()
        {
            return await _recipes.GetRecipes();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<Recipe> Get(Guid id)
        {
            return await _recipes.GetRecipeByID(id);
        }
    }
}
