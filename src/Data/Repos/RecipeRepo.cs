using BadMelon.Data.Entities;
using BadMelon.Data.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BadMelon.Data.Repos
{
    public class RecipeRepo : IRecipeRepo
    {
        private readonly BadMelonDataContext _db;

        public RecipeRepo(BadMelonDataContext db)
        {
            _db = db;
        }

        public async Task<Recipe> Get(Guid ID)
        {
            var recipe = await _db.Recipes
                .Include(i => i.Ingredients)
                .ThenInclude(t => t.IngredientType)
                .Include(s => s.Steps)
                .SingleOrDefaultAsync(recipe => recipe.ID == ID);
            if (recipe == null) throw new EntityNotFoundException("Cannot find recipe");
            return recipe;
        }

        public async Task<Recipe[]> Get()
        {
            return await _db.Recipes.ToArrayAsync();
        }

        public async Task<Recipe> AddRecipe(Recipe recipe)
        {
            if (recipe.Ingredients == null || recipe.Ingredients.Count == 0)
                throw new RepoException("Cannot add recipe without an Ingredient");
            if (recipe.Steps == null || recipe.Steps.Count == 0)
                throw new RepoException("Cannot add recipe without a Step");

            recipe.ID = Guid.NewGuid();
            foreach (var i in recipe.Ingredients) i.ID = Guid.NewGuid();
            foreach (var s in recipe.Steps) s.ID = Guid.NewGuid();

            try
            {
                await _db.Recipes.AddAsync(recipe);
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new RepoException("Error adding recipe", e.InnerException);
            }

            return await Get(recipe.ID);
        }

        public async Task<Recipe> UpdateRecipe(Recipe recipe)
        {
            var updatingRecipe = await _db.Recipes.SingleOrDefaultAsync(r => r.ID == recipe.ID);
            if (updatingRecipe is null) return null;

            updatingRecipe.Name = recipe.Name;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new RepoException("Error updating recipe", e);
            }
            return updatingRecipe;
        }

        public async Task DeleteRecipe(Guid recipeId)
        {
            var deleting = await Get(recipeId);

            try
            {
                _db.Remove(deleting);
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new RepoException("Error removing recipe", e);
            }
        }

        public async Task DeleteRecipe(Recipe recipe) => await DeleteRecipe(recipe.ID);

        public async Task<Recipe> AddIngredientToRecipe(Guid recipeId, Ingredient ingredient)
        {
            if (recipeId == Guid.Empty) throw new ArgumentNullException("recipeId");
            if (ingredient == null) throw new ArgumentNullException("ingredient");

            var recipe = await Get(recipeId);

            var ingredientType = await _db.IngredientTypes.SingleOrDefaultAsync(it => it.ID == ingredient.IngredientTypeID);
            if (ingredientType == null) throw new EntityNotFoundException("Could not find ingredient type");

            try
            {
                await _db.Ingredients.AddAsync(ingredient);
                recipe.Ingredients.Add(ingredient);
                await _db.SaveChangesAsync();
                return recipe;
            }
            catch (Exception e)
            {
                throw new RepoException("Error adding Ingredient to Recipe", e);
            }
        }

        public async Task<Recipe> RemoveIngredientFromRecipe(Guid recipeId, Guid ingredientId)
        {
            var recipe = await Get(recipeId);

            var ingredient = recipe.Ingredients.SingleOrDefault(i => i.ID == ingredientId);
            if (ingredient == null) throw new EntityNotFoundException("Cannot find ingredient");

            try
            {
                recipe.Ingredients.Remove(ingredient);
                await _db.SaveChangesAsync();
                return recipe;
            }
            catch (Exception e)
            {
                throw new RepoException("Error removing Ingredient from Recipe", e);
            }
        }

        public async Task<Recipe> UpdateIngredientInRecipe(Guid recipeId, Ingredient ingredient)
        {
            var recipe = await Get(recipeId);

            var ingredientFound = recipe.Ingredients.SingleOrDefault(i => i.ID == ingredient.ID);
            if (ingredientFound == null) throw new EntityNotFoundException("Could not find Ingredient to update");

            try
            {
                ingredientFound.IngredientTypeID = ingredient.IngredientTypeID;
                ingredientFound.Weight = ingredient.Weight;
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new RepoException("Error updating Ingredient on Recipe", e);
            }

            return await Get(recipeId);
        }

        public async Task<Recipe> AddStepToRecipe(Guid recipeId, Step step)
        {
            if (step == null) throw new ArgumentNullException("step");
            var recipe = await Get(recipeId);
            step.Recipe = recipe;
            step.RecipeId = recipe.ID;

            try
            {
                await _db.Steps.AddAsync(step);
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new RepoException("Error adding step to recipe", e);
            }

            return recipe;
        }

        public async Task<Recipe> RemoveStepFromRecipe(Guid recipeId, Guid stepId)
        {
            var recipe = await Get(recipeId);

            var step = recipe.Steps.SingleOrDefault(s => s.ID == stepId);
            if (step == null) throw new EntityNotFoundException("Cannot find step");

            try
            {
                recipe.Steps.Remove(step);
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new RepoException("Error removing Step from Recipe", e);
            }
            return recipe;
        }

        public async Task<Recipe> UpdateStepInRecipe(Guid recipeId, Step step)
        {
            var recipe = await Get(recipeId);

            var stepFound = recipe.Steps.SingleOrDefault(s => s.ID == step.ID);
            if (stepFound == null) throw new EntityNotFoundException("Cannot find step to update");

            try
            {
                stepFound.Text = step.Text;
                stepFound.Order = step.Order;
                stepFound.CookTime = step.CookTime;
                stepFound.PrepTime = step.PrepTime;
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new RepoException("Error updating Step in Recipe", e);
            }

            return recipe;
        }
    }
}