using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BadMelon.API.DTOs
{
    public class Recipe
    {
        public Guid ID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Recipe must have a Name")]
        [StringLength(250, ErrorMessage = "Recipe Name must be between 1 and 250 characters long.")]
        public string Name { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Cannot create a Recipe without an Ingredient")]
        public List<Ingredient> Ingredients { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Cannot create a Recipe without a Step")]
        public List<Step> Steps { get; set; }
    }
}