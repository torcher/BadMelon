using System;
using System.ComponentModel.DataAnnotations;

namespace BadMelon.Data.DTOs
{
    public class Step
    {
        public Guid ID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Step must have text")]
        [StringLength(1000, ErrorMessage = "Step Text cannot be longer than 1000 characters")]
        public string Text { get; set; }

        public int Order { get; set; }
        public string CookTime { get; set; }
        public string PrepTime { get; set; }
    }
}