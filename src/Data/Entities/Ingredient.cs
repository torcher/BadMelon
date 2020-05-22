using System;

namespace BadMelon.Data.Entities
{
    public class Ingredient : Entity
    {
        public double Weight { get; set; }
        public Guid IngredientTypeID { get; set; }
        public virtual IngredientType IngredientType { get; set; }
    }
}