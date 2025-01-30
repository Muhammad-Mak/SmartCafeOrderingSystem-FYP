using System.ComponentModel.DataAnnotations;

namespace SmartCafeOrderingSystem_Api_V2.Models
{
    public class Payment
    {
        [Key] public int PaymentID { get; set; }
        public int OrderID { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty; // "Easypaisa" or "Debit Card"
        public string PaymentStatus { get; set; } = "Pending"; // "Successful" or "Failed"


        public string? TransactionID { get; set; }
        public string? GatewayResponse { get; set; }
        // Relationships
        public Order Order { get; set; } = null!;
    }

}
