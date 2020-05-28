using System;
using System.ComponentModel.DataAnnotations;

namespace BadMelon.Data.DTOs
{
    public class IngredientType
    {
        public Guid ID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Must include an Ingredient Type Name")]
        [StringLength(250, ErrorMessage = "Ingredient Type Name cannot be longer than 250 characters")]
        public string Name { get; set; }
    }
}