using BadMelon.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BadMelon.Data
{
    public class DataSamples
    {
        public (User, DTOs.Login)[] Users { get => _Users?.ToArray() ?? new (User, DTOs.Login)[] { }; }
        private List<(User, DTOs.Login)> _Users;

        public Recipe[] Recipes { get => _Recipes.ToArray(); }
        private List<Recipe> _Recipes;

        public IngredientType[] IngredientTypes { get => _IngredientTypes.ToArray(); }
        private List<IngredientType> _IngredientTypes;

        public DataSamples()
        {
            var user0 = new User { UserName = "user0_xyz", Email = "user0_xyz@badmelon.fake", EmailConfirmed = true, IsPasswordSet = true };
            var user0Login = new DTOs.Login { Username = "user0_xyz", Password = "long enough password to not be complex" };

            var user1 = new User { UserName = "user1_xyz", Email = "user1_xyz@badmelon.fake", EmailConfirmed = true, IsPasswordSet = true };
            var user1Login = new DTOs.Login { Username = "user1_xyz", Password = "long enough password to not be complex" };

            var user2_passwordNotConfirmed = new User { UserName = "user2_xyz", NormalizedUserName = "USER2_XYZ", Email = "user2_xyz@badmelon.fake", EmailConfirmed = true };
            var user2Login = new DTOs.Login { Username = "user2_xyz" };

            var joshUser = new User { UserName = "jge", Email = "joshua.geurts@gmail.com", EmailConfirmed = true, IsPasswordSet = true };
            var joshUserLogin = new DTOs.Login { Username = "jge", Password = "Qwerty123!Qwerty123!Qwerty123!" };

            _Users = new List<(User, DTOs.Login)>();
            _Users.Add((user0, user0Login));
            _Users.Add((user1, user1Login));
            _Users.Add((user2_passwordNotConfirmed, user2Login));
            _Users.Add((joshUser, joshUserLogin));

            _IngredientTypes = new List<IngredientType>();
            var water = new IngredientType { ID = Guid.NewGuid(), Name = "Water" };
            var corn = new IngredientType { ID = Guid.NewGuid(), Name = "Corn" };
            _IngredientTypes.AddRange(new IngredientType[] { water, corn });

            var cornSoup = new Recipe
            {
                ID = Guid.NewGuid(),
                User = user0,
                Name = "Cold Corn Soup",
                Ingredients = new List<Ingredient>(),
                Steps = new List<Step>()
            };

            cornSoup.Ingredients.Add(new Ingredient { ID = Guid.NewGuid(), IngredientType = corn, IngredientTypeID = corn.ID, Weight = 1d });
            cornSoup.Ingredients.Add(new Ingredient { ID = Guid.NewGuid(), IngredientType = water, IngredientTypeID = water.ID, Weight = 2d });
            cornSoup.Steps.Add(new Step { ID = Guid.NewGuid(), Order = 1, Recipe = cornSoup, RecipeId = cornSoup.ID, Text = "Mix Corn and Water" });
            cornSoup.Steps.Add(new Step { ID = Guid.NewGuid(), Order = 2, Recipe = cornSoup, RecipeId = cornSoup.ID, Text = "Serve" });

            var hotWater = new Recipe
            {
                ID = Guid.NewGuid(),
                User = user0,
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
                User = user0,
                Name = "Cold Water",
                Ingredients = new List<Ingredient>(),
                Steps = new List<Step>()
            };
            coldWater.Ingredients.Add(new Ingredient { ID = Guid.NewGuid(), IngredientType = water, IngredientTypeID = water.ID, Weight = 1d });
            coldWater.Steps.Add(new Step
            {
                ID = Guid.NewGuid(),
                Order = 1,
                Text = "Cool water",
                CookTime = new TimeSpan(0, 1, 0),
                PrepTime = new TimeSpan(0, 1, 0),
                Recipe = coldWater,
                RecipeId = coldWater.ID
            });

            _Recipes = new List<Recipe> { cornSoup, coldWater, hotWater };
        }

        public void AddRecipeToStorage(Recipe recipe)
        {
            _Recipes.Add(recipe);
        }

        public void AddIngredientTypeToStorage(IngredientType ingredientType)
        {
            _IngredientTypes.Add(ingredientType);
        }

        public void AddIngredientToFirstRecipeInStorage(Ingredient ingredient)
        {
            _Recipes.First().Ingredients.Add(ingredient);
        }
    }
}