using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BadMelon.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BadMelon.Data.Repos
{
    public class RecipeRepo : IRecipeRepo
    {
        private readonly BadMelonDataContext _db;
        public RecipeRepo(BadMelonDataContext db)
        {
            _db = db;
        }

        public async Task<Recipe> GetOne(Guid ID)
        {
            return await _db.Recipes.SingleOrDefaultAsync(recipe => recipe.ID == ID);
        }

        public async Task<Recipe[]> GetAll()
        {
            return await _db.Recipes.ToArrayAsync();
        }
    }
}
