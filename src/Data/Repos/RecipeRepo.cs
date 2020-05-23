using BadMelon.Data.Entities;
using BadMelon.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BadMelon.Data.Repos
{
    public class RecipeRepo : IRecipeRepo
    {
        private readonly BadMelonDataContext _db;

        public RecipeRepo(BadMelonDataContext db)
        {
            _db = db;
        }

        public async Task<Recipe> Get(Guid ID)
        {
            return await _db.Recipes
                .Include(i => i.Ingredients)
                .ThenInclude(t => t.IngredientType)
                .Include(s => s.Steps)
                .SingleOrDefaultAsync(recipe => recipe.ID == ID);
        }

        public async Task<Recipe[]> Get()
        {
            return await _db.Recipes
                .Include(I => I.Ingredients)
                .ThenInclude(IT => IT.IngredientType)
                .ToArrayAsync();
        }

        public async Task<Recipe> AddRecipe(Recipe recipe)
        {
            if (recipe.Ingredients == null || recipe.Ingredients.Count == 0)
                throw new RepoException("Cannot add recipe without an Ingredient");
            if (recipe.Steps == null || recipe.Steps.Count == 0)
                throw new RepoException("Cannot add recipe without a Step");

            recipe.ID = Guid.NewGuid();
            foreach (var i in recipe.Ingredients) i.ID = Guid.NewGuid();
            foreach (var s in recipe.Steps) s.ID = Guid.NewGuid();

            try
            {
                await _db.Recipes.AddAsync(recipe);
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new RepoException(e.Message, e.InnerException);
            }

            return await Get(recipe.ID);
        }
    }
}