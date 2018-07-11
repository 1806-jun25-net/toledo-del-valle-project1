﻿using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<Users> GetUsersWithLocationName()
        {
            List<Users> users = _db.Users.Include(m => m.Location).AsNoTracking().ToList();
            return users;
        }

        public IEnumerable<Locations> GetLocations()
        {
            List<Locations> locations = _db.Locations.AsNoTracking().ToList();
            return locations;
        }

        public List<Orders> GetUserOrders(int userId)
        {
            var orders = _db.Orders.Where(x => x.UserId == userId).Include(m => m.Location).Include(n => n.User).ToList();
            return orders;
        }

        public List<PizzaOrders> GetJunctions(int orderId)
        {
            var junctions = _db.PizzaOrders.Where(x => x.OrderId == orderId).Include(m => m.Pizza).ToList();
            return junctions;
        }
        
        public int GetLocationId(string locationName)
        {
            var location = _db.Locations.FirstOrDefault(x => x.LocationName == locationName);
            return location.Id;
        }

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

        public bool UserExists(string firstName, string lastName)
        {
            var user = _db.Users.FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public Users GetUser(string firstName, string lastName)
        {
            var user = _db.Users.FirstOrDefault(x => x.FirstName == firstName && x.LastName == lastName);
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
            var orderTime = _db.Orders.FirstOrDefault(x => DateTime.Compare(x.OrderTime,order.TimeOfOrder) == 0).Id;
            return orderTime;
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

        // Adds pizzas to the db
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

        // Gets the id of a pizza
        public int GetPizzaId(Pizza pizza)
        {
            var pizzaId = _db.Pizza.FirstOrDefault(x => x.Size == pizza.Size && x.Souce == pizza.Sauce && x.Cheese == pizza.Cheese && x.ExtraCheese == pizza.ExtraCheese && x.Pepperoni == pizza.Pepperoni).Id;
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

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}