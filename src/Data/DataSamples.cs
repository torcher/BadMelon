using BadMelon.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BadMelon.Data
{
    public class DataSamples
    {
        public (User user, DTOs.Login login)[] Users { get => _Users?.ToArray() ?? new (User user, DTOs.Login login)[] { }; }
        private List<(User user, DTOs.Login login)> _Users;

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

            _Users = new List<(User user, DTOs.Login login)>();
            _Users.Add((user0, user0Login));
            _Users.Add((user1, user1Login));
            _Users.Add((user2_passwordNotConfirmed, user2Login));
            _Users.Add((joshUser, joshUserLogin));

            _IngredientTypes = new List<IngredientType>();
            var water = new IngredientType { ID = Guid.Parse("36de307c-1003-4200-a243-604d31ccd9de"), Name = "Water" };
            var corn = new IngredientType { ID = Guid.Parse("b233e6fe-82ab-4e2c-b13d-0639365a5e9b"), Name = "Corn" };
            _IngredientTypes.AddRange(new IngredientType[] { water, corn });

            var cornSoup = new Recipe
            {
                ID = Guid.Parse("bcede8a8-22e8-41a7-995a-f599052a35eb"),
                User = user0,
                Name = "Cold Corn Soup",
                Ingredients = new List<Ingredient>(),
                Steps = new List<Step>()
            };

            cornSoup.Ingredients.Add(new Ingredient { ID = Guid.Parse("c54d4bf8-5e85-48de-959d-4cfe6ef5f206"), IngredientType = corn, IngredientTypeID = corn.ID, Weight = 1d });
            cornSoup.Ingredients.Add(new Ingredient { ID = Guid.Parse("41dd3cdb-8552-4bf8-bb12-4dfce5492fb4"), IngredientType = water, IngredientTypeID = water.ID, Weight = 2d });
            cornSoup.Steps.Add(new Step { ID = Guid.Parse("a6b2cde9-95c4-4840-91e3-1db7d005d9e0"), Order = 1, Recipe = cornSoup, RecipeId = cornSoup.ID, Text = "Mix Corn and Water" });
            cornSoup.Steps.Add(new Step { ID = Guid.Parse("8c3156aa-ff28-48d9-95a4-ec6de51693a1"), Order = 2, Recipe = cornSoup, RecipeId = cornSoup.ID, Text = "Serve" });

            var hotWater = new Recipe
            {
                ID = Guid.Parse("1e8a7973-d161-436a-90d4-b9e2b79fcf2c"),
                User = user0,
                Name = "Hot Water",
                Ingredients = new List<Ingredient>(),
                Steps = new List<Step>()
            };
            hotWater.Ingredients.Add(new Ingredient { ID = Guid.Parse("bac2506f-5acf-4703-b88d-cee25d1e64aa"), IngredientType = water, IngredientTypeID = water.ID, Weight = 1d });
            hotWater.Steps.Add(new Step
            {
                ID = Guid.Parse("bdf0c3f7-665d-4967-8fb8-027e64c3572c"),
                Order = 1,
                Text = "Heat water",
                CookTime = new TimeSpan(0, 1, 0),
                PrepTime = new TimeSpan(0, 1, 0),
                Recipe = hotWater,
                RecipeId = hotWater.ID
            });

            var coldWater = new Recipe
            {
                ID = Guid.Parse("89125059-59cd-4fbf-91a1-f9691a8e86a9"),
                User = user0,
                Name = "Cold Water",
                Ingredients = new List<Ingredient>(),
                Steps = new List<Step>()
            };
            coldWater.Ingredients.Add(new Ingredient { ID = Guid.Parse("1d809fa4-f146-4f8c-a3e4-d278ec8e0ef7"), IngredientType = water, IngredientTypeID = water.ID, Weight = 1d });
            coldWater.Steps.Add(new Step
            {
                ID = Guid.Parse("a79ffd57-86f3-44ab-9088-dcabcffe03a5"),
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