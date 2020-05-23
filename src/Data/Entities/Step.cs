using System;
using System.ComponentModel.DataAnnotations;

namespace BadMelon.Data.Entities
{
    public class Step : Entity
    {
        [Required]
        [MaxLength(1000)]
        public string Text { get; set; }

        public int Order { get; set; }
        public TimeSpan PrepTime { get; set; }
        public TimeSpan CookTime { get; set; }

        [Required]
        public Guid RecipeId { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}