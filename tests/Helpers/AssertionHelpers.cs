using BadMelon.Data.DTOs;
using Xunit;

namespace BadMelon.Tests.Helpers
{
    public static class AssertionHelpers
    {
        public static void AssertSameRecipe(this Recipe recipe, Recipe otherRecipe)
        {
            Assert.Equal(recipe.ID, otherRecipe.ID);
            Assert.Equal(recipe.Name, otherRecipe.Name);
            Assert.Equal(recipe.Ingredients.Count, otherRecipe.Ingredients.Count);
            for (int i = 0; i < recipe.Ingredients.Count; i++)
            {
                Assert.Equal(recipe.Ingredients[i].Type, otherRecipe.Ingredients[i].Type);
                Assert.Equal(recipe.Ingredients[i].TypeID, otherRecipe.Ingredients[i].TypeID);
                Assert.Equal(recipe.Ingredients[i].Weight, otherRecipe.Ingredients[i].Weight);
            }

            Assert.Equal(recipe.Steps.Count, otherRecipe.Steps.Count);
            for (int i = 0; i < recipe.Steps.Count; i++)
            {
                Assert.Equal(recipe.Steps[i].Text, otherRecipe.Steps[i].Text);
                Assert.Equal(recipe.Steps[i].Order, otherRecipe.Steps[i].Order);
                Assert.Equal(recipe.Steps[i].PrepTime, otherRecipe.Steps[i].PrepTime);
                Assert.Equal(recipe.Steps[i].CookTime, otherRecipe.Steps[i].CookTime);
            }
        }
    }
}