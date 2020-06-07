using BadMelon.Data.DTOs;
using BadMelon.Data.Repos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BadMelon.Data.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepo _recipeRepo;

        public RecipeService(IRecipeRepo recipeRepo)
        {
            _recipeRepo = recipeRepo;
        }

        public async Task<Recipe[]> GetRecipes()
        {
            return (await _recipeRepo.Get())
                .ConvertToDTOs()
                .ToArray();
        }

        public async Task<Recipe> GetRecipeByID(Guid ID)
        {
            return (await _recipeRepo.Get(ID))?.ConvertToDTO();
        }

        public async Task<Recipe> AddRecipe(Recipe recipe)
        {
            return (await _recipeRepo.AddRecipe(recipe.ConvertFromDTO())).ConvertToDTO();
        }

        public async Task<Recipe> AddIngredientToRecipe(Guid recipeId, Ingredient ingredient)
        {
            var recipe = await _recipeRepo.Get(recipeId);
            var currentIngredient = recipe.Ingredients.SingleOrDefault(i => i.IngredientTypeID == ingredient.TypeID);
            if (currentIngredient == null)
            {
                recipe = await _recipeRepo.AddIngredientToRecipe(recipeId, ingredient.ConvertFromDTO());
            }
            else
            {
                currentIngredient.Weight += ingredient.Weight;
                recipe = await _recipeRepo.UpdateIngredientInRecipe(recipeId, currentIngredient);
            }

            return recipe.ConvertToDTO();
        }

        public async Task<Recipe> UpdateIngredientInRecipe(Guid recipeId, Ingredient ingredient)
        {
            return (await _recipeRepo.UpdateIngredientInRecipe(recipeId, ingredient.ConvertFromDTO())).ConvertToDTO();
        }

        public async Task<Recipe> DeleteIngredientInRecipe(Guid recipeId, Ingredient ingredient) => await DeleteIngredientInRecipe(recipeId, ingredient.ID);

        public async Task<Recipe> DeleteIngredientInRecipe(Guid recipeId, Guid ingredientId)
        {
            return (await _recipeRepo.RemoveIngredientFromRecipe(recipeId, ingredientId)).ConvertToDTO();
        }

        public async Task<Recipe> AddStepToRecipe(Guid recipeId, Step step)
        {
            return (await _recipeRepo.AddStepToRecipe(recipeId, step.ConvertFromDTO())).ConvertToDTO();
        }

        public async Task<Recipe> UpdateStepInRecipe(Guid recipeId, Step step)
        {
            return (await _recipeRepo.UpdateStepInRecipe(recipeId, step.ConvertFromDTO())).ConvertToDTO();
        }

        public async Task<Recipe> DeleteStepInRecipe(Guid recipeId, Step step)
        {
            return (await _recipeRepo.RemoveStepFromRecipe(recipeId, step.ID)).ConvertToDTO();
        }

        public async Task<Recipe> DeleteStepInRecipe(Guid recipeId, Guid stepId)
        {
            return (await _recipeRepo.RemoveStepFromRecipe(recipeId, stepId)).ConvertToDTO();
        }
    }
}