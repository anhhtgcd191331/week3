using System;
using System.Collections.Generic;

namespace week3._1.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProdId { get; set; }
        public string? ProdName { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
