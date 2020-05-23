using BadMelon.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BadMelon.Tests.Data
{
    public class DataSamples
    {
        public Recipe[] Recipes { get => _Recipes.ToArray(); }
        private List<Recipe> _Recipes;

        public IngredientType[] IngredientTypes { get => _IngredientTypes.ToArray(); }
        private List<IngredientType> _IngredientTypes;

        public DataSamples()
        {
            _IngredientTypes = new List<IngredientType>();
            var water = new IngredientType { ID = Guid.NewGuid(), Name = "Water" };
            var corn = new IngredientType { ID = Guid.NewGuid(), Name = "Corn" };
            _IngredientTypes.AddRange(new IngredientType[] { water, corn });

            var hotWater = new Recipe
            {
                ID = Guid.NewGuid(),
                Name = "Hot Water",
                Ingredients = new List<Ingredient>(),
                Steps = new List<Step>()
            };
            hotWater.Ingredients.Add(new Ingredient { ID = Guid.NewGuid(), IngredientType = water, IngredientTypeID = water.ID, Weight = 1d });
            hotWater.Steps.Add(new Step
            {
                ID = Guid.NewGuid(),
                Order = 1,
                Text = "Heat water",
                CookTime = new TimeSpan(0, 1, 0),
                PrepTime = new TimeSpan(0, 1, 0),
                Recipe = hotWater,
                RecipeId = hotWater.ID
            });

            var coldWater = new Recipe
            {
                ID = Guid.NewGuid(),
                Name = "Hot Water",
                Ingredients = new List<Ingredient>(),
                Steps = new List<Step>()
            };
            coldWater.Ingredients.Add(new Ingredient { ID = Guid.NewGuid(), IngredientType = water, IngredientTypeID = water.ID, Weight = 1d });
            coldWater.Steps.Add(new Step
            {
                ID = Guid.NewGuid(),
                Order = 1,
                Text = "Heat water",
                CookTime = new TimeSpan(0, 1, 0),
                PrepTime = new TimeSpan(0, 1, 0),
                Recipe = coldWater,
                RecipeId = coldWater.ID
            });

            _Recipes = new List<Recipe> { hotWater, coldWater };
        }

        public Recipe NewRecipe
        {
            get
            {
                var ingredientType = IngredientTypes.First();
                return new Recipe
                {
                    Name = "New Recipe",
                    Ingredients = new List<Ingredient>
                    {
                        new Ingredient{ IngredientTypeID = ingredientType.ID, Weight = 1d}
                    },
                    Steps = new List<Step>
                    {
                        new Step { Order = 1, Text = "New Step", CookTime = new TimeSpan(0, 1, 0), PrepTime = new TimeSpan(0, 1, 0) }
                    }
                };
            }
        }

        public IngredientType NewIngredientType
        {
            get
            {
                return new IngredientType
                {
                    Name = "Strawberry"
                };
            }
        }
    }
}