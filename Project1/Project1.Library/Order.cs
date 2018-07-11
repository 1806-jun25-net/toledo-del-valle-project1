using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Library
{
    public class Order
    {
        public string LocationName { set;  get; }
        public User User { set;  get; }
        public List<Pizza> Pizza { set;  get; }
        public DateTime TimeOfOrder { set;  get; }

        public Order() { }

        public Order(string locationName, User user, List<Pizza> pizza)
        {
            LocationName = locationName;
            User = user;
            Pizza = pizza;
            TimeOfOrder = DateTime.Now;
        }

        public Order(string locationName, User user, List<Pizza> pizza, DateTime time)
        {
            LocationName = locationName;
            User = user;
            Pizza = pizza;
            TimeOfOrder = time;
        }

        public string GetTimeOfOderString()
        {
            return TimeOfOrder.ToString("h:mm:ss tt");
        }
    }
}