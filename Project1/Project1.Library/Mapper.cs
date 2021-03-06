﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Library
{
    public class Mapper
    {
        public static User Map(Data.Users user, string userLocation) => new User
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            LocationName = userLocation
        };

        public static Data.Users Map(User user, int locationId, int id) => new Data.Users
        {
            Id = id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            LocationId = locationId
        };

        public static Data.Users Map(User user, int locationId) => new Data.Users
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            LocationId = locationId
        };
        
        public static Location Map(Data.Locations location) => new Location
        {
            Name = location.LocationName,
            Dough = location.DoughQ.Value,
            Sauce = location.SouceQ.Value,
            Cheese = location.CheeseQ.Value,
            Pepperoni = location.PepperoniQ.Value
        };

        public static Data.Locations Map(Location location, int id) => new Data.Locations
        {
            Id = id,
            LocationName = location.Name,
            DoughQ = location.Dough,
            SouceQ = location.Dough,
            CheeseQ = location.Cheese,
            PepperoniQ = location.Pepperoni
        };

        public static Data.Orders Map(Order order , int userId, int locationId) => new Data.Orders
        {
            UserId = userId,
            LocationId = locationId,
            NumberOfPizzas = order.Pizza.Count,
            OrderTime = order.TimeOfOrder
        };

        public static Order Map(Data.Orders order, List<Pizza> pizzas) => new Order
        {
            LocationName = order.Location.LocationName,
            User = Map(order.User, order.User.Location.LocationName),
            Pizza = pizzas,
            TimeOfOrder = order.OrderTime
        };
        
        public static Data.Pizza Map(Pizza pizza) => new Data.Pizza
        {
            Size = pizza.Size,
            Souce = pizza.Sauce,
            Cheese = pizza.Cheese,
            ExtraCheese = pizza.ExtraCheese,
            Pepperoni = pizza.Pepperoni
        };

        public static Pizza Map(Data.Pizza pizza) => new Pizza
        {
            Size = pizza.Size,
            Sauce = pizza.Souce ?? false,
            Cheese = pizza.Cheese ?? false,
            ExtraCheese = pizza.ExtraCheese ?? false,
            Pepperoni = pizza.Pepperoni ?? false
        };
    }
}
