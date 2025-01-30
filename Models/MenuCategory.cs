using System.ComponentModel.DataAnnotations;

namespace SmartCafeOrderingSystem_Api_V2.Models
{
    public class MenuCategory
    {
        [Key]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Relationships
        public ICollection<MenuItem>? MenuItems { get; set; } = new List<MenuItem>();
    }
}
