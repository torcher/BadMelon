using BadMelon.Data.DTOs;
using BadMelon.Data.Exceptions;
using BadMelon.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BadMelon.Data.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly BadMelonDataContext _db;
        private readonly Entities.User _LoggedInUser;

        public RecipeService(BadMelonDataContext db, IUserService userService)
        {
            _db = db;
            _LoggedInUser = userService.GetLoggedInUser().Result;
        }

        public async Task<Recipe[]> GetRecipes()
        {
            return _LoggedInUser.Recipes.ToArray().ConvertToDTOs();
        }

        public async Task<Recipe> GetRecipeByID(Guid ID) => (await GetRecipe(ID)).ConvertToDTO();

        public async Task<Recipe> AddRecipe(Recipe recipe)
        {
            var recipeEntity = recipe.ConvertToEntity();
            recipeEntity.User = _LoggedInUser;
            var newRecipe = _db.Recipes.CreateProxy();
            EntityCopier.Copy(recipeEntity, newRecipe);
            await _db.Recipes.AddAsync(newRecipe);
            await _db.SaveChangesAsync();
            return (await GetRecipe(newRecipe.ID)).ConvertToDTO();
        }

        public async Task<Recipe> AddIngredientToRecipe(Guid recipeId, Ingredient ingredient)
        {
            var recipe = await GetRecipe(recipeId);
            var currentIngredient = recipe.Ingredients.SingleOrDefault(i => i.IngredientTypeID == ingredient.TypeID);
            if (currentIngredient == null)
            {
                var ingredientType = await GetIngredientType(ingredient.TypeID);
                var newIngredient = _db.Ingredients.CreateProxy();
                EntityCopier.Copy(ingredient.ConvertToEntity(), newIngredient);
                recipe.Ingredients.Add(newIngredient);
            }
            else
            {
                currentIngredient.Weight += ingredient.Weight;
            }

            await _db.SaveChangesAsync();

            return recipe.ConvertToDTO();
        }

        public async Task<Recipe> UpdateIngredientInRecipe(Guid recipeId, Ingredient ingredient)
        {
            var recipe = await GetRecipe(recipeId);
            var ingredientToUpdate = GetIngredient(ingredient.ID, recipe);
            ingredientToUpdate.Weight = ingredient.Weight;
            await _db.SaveChangesAsync();

            return recipe.ConvertToDTO();
        }

        public async Task<Recipe> DeleteIngredientInRecipe(Guid recipeId, Ingredient ingredient) => await DeleteIngredientInRecipe(recipeId, ingredient.ID);

        public async Task<Recipe> DeleteIngredientInRecipe(Guid recipeId, Guid ingredientId)
        {
            var recipe = await GetRecipe(recipeId);
            var ingredient = GetIngredient(ingredientId, recipe);
            recipe.Ingredients.Remove(ingredient);
            await _db.SaveChangesAsync();

            return recipe.ConvertToDTO();
        }

        public async Task<Recipe> AddStepToRecipe(Guid recipeId, Step step)
        {
            var recipe = await GetRecipe(recipeId);
            var newStep = _db.Steps.CreateProxy();
            EntityCopier.Copy(step.ConvertToEntity(recipe), newStep);
            recipe.Steps.Add(newStep);
            await _db.SaveChangesAsync();
            return recipe.ConvertToDTO();
        }

        public async Task<Recipe> UpdateStepInRecipe(Guid recipeId, Step step)
        {
            var recipe = await GetRecipe(recipeId);
            var stepToUpdate = await GetStep(step.ID, recipe);

            EntityCopier.Copy(step.ConvertToEntity(recipe), stepToUpdate);
            await _db.SaveChangesAsync();

            return recipe.ConvertToDTO();
        }

        public async Task<Recipe> DeleteStepInRecipe(Guid recipeId, Step step) => await DeleteStepInRecipe(recipeId, step.ID);

        public async Task<Recipe> DeleteStepInRecipe(Guid recipeId, Guid stepId)
        {
            var recipe = await GetRecipe(recipeId);
            var stepToDelete = await GetStep(stepId, recipe);
            recipe.Steps.Remove(stepToDelete);
            await _db.SaveChangesAsync();
            return recipe.ConvertToDTO();
        }

        private async Task<Entities.Recipe> GetRecipe(Guid recipeId)
        {
            var recipe = await _db.Recipes
                .Where(r => r.UserId == _LoggedInUser.Id)
                .Include(r => r.Ingredients)
                    .ThenInclude(i => i.IngredientType)
                .Include(r => r.Steps)
                .SingleOrDefaultAsync(r => r.ID == recipeId);
            if (recipe == null)
                throw new EntityNotFoundException("recipe");
            return recipe;
        }

        private async Task<Entities.IngredientType> GetIngredientType(Guid ingredientTypeId)
        {
            var ingredientType = await _db.IngredientTypes.SingleOrDefaultAsync(it => it.ID == ingredientTypeId);
            if (ingredientType == null)
                throw new EntityNotFoundException("ingredient type");
            return ingredientType;
        }

        private Entities.Ingredient GetIngredient(Guid ingredientId, Entities.Recipe recipe)
        {
            var ingredient = recipe.Ingredients.SingleOrDefault(i => i.ID == ingredientId);
            if (ingredient == null)
                throw new EntityNotFoundException("ingredient");
            return ingredient;
        }

        private async Task<Entities.Step> GetStep(Guid stepId, Entities.Recipe recipe)
        {
            var step = await _db.Steps.SingleOrDefaultAsync(s => s.ID == stepId && s.RecipeId == recipe.ID);
            if (step == null)
                throw new EntityNotFoundException("step");
            return step;
        }
    }
}