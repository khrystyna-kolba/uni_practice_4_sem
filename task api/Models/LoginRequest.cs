using System.ComponentModel.DataAnnotations;

namespace ContainersApiTask.Models
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
