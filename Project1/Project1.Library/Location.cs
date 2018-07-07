using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Library
{
    public class Location
    {
        private readonly Dictionary<string, int> Ingridients = new Dictionary<string, int>
        {
            // quantity of the ingridients is measured in servings
            // ex. 1 small pizza will take 1 serving of dough, sauce and cheese
            // but a large pizza will take 3 servings of each
            { "sauce" , 150 },
            { "cheese" , 150 },
            { "pepperoni" , 80 },
            { "ham" , 80 },
            { "bacon" , 80 },
            { "olives" , 30 },
            {"mushrooms" , 30 },
            { "dough", 100 }
        };

        private List<User> users;
        private string Address { get; }

        public void GetOrder(User user, List<Order> order)
        {
        }

        //    public bool UserExists(string firstName, string lastName)
        //    {
        //        if (
    }
}