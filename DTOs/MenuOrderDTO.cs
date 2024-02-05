namespace FoodOrderSystemAPI.DTOs
{
    public class MenuOrderDTO
    {
        public int MenuId { get; set; }
        public string Menu { get; set; } = string.Empty;
        public int OrderId { get; set; }
        public int Qty { get; set; }
    }
}
