using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Library
{
    public class Location
    {    
        public string Name { get; }
        public int Dough { get; set; }
        public int Sauce { get; set; }
        public int Cheese { get; set; }
        public int Pepperoni { get; set; }
        private List<Order> OrderHistory;
        private List<Topping> Toppings;

        // Create a new location
        public Location(string name)
        {
            Name = name;
            OrderHistory = new List<Order>();
            Dough = 150;
            Sauce = 150;
            Cheese = 150;
            Pepperoni = 150;
            StockIngridients();
        }

        private void StockIngridients()
        {
            Topping dough = new Topping("dough", 150, 5.00M);
            Toppings.Add(dough);
            Topping sauce = new Topping("sauce", 150, .99M);
            Toppings.Add(sauce);
            Topping cheese = new Topping("chees", 150, .99M);
            Toppings.Add(cheese);
            Topping pepperoni = new Topping("pepperoni", 150, .99M);
            Toppings.Add(pepperoni);            
        }
        
        public decimal OrderPrice(Order order)
        {
            decimal price = 0;
            foreach(var pizza in order.Pizza)
            {
                price += pizza.Size * 5.00M;
                if (pizza.Sauce == true) { price += pizza.Size * .99M; }
                if (pizza.Cheese == true)
                {
                    price += pizza.Size * .99M;
                    if (pizza.ExtraCheese == true) { price += pizza.Size * .99M; }
                }                
                if (pizza.Pepperoni == true) { price += pizza.Size * .99M; }
            }
            return price;
        }
    }
}