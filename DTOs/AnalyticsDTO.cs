namespace SmartCafeOrderingSystem_Api_V2.DTOs
{
    public class AnalyticsDTO
    {
        public int AnalyticsID { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public string PopularItems { get; set; } = string.Empty;
    }
}
