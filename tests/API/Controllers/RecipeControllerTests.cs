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
    public class RecipeControllerTests : ControllerTestsFixture
    {
        [Fact]
        public async Task Get_Recipe_ExpectAllRecipes()
        {
            var expectedRecipes = dataSamples.Recipes.ConvertToDTOs();
            var response = await _http.GetAsync("api/recipe");
            response.EnsureSuccessStatusCode();

            var recipesJson = await response.Content.ReadAsStringAsync();
            var recipes = JsonConvert.DeserializeObject<Recipe[]>(recipesJson);
            Assert.True(expectedRecipes.Length == recipes.Length, "Recipes returned should be the same");
            for (int i = 0; i < expectedRecipes.Length; i++)
            {
                Assert.True(recipes[i].Name == expectedRecipes[i].Name, "Recipe names should be the same");
                ValidateRecipe(recipes[i]);
            }
        }

        [Fact]
        public async Task Get_RecipeByID_ExpectRecipe()
        {
            var recipesResponse = await _http.GetAsync("api/recipe");
            recipesResponse.EnsureSuccessStatusCode();

            var recipesJson = await recipesResponse.Content.ReadAsStringAsync();
            var recipes = JsonConvert.DeserializeObject<Recipe[]>(recipesJson);

            var response = await _http.GetAsync("api/recipe/" + recipes.First().ID);
            response.EnsureSuccessStatusCode();

            var recipeJson = await response.Content.ReadAsStringAsync();
            var recipe = JsonConvert.DeserializeObject<Recipe>(recipeJson);
            Assert.True(recipe.ID == recipes.First().ID, "ID shouldn't change");
            ValidateRecipe(recipe);
        }

        [Fact]
        public async Task Get_RecipeByID_DoesntExist_ExpectError()
        {
            var response = await _http.GetAsync("api/recipe/" + Guid.NewGuid());
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.NotFound, "HTTP Code should be Not Found");
        }

        [Fact]
        public async Task Post_Recipe_ExpectRecipe()
        {
            var getallResponse = await _http.GetAsync("api/recipe");
            getallResponse.EnsureSuccessStatusCode();
            var c = await getallResponse.Content.ReadAsStringAsync();
            var recipes = JsonConvert.DeserializeObject<Recipe[]>(c);

            var newRecipe = new RecipeFixture("new recipe")
                .WithIngredient(new Ingredient { Weight = 1d, TypeID = recipes.First().Ingredients.First().TypeID })
                .WithStep(new Step { Text = "Cook me", CookTime = "00:10:00", PrepTime = "00:20:00" }).Build();

            newRecipe.Ingredients.First().TypeID = recipes.First().Ingredients.First().TypeID;
            var newRecipeJson = JsonConvert.SerializeObject(newRecipe);
            var requestBody = new StringContent(newRecipeJson, System.Text.Encoding.UTF8, "application/json");
            var response = await _http.PostAsync("api/recipe", requestBody);
            var cc = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            var updatedRecipe = JsonConvert.DeserializeObject<Recipe>(await response.Content.ReadAsStringAsync());
            dataSamples.AddRecipeToStorage(updatedRecipe.ConvertFromDTO());

            var updatedRecipesResponse = await _http.GetAsync("api/recipe");
            updatedRecipesResponse.EnsureSuccessStatusCode();

            var updatedRecipes = JsonConvert.DeserializeObject<Recipe[]>(await updatedRecipesResponse.Content.ReadAsStringAsync());
            Assert.False(updatedRecipe == null, "Recipe should not be null");
            Assert.True(updatedRecipe.Name == newRecipe.Name, "Names should be the same");
            Assert.True(recipes.Length + 1 == updatedRecipes.Length, "There should be one new recipe");
        }

        [Theory]
        [ClassData(typeof(BadRecipesTestData))]
        public async Task Post_BadRecipe_ExpectError(Recipe newRecipe, int statusCodeExpected)
        {
            var body = new StringContent(JsonConvert.SerializeObject(newRecipe), Encoding.UTF8, "application/json");
            var response = await _http.PostAsync("api/recipe", body);
            Assert.True((int)response.StatusCode == statusCodeExpected, "Status code should be " + statusCodeExpected + " but was " + (int)response.StatusCode);
        }

        [Fact]
        public async Task Post_RecipeIngredient_DifferentType_ExpectRecipe()
        {
            var getAllRecipesResponse = await _http.GetAsync("api/recipe");
            getAllRecipesResponse.EnsureSuccessStatusCode();
            var allRecipes = JsonConvert.DeserializeObject<Recipe[]>(await getAllRecipesResponse.Content.ReadAsStringAsync());
            var oldRecipe = allRecipes.First();
            var newIngredientType = new IngredientType { Name = "Twinkleberries" };
            var newIngredientTypeResponse = await _http.PostAsync("api/ingredienttype", new StringContent(
                    JsonConvert.SerializeObject(newIngredientType),
                    Encoding.UTF8,
                    "application/json"
                ));
            newIngredientTypeResponse.EnsureSuccessStatusCode();
            var newIngredientTypeWithID = JsonConvert.DeserializeObject<IngredientType>(await newIngredientTypeResponse.Content.ReadAsStringAsync());

            var newIngredient = new Ingredient { Weight = 2d, TypeID = newIngredientTypeWithID.ID };
            var body = new StringContent(JsonConvert.SerializeObject(newIngredient), Encoding.UTF8, "application/json");
            var updatedRecipeResponse = await _http.PostAsync($"api/recipe/{oldRecipe.ID}/ingredients", body);
            updatedRecipeResponse.EnsureSuccessStatusCode();
            var updatedRecipe = JsonConvert.DeserializeObject<Recipe>(await updatedRecipeResponse.Content.ReadAsStringAsync());

            Assert.NotNull(updatedRecipe);
            Assert.True(oldRecipe.Ingredients.Count + 1 == updatedRecipe.Ingredients.Count, "Ingredient count should be increased by 1");
            var includesIngredientType = updatedRecipe.Ingredients.SingleOrDefault(i => i.TypeID == newIngredientTypeWithID.ID);
            Assert.False(includesIngredientType == null, "New ingredient should have been added.");
        }

        [Fact]
        public async Task Post_RecipeIngredient_SameType_ExpectRecipe()
        {
            var getAllRecipesResponse = await _http.GetAsync("api/recipe");
            getAllRecipesResponse.EnsureSuccessStatusCode();
            var allRecipes = JsonConvert.DeserializeObject<Recipe[]>(await getAllRecipesResponse.Content.ReadAsStringAsync());
            var oldRecipe = allRecipes.First();
            var oldIngredientType = oldRecipe.Ingredients.First().TypeID;

            var newIngredient = new Ingredient { Weight = 2d, TypeID = oldIngredientType };
            var body = new StringContent(JsonConvert.SerializeObject(newIngredient), Encoding.UTF8, "application/json");
            var updatedRecipeResponse = await _http.PostAsync($"api/recipe/{oldRecipe.ID}/ingredients", body);
            updatedRecipeResponse.EnsureSuccessStatusCode();
            var updatedRecipe = JsonConvert.DeserializeObject<Recipe>(await updatedRecipeResponse.Content.ReadAsStringAsync());

            Assert.NotNull(updatedRecipe);
            Assert.True(oldRecipe.Ingredients.Count == updatedRecipe.Ingredients.Count, "Ingredient count should be the same");
            var includesIngredientType = updatedRecipe.Ingredients.SingleOrDefault(i => i.TypeID == oldIngredientType);
            Assert.False(includesIngredientType == null, "New ingredient should have been added.");
        }

        [Fact]
        public async Task Post_RecipeIngredient_WhenRecipeMissing_ExpectNotFound()
        {
            var getAllRecipesResponse = await _http.GetAsync("api/recipe");
            getAllRecipesResponse.EnsureSuccessStatusCode();
            var allRecipes = JsonConvert.DeserializeObject<Recipe[]>(await getAllRecipesResponse.Content.ReadAsStringAsync());
            var oldRecipe = allRecipes.First();
            var oldIngredient = oldRecipe.Ingredients.First();

            var newIngredient = new Ingredient { Weight = 2d, TypeID = oldIngredient.TypeID };
            var body = new StringContent(JsonConvert.SerializeObject(newIngredient), Encoding.UTF8, "application/json");
            var updatedRecipeResponse = await _http.PostAsync($"api/recipe/{Guid.NewGuid()}/ingredients", body);
            Assert.True(updatedRecipeResponse.StatusCode == System.Net.HttpStatusCode.NotFound, "Should be not found");
        }

        [Fact]
        public async Task Post_RecipeIngredient_WhenIngredientTypeMissing_ExpectNotFound()
        {
            var getAllRecipesResponse = await _http.GetAsync("api/recipe");
            getAllRecipesResponse.EnsureSuccessStatusCode();
            var allRecipes = JsonConvert.DeserializeObject<Recipe[]>(await getAllRecipesResponse.Content.ReadAsStringAsync());
            var oldRecipe = allRecipes.First();
            var oldIngredient = oldRecipe.Ingredients.First();

            var newIngredient = new Ingredient { Weight = 2d, TypeID = Guid.NewGuid() };
            var body = new StringContent(JsonConvert.SerializeObject(newIngredient), Encoding.UTF8, "application/json");
            var updatedRecipeResponse = await _http.PostAsync($"api/recipe/{oldRecipe.ID}/ingredients", body);
            var c = await updatedRecipeResponse.Content.ReadAsStringAsync();
            Assert.True(updatedRecipeResponse.StatusCode == System.Net.HttpStatusCode.NotFound, "Should be not found");
        }

        [Fact]
        public async Task Post_RecipeIngredient_BadWhenIngredient_ExpectBadRequest()
        {
            var getAllRecipesResponse = await _http.GetAsync("api/recipe");
            getAllRecipesResponse.EnsureSuccessStatusCode();
            var allRecipes = JsonConvert.DeserializeObject<Recipe[]>(await getAllRecipesResponse.Content.ReadAsStringAsync());
            var oldRecipe = allRecipes.First();
            var oldIngredient = oldRecipe.Ingredients.First();

            var newIngredient = new Ingredient { Weight = -2d, TypeID = oldIngredient.TypeID };
            var body = new StringContent(JsonConvert.SerializeObject(newIngredient), Encoding.UTF8, "application/json");
            var updatedRecipeResponse = await _http.PostAsync($"api/recipe/{oldRecipe.ID}/ingredients", body);
            Assert.True(updatedRecipeResponse.StatusCode == System.Net.HttpStatusCode.BadRequest, "Should be bad request");
        }

        private void ValidateRecipe(Recipe recipe)
        {
            Assert.False(recipe.ID == Guid.Empty, "Recipe ID cannot be empty");
            Assert.False(string.IsNullOrEmpty(recipe.Name), "Name cannot be blank");
            Assert.False(recipe.Ingredients == null || recipe.Ingredients.Count == 0, "There should be at least one Ingredient");
            foreach (var i in recipe.Ingredients)
            {
                Assert.False(string.IsNullOrEmpty(i.Type), "Type should exist");
                Assert.False(i.TypeID == Guid.Empty, "ID cannot be empty");
                Assert.True(i.Weight >= 0d, "Weight cannot be less than 0");
            }
            Assert.False(recipe.Steps == null || recipe.Steps.Count == 0, "There should be at least one Step");
        }
    }
}