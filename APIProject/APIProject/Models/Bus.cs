using System.ComponentModel.DataAnnotations;

namespace APIProject.Models
{
    public class Bus
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Name { get; set; }
        [Required]
        public string FromLocationId { get; set; }
        [Required]
        public string ToLocationId { get; set; }
        [Required]
        public DateTime DepartTime { get; set; }
        [Required]
        public DateTime ArriveTime { get; set; }
        [Required]
        public int TotalCapacity { get; set; }
        [Required]
        public int Price { get; set; }
    }
}