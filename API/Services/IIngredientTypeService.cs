using BadMelon.API.DTOs;
using System;
using System.Threading.Tasks;

namespace BadMelon.API.Services
{
    public interface IIngredientTypeService
    {
        Task<IngredientType[]> GetIngredientTypes();

        Task<IngredientType> GetIngredientType(Guid id);

        Task<IngredientType> AddIngredientType(IngredientType ingredientType);
    }
}