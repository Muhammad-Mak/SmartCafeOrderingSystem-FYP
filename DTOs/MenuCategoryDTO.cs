namespace SmartCafeOrderingSystem_Api_V2.DTOs
{
    public class MenuCategoryDTO
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
