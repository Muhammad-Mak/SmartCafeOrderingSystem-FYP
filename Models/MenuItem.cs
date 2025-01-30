using System.ComponentModel.DataAnnotations;

namespace SmartCafeOrderingSystem_Api_V2.Models
{
    public class MenuItem
    {
        [Key] public int ItemID { get; set; }
        public int CategoryID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageURL { get; set; } = string.Empty;
        public bool IsPopular { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Relationships
        public MenuCategory? Category { get; set; } = null!;
        public ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<Recommendation>? Recommendations { get; set; } = new List<Recommendation>();
    }

}
