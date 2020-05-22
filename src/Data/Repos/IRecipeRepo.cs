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
    }
}