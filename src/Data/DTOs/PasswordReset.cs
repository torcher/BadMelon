using System.ComponentModel.DataAnnotations;

namespace BadMelon.Data.DTOs
{
    public class PasswordReset
    {
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}