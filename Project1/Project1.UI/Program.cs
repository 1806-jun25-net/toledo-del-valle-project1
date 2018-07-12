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
            //------------------------------------------------------------------------------
                        
            string input;
            int uid;
            List<Library.Pizza> pizzas = new List<Library.Pizza>();
            TimeSpan timeSpan;
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
                    Console.WriteLine();
                    Console.Write("Enter your first name: ");
                    string firstName = Console.ReadLine();
                    Console.WriteLine();
                    Console.Write("Enter your last name: ");
                    string lastName = Console.ReadLine();
                    // Use this to validate if user exists and if he wants to order from the same place
                    bool userEx = false;
                    bool changeLocation = false;
                    // Check if user exists in the db
                    userEx = repo.UserExists(firstName, lastName);
                    // If he exists greet him and ask if he wants to order from the same location
                    if (userEx)
                    {
                        uid = repo.GetUserID(firstName, lastName);
                        Console.WriteLine();
                        Console.WriteLine($"Welcome back {firstName} {lastName}");
                        Console.WriteLine();
                        locName = repo.GetUserLocation(firstName, lastName);
                        Console.WriteLine($"Your ordering location is {locName}");
                        Console.WriteLine();
                        while (true)
                        {
                            changeLocation = GetYesNoInput("Would you like to order from that location? (Y/N): ");
                            if (changeLocation == true)
                            {
                                timeSpan = DateTime.Now.Subtract(repo.GetLastOrderFromLocation(repo.GetUserID(firstName, lastName), locName).OrderTime);
                                if (timeSpan.Hours >= 2)
                                {
                                    break;
                                }
                                Console.WriteLine();
                                Console.WriteLine($"Your last order from this locations was {timeSpan.Hours} : {timeSpan.Minutes} : {timeSpan.Seconds}");
                                Console.WriteLine();
                                Console.WriteLine("Need to wait at least 2 hours to order from the same location");
                                changeLocation = false;
                                break;
                            }
                            break;
                        }
                    }
                    // If it's a new user greet him 
                    else
                    {
                        Console.WriteLine($"Hi {firstName} {lastName}");
                    }
                    // New user or existing user decides from where they want to order
                    if (!changeLocation)
                    {
                        var locations = repo.GetLocations();
                        while (true)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Select the location from which you want to order");
                            Console.WriteLine("Valid locations displayed below");
                            Console.WriteLine();
                            // display the existing locations
                            DisplayLocations();
                            Console.Write("Enter menu option: ");
                            input = Console.ReadLine();
                            Console.WriteLine();
                            // check if the input location exists
                            //userEx = repo.LocationExists(input);
                            if (repo.LocationExists(input))
                            {
                                locName = input;
                                if (repo.OrderedFromLocation(repo.GetUserID(firstName, lastName), locName))
                                {
                                    timeSpan = DateTime.Now.Subtract(repo.GetLastOrderFromLocation(repo.GetUserID(firstName, lastName), locName).OrderTime);
                                    if (timeSpan.Hours >= 2)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine();
                                        Console.WriteLine($"Your last order from this locations was {timeSpan.Hours} : {timeSpan.Minutes} : {timeSpan.Seconds}");
                                        Console.WriteLine();
                                        Console.WriteLine("Need to wait at least 2 hours to order from the same location");
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine($"Invalid input \"{input}\".");
                            }
                        } // end selecting location while
                        Console.WriteLine($"Your order will be sent to {locName}");
                        //Console.ReadLine();
                    }
                    // Ask the user how many pizzas he wants in the order
                    while (true)
                    {
                        int num;
                        bool I = false;
                        // Asks the user if he wants to make the same order as last time
                        if (userEx)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Your last order ");
                            Console.WriteLine();
                            Orders o = repo.GetLastOrder(repo.GetUserID(firstName, lastName));
                            List<Data.PizzaOrders> p = repo.GetJunctions(o.Id);
                            Console.WriteLine($"ID: {o.Id} | Location: {o.Location.LocationName} | Amount of pizzas: {o.NumberOfPizzas} | Order time: {o.OrderTime}");
                            Console.WriteLine();
                            foreach (var pizza in p)
                            {
                                Console.WriteLine($"Size: {pizza.Pizza.Size} | Sauce: {pizza.Pizza.Souce} | Cheese: {pizza.Pizza.Cheese} | Extra Cheese: {pizza.Pizza.ExtraCheese} | Pepperoni: {pizza.Pizza.Pepperoni}");
                            }
                            Console.WriteLine();
                            I = GetYesNoInput("Would you like to order the same? (Y/N): ");
                            if(I == true)
                            {
                                foreach(var item in p)
                                {
                                    pizzas.Add(Mapper.Map(item.Pizza));
                                }
                                break;
                            }
                        }
                        // If he wants to make a new order or if he is a new user
                        if (I == false)
                        {
                            while (true)
                            {
                                Console.WriteLine();
                                Console.Write("How many pizzas do you want to order? (12 pizzas max): ");
                                input = Console.ReadLine();
                                if (int.TryParse(input, out num) == true)
                                {
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine();
                                    Console.WriteLine($"Invalid input \"{input}\".");
                                }
                            }
                            if (num < 1 || num > 12)
                            {
                                Console.WriteLine();
                                Console.WriteLine($"Invalid input \"{input}\", please pick a number between 1 and 12.");
                            }
                            else
                            {
                                int size;
                                for (int i = 0; i < num; i++)
                                {
                                    Console.WriteLine();
                                    Console.WriteLine($"Pizza #{i + 1}");
                                    size = PizzaSizeMenu();
                                    pizzas.Add(CreatePizza(size));
                                }
                                break;
                            }
                        }
                    } // end of pizza menu while
                    Location loc = Mapper.Map(repo.GetLocation(locName));
                    User user = new User(firstName, lastName, locName);
                    Order order = new Order(loc.Name, user, pizzas);

                    if ( loc.EnoughIngridients(order.Pizza))
                    {
                        decimal orderPrice = 0;
                        orderPrice = Location.OrderPrice(order);
                        Console.WriteLine();
                        Console.WriteLine($"The total price of the order will be: ${orderPrice}");
                        Console.WriteLine();
                        if(GetYesNoInput("Would you like to send your order? (Y/N): "))
                        {
                            if (repo.UserExists(user.FirstName, user.LastName))
                            {
                                repo.UpdateUser(user);
                            }
                            else
                            {
                                repo.AddUser(user);
                                repo.Save();
                            }
                            loc.SubstractIngridients(order.Pizza);
                            repo.UpdateLocation(loc);
                            repo.AddOrder(order, repo.GetUserID(user.FirstName, user.LastName), repo.GetLocationId(loc.Name));
                            repo.Save();
                            int orderId = repo.GetOrderId(order);
                            List<int> pizzaIds = repo.AddPizzas(order.Pizza);
                            Console.WriteLine($"The amount of pizzas you ordered is: {pizzaIds.Count}");
                            foreach (var item in pizzaIds)
                            {
                                repo.AddPizzaOrders(orderId, item);
                            }
                            repo.Save();
                            Console.WriteLine("Your order has been made");
                            Console.WriteLine();
                            Console.WriteLine("Press enter to continue");
                            Console.ReadLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Sorry, not enough ingridients to complete your order");
                        Console.WriteLine("Press enter to continue");
                        Console.ReadLine();
                    }
                    pizzas.Clear();
                }
                else if (input == "2")
                {
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("\tManager Menu");
                        Console.WriteLine("1.\tDisplay all users");
                        Console.WriteLine("2.\tDisplay user history");
                        Console.WriteLine("3.\tDisplay order history by earliest");
                        Console.WriteLine("4.\tDisplay order history by latest");
                        Console.WriteLine("5.\tExit manager menu");
                        Console.Write("Enter menu option: ");
                        input = Console.ReadLine();
                        Console.WriteLine();
                        if (input == "1")
                        {
                            IEnumerable<Users> ul = repo.GetUsersWithLocationName();
                            Console.WriteLine("-------------------------------------------------------------");
                            Console.WriteLine("|{0,19}|{1,19}|{2,19}|", "First Name", "Last Name", "Default Location");
                            Console.WriteLine("-------------------------------------------------------------");
                            int count = 0;
                            foreach (var item in ul)
                            {
                                count++;
                                Console.WriteLine("|{0,19}|{1,19}|{2,19}|", item.FirstName, item.LastName, item.Location.LocationName);
                                Console.WriteLine("-------------------------------------------------------------");
                            }
                            Console.ReadLine();
                        }
                        else if(input == "2")
                        {
                            string firstName;
                            string lastName;
                            int userId;
                            bool userEx;
                            while (true)
                            {
                                Console.Write("Enter the first name: ");
                                firstName = Console.ReadLine();
                                Console.WriteLine();
                                Console.Write("Enter the last name: ");
                                lastName = Console.ReadLine();
                                Console.WriteLine();
                                userEx = repo.UserExists(firstName, lastName);
                                if (userEx)
                                {
                                    userId = repo.GetUserID(firstName, lastName);
                                    List<Orders> orders = repo.GetUserOrders(userId);
                                    List<List<PizzaOrders>> orderPizzas = new List<List<PizzaOrders>>();
                                    foreach(var item in orders)
                                    {
                                        orderPizzas.Add(repo.GetJunctions(item.Id));
                                    }
                                    int count = 0;
                                    int count2 = 0;
                                    int count3 = 0;
                                    List<Library.Pizza> pi = new List<Library.Pizza>();
                                    foreach (var item in orders)
                                    {                                        
                                        Console.WriteLine($"Order #{count + 1} ID: {item.Id} | Location: {item.Location.LocationName} | Amount of pizzas: {item.NumberOfPizzas} | Order time: {item.OrderTime}");
                                        Console.WriteLine();
                                        Console.WriteLine($"\tPizzas in the order");
                                        Console.WriteLine();
                                        count3 = 0;
                                        foreach (var pizza in orderPizzas[count2])
                                        {
                                            Console.WriteLine($"\tPizza #{count3 + 1} | Size: {pizza.Pizza.Size} | Sauce: {pizza.Pizza.Souce} | Cheese: {pizza.Pizza.Cheese} | Extra Cheese: {pizza.Pizza.ExtraCheese} | Pepperoni: {pizza.Pizza.Pepperoni}");
                                            pi.Add(Mapper.Map(pizza.Pizza));
                                            count3++;
                                        }
                                        Console.WriteLine();
                                        Console.WriteLine($"Order price: ${Location.OrderPrice(Mapper.Map(item, pi))}");
                                        Console.WriteLine();
                                        count++;
                                        count2++;
                                        pi.Clear();
                                    }
                                    Console.WriteLine("Press enter to continue");
                                    Console.ReadLine();
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine();
                                    string bad = firstName + lastName;
                                    Console.WriteLine($"User does not exist \"{bad}\".");
                                }
                            }
                        }
                        else if(input == "3")
                        {
                            List<Orders> orders = repo.GetAllOrdersEarliest();
                            foreach(var item in orders)
                            {                                
                                Console.WriteLine("ID: {0,3} | Location: {1,10} | Amount of pizzas: {2,3} | Order Time: {3}", item.Id, item.Location.LocationName, item.NumberOfPizzas, item.OrderTime);
                                Console.WriteLine();
                            }
                            Console.WriteLine("Press enter to continue");
                            Console.ReadLine();
                        }
                        else if(input == "4")
                        {
                            List<Orders> orders = repo.GetAllOrdersLatest();
                            foreach (var item in orders)
                            {
                                Console.WriteLine("ID: {0,3} | Location: {1,10} | Amount of pizzas: {2,3} | Order Time: {3}", item.Id, item.Location.LocationName, item.NumberOfPizzas, item.OrderTime);
                                Console.WriteLine();
                            }
                            Console.WriteLine("Press enter to continue");
                            Console.ReadLine();
                        }
                        else if (input == "5")
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine($"Invalid input \"{input}\".");
                        }
                    }
                }
                else if (input == "3")
                {
                    break;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine($"Invalid input \"{input}\".");
                }
                Console.Clear();
            }
        } // end main

        // Displays all existing locations
        public static void DisplayLocations()
        {
            using (var db = new Project1DBContext())
            {
                foreach (var item in db.Locations)
                {
                    Console.WriteLine($"\t{item.LocationName}");
                    Console.WriteLine();
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
                Console.WriteLine();
                Console.WriteLine("1.\tSmall");
                Console.WriteLine("2.\tMedium");
                Console.WriteLine("3.\tLarge");
                Console.WriteLine();
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