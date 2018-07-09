using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Library
{
    public class Pizza
    {
        public int Size { get; }
        public bool Sauce { get; }
        public bool Cheese { get; }
        public bool ExtraCheese { get; }
        public bool Pepperoni { get; }

        public Pizza(int size, bool sauce, bool cheese, bool extraCheese, bool pepperoni)
        {
            Size = size;
            Sauce = sauce;
            Cheese = Cheese;
            ExtraCheese = extraCheese;
            Pepperoni = pepperoni;
        }
    }
}