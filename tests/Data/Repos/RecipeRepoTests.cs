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
            var recipes = await recipeRepo.Get();
            var newGuid = Guid.NewGuid();
            var newRecipe = new RecipeFixture("new recipe")
                .WithIngredient(new Ingredient { Weight = 1d, IngredientTypeID = recipes.First().Ingredients.First().IngredientTypeID })
                .WithStep(new Step { Text = "Make me" })
                .WithID(newGuid)
                .Build();

            var createdRecipe = await recipeRepo.AddRecipe(newRecipe);
            Assert.True(createdRecipe != null, "Created Recipe should not be null");
            Assert.True(createdRecipe.ID != newGuid, "Created Recipe should have new ID");
            ValidateRecipe(createdRecipe);
        }

        [Fact]
        public async Task Add_WhenRecipeMissingIngredients_ExpectExcecption()
        {
            var newRecipe = new RecipeFixture("new recipe").WithStep(new Step { Text = "Make me" }).Build();
            newRecipe.Ingredients = null;
            await Assert.ThrowsAsync<RepoException>(() => recipeRepo.AddRecipe(newRecipe));
        }

        [Fact]
        public async Task Add_WhenRecipeMissingSteps_ExpectExcecption()
        {
            var newRecipe = new RecipeFixture("new recipe").WithIngredient(new Ingredient()).Build();
            newRecipe.Steps = null;
            await Assert.ThrowsAsync<RepoException>(() => recipeRepo.AddRecipe(newRecipe));
        }

        [Fact]
        public async Task AddIngredient_WhenValid_ExpectSuccess()
        {
            var recipes = await recipeRepo.Get();
            var startIngredientCount = recipes.First().Ingredients.Count;
            var newIngredient = new IngredientFixture(recipes.First().Ingredients.First().IngredientTypeID).Build();
            var updatedRecipe = await recipeRepo.AddIngredientToRecipe(recipes.First().ID, newIngredient);
            Assert.NotNull(updatedRecipe);
            Assert.True(updatedRecipe.Ingredients.Count == startIngredientCount + 1, "There should be one new ingredient");
        }

        [Fact]
        public async Task AddIngredient_WhenRecipeMissing_ExpectExeption()
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(() => recipeRepo.AddIngredientToRecipe(Guid.NewGuid(), new IngredientFixture(Guid.NewGuid()).Build()));
        }

        [Fact]
        public async Task AddIngredient_WhenIngredientTypeMissing_ExpectExeption()
        {
            var newIngredient = new IngredientFixture(Guid.Empty).WithWeight(1d).Build();
            await Assert.ThrowsAsync<EntityNotFoundException>(() => recipeRepo.AddIngredientToRecipe(Guid.NewGuid(), newIngredient));
        }

        [Fact]
        public async Task AddIngredient_WhenIngredientInvalid_ExpectException()
        {
            var recipes = await recipeRepo.Get();
            var newIngredient = new IngredientFixture(recipes.First().Ingredients.First().IngredientTypeID).WithWeight(-1d).Build();
            await Assert.ThrowsAsync<EntityNotFoundException>(() => recipeRepo.AddIngredientToRecipe(Guid.NewGuid(), newIngredient));
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