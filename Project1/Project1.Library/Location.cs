using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Library
{
    public class Location
    {    
        public string Name { get; set; }
        public int Dough { get; set; }
        public int Sauce { get; set; }
        public int Cheese { get; set; }
        public int Pepperoni { get; set; }
        private List<Order> OrderHistory;

        //
        public Location() { }

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

        // 
        public Location(string name, int dough, int sauce, int cheese, int pepperoni)
        {
            Name = name;
            Dough = dough;
            Sauce = sauce;
            Cheese = cheese;
            Pepperoni = pepperoni;
        }

        private void StockIngridients()
        {
            Dough = 150;
            Sauce = 150;
            Cheese = 150;
            Pepperoni = 150;
        }
        
        public static decimal OrderPrice(Order order)
        {
            decimal price;
            price = 0;
            foreach(var pizza in order.Pizza)
            {
                price += pizza.Size * 5.00M;
                if (pizza.Sauce) { price += pizza.Size * .99M; }
                if (pizza.Cheese)
                {
                    price += pizza.Size * .99M;
                    if (pizza.ExtraCheese) { price += pizza.Size * .99M; }
                }                
                if (pizza.Pepperoni) { price += pizza.Size * .99M; }
            }
            return price;
        }

        public static decimal OrderPrice(List<Data.Pizza> pizzas)
        {
            decimal price;
            price = 0;
            foreach (var pizza in pizzas)
            {
                price += pizza.Size * 5.00M;
                if (pizza.Souce == true) { price += pizza.Size * .99M; }
                if (pizza.Cheese == true)
                {
                    price += pizza.Size * .99M;
                    if (pizza.ExtraCheese == true) { price += pizza.Size * .99M; }
                }
                if (pizza.Pepperoni == true) { price += pizza.Size * .99M; }
            }
            return price;
        }

        public static decimal PizzaPrice(Pizza pizza)
        {
            decimal price;
            price = 0;
            price += pizza.Size * 5.00M;
            if (pizza.Sauce) { price += pizza.Size * .99M; }
            if (pizza.Cheese)
            {
                price += pizza.Size * .99M;
                if (pizza.ExtraCheese) { price += pizza.Size * .99M; }
            }
            if (pizza.Pepperoni) { price += pizza.Size * .99M; }
            return price;
        }

        public bool EnoughIngridients(List<Pizza> pizzas)
        {
            int dough = 0;
            int sauce = 0;
            int cheese = 0;
            int pepperoni = 0;
            foreach(var pizza in pizzas)
            {
                dough += pizza.Size;
                if (pizza.Sauce)
                {
                    sauce += pizza.Size;
                }
                if (pizza.Cheese)
                {
                    cheese += pizza.Size;
                }
                if (pizza.ExtraCheese)
                {
                    cheese += pizza.Size;
                }
                if (pizza.Pepperoni)
                {
                    pepperoni += pizza.Size;
                }
            }
            if (Dough - dough >= 0 && Sauce - sauce >= 0 && Cheese - cheese >= 0 && Pepperoni - pepperoni >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SubstractIngridients(List<Pizza> pizzas)
        {
            foreach (var pizza in pizzas)
            {
                Dough -= pizza.Size;
                Sauce -= pizza.Size;
                Cheese -= pizza.Size;
                Pepperoni -= pizza.Size;
            }
        }
    }
}