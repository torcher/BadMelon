using BadMelon.Data.Entities;
using System;
using System.Collections.Generic;

namespace BadMelon.Tests.Data.Fixtures
{
    public class RecipeFixture
    {
        private string _name;
        private Guid _id = Guid.Empty;
        private List<Ingredient> _ingredients = new List<Ingredient>();
        private List<Step> _steps = new List<Step>();

        public RecipeFixture(string name)
        {
            _name = name;
        }

        public RecipeFixture WithID(Guid id)
        {
            _id = id;
            return this;
        }

        public RecipeFixture WithIngredient(Ingredient ingredient)
        {
            _ingredients.Add(ingredient);
            return this;
        }

        public RecipeFixture WithStep(Step step)
        {
            _steps.Add(step);
            return this;
        }

        public Recipe Build()
        {
            return new Recipe
            {
                ID = _id,
                Name = _name,
                Ingredients = _ingredients,
                Steps = _steps
            };
        }
    }
}