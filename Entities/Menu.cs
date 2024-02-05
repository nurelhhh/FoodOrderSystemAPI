using System;
using System.Collections.Generic;

namespace FoodOrderSystemAPI.Entities
{
    public partial class Menu
    {
        public Menu()
        {
            MenuOrders = new HashSet<MenuOrder>();
        }

        public int MenuId { get; set; }
        public string Name { get; set; } = null!;
        public int MenuStatusId { get; set; }

        public virtual MenuStatus MenuStatus { get; set; } = null!;
        public virtual ICollection<MenuOrder> MenuOrders { get; set; }
    }
}
