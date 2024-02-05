using System;
using System.Collections.Generic;

namespace FoodOrderSystemAPI.Entities
{
    public partial class Order
    {
        public Order()
        {
            MenuOrders = new HashSet<MenuOrder>();
        }

        public int OrderId { get; set; }
        public string OrderNo { get; set; } = null!;
        public int TableNo { get; set; }
        public DateTime Date { get; set; }
        public string Username { get; set; } = null!;
        public int OrderStatusId { get; set; }

        public virtual OrderStatus OrderStatus { get; set; } = null!;
        public virtual User UsernameNavigation { get; set; } = null!;
        public virtual ICollection<MenuOrder> MenuOrders { get; set; }
    }
}
