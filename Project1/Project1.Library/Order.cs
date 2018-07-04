using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Library
{
    public class Order
    {
        private Location Location { get; }
        private User User { get; }
        private Pizza Pizza { get;  }
        private DateTime TimeOfOrder { get; }

        public Order(Location location, User user, Pizza pizza)
        {
            Location = location;
            User = user;
            Pizza = pizza;
            TimeOfOrder = DateTime.Now;
        }        

        public string GetTimeOfOderString()
        {
            return TimeOfOrder.ToString("h:mm:ss tt");
        }
    }
}