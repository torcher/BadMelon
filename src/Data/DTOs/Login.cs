using System.ComponentModel.DataAnnotations;

namespace BadMelon.Data.DTOs
{
    public class Login
    {
        [Required]
        [StringLength(20, ErrorMessage = "Username must be 20 characters or fewer")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
    }
}