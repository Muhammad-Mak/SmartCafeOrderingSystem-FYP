namespace SmartCafeOrderingSystem_Api_V2.DTOs
{
    public class RecommendationDTO
    {
        public int RecommendationID { get; set; }
        public int ItemID1 { get; set; }
        public string ItemName1 { get; set; } = string.Empty;
        public int ItemID2 { get; set; }
        public string ItemName2 { get; set; } = string.Empty;
        public decimal Score { get; set; }
    }
}
