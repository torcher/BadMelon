using BadMelon.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BadMelon.API.Services
{
    public interface IRecipeService
    {
        Task<Recipe[]> GetRecipes();
        Task<Recipe> GetRecipeByID(Guid ID);
    }
}
