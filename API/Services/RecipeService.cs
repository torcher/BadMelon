using BadMelon.API.DTOs;
using BadMelon.Data.Repos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BadMelon.API.Services
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
    }
}