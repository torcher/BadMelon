using BadMelon.Data;
using BadMelon.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace BadMelon.Tests.Data.Fixtures
{
    public class BadMelonDataContextFixture : IDisposable
    {
        public BadMelonDataContext BadMelonDataContext { get; }

        public BadMelonDataContextFixture()
        {
            BadMelonDataContext = new BadMelonDataContext(new DbContextOptionsBuilder<BadMelonDataContext>()
                                                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                                              .Options);
        }

        public void Dispose()
        {
            BadMelonDataContext.Dispose();
        }

        public void WithRecipes(IEnumerable<Recipe> recipes)
        {
            BadMelonDataContext.Recipes.AddRange(recipes);
            BadMelonDataContext.SaveChanges();
        }

        public void WithIngredientTypes(IEnumerable<IngredientType> ingredientTypes)
        {
            BadMelonDataContext.IngredientTypes.AddRange(ingredientTypes);
            BadMelonDataContext.SaveChanges();
        }
    }
}