using BadMelon.Data.DTOs;
using System;
using System.Threading.Tasks;

namespace BadMelon.Data.Services
{
    public interface IRecipeService
    {
        Task<Recipe[]> GetRecipes();

        Task<Recipe> GetRecipeByID(Guid ID);

        Task<Recipe> AddRecipe(Recipe recipe);

        Task<Recipe> AddIngredientToRecipe(Guid recipeId, Ingredient ingredient);

        Task<Recipe> UpdateIngredientInRecipe(Guid recipeId, Ingredient ingredient);

        Task<Recipe> DeleteIngredientInRecipe(Guid recipeId, Ingredient ingredient);

        Task<Recipe> DeleteIngredientInRecipe(Guid recipeId, Guid ingredientId);

        Task<Recipe> AddStepToRecipe(Guid recipeId, Step step);

        Task<Recipe> UpdateStepInRecipe(Guid recipeId, Step step);

        Task<Recipe> DeleteStepInRecipe(Guid recipeId, Step step);

        Task<Recipe> DeleteStepInRecipe(Guid recipeId, Guid stepId);
    }
}