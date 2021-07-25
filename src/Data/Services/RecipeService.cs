using BadMelon.Data.DTOs;
using BadMelon.Data.Exceptions;
using BadMelon.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BadMelon.Data.Services
{
    public class RecipeService : CrudServiceAbstract<Entities.Recipe>, IRecipeService
    {
        private readonly Entities.User _LoggedInUser;

        public RecipeService(BadMelonDataContext db, IUserService userService) : base(db)
        {
            _LoggedInUser = userService.GetLoggedInUser();
        }

        public async Task<Recipe[]> GetRecipes()
        {
            var recipes = await _db.Recipes
                .Where(r => r.UserId == _LoggedInUser.Id)
                .Include(r => r.Ingredients)
                    .ThenInclude(i => i.IngredientType)
                .Include(r => r.Steps)
                .OrderBy(r => r.Name)
                .ToArrayAsync();
            return recipes.ConvertToDTOs();
        }

        public async Task<Recipe> GetRecipeByID(Guid ID) => (await GetOne(ID)).ConvertToDTO();

        public async Task<Recipe> AddRecipe(Recipe recipe)
        {
            var recipeEntity = recipe.ConvertToEntity();
            recipeEntity.User = _LoggedInUser;
            await AddOne(recipeEntity);
            return await GetRecipeByID(recipeEntity.ID);
        }

        public async Task<Recipe> UpdateRecipe(Recipe recipe)
        {
            var currentRecipe = await GetOne(recipe.ID);
            if (currentRecipe.Name != recipe.Name)
                currentRecipe.Name = recipe.Name;
            await _db.SaveChangesAsync();
            await UpdateIngredientList(currentRecipe, recipe.Ingredients);
            await UpdateStepList(currentRecipe, recipe.Steps);
            return await GetRecipeByID(currentRecipe.ID);
        }

        private async Task UpdateIngredientList(Entities.Recipe currentRecipe, List<Ingredient> updatedIngedients)
        {
            var deleteIngredients = currentRecipe.Ingredients
                    .Where(x => !updatedIngedients.Any(y => y.TypeID == x.IngredientTypeID))
                    .ToArray();
            foreach (var i in deleteIngredients)
                currentRecipe.Ingredients.Remove(i);

            foreach (var i in updatedIngedients)
            {
                var existing = currentRecipe.Ingredients.SingleOrDefault(x => x.IngredientTypeID == i.TypeID);
                if (existing == null)
                {
                    i.ID = Guid.Empty;
                    currentRecipe.Ingredients.Add(i.ConvertToEntity());
                }
                else
                {
                    existing.Weight = i.Weight;
                }
            }
            await _db.SaveChangesAsync();
        }

        private async Task UpdateStepList(Entities.Recipe currentRecipe, List<Step> steps)
        {
            var removeStepCount = currentRecipe.Ingredients.Count - steps.Count;
            for (int i = 0; i < removeStepCount; i++)
                currentRecipe.Steps.Remove(currentRecipe.Steps.Last());
            for (int i = 0; i < steps.Count; i++)
            {
                if (i >= currentRecipe.Steps.Count)
                {
                    steps[i].Order = i + 1;
                    currentRecipe.Steps.Add(steps[i].ConvertToEntity(currentRecipe));
                }
                else
                {
                    var updatingStep = currentRecipe.Steps.ElementAt(i);
                    updatingStep.Order = steps[i].Order;
                    updatingStep.Text = steps[i].Text;
                    updatingStep.CookTime = steps[i].CookTime.ConvertToEntities();
                    updatingStep.PrepTime = steps[i].PrepTime.ConvertToEntities();
                }
            }

            await _db.SaveChangesAsync();
        }

        public async Task DeleteRecipe(Recipe recipe) => await DeleteRecipe(recipe.ID);

        public async Task DeleteRecipe(Guid recipeId)
        {
            var recipe = await GetOne(recipeId);
            await Delete(recipe);
        }

        public async Task<Recipe> AddIngredientToRecipe(Guid recipeId, Ingredient ingredient)
        {
            var recipe = await GetOne(recipeId);
            var currentIngredient = recipe.Ingredients.SingleOrDefault(i => i.IngredientTypeID == ingredient.TypeID);
            if (currentIngredient == null)
            {
                var ingredientType = await GetIngredientType(ingredient.TypeID);
                var ingredientEntity = ingredient.ConvertToEntity();
                ingredientEntity.IngredientType = ingredientType;
                recipe.Ingredients.Add(ingredientEntity);
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
            var recipe = await GetOne(recipeId);
            var ingredientToUpdate = GetIngredient(ingredient.ID, recipe);
            ingredientToUpdate.Weight = ingredient.Weight;
            await _db.SaveChangesAsync();

            return recipe.ConvertToDTO();
        }

        public async Task<Recipe> DeleteIngredientInRecipe(Guid recipeId, Ingredient ingredient) => await DeleteIngredientInRecipe(recipeId, ingredient.ID);

        public async Task<Recipe> DeleteIngredientInRecipe(Guid recipeId, Guid ingredientId)
        {
            var recipe = await GetOne(recipeId);
            var ingredient = GetIngredient(ingredientId, recipe);
            recipe.Ingredients.Remove(ingredient);
            await _db.SaveChangesAsync();

            return recipe.ConvertToDTO();
        }

        public async Task<Recipe> AddStepToRecipe(Guid recipeId, Step step)
        {
            var recipe = await GetOne(recipeId);
            var newStep = step.ConvertToEntity(recipe);
            recipe.Steps.Add(newStep);
            await _db.SaveChangesAsync();
            return recipe.ConvertToDTO();
        }

        public async Task<Recipe> UpdateStepInRecipe(Guid recipeId, Step step)
        {
            var recipe = await GetOne(recipeId);
            var stepToUpdate = await GetStep(step.ID, recipe);

            EntityCopier.Copy(step.ConvertToEntity(recipe), stepToUpdate);
            await _db.SaveChangesAsync();

            return recipe.ConvertToDTO();
        }

        public async Task<Recipe> DeleteStepInRecipe(Guid recipeId, Step step) => await DeleteStepInRecipe(recipeId, step.ID);

        public async Task<Recipe> DeleteStepInRecipe(Guid recipeId, Guid stepId)
        {
            var recipe = await GetOne(recipeId);
            var stepToDelete = await GetStep(stepId, recipe);
            recipe.Steps.Remove(stepToDelete);
            await _db.SaveChangesAsync();
            return recipe.ConvertToDTO();
        }

        public override async Task<Entities.Recipe> GetOne(Guid id)
        {
            var recipe = await _db.Recipes
                .Where(r => r.UserId == _LoggedInUser.Id)
                .Include(r => r.Ingredients)
                    .ThenInclude(i => i.IngredientType)
                .Include(r => r.Steps)
                .SingleOrDefaultAsync(r => r.ID == id);

            if (recipe == null)
                throw new EntityNotFoundException("recipe");

            recipe.Steps = recipe.Steps.OrderBy(s => s.Order).ToList();
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