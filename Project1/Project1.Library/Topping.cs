using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Library
{
    public class Topping
    {
        private string Name { set; get; }
        private int Quantity { set; get; }
        private decimal Price { set; get; }

        public Topping(string name, int quantity, decimal price)
        {
            Name = name;
            Quantity = quantity;
            Price = price;
        }
    }
}
