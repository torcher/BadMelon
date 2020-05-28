using BadMelon.Data.Domain;
using BadMelon.Exceptions;
using System;
using Xunit;

namespace BadMelon.Tests.Data.Domain
{
    public class IngredientTests
    {
        [Fact]
        public void Ingredients_WhenOfSameType_ExpectAddSuccessfully()
        {
            var it = new IngredientType(Guid.NewGuid(), "Ingredient Type");
            var i1 = new Ingredient(1.5, it);
            var i2 = new Ingredient(1.5, it);

            var i3 = i1 + i2;
            Assert.True(i3 != null, "Ingredient should not be null");
            Assert.True(i3.Weight == 3d, "Ingredient weight should be 1+1");
        }

        [Fact]
        public void Ingredients_WhenOfSameType_ExpectSubtractSuccessfully()
        {
            var it = new IngredientType(Guid.NewGuid(), "Ingredient Type");
            var i1 = new Ingredient(3d, it);
            var i2 = new Ingredient(1.5, it);

            var i3 = i1 - i2;
            Assert.True(i3 != null, "Ingredient should not be null");
            Assert.True(i3.Weight == 1.5, "Ingredient weight should be 1.5");
        }

        [Fact]
        public void Ingredients_WhenOfDifferentType_ExpectError()
        {
            var it1 = new IngredientType(Guid.NewGuid(), "Ingredient Type 1");
            var it2 = new IngredientType(Guid.NewGuid(), "Ingredient Type 2");
            var i1 = new Ingredient(2d, it1);
            var i2 = new Ingredient(1d, it2);

            Assert.Throws<IngredientMismatchException>(() => i1 + i2);
            Assert.Throws<IngredientMismatchException>(() => i1 - i2);
        }

        [Fact]
        public void Ingredients_WhenSubtractorIsLarger_Expect0()
        {
            var it = new IngredientType(Guid.NewGuid(), "Ingredient Type");
            var i1 = new Ingredient(1d, it);
            var i2 = new Ingredient(3d, it);

            var i3 = i1 - i2;
            Assert.True(i3 != null, "Ingredient should not be null");
            Assert.True(i3.Weight == 0d, "Ingredient weight should be 1+1");
        }
    }
}