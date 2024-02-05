namespace FoodOrderSystemAPI.DTOs
{
    public class MenuDTO
    {
        public int MenuId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int MenuStatusId { get; set; }
        public string MenuStatus { get; set; } = string.Empty;
    }
}
