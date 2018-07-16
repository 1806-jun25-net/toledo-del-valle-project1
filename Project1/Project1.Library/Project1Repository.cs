using Microsoft.EntityFrameworkCore;
using Project1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project1.Library
{
    public class Project1Repository
    {
        private readonly Project1DBContext _db;

        public Project1Repository(Project1DBContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        // Gets the users and the locations they're pointing to
        public IEnumerable<Users> GetUsersWithLocationName()
        {
            List<Users> users = _db.Users.Include(m => m.Location).AsNoTracking().ToList();
            return users;
        }

        // Gets all the locations
        public IEnumerable<Locations> GetLocations()
        {
            List<Locations> locations = _db.Locations.AsNoTracking().ToList();
            return locations;
        }

        // Gets all the orders of an user
        public List<Orders> GetUserOrders(int userId)
        {
            var orders = _db.Orders.Where(x => x.UserId == userId).Include(m => m.Location).Include(n => n.User).ToList();
            return orders;
        }

        // Gets the id of all the pizzas in an order
        public List<PizzaOrders> GetJunctions(int orderId)
        {
            var junctions = _db.PizzaOrders.Where(x => x.OrderId == orderId).Include(m => m.Pizza).ToList();
            return junctions;
        }
        
        // Gets the id of a location based on its name
        public int GetLocationId(string locationName)
        {
            var location = _db.Locations.FirstOrDefault(x => x.LocationName == locationName);
            return location.Id;
        }

        // Gets the default location of an user
        public string GetUserLocation(string firstName, string lastName)
        {
            var user = _db.Users.Include(m => m.Location).FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName);
            if (user == null)
            {
                string name = firstName + " " + lastName;
                throw new ArgumentException("no such user with that name", nameof(name));
            }
            return user.Location.LocationName;
        }

        // Checks if an user exists 
        public bool UserExists(string firstName, string lastName)
        {
            var user = _db.Users.FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        // Gets the user searched by firstname lastname
        public Users GetUser(string firstName, string lastName)
        {
            var user = _db.Users.Include(x => x.Location).FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName);
            if (user == null)
            {
                string name = firstName + " " + lastName;
                throw new ArgumentException("no such user with that name", nameof(name));
            }
            return user;
        }

        // Gets the id of an user serched with the firstname lastname
        public int GetUserID(string firstName, string lastName)
        {
            var id = _db.Users.FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName).Id;
            if (id == 0)
            {
                string name = firstName + " " + lastName;
                throw new ArgumentException("no such user with that name", nameof(name));
            }
            return id;
        }

        // Gets the location with the same name as the parameter
        public Locations GetLocation(string locationName)
        {
            var location = _db.Locations.FirstOrDefault(x => x.LocationName == locationName);
            if (location == null)
            {
                throw new ArgumentException("no such location with that name", nameof(locationName));
            }
            return location;
        }

        // Verifies that a location exists
        public bool LocationExists(string locationName)
        {
            var location = _db.Locations.FirstOrDefault(x => x.LocationName == locationName);
            if (location == null)
            {
                return false;
            }
            return true;
        }

        // Gets the id of an order
        public int GetOrderId(Order order)
        {
            var orderId = _db.Orders.FirstOrDefault(x => DateTime.Compare(x.OrderTime,order.TimeOfOrder) == 0).Id;
            return orderId;
        }

        // Gets the id of an order
        public int GetOrderId(Orders order)
        {
            var orderId = _db.Orders.FirstOrDefault(x => DateTime.Compare(x.OrderTime, order.OrderTime) == 0).Id;
            return orderId;
        }

        // Updates a user
        public void UpdateUser(User user)
        {
            _db.Entry(_db.Users.FirstOrDefault(x => x.FirstName == user.FirstName && x.LastName == user.LastName)).CurrentValues.SetValues(Mapper.Map(user, GetLocationId(user.LocationName), GetUserID(user.FirstName, user.LastName)));
        }

        // Updates a location
        public void UpdateLocation(Location location)
        {
            _db.Entry(_db.Locations.FirstOrDefault(x => x.LocationName == location.Name)).CurrentValues.SetValues(Mapper.Map(location, GetLocationId(location.Name)));
        }

        // Adds user to the db
        public void AddUser(User user)
        {            
            _db.Add(Mapper.Map(user, GetLocationId(user.LocationName)));
        }

        // Adds order to the db
        public void AddOrder(Order order, int userId, int locationId)
        {
            _db.Add(Mapper.Map(order, userId, locationId));
        }

        // Adds order to the db
        public void AddOrder(Orders order)
        {
            _db.Add(order);
        }

        // Adds pizzas to the db and returns the ids of the inserted pizzas
        public List<int> AddPizzas(List<Pizza> pizzas)
        {
            List<int> PizzaIds = new List<int>();
            foreach (var item in pizzas)
            {
                var pizza = _db.Pizza.FirstOrDefault(x => x.Size == item.Size && x.Souce == item.Sauce && x.Cheese == item.Cheese && x.ExtraCheese == item.ExtraCheese && x.Pepperoni == item.Pepperoni);
                if(pizza == null)
                {
                    _db.Add(Mapper.Map(item));
                    Save();
                    PizzaIds.Add(GetPizzaId(item));
                }
                else
                {
                    PizzaIds.Add(GetPizzaId(item));
                }
            }
            return PizzaIds;
        }

        // Adds pizzas to the db and returns the ids of the inserted pizzas
        public List<int> AddPizzas(List<Data.Pizza> pizzas)
        {
            List<int> PizzaIds = new List<int>();
            foreach (var item in pizzas)
            {
                var pizza = _db.Pizza.FirstOrDefault(x => x.Size == item.Size && x.Souce == item.Souce && x.Cheese == item.Cheese && x.ExtraCheese == item.ExtraCheese && x.Pepperoni == item.Pepperoni);
                if (pizza == null)
                {
                    _db.Add(item);
                    Save();
                    PizzaIds.Add(GetPizzaId(item));
                }
                else
                {
                    PizzaIds.Add(GetPizzaId(item));
                }
            }
            return PizzaIds;
        }

        // Gets the id of a pizza
        public int GetPizzaId(Pizza pizza)
        {
            var pizzaId = _db.Pizza.FirstOrDefault(x => x.Size == pizza.Size && x.Souce == pizza.Sauce && x.Cheese == pizza.Cheese && x.ExtraCheese == pizza.ExtraCheese && x.Pepperoni == pizza.Pepperoni).Id;
            return pizzaId;
        }

        // Gets the id of a pizza
        public int GetPizzaId(Data.Pizza pizza)
        {
            var pizzaId = _db.Pizza.FirstOrDefault(x => x.Size == pizza.Size && x.Souce == pizza.Souce && x.Cheese == pizza.Cheese && x.ExtraCheese == pizza.ExtraCheese && x.Pepperoni == pizza.Pepperoni).Id;
            return pizzaId;
        }

        // Adds values to the junction table
        public void AddPizzaOrders(int orderId, int PizzaId)
        {
            PizzaOrders po = new PizzaOrders
            {
                OrderId = orderId,
                PizzaId = PizzaId
            };
            _db.PizzaOrders.Add(po);
        }

        // Gets the last order of the user
        public Orders GetLastOrder(int userId)
        {
            var order = _db.Orders.Include(x => x.Location).LastOrDefault(x => x.UserId == userId);
            return order;
        }

        // Gets the last order from a specific location
        public Orders GetLastOrderFromLocation(int userId, string location)
        {
            var order = _db.Orders.Include(x => x.Location).LastOrDefault(x => x.UserId == userId && x.Location.LocationName == location);
            return order;
        }

        public List<Orders> GetAllOrdersEarliest()
        {
            var orders = _db.Orders.Include(x => x.Location).OrderBy(x => x.OrderTime).ToList();
            return orders;
        }

        public List<Orders> GetAllOrdersLatest()
        {
            var orders = _db.Orders.Include(x => x.Location).OrderByDescending(x => x.OrderTime).ToList();
            return orders;
        }

        public bool OrderedFromLocation(int userId, string location)
        {
            var order = _db.Orders.Include(x => x.Location).LastOrDefault(x => x.UserId == userId && x.Location.LocationName == location);
            if(order == null)
            {
                return false;
            }
            return true;
        }

        public Users GetUserById(int Id)
        {
            var user = _db.Users.Include(x => x.Location).FirstOrDefault(x => x.Id == Id);
            return user;
        }

        public List<Data.Pizza> GetPizzasFromOder(int orderId)
        {
            List<Data.Pizza> pizzas = new List<Data.Pizza>();
            var junctions = GetJunctions(orderId);

            foreach(var item in junctions)
            {
                pizzas.Add(item.Pizza);
            }
            return pizzas;
        }

        public Data.Orders GetOrderById(int orderId)
        {
            var order = _db.Orders.Include(x => x.Location).Include(x => x.User).FirstOrDefault(x => x.Id == orderId);
            return order;
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
