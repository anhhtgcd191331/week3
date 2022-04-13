using System;
using System.Collections.Generic;

namespace week3._1.Models
{
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string? Firstname { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
