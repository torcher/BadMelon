using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BadMelon.API.DTOs
{
    public class Recipe
    {
        public Guid ID { get; set; }

        [StringLength(250, ErrorMessage = "Recipe Name must be between 1 and 250 characters long.")]
        public string Name { get; set; }

        public List<Ingredient> Ingredients { get; set; }
        public List<Step> Steps { get; set; }
    }
}