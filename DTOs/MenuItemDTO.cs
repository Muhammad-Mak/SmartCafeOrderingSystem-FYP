namespace SmartCafeOrderingSystem_Api_V2.DTOs
{
    public class MenuItemDTO
    {
        public int ItemID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageURL { get; set; } = string.Empty;
        public bool IsPopular { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
