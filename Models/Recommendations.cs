using System.ComponentModel.DataAnnotations;

namespace SmartCafeOrderingSystem_Api_V2.Models
{
    public class Recommendation
    {
        [Key] public int RecommendationID { get; set; }
        public int ItemID1 { get; set; }
        public int ItemID2 { get; set; }
        public decimal Score { get; set; }

        // Relationships
        public MenuItem Item1 { get; set; } = null!;
        public MenuItem Item2 { get; set; } = null!;
    }

}
