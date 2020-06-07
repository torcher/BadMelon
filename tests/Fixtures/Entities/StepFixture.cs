using BadMelon.Data.Entities;
using System;

namespace BadMelon.Tests.Fixtures.Entities
{
    public class StepFixture
    {
        private readonly string Text;
        private int Order = 1;
        private TimeSpan PrepTime = new TimeSpan(0, 1, 0);
        private TimeSpan CookTime = new TimeSpan(0, 1, 0);
        private Guid Id = Guid.Empty;
        private Guid RecipeId;

        public StepFixture(string Text)
        {
            this.Text = Text;
        }

        public StepFixture WithOrder(int order)
        {
            Order = order;
            return this;
        }

        public StepFixture WithCookTime(TimeSpan cookTime)
        {
            CookTime = cookTime;
            return this;
        }

        public StepFixture WithPrepTime(TimeSpan prepTime)
        {
            PrepTime = prepTime;
            return this;
        }

        public Step Build()
        {
            return new Step
            {
                ID = Id,
                Text = Text,
                Order = Order,
                PrepTime = PrepTime,
                CookTime = CookTime,
                RecipeId = RecipeId
            };
        }
    }
}