using System.ComponentModel.DataAnnotations;

namespace BadMelon.Data.Entities
{
    public class IngredientType : Entity
    {
        [StringLength(250, MinimumLength = 1, ErrorMessage = "Name must be 1 to 250 characters long")]
        public string Name { get; set; }
    }
}