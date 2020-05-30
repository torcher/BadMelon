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
            if (recipe == null) throw new EntityNotFoundException("Cannot find recipe of id " + ID);
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

            var recipe = await _db.Recipes.SingleOrDefaultAsync(r => r.ID == recipeId);
            if (recipe == null) throw new EntityNotFoundException("Could not find recipe of id " + recipeId);

            var ingredientType = await _db.IngredientTypes.SingleOrDefaultAsync(it => it.ID == ingredient.IngredientTypeID);
            if (ingredientType == null) throw new EntityNotFoundException("Could not find ingredient type of id " + ingredient.IngredientTypeID);

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
            if (ingredient == null) throw new EntityNotFoundException("Cannot find ingredient of id " + ingredientId);

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
            if (ingredientFound == null) return null;

            try
            {
                ingredientFound.IngredientType = null;
                ingredientFound.IngredientTypeID = ingredient.IngredientTypeID;
                ingredientFound.Weight = ingredient.Weight;
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new RepoException("Error updating Ingredient on Recipe", e);
            }
            return recipe;
        }
    }
}