using System.ComponentModel.DataAnnotations;

namespace ContainersApiTask.Models
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z][a-z-]+$", ErrorMessage = "first name should begin with capital letter and consist of letters or -")]
        //[StringLength(50, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z][a-z-]+$", ErrorMessage = "last name should begin with capital letter and consist of letters or -")]
        //[StringLength(50, MinimumLength = 1)]
        public string LastName { get; set; }

        [Required]
        //[StringLength(50, MinimumLength = 1)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z]).{8,}$", ErrorMessage = "password should contain at least one capital letter, one letter and be at least 8 characters long")]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
