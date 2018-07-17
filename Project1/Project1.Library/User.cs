using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Library
{
    public class User
    {
        public string FirstName { get; set; } = "Rolando";
        public string LastName { get; set; } = "Toledo";
        public string LocationName { get; set; } = "PizzaStore1";

        public User()
        {
        }

        public User(string firstName, string lastName, string locationName)
        {
            FirstName = firstName;
            LastName = lastName;
            LocationName = locationName;
        }
        

        public static bool Equals(User a, User b) => (a.FirstName == b.FirstName && a.LastName == b.LastName) ? true : false;
    }
}
