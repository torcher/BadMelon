using BadMelon.Data;
using BadMelon.Data.Entities;
using BadMelon.Data.Exceptions;
using BadMelon.Data.Repos;
using BadMelon.Tests.Fixtures;
using BadMelon.Tests.Fixtures.Entities;
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

        [Fact]
        public async Task RemoveIngredient_WhenIngredientExists_ExpectRemoved()
        {
            var recipes = await recipeRepo.Get();
            var testRecipe = recipes.First();
            var removingIngredient = testRecipe.Ingredients.First();
            var ingredientCount = testRecipe.Ingredients.Count;
            Assert.True(ingredientCount > 1, "There needs to be 2 ingredients for this test");

            var updatedRecipe = await recipeRepo.RemoveIngredientFromRecipe(testRecipe.ID, removingIngredient.ID);
            Assert.NotNull(updatedRecipe);
            Assert.True(ingredientCount - 1 == updatedRecipe.Ingredients.Count, "Ingredients should have 1 less");
            Assert.True(updatedRecipe.Ingredients.SingleOrDefault(i => i.ID == removingIngredient.ID) == null, "Ingredient shouldn't exist");
        }

        [Fact]
        public async Task RemoveIngredient_WhenRecipeDoesExist_ExpectException()
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(() => recipeRepo.RemoveIngredientFromRecipe(Guid.NewGuid(), Guid.NewGuid()));
        }

        [Fact]
        public async Task RemoveIngredient_WhenIngredientDoesExist_ExpectException()
        {
            var recipes = await recipeRepo.Get();
            await Assert.ThrowsAsync<EntityNotFoundException>(() => recipeRepo.RemoveIngredientFromRecipe(recipes.First().ID, Guid.NewGuid()));
        }

        [Fact]
        public async Task UpdateIngredient_WhenWeightUpdate_ExpectUpdatedWeight()
        {
            var recipes = await recipeRepo.Get();
            var updatingRecipe = recipes.First();
            var updatingIngredient = updatingRecipe.Ingredients.First();
            var newWeight = updatingIngredient.Weight + 1d;

            updatingIngredient.Weight = newWeight;
            var updatedRecipe = await recipeRepo.UpdateIngredientInRecipe(updatingRecipe.ID, updatingIngredient);
            Assert.NotNull(updatedRecipe);
            Assert.True(updatedRecipe.Ingredients.First().Weight == newWeight, "Weight should be updated");
        }

        [Fact]
        public async Task UpdateIngredient_WhenRecipeMissing_ExpectException()
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(() => recipeRepo.UpdateIngredientInRecipe(Guid.NewGuid(), new Ingredient()));
        }

        [Fact]
        public async Task UpdateIngredient_WhenIngredientMissing_ExpectException()
        {
            var recipes = await recipeRepo.Get();
            await Assert.ThrowsAsync<EntityNotFoundException>(() => recipeRepo.UpdateIngredientInRecipe(recipes.First().ID, new Ingredient()));
        }

        [Fact]
        public async Task AddStep_WhenValid_ExpectSuccess()
        {
            var recipes = await recipeRepo.Get();
            var updatingRecipe = recipes.First();
            var startStepCount = updatingRecipe.Steps.Count;
            var newStep = new StepFixture("New step").Build();
            _ = await recipeRepo.AddStepToRecipe(updatingRecipe.ID, newStep);
            var updatedRecipe = await recipeRepo.Get(updatingRecipe.ID);
            Assert.NotNull(updatedRecipe);
            Assert.True(startStepCount + 1 == updatedRecipe.Steps.Count, "There should be one new recipe");
        }

        [Fact]
        public async Task AddStep_WhenRecipeMissing_ExpectException()
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(() => recipeRepo.AddStepToRecipe(Guid.NewGuid(), new Step()));
        }

        [Fact]
        public async Task RemoveStep_WhenStepExists_ExpectRemoved()
        {
            var recipes = await recipeRepo.Get();
            var updatingRecipe = recipes.First();
            Assert.True(updatingRecipe.Steps.Count > 1, "Need more than 1 step for this step");
            var stepCountStart = updatingRecipe.Steps.Count;

            var removeRecipe = await recipeRepo.RemoveStepFromRecipe(updatingRecipe.ID, updatingRecipe.Steps.First().ID);
            var updatedRecipe = await recipeRepo.Get(updatingRecipe.ID);
            Assert.NotNull(updatedRecipe);
            Assert.True(stepCountStart - 1 == updatedRecipe.Steps.Count, "Steps should have 1 less step");
        }

        [Fact]
        public async Task RemoveStep_WhenRecipeDoesntExist_ExpectException()
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(() => recipeRepo.RemoveStepFromRecipe(Guid.NewGuid(), Guid.NewGuid()));
        }

        [Fact]
        public async Task RemoveStep_WhenStepDoesntExist_ExpectException()
        {
            var recipes = await recipeRepo.Get();
            await Assert.ThrowsAsync<EntityNotFoundException>(() => recipeRepo.RemoveStepFromRecipe(recipes.First().ID, Guid.NewGuid()));
        }

        [Fact]
        public async Task UpdateStep_WhenStepValid_ExpectUpdatedValues()
        {
            var recipes = await recipeRepo.Get();
            var updatingRecicpe = recipes.First();
            var updatingStep = updatingRecicpe.Steps.First();
            var newStep = new Step
            {
                ID = updatingStep.ID,
                Order = updatingStep.Order + 1,
                Text = "New Text - UpdateStepTest",
                CookTime = 2 * updatingStep.CookTime,
                PrepTime = updatingStep.PrepTime + new TimeSpan(0, 1, 0),
                RecipeId = Guid.NewGuid()
            };

            _ = await recipeRepo.UpdateStepInRecipe(updatingRecicpe.ID, newStep);
            var updatedRecipe = await recipeRepo.Get(updatingRecicpe.ID);
            Assert.NotNull(updatingRecicpe);
            var updatedStep = updatingRecicpe.Steps.SingleOrDefault(s => s.ID == newStep.ID);
            Assert.NotNull(updatedStep);
            Assert.Equal(updatedStep.Text, newStep.Text);
            Assert.Equal(updatedStep.Order, newStep.Order);
            Assert.Equal(updatedStep.CookTime, newStep.CookTime);
            Assert.Equal(updatedStep.PrepTime, newStep.PrepTime);
            Assert.Equal(updatedStep.RecipeId, updatingRecicpe.ID);
        }

        [Fact]
        public async Task UpdateStep_WhenRecipeMissing_ExpectException()
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(() => recipeRepo.UpdateStepInRecipe(Guid.NewGuid(), new Step()));
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