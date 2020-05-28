using BadMelon.Data.DTOs;
using BadMelon.Data.Services;
using BadMelon.Tests.API.Data;
using BadMelon.Tests.API.Fixtures;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BadMelon.Tests.API.Controllers
{
    [Collection("SynchronousTests")]
    public class IngredientTypeControllerTests : ControllerTestsFixture
    {
        [Fact]
        public async Task Get_IngredientType_ExpectAllIngredientTypes()
        {
            var response = await _http.GetAsync("api/ingredienttype");
            response.EnsureSuccessStatusCode();

            var ingredientTypeJson = await response.Content.ReadAsStringAsync();
            var ingredientTypes = JsonConvert.DeserializeObject<IngredientType[]>(ingredientTypeJson);
            Assert.False(ingredientTypes == null || ingredientTypes.Length == 0, "Should have returned at least one ingredient type");
            foreach (var it in ingredientTypes)
                ValidateIngredientType(it);
        }

        [Fact]
        public async Task Get_IngredientTypeByID_ExpectIngredientType()
        {
            var getAllResponse = await _http.GetAsync("api/ingredienttype");
            getAllResponse.EnsureSuccessStatusCode();

            var ingredientTypesJson = await getAllResponse.Content.ReadAsStringAsync();
            var ingredientTypes = JsonConvert.DeserializeObject<IngredientType[]>(ingredientTypesJson);

            var getOneResponse = await _http.GetAsync("api/ingredienttype/" + ingredientTypes.First().ID);
            getOneResponse.EnsureSuccessStatusCode();

            var ingredientTypeJson = await getOneResponse.Content.ReadAsStringAsync();
            var ingredientType = JsonConvert.DeserializeObject<IngredientType>(ingredientTypeJson);
            Assert.True(ingredientType.ID == ingredientTypes.First().ID, "IngredientType IDs should be the same");
            ValidateIngredientType(ingredientType);
        }

        [Fact]
        public async Task Get_IngredientTypeByID_DoesntExist_ExpectError()
        {
            var getOneResponse = await _http.GetAsync("api/ingredienttype/" + Guid.NewGuid());
            Assert.True(getOneResponse.StatusCode == System.Net.HttpStatusCode.NotFound, "Should not be able to find random Ingredient Type");
            var content = await getOneResponse.Content.ReadAsStringAsync();
            Assert.True(string.IsNullOrEmpty(content), "Response should be empty");
        }

        [Fact]
        public async Task Post_IngredientType_ExpectIngredientType()
        {
            var getallResponse = await _http.GetAsync("api/ingredientType");
            getallResponse.EnsureSuccessStatusCode();
            var ingredientTypes = JsonConvert.DeserializeObject<IngredientType[]>(await getallResponse.Content.ReadAsStringAsync());

            var newIngredientType = dataSamples.NewIngredientType.ConvertToDTO();
            var newRecipeJson = JsonConvert.SerializeObject(newIngredientType);
            var requestBody = new StringContent(newRecipeJson, Encoding.UTF8, "application/json");
            var response = await _http.PostAsync("api/ingredientType", requestBody);
            response.EnsureSuccessStatusCode();
            dataSamples.AddNewIngredientTypeToStorage();

            var updatedIngredientTypesResponse = await _http.GetAsync("api/ingredientType");
            updatedIngredientTypesResponse.EnsureSuccessStatusCode();

            var updatedIngredientType = JsonConvert.DeserializeObject<Recipe>(await response.Content.ReadAsStringAsync());
            var updatedIngredientTypes = JsonConvert.DeserializeObject<Recipe[]>(await updatedIngredientTypesResponse.Content.ReadAsStringAsync());
            Assert.False(updatedIngredientType == null, "Ingredient Type should not be null");
            Assert.True(updatedIngredientType.Name == newIngredientType.Name, "Names should be the same");
            Assert.True(ingredientTypes.Length + 1 == updatedIngredientTypes.Length, "There should be one new Ingredient Type");
        }

        [Theory]
        [ClassData(typeof(BadIngredientTypesTestData))]
        public async Task Post_BadRecipe_ExpectError(IngredientType newIngredientType, int statusCodeExpected)
        {
            var body = new StringContent(JsonConvert.SerializeObject(newIngredientType), Encoding.UTF8, "application/json");
            var response = await _http.PostAsync("api/ingredientType", body);
            Assert.True((int)response.StatusCode == statusCodeExpected, "Status code should be " + statusCodeExpected + " but was " + (int)response.StatusCode);
        }

        private void ValidateIngredientType(IngredientType ingredientType)
        {
            Assert.False(ingredientType.ID == Guid.Empty, "Ingredient Type ID should not be empty");
            Assert.False(string.IsNullOrEmpty(ingredientType.Name), "Name cannot be empty");
        }
    }
}