using System;
using System.Collections.Generic;

namespace FoodOrderSystemAPI.Entities
{
    public partial class MenuStatus
    {
        public MenuStatus()
        {
            Menus = new HashSet<Menu>();
        }

        public int MenuStatusId { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Menu> Menus { get; set; }
    }
}
