using BadMelon.Data.DTOs;
using BadMelon.Data.Services;
using BadMelon.Tests.Fixtures;
using BadMelon.Tests.Fixtures.DTOs;
using BadMelon.Tests.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BadMelon.Tests.Controllers
{
    [Collection("SynchronousTests")]
    public class IngredientTypeControllerTests : ControllerTestsFixture
    {
        [Fact]
        public async Task Get_IngredientType_ExpectAllIngredientTypes()
        {
            var response = await _http.GetAsync("api/ingredienttype");
            response.EnsureSuccessStatusCode();

            var ingredientTypes = await response.GetObject<IngredientType[]>();
            Assert.False(ingredientTypes == null || ingredientTypes.Length == 0, "Should have returned at least one ingredient type");
            foreach (var it in ingredientTypes)
                ValidateIngredientType(it);
        }

        [Fact]
        public async Task Get_IngredientTypeByID_ExpectIngredientType()
        {
            var getAllResponse = await _http.GetAsync("api/ingredienttype");
            getAllResponse.EnsureSuccessStatusCode();

            var ingredientTypes = await getAllResponse.GetObject<IngredientType[]>();

            var getOneResponse = await _http.GetAsync("api/ingredienttype/" + ingredientTypes.First().ID);
            getOneResponse.EnsureSuccessStatusCode();

            var ingredientType = await getOneResponse.GetObject<IngredientType>();
            Assert.True(ingredientType.ID == ingredientTypes.First().ID, "IngredientType IDs should be the same");
            ValidateIngredientType(ingredientType);
        }

        [Fact]
        public async Task Get_IngredientTypeByID_DoesntExist_ExpectError()
        {
            var getOneResponse = await _http.GetAsync("api/ingredienttype/" + Guid.NewGuid());
            Assert.True(getOneResponse.StatusCode == System.Net.HttpStatusCode.NotFound, "Should not be able to find random Ingredient Type");
            var content = await getOneResponse.Content.ReadAsStringAsync();
            Assert.True(content == "{\"Message\":\"Could not find ingredient type\"}", "Response should be 'Could not find ingredient type'");
        }

        [Fact]
        public async Task Post_IngredientType_ExpectIngredientType()
        {
            var getallResponse = await _http.GetAsync("api/ingredientType");
            getallResponse.EnsureSuccessStatusCode();
            var ingredientTypes = await getallResponse.GetObject<IngredientType[]>();

            var newIngredientType = new IngredientTypeFixture("uranium").Build();
            var response = await _http.PostAsync("api/ingredientType", newIngredientType.GetStringContent());
            response.EnsureSuccessStatusCode();
            var newIngredientReturned = await response.GetObject<IngredientType>();
            dataSamples.AddIngredientTypeToStorage(newIngredientReturned.ConvertFromDTO());

            var updatedIngredientTypesResponse = await _http.GetAsync("api/ingredientType");
            updatedIngredientTypesResponse.EnsureSuccessStatusCode();

            var updatedIngredientType = await response.GetObject<Recipe>();
            var updatedIngredientTypes = await updatedIngredientTypesResponse.GetObject<Recipe[]>();
            Assert.False(updatedIngredientType == null, "Ingredient Type should not be null");
            Assert.True(updatedIngredientType.Name == newIngredientType.Name, "Names should be the same");
            Assert.True(ingredientTypes.Length + 1 == updatedIngredientTypes.Length, "There should be one new Ingredient Type");
        }

        [Theory]
        [ClassData(typeof(BadIngredientTypesTestData))]
        public async Task Post_BadRecipe_ExpectError(IngredientType newIngredientType, int statusCodeExpected)
        {
            var response = await _http.PostAsync("api/ingredientType", newIngredientType.GetStringContent());
            Assert.True((int)response.StatusCode == statusCodeExpected, "Status code should be " + statusCodeExpected + " but was " + (int)response.StatusCode);
        }

        private void ValidateIngredientType(IngredientType ingredientType)
        {
            Assert.False(ingredientType.ID == Guid.Empty, "Ingredient Type ID should not be empty");
            Assert.False(string.IsNullOrEmpty(ingredientType.Name), "Name cannot be empty");
        }
    }
}