using System.ComponentModel.DataAnnotations;

namespace BadMelon.Data.DTOs
{
    public class Registration
    {
        [Required(ErrorMessage = "Must provide a valid email address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Must provide a valid email address")]
        [MaxLength(100, ErrorMessage = "Email address must be 100 characters or fewer")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Must provide a username")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Usernames must be between 5 and 20 characters")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}