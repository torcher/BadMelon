using BadMelon.Data.Entities;
using System;
using System.Threading.Tasks;

namespace BadMelon.Data.Repos
{
    public interface IIngredientTypeRepo
    {
        Task<IngredientType[]> Get();

        Task<IngredientType> Get(Guid id);

        Task<IngredientType> Add(IngredientType ingredientType);
    }
}