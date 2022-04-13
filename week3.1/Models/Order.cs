using System;
using System.Collections.Generic;

namespace week3._1.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public int? UId { get; set; }
        public DateTime? OrderDate { get; set; }

        public virtual User? UIdNavigation { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
