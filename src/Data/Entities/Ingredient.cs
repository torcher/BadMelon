using System;
using System.ComponentModel.DataAnnotations;

namespace BadMelon.Data.Entities
{
    public class Ingredient : Entity
    {
        public double Weight { get; set; }

        [Required]
        public Guid IngredientTypeID { get; set; }

        public virtual IngredientType IngredientType { get; set; }
    }
}