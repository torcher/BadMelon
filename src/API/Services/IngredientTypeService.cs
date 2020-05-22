using BadMelon.API.DTOs;
using BadMelon.Data.Repos;
using System;
using System.Threading.Tasks;

namespace BadMelon.API.Services
{
    public class IngredientTypeService : IIngredientTypeService
    {
        private readonly IIngredientTypeRepo _repo;

        public IngredientTypeService(IIngredientTypeRepo repo)
        {
            _repo = repo;
        }

        public async Task<IngredientType> GetIngredientType(Guid id) => (await _repo.Get(id))?.ConvertToDTO();

        public async Task<IngredientType[]> GetIngredientTypes() => (await _repo.Get())?.ConvertToDTO();

        public async Task<IngredientType> AddIngredientType(IngredientType ingredientType) => (await _repo.Add(ingredientType.ConvertFromDTO())).ConvertToDTO();
    }
}