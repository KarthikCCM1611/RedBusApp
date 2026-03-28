using System.ComponentModel.DataAnnotations;

namespace APIProject.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PhoneNo { get; set; }
        [Required]
        public string City { get; set; }
        public Role Role { get; set; } = Role.User;
    }

    public enum Role
    {
        Admin,
        User
    }
}