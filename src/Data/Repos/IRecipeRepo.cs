using BadMelon.Data.Entities;
using System;
using System.Threading.Tasks;

namespace BadMelon.Data.Repos
{
    public interface IRecipeRepo
    {
        Task<Recipe[]> Get();

        Task<Recipe> Get(Guid ID);

        Task<Recipe> AddRecipe(Recipe recipe);

        Task<Recipe> UpdateRecipe(Recipe recipe);

        Task DeleteRecipe(Guid recipeId);

        Task DeleteRecipe(Recipe recipe);

        Task<Recipe> AddIngredientToRecipe(Guid recipeId, Ingredient ingredient);

        Task<Recipe> RemoveIngredientFromRecipe(Guid recipeId, Guid ingredientId);

        Task<Recipe> UpdateIngredientInRecipe(Guid recipeId, Ingredient ingredient);

        Task<Recipe> AddStepToRecipe(Guid recipeId, Step step);

        Task<Recipe> RemoveStepFromRecipe(Guid recipeId, Guid stepId);

        Task<Recipe> UpdateStepInRecipe(Guid recipeId, Step step);
    }
}