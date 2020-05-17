using BadMelon.API.Services;
using BadMelon.RecipeMath;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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

        [HttpGet("{id}")]
        public async Task<Recipe> Get(Guid id)
        {
            return await _recipes.GetRecipeByID(id);
        }
    }
}