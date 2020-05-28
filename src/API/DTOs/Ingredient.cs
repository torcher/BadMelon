using System;
using System.ComponentModel.DataAnnotations;

namespace BadMelon.API.DTOs
{
    public class Ingredient
    {
        public Guid ID { get; set; }

        [Range(0d, double.MaxValue, ErrorMessage = "Weight must be positive")]
        public double Weight { get; set; }

        [Required(ErrorMessage = "Must include Ingredient's Type ID")]
        public Guid TypeID { get; set; }

        public string Type { get; set; }
    }
}