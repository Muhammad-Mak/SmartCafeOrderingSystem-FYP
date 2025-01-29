using System.ComponentModel.DataAnnotations;

namespace SmartCafeOrderingSystem_Api_V2.Models
{
    public class Analytics
    {
        [Key] public int AnalyticsID { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public string PopularItems { get; set; } = string.Empty; // JSON list of popular items
    }

}
