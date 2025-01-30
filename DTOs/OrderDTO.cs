namespace SmartCafeOrderingSystem_Api_V2.DTOs
{
    public class OrderDTO
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderType { get; set; } = "Pickup";
        public int? TableNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Placed";
        public string PaymentStatus { get; set; } = "Pending";
    }
}
