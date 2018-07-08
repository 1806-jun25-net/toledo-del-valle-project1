using System;
using System.Collections.Generic;

namespace Project1.Data
{
    public partial class Locations
    {
        public Locations()
        {
            Orders = new HashSet<Orders>();
            Users = new HashSet<Users>();
        }

        public int Id { get; set; }
        public string LocationName { get; set; }
        public int? DoughQ { get; set; }
        public int? SouceQ { get; set; }
        public int? CheeseQ { get; set; }
        public int? PepperoniQ { get; set; }

        public ICollection<Orders> Orders { get; set; }
        public ICollection<Users> Users { get; set; }
    }
}
