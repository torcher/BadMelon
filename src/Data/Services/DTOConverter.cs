﻿using BadMelon.Data.DTOs;
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
            List<Ingredient> ingredients = recipe.Ingredients
                                                .Select(i => new Ingredient
                                                {
                                                    ID = i.ID,
                                                    Weight = i.Weight,
                                                    Type = i.IngredientType.Name,
                                                    TypeID = i.IngredientTypeID
                                                })
                                               .ToList();
            List<Step> steps = recipe.Steps.Select(s => new Step()
            {
                ID = s.ID,
                Text = s.Text,
                Order = s.Order,
                CookTime = s.CookTime.ToString("g"),
                PrepTime = s.PrepTime.ToString("g")
            })
                                                .ToList();

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

        public static Data.Entities.IngredientType ConvertFromDTO(this IngredientType ingredientType)
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

        public static Entities.Ingredient ConvertFromDTO(this Ingredient ingredient)
        {
            return new Entities.Ingredient
            {
                ID = ingredient.ID,
                IngredientTypeID = ingredient.TypeID,
                Weight = ingredient.Weight
            };
        }

        public static ModelState ConvertToDTO(this Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary state)
        {
            return new ModelState(state.Select(s => s.Value.ToString()).ToList());
        }

        public static TimeSpan ConvertFromString(this string ts) => TimeSpan.TryParse(ts, out TimeSpan result) ? result : new TimeSpan(0, 0, 0);
    }
}