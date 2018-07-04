using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Library
{
    public class Pizza
    {
        private string Size { get; }
        private List<string> Ingridients { get; }

        public Pizza(string size, List<string> ingridients)
        {
            Size = size;
            Ingridients = ingridients;
        }
    }
}