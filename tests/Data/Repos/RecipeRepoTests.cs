using BadMelon.Data;
using BadMelon.Data.Entities;
using BadMelon.Data.Exceptions;
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
            WithSeedData();
            recipeRepo = new RecipeRepo(BadMelonDataContext);
        }

        [Fact]
        public async Task Get_WhenNoParams_ExpectAllRecipes()
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
        public async Task Get_WhenRecipeDoestExist_ExpectException()
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(() => recipeRepo.Get(Guid.NewGuid()));
        }

        [Fact]
        public async Task Add_WhenRecipeIDIsSet_ExpectIDReset()
        {
            var newRecipe = dataSamples.NewRecipe;
            var newGuid = Guid.NewGuid();
            newRecipe.ID = newGuid;
            var createdRecipe = await recipeRepo.AddRecipe(newRecipe);
            Assert.True(createdRecipe != null, "Created Recipe should not be null");
            Assert.True(createdRecipe.ID != newGuid, "Created Recipe should have new ID");
            ValidateRecipe(createdRecipe);
        }

        [Fact]
        public async Task Add_WhenRecipeMissingIngredients_ExpectExcecption()
        {
            var newRecipe = dataSamples.NewRecipe;
            newRecipe.Ingredients = null;
            await Assert.ThrowsAsync<RepoException>(() => recipeRepo.AddRecipe(newRecipe));
        }

        [Fact]
        public async Task Add_WhenRecipeMissingSteps_ExpectExcecption()
        {
            var newRecipe = dataSamples.NewRecipe;
            newRecipe.Steps = null;
            await Assert.ThrowsAsync<RepoException>(() => recipeRepo.AddRecipe(newRecipe));
        }

        [Fact]
        public async Task AddIngredient_WhenValid_ExpectSuccess()
        {
            var recipes = await recipeRepo.Get();
            var startIngredientCount = recipes.First().Ingredients.Count;
            var newIngredient = dataSamples.NewIngredient;
            var updatedRecipe = await recipeRepo.AddIngredientToRecipe(recipes.First().ID, newIngredient);
            Assert.NotNull(updatedRecipe);
            Assert.True(updatedRecipe.Ingredients.Count == startIngredientCount + 1, "There should be one new ingredient");
        }

        [Fact]
        public async Task AddIngredient_WhenRecipeMissing_ExpectExeption()
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(() => recipeRepo.AddIngredientToRecipe(Guid.NewGuid(), dataSamples.NewIngredient));
        }

        [Fact]
        public async Task AddIngredient_WhenIngredientTypeMissing_ExpectExeption()
        {
            var newIngredient = dataSamples.NewIngredient;
            newIngredient.IngredientType = null;
            newIngredient.IngredientTypeID = Guid.Empty;
            await Assert.ThrowsAsync<EntityNotFoundException>(() => recipeRepo.AddIngredientToRecipe(Guid.NewGuid(), dataSamples.NewIngredient));
        }

        [Fact]
        public async Task AddIngredient_WhenIngredientInvalid_ExpectException()
        {
            var newIngredient = dataSamples.NewIngredient;
            newIngredient.Weight = -1d;
            await Assert.ThrowsAsync<EntityNotFoundException>(() => recipeRepo.AddIngredientToRecipe(Guid.NewGuid(), dataSamples.NewIngredient));
        }

        private void ValidateRecipe(Recipe recipe)
        {
            Assert.True(recipe.ID != Guid.Empty, "Recipe ID cannot be empty");
            Assert.True(!string.IsNullOrEmpty(recipe.Name), "Recipe Name cannot be empty");
            Assert.True(recipe.Ingredients != null && recipe.Ingredients.Count > 0, "All Recipes must have ingredients");
            Assert.True(recipe.Steps != null && recipe.Steps.Count > 0, "All Recipes must have a step");
        }
    }
}