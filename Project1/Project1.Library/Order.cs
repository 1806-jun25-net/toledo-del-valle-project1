using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Library
{
    public class Order
    {
        public Location Location { get; }
        public User User { get; }
        public List<Pizza> Pizza { get; }
        public DateTime TimeOfOrder { get; }

        public Order(Location location, User user, List<Pizza> pizza)
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