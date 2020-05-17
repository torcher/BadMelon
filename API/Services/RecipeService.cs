using System;
using System.Linq;
using System.Threading.Tasks;
using BadMelon.Data.Repos;
using BadMelon.RecipeMath;

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
            return (await _recipeRepo.GetAll())
                .ConvertToDomain()
                .ToArray();
        }
        public async Task<Recipe> GetRecipeByID(Guid ID)
        {
            return (await _recipeRepo.GetOne(ID))?.ConvertToDomain();
        }
    }
}
