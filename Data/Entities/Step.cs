using System;

namespace BadMelon.Data.Entities
{
    public class Step : Entity
    {
        public string Text { get; set; }
        public int Order { get; set; }
        public TimeSpan PrepTime { get; set; }
        public TimeSpan CookTime { get; set; }

        public Guid RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}