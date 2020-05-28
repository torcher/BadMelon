using BadMelon.Data.DTOs;
using System;
using System.Threading.Tasks;

namespace BadMelon.Data.Services
{
    public interface IIngredientTypeService
    {
        Task<IngredientType[]> GetIngredientTypes();

        Task<IngredientType> GetIngredientType(Guid id);

        Task<IngredientType> AddIngredientType(IngredientType ingredientType);
    }
}