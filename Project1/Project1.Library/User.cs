using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Library
{
    public class User
    {
        private string Name { get; set; }
        private string LastName { get; set; }
        private string Location { get; set; }

        public User(string name, string lastName, string location)
        {
            Name = name;
            LastName = lastName;
            Location = location;
        }

        public void PlaceOrder()
        {
        }

    }
}
