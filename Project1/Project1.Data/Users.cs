using System;
using System.Collections.Generic;

namespace Project1.Data
{
    public partial class Users
    {
        public Users()
        {
            Orders = new HashSet<Orders>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int LocationId { get; set; }

        public Locations Location { get; set; }
        public ICollection<Orders> Orders { get; set; }
    }
}
