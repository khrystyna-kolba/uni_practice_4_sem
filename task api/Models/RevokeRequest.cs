using System.ComponentModel.DataAnnotations;

namespace ContainersApiTask.Models
{
    public class RevokeRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
