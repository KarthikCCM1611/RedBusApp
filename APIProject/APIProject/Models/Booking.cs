using System.ComponentModel.DataAnnotations;

namespace APIProject.Models
{
    public class Booking
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string UserId { get; set; }
        [Required]
        public string BusId { get; set; }
        [Required]
        public string FromLocationId { get; set; }
        [Required]
        public string ToLocationId { get; set; }
        [Required]
        public string[] SeatNos { get; set; }
        public int TotalPrice { get; set; }
    }
}