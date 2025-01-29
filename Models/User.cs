using System.ComponentModel.DataAnnotations;

namespace SmartCafeOrderingSystem_Api_V2.Models
{
    public class User
    {
        [Key] public int UserID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int Role { get; set; } = 0; // Default is Customer
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

        // Relationships
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }


}
