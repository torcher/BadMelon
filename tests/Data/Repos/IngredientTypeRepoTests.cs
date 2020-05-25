using BadMelon.Data;
using BadMelon.Data.Entities;
using BadMelon.Data.Repos;
using BadMelon.Tests.Data.Fixtures;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BadMelon.Tests.Data.Repos
{
    public class IngredientTypeRepoTests : BadMelonDataContextFixture
    {
        private readonly DataSamples dataSamples;
        private readonly IngredientTypeRepo ingredientTypeRepo;

        public IngredientTypeRepoTests()
        {
            dataSamples = new DataSamples();
            WithSeedData();
            ingredientTypeRepo = new IngredientTypeRepo(BadMelonDataContext);
        }

        [Fact]
        public async Task Get_WhenGetAll_ExpectAllPropertiesAndJoins()
        {
            var ingredientTypes = await ingredientTypeRepo.Get();

            Assert.True(ingredientTypes != null && ingredientTypes.Length != 0, "Ingredient Type should not be null or empty");
            Assert.True(dataSamples.IngredientTypes.Length == ingredientTypes.Length, "Should be returning all Ingredient Types.");
            for (int i = 0; i < ingredientTypes.Length; i++)
                ValidateIngredientType(ingredientTypes[i]);
        }

        [Fact]
        public async Task Get_WhenRecipeExists_ExpectRecipe()
        {
            var ingredientTypeId = (await ingredientTypeRepo.Get()).First().ID;
            var ingredientType = await ingredientTypeRepo.Get(ingredientTypeId);

            Assert.True(ingredientType != null, "Ingredient Type should exist");
            ValidateIngredientType(ingredientType);
        }

        [Fact]
        public async Task Get_WhenIngredientTypeDoestExist_ExpectNull()
        {
            var ingredientType = await ingredientTypeRepo.Get(Guid.NewGuid());
            Assert.True(ingredientType == null, "Ingredient Type with random GUID should not be found");
        }

        [Fact]
        public async Task Post_WhenRecipeIDIsSet_ExpectIDReset()
        {
            var newIngredientType = dataSamples.NewIngredientType;
            var newGuid = Guid.NewGuid();
            newIngredientType.ID = newGuid;
            var createdIngredientType = await ingredientTypeRepo.Add(newIngredientType);
            Assert.True(createdIngredientType != null, "Created Ingredient Type should not be null");
            Assert.True(createdIngredientType.ID != newGuid, "Created Ingredient Type should have new ID");
            ValidateIngredientType(createdIngredientType);
        }

        private void ValidateIngredientType(IngredientType ingredientType)
        {
            Assert.False(ingredientType.ID == Guid.Empty, "Ingredient Type ID cannot be empty");
            Assert.False(string.IsNullOrEmpty(ingredientType.Name), "Ingredient Type must have a name");
        }
    }
}