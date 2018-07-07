using Project1.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.UI
{
    public class Program
    {
        static void Main(string[] args)
        {
            User user = new User("Rolando", "Toledo", "Dulles Green");
            Menu();
            Console.ReadLine();
        }            

        static void Menu()
        {
            Console.WriteLine("Menu: \n1.New User");
            User a = new User();
            User b = new User();
            var c = a == b;
        }
    }
}