using System.ComponentModel.DataAnnotations;

namespace BadMelon.Data.Entities
{
    public class IngredientType : Entity
    {
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
    }
}