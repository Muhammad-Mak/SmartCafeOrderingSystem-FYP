using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartCafeOrderingSystem_Api_V2.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; } // Primary key

        [Required]
        public int UserID { get; set; } // Foreign key for the customer placing the order

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow; // Date and time the order was placed

        [Required]
        [StringLength(20)]
        public string OrderType { get; set; } = "Pickup"; // Pickup or Table Delivery

        public int? TableNumber { get; set; } // Nullable for table number (only for table delivery)

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; } // Total amount for the order

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Placed"; // Placed, Preparing, Ready, Completed, Cancelled

        [Required]
        [StringLength(20)]
        public string PaymentStatus { get; set; } = "Pending"; // Paid or Pending

        // Relationships
        public User? User { get; set; } = null!; // Navigation property for the user who placed the order
        public ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>(); // List of items in the order
        public Payment? Payment { get; set; } = null!; // Associated payment record
    }

}
