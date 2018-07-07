using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Library
{
    public class User
    {
        private string FirstName { get; set; } = "Rolando";
        private string LastName { get; set; } = "Toledo";
        private string LocationName { get; set; } = "PizzaStore1";

        public User()
        {
        }

        public User(string firstName, string lastName, string locationName)
        {
            FirstName = firstName;
            LastName = lastName;
            LocationName = locationName;
        }

        public void PlaceOrder()
        {
        }

        public static bool UserExists(string a, string b, User user) => (a == user.FirstName && b == user.LastName) ? true : false;

        public static bool Equals(User a, User b) => (a.FirstName == b.FirstName && a.LastName == b.LastName) ? true : false;
    }
}
