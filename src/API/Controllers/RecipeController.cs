using BadMelon.Data.DTOs;
using BadMelon.Data.Extensions;
using BadMelon.Data.Services;
using Microsoft.AspNetCore.Http;
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
            var recipe = await _recipes.GetRecipeByID(id);
            if (recipe == null)
                HttpContext.SetResponseNotFound();
            return recipe;
        }

        [HttpPost]
        [Produces(typeof(Recipe))]
        public async Task<IActionResult> Post(Recipe recipe)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ConvertToDTO());

            var newRecipe = await _recipes.AddRecipe(recipe);
            return Ok(newRecipe);
        }

        [HttpPost("{id}/ingredients")]
        [Produces(typeof(Recipe))]
        public async Task<IActionResult> PostIngredient(Guid id, Ingredient ingredient)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ConvertToDTO());
            var updatedRecipe = await _recipes.AddIngredientToRecipe(id, ingredient);
            return Ok(updatedRecipe);
        }

        [HttpPut("{id}/ingredients")]
        [Produces(typeof(Recipe))]
        public async Task<IActionResult> PutIngredient(Guid id, Ingredient ingredient)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ConvertToDTO());
            var updatedRecipe = await _recipes.UpdateIngredientInRecipe(id, ingredient);
            return Ok(updatedRecipe);
        }

        [HttpDelete("{id}/ingredients/{ingredientId}")]
        [Produces(typeof(Recipe))]
        public async Task<IActionResult> DeleteIngredient(Guid id, Guid ingredientId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ConvertToDTO());
            await _recipes.DeleteIngredientInRecipe(id, ingredientId);
            return NoContent();
        }
    }
}