using System.ComponentModel.DataAnnotations;

namespace SmartCafeOrderingSystem_Api_V2.Models
{
    public class OrderItem
    {
        [Key] public int OrderItemID { get; set; }
        public int OrderID { get; set; }
        public int ItemID { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }

        // Relationships
        public Order? Order { get; set; } = null!;
        public MenuItem? MenuItem { get; set; } = null!;
    }

}
