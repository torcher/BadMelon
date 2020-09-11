using BadMelon.Data.DTOs;
using BadMelon.Data.Exceptions;
using BadMelon.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BadMelon.Data.Services
{
    public class IngredientTypeService : IIngredientTypeService
    {
        private readonly BadMelonDataContext _db;

        public IngredientTypeService(BadMelonDataContext db)
        {
            _db = db;
        }

        public async Task<IngredientType> GetIngredientType(Guid id)
        {
            var ingredientType = await _db.IngredientTypes.SingleOrDefaultAsync(it => it.ID == id);
            if (ingredientType == null)
                throw new EntityNotFoundException("ingredient type");
            return ingredientType.ConvertToDTO();
        }

        public async Task<IngredientType[]> GetIngredientTypes() => (await _db.IngredientTypes.ToArrayAsync())?.ConvertToDTO();

        public async Task<IngredientType> AddIngredientType(IngredientType ingredientType)
        {
            var ingredientTypeEntity = ingredientType.ConvertToEntity();
            var newIngredientType = _db.IngredientTypes.CreateProxy();
            EntityCopier.Copy(ingredientTypeEntity, newIngredientType);
            await _db.IngredientTypes.AddAsync(newIngredientType);
            await _db.SaveChangesAsync();
            return newIngredientType.ConvertToDTO();
        }
    }
}