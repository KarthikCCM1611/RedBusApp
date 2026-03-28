using System.ComponentModel.DataAnnotations;

namespace APIProject.Models
{
    public class BookingModel: Booking
    {
        public string FromLocationName { get; set; } = String.Empty;
        public string ToLocationName { get; set; } = String.Empty;
        public string BusName { get; set; } = String.Empty;
    }
}