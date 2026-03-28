using System.ComponentModel.DataAnnotations;

namespace APIProject.Models
{
    public class BusModel: Bus
    {
        public string FromLocationName { get; set; } = String.Empty;
        public string ToLocationName { get; set; } = String.Empty;
        public string[] SeatNos { get; set; } = [];
    }
}