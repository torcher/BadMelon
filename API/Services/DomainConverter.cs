using BadMelon.RecipeMath;
using System.Collections.Generic;
using System.Linq;

namespace BadMelon.API.Services
{
    public static class DomainConverter
    {
        public static Recipe[] ConvertToDomain(this Data.Entities.Recipe[] recipes)
        {
            return recipes.Select(recipe => recipe.ConvertToDomain()).ToArray();
        }

        public static Recipe ConvertToDomain(this Data.Entities.Recipe recipe)
        {
            List<Ingredient> ingredients = recipe.Ingredients
                                                .Select(i => new Ingredient(i.Weight, new IngredientType(i.IngredientType.Name)))
                                               .ToList();
            List<Step> steps = recipe.Steps.Select(s => new Step() { Text = s.Text, Order = s.Order, CookTimeSpan = s.CookTime, PrepTimeSpan = s.PrepTime }).ToList();

            return new Recipe(recipe.ID, recipe.Name, ingredients, steps);
        }
    }
}