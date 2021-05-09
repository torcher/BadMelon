using BadMelon.Data.DTOs;
using System;
using System.Threading.Tasks;

namespace BadMelon.Data.Services
{
    public class IngredientTypeService : CrudServiceAbstract<Entities.IngredientType>, IIngredientTypeService
    {
        public IngredientTypeService(BadMelonDataContext db) : base(db)
        {
        }

        public async Task<IngredientType> GetIngredientType(Guid id)
        {
            var ingredientType = await GetOne(id);
            return ingredientType.ConvertToDTO();
        }

        public async Task<IngredientType[]> GetIngredientTypes() => (await GetAll()).ConvertToDTO();

        public async Task<IngredientType> AddIngredientType(IngredientType ingredientType)
        {
            var ingredientTypeEntity = ingredientType.ConvertToEntity();
            await AddOne(ingredientTypeEntity);
            return ingredientTypeEntity.ConvertToDTO();
        }
    }
}