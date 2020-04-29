using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BadMelon.API.Models;
using BadMelon.Data;
using BadMelon.Data.Repos;
using Microsoft.EntityFrameworkCore;

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
