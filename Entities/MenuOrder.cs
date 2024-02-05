using System;
using System.Collections.Generic;

namespace FoodOrderSystemAPI.Entities
{
    public partial class MenuOrder
    {
        public int MenuOrderId { get; set; }
        public int Qty { get; set; }
        public int MenuId { get; set; }
        public int OrderId { get; set; }

        public virtual Menu Menu { get; set; } = null!;
        public virtual Order Order { get; set; } = null!;
    }
}
