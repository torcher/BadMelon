using BadMelon.Data.Entities;
using BadMelon.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BadMelon.Data.Repos
{
    public class IngredientTypeRepo : IIngredientTypeRepo
    {
        private readonly BadMelonDataContext _db;

        public IngredientTypeRepo(BadMelonDataContext db)
        {
            _db = db;
        }

        public async Task<IngredientType> Get(Guid id)
        {
            return await _db.IngredientTypes.SingleOrDefaultAsync(it => it.ID == id);
        }

        public async Task<IngredientType[]> Get()
        {
            return await _db.IngredientTypes.ToArrayAsync();
        }

        public async Task<IngredientType> Add(IngredientType ingredientType)
        {
            ingredientType.ID = Guid.NewGuid();
            try
            {
                await _db.IngredientTypes.AddAsync(ingredientType);
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new RepoException("Error adding Ingredient type to database.", e);
            }

            return await Get(ingredientType.ID);
        }
    }
}