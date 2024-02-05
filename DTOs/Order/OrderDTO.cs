namespace FoodOrderSystemAPI.DTOs.Order
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public string OrderNo { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public int TableNo { get; set; }
        public int OrderStatusId { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public List<MenuOrderDTO> MenuOrders { get; set; } = new List<MenuOrderDTO>();
    }
}
