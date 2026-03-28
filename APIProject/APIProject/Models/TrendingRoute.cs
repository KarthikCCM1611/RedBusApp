using System.ComponentModel.DataAnnotations;

namespace APIProject.Models
{
    public class TrendingRoute
    {
        public string FromLocationId { get; set; } = String.Empty;
        public string FromLocationName { get; set; } = String.Empty;
        public string ToLocationId { get; set; } = String.Empty;
        public string ToLocationName { get; set; } = String.Empty;
        public int Count { get; set; }
    }
}
