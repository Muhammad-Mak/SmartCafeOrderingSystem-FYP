namespace SmartCafeOrderingSystem_Api_V2.DTOs
{
    public class PaymentDTO
    {
        public int PaymentID { get; set; }
        public int OrderID { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty; // e.g., "Easypaisa", "Debit Card"
        public string PaymentStatus { get; set; } = "Pending"; // e.g., "Successful", "Failed"
        public string TransactionID { get; set; } = string.Empty; // Unique identifier from the payment gateway
        public string GatewayResponse { get; set; } = string.Empty; // Response message from the gateway
    }
}
