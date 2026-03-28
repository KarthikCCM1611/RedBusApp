using System.ComponentModel.DataAnnotations;

namespace APIProject.Models
{
    public class Login
    {
        [Required]
        public string Email { get; set; } = String.Empty;
        [Required]
        public string Password { get; set; } = String.Empty;
    }
}
