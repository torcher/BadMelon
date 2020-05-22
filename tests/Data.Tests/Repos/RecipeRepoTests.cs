using BadMelon.Data.Entities;
using BadMelon.Data.Repos;
using BadMelon.Tests.Data.Fixtures;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BadMelon.Tests.Data.Repos
{
    public class RecipeRepoTests : BadMelonDataContextFixture
    {
        private readonly DataSamples dataSamples;
        private readonly RecipeRepo recipeRepo;

        public RecipeRepoTests()
        {
            dataSamples = new DataSamples();
            WithRecipes(dataSamples.Recipes);
            recipeRepo = new RecipeRepo(BadMelonDataContext);
        }

        [Fact]
        public async Task Get_WhenGetAll_ExpectAllPropertiesAndJoins()
        {
            var recipes = await recipeRepo.Get();

            Assert.True(recipes != null && recipes.Length != 0, "Recipes should not be null or empty");
            Assert.True(dataSamples.Recipes.Length == recipes.Length, "Should be returning all recipes.");
            for (int i = 0; i < recipes.Length; i++)
                ValidateRecipe(recipes[i]);
        }

        [Fact]
        public async Task Get_WhenRecipeExists_ExpectRecipe()
        {
            var recipeId = (await recipeRepo.Get()).First().ID;
            var recipe = await recipeRepo.Get(recipeId);

            Assert.True(recipe != null, "Recipe should exist");
            ValidateRecipe(recipe);
        }

        [Fact]
        public async Task Get_WhenRecipeDoestExist_ExpectNull()
        {
            var recipe = await recipeRepo.Get(Guid.NewGuid());
            Assert.True(recipe == null, "Recipe with random GUID should not be found");
        }

        [Fact]
        public async Task Post_WhenRecipeIDIsSet_ExpectIDReset()
        {
            var newRecipe = dataSamples.NewRecipe;
            var newGuid = Guid.NewGuid();
            newRecipe.ID = newGuid;
            var createdRecipe = await recipeRepo.AddRecipe(newRecipe);
            Assert.True(createdRecipe != null, "Created Recipe should not be null");
            Assert.True(createdRecipe.ID != newGuid, "Created Recipe should have new ID");
            ValidateRecipe(createdRecipe);
        }

        //TODO: Fill out these tests
        //Add - Name is too long - Error thrown
        //Add - No ingredients - Error thrown
        //Add - Invalid ingredient - Error thrown
        //Add - No steps - Error thrown
        //Add - Invalid steps - Error thrown

        private void ValidateRecipe(Recipe recipe)
        {
            Assert.True(recipe.ID != Guid.Empty, "Recipe ID cannot be empty");
            Assert.True(!string.IsNullOrEmpty(recipe.Name), "Recipe Name cannot be empty");
            Assert.True(recipe.Ingredients != null && recipe.Ingredients.Count > 0, "All Recipes must have ingredients");
            Assert.True(recipe.Steps != null && recipe.Steps.Count > 0, "All Recipes must have a step");
        }
    }
}