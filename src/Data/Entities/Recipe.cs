using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BadMelon.Data.Entities
{
    public class Recipe : Entity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public virtual ICollection<Step> Steps { get; set; }

        public virtual ICollection<Ingredient> Ingredients { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}