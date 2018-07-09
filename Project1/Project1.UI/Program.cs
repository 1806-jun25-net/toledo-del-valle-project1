using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Project1.Data;
using Project1.Library;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Project1.UI
{
    public class Program
    {
        static void Main(string[] args)
        {
            // get the configuration from file
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            // now i can access the connection string like this
            // this will be null if there isn't an appsettings with this connection string
            Console.WriteLine(configuration.GetConnectionString("Project1DB"));

            // ORM: object-relational mapper
            // our ORM in .NET is: Entity Framework
            // we will use database-first approach to EF
            // provide the connection string to the dbcontext
            var optionsBuilder = new DbContextOptionsBuilder<Project1DBContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("Project1DB"));

            var repo = new Project1Repository(new Project1DBContext(optionsBuilder.Options));
            //////////////////////////////////////////////////////////////////////////////////////
            string input;
            List<Library.Pizza> pizzas = new List<Library.Pizza>();
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("\tMenu");
                Console.WriteLine("1.\tMake order");
                Console.WriteLine("2.\tManager options");
                Console.WriteLine("3.\tExit");
                Console.WriteLine();
                Console.Write("Enter menu option: ");
                input = Console.ReadLine();
                if (input == "1") // make order
                {
                    string locName = "location1";
                    Console.Write("Enter your first name: ");
                    string firstName = Console.ReadLine();
                    Console.Write("Enter your last name: ");
                    string lastName = Console.ReadLine();
                    // Use this to validate if user exists and if he wants to order from the same place
                    bool userEx = false;
                    // Check if user exists in the db
                    userEx = repo.UserExists(firstName, lastName);
                    if (userEx)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Welcome back {firstName} {lastName}");
                        locName = repo.GetUserLocation(firstName, lastName);
                        Console.WriteLine($"Your ordering location is {locName}");
                        userEx = GetYesNoInput("Would you like to order from that location? (Y/N): ");
                    }
                    // If it's a new user greet him 
                    else
                    {
                        Console.WriteLine($"Hi {firstName} {lastName}");
                    }
                    // New user or existing user decides from where they want to order
                    if (!userEx)
                    {
                        var locations = repo.GetLocations();
                        while (true)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Select the location from which you want to order");
                            Console.WriteLine("Valid locations displayed below");
                            // display the existing locations
                            DisplayLocations();
                            Console.Write("Enter menu option: ");
                            input = Console.ReadLine();
                            // check if the input location exists
                            userEx = repo.LocationExists(input);
                            if (userEx)
                            {
                                locName = input;
                                break;
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine($"Invalid input \"{input}\".");
                            }
                        } // end selecting location while
                        Console.WriteLine($"Your order will be sent to {locName}");
                        Console.ReadLine();
                    }
                    while (true)
                    {
                        int num;
                        while (true)
                        {
                            Console.WriteLine("How many pizzas do you want to order? (12 pizzas max)");
                            input = Console.ReadLine();
                            if(int.TryParse(input, out num) == true)
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine($"Invalid input \"{input}\".");
                            }
                        } // end of input validation for pizza quantity while
                        if (num < 1 || num > 12)
                        {
                            Console.WriteLine($"Invalid input \"{input}\", please pick a number between 1 and 12.");
                        }
                        else
                        {
                            int size;
                            if (num == 1)
                            {
                                size = PizzaSizeMenu();
                                pizzas.Add(CreatePizza(size));
                                break;
                            }
                            else {
                                for (int i = 0; i < num; i++)
                                {
                                    Console.WriteLine($"Pizza #{i+1}");
                                    size = PizzaSizeMenu();
                                    pizzas.Add(CreatePizza(size));
                                }
                                break;
                            }
                        }
                    } // end of pizza menu while
                    
                    User newUser = new User(firstName, lastName, locName);
                    //Order newOrder = new Order(newUser, pizzas);
                    
                    //Console.WriteLine(newUser.FirstName);
                    //Console.ReadLine();
                }
                else if (input == "2")
                {

                }else if (input == "3")
                {
                    break;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine($"Invalid input \"{input}\".");
                }
            }
        } // end main
        
        // Displays all existing locations
        public static void DisplayLocations()
        {
            using (var db = new Project1DBContext())
            {
                foreach (var item in db.Locations)
                {
                    Console.WriteLine(item.LocationName);
                }
            }
        }

        // Gets the size of the pizza from the user
        public static int PizzaSizeMenu()
        {
            string input;
            while (true)
            {
                Console.WriteLine("Choose the size of the pizza");
                Console.WriteLine("1.\tSmall");
                Console.WriteLine("2.\tMedium");
                Console.WriteLine("3.\tLarge");
                Console.Write("Enter option: ");
                input = Console.ReadLine();
                if (input == "1" || input == "2" || input == "3")
                {
                    return int.Parse(input);
                }
                else
                {
                    Console.WriteLine($"Invalid input \"{input}\".");
                }
            }
        }

        // Gets the toppings the user wants and creates the pizza
        public static Library.Pizza CreatePizza(int size)
        {
            bool sauce = false;
            bool cheese = false;
            bool exCheese = false;
            bool pepperoni = false;
            sauce = GetYesNoInput("Would you like sauce in your pizza? (Y/N) \n-> ");
            cheese = GetYesNoInput("Would you like cheese in your pizza? (Y/N) \n-> ");
            if (cheese == true)
            {
                exCheese = GetYesNoInput("Would you like extra cheese in your pizza ? (Y / N) \n-> ");
            }
            pepperoni = GetYesNoInput("Would you like pepperoni in your pizza? (Y/N) \n-> ");
            return new Library.Pizza(size, sauce, cheese, exCheese, pepperoni);
        }

        // Gets yes or no input 
        public static bool GetYesNoInput(string msg)
        {
            string input;
            while (true)
            {
                Console.Write(msg);
                input = Console.ReadLine();
                if (input == "Y" || input == "y")
                {
                    return true;
                }
                else if (input == "N" || input == "n")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine($"Invalid input \"{input}\".");
                }
            }
        }
    }
}