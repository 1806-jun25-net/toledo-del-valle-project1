using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Library
{
    public class Location
    {    
        private string Name { get; }
        private List<Order> OrderHistory;
        private List<Topping> Toppings;

        // Create a new location
        public Location(string name)
        {
            Name = name;
            OrderHistory = new List<Order>();
            Toppings = new List<Topping>();
            StockIngridients();
        }

        // Create existing location
        public Location(string name, List<Order> orders, List<Topping> toppings)
        {
            Name = name;
            OrderHistory = orders;
            Toppings = toppings;
        }        

        //public void GetOrder(Order order)
        //{
        //}

        private void StockIngridients()
        {
            Topping dough = new Topping("dough", 150, 5.00M);
            Toppings.Add(dough);
            Topping sauce = new Topping("sauce", 150, .99M);
            Toppings.Add(sauce);
            Topping cheese = new Topping("chees", 150, .99M);
            Toppings.Add(cheese);
            //Topping extraCheese = new Topping("extraChees", 150, .99M);
            //Toppings.Add(cheese);
            Topping pepperoni = new Topping("pepperoni", 150, .99M);
            Toppings.Add(pepperoni);            
        }
        
    }
}