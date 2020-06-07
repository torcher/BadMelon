using BadMelon.Data.DTOs;
using BadMelon.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BadMelon.Data.Services
{
    public static class DTOConverter
    {
        public static Recipe[] ConvertToDTOs(this Entities.Recipe[] recipes)
        {
            return recipes.Select(recipe => recipe.ConvertToDTO()).ToArray();
        }

        public static Recipe ConvertToDTO(this Entities.Recipe recipe)
        {
            List<Ingredient> ingredients = recipe.Ingredients.ConvertToDTOs();
            var steps = recipe.Steps.ConvertToDTOs();
            return new Recipe { ID = recipe.ID, Name = recipe.Name, Ingredients = ingredients, Steps = steps };
        }

        public static Entities.Recipe ConvertFromDTO(this Recipe recipe)
        {
            return new Entities.Recipe
            {
                ID = recipe.ID,
                Name = recipe.Name,
                Ingredients = recipe.Ingredients.Select(i => new Data.Entities.Ingredient { Weight = i.Weight, IngredientTypeID = i.TypeID }).ToArray(),
                Steps = recipe.Steps.Select(s => new Data.Entities.Step { Text = s.Text, Order = s.Order, PrepTime = s.PrepTime.ConvertFromString(), CookTime = s.CookTime.ConvertFromString() }).ToArray()
            };
        }

        public static IngredientType[] ConvertToDTO(this Data.Entities.IngredientType[] ingredientTypes) => ingredientTypes.Select(it => ConvertToDTO(it)).ToArray();

        public static IngredientType ConvertToDTO(this Data.Entities.IngredientType ingredientType)
        {
            return new IngredientType
            {
                ID = ingredientType.ID,
                Name = ingredientType.Name
            };
        }

        public static Entities.IngredientType ConvertFromDTO(this IngredientType ingredientType)
        {
            return new Entities.IngredientType { ID = ingredientType.ID, Name = ingredientType.Name };
        }

        public static Ingredient ConvertToDTO(this Entities.Ingredient ingredient)
        {
            return new Ingredient
            {
                ID = ingredient.ID,
                Weight = ingredient.Weight,
                TypeID = ingredient.IngredientType.ID,
                Type = ingredient.IngredientType.Name
            };
        }

        public static List<Ingredient> ConvertToDTOs(this ICollection<Entities.Ingredient> ingredients) => ingredients.Select(i => i.ConvertToDTO()).ToList();

        public static Entities.Ingredient ConvertFromDTO(this Ingredient ingredient)
        {
            return new Entities.Ingredient
            {
                ID = ingredient.ID,
                IngredientTypeID = ingredient.TypeID,
                Weight = ingredient.Weight
            };
        }

        public static ICollection<Entities.Ingredient> ConvertFromDTOs(this IEnumerable<Ingredient> ingredients) => ingredients.Select(i => i.ConvertFromDTO()).ToArray();

        public static Entities.Step ConvertFromDTO(this Step step)
        {
            return new Entities.Step
            {
                ID = step.ID,
                Order = step.Order,
                Text = step.Text,
                CookTime = step.CookTime.FromRecipeFormat(),
                PrepTime = step.PrepTime.FromRecipeFormat()
            };
        }

        public static ICollection<Entities.Step> ConvertFromDTOs(this Step[] steps) => steps.Select(s => s.ConvertFromDTO()).ToArray();

        public static Step ConvertToDTO(this Entities.Step step)
        {
            return new Step
            {
                ID = step.ID,
                Order = step.Order,
                Text = step.Text,
                CookTime = step.CookTime.ToRecipeFormat(),
                PrepTime = step.PrepTime.ToRecipeFormat()
            };
        }

        public static List<Step> ConvertToDTOs(this ICollection<Entities.Step> steps) => steps.Select(s => s.ConvertToDTO()).ToList();

        public static ModelState ConvertToDTO(this Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary state)
        {
            return new ModelState(state.Select(s => s.Value.ToString()).ToList());
        }

        public static TimeSpan ConvertFromString(this string ts) => TimeSpan.TryParse(ts, out TimeSpan result) ? result : new TimeSpan(0, 0, 0);
    }
}