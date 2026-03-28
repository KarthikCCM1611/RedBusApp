using System.ComponentModel.DataAnnotations;

namespace APIProject.Models
{
    public class Location
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Name { get; set; }
    }
}
