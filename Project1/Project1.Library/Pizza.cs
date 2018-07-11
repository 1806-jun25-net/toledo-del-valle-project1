using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Library
{
    public class Pizza
    {
        public int Size { set; get; }
        public bool Sauce { set; get; }
        public bool Cheese { set; get; }
        public bool ExtraCheese { set; get; }
        public bool Pepperoni { set; get; }

        public Pizza() { }

        public Pizza(int size, bool sauce, bool cheese, bool extraCheese, bool pepperoni)
        {
            Size = size;
            Sauce = sauce;
            Cheese = cheese;
            ExtraCheese = extraCheese;
            Pepperoni = pepperoni;
        }
    }
}