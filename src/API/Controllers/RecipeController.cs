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
    [JwtAuthorizedFilter]
    [DTOValidationFilter]
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
            return recipe;
        }

        [HttpPost]
        [Produces(typeof(Recipe))]
        public async Task<IActionResult> Post(Recipe recipe)
        {
            var newRecipe = await _recipes.AddRecipe(recipe);
            return Ok(newRecipe);
        }

        [HttpPost("{id}/ingredients")]
        [Produces(typeof(Recipe))]
        public async Task<IActionResult> PostIngredient(Guid id, Ingredient ingredient)
        {
            var updatedRecipe = await _recipes.AddIngredientToRecipe(id, ingredient);
            return Ok(updatedRecipe);
        }

        [HttpPut("{id}/ingredients")]
        [Produces(typeof(Recipe))]
        public async Task<IActionResult> PutIngredient(Guid id, Ingredient ingredient)
        {
            var updatedRecipe = await _recipes.UpdateIngredientInRecipe(id, ingredient);
            return Ok(updatedRecipe);
        }

        [HttpDelete("{id}/ingredients/{ingredientId}")]
        [Produces(typeof(Recipe))]
        public async Task<IActionResult> DeleteIngredient(Guid id, Guid ingredientId)
        {
            await _recipes.DeleteIngredientInRecipe(id, ingredientId);
            return NoContent();
        }

        [HttpPost("{id}/steps")]
        [Produces(typeof(Recipe))]
        public async Task<IActionResult> PostStep(Guid id, Step step)
        {
            var updatedRecipe = await _recipes.AddStepToRecipe(id, step);
            return Ok(updatedRecipe);
        }

        [HttpPut("{id}/steps")]
        [Produces(typeof(Recipe))]
        public async Task<IActionResult> PutStep(Guid id, Step step)
        {
            var updatedRecipe = await _recipes.UpdateStepInRecipe(id, step);
            return Ok(updatedRecipe);
        }

        [HttpDelete("{id}/steps/{stepId}")]
        [Produces(typeof(Recipe))]
        public async Task<IActionResult> DeleteStep(Guid id, Guid stepId)
        {
            await _recipes.DeleteStepInRecipe(id, stepId);
            return NoContent();
        }
    }
}