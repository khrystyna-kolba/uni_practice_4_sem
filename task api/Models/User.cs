using Microsoft.AspNetCore.Identity;

namespace ContainersApiTask.Models
{
    public class User: IdentityUser
    {
        // these fields already exist because of : IdentityUser
        // public int Id { get; set; }
        // public string Email { get; set; }
        // public string Password { get; set; }
        public string? RefreshToken { get; set; }
        public string LastName { get; set; }
        public string GetFullName()
        {
            return UserName + " " + LastName;
        }
    }
}
