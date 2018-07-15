using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.WebApp.Models
{
    public class PizzaW
    {
        public int Id { set; get; }
        public int Size { set; get; }
        public bool Sauce { set; get; }
        public bool Cheese { set; get; }
        public bool ExtraCheese { set; get; }
        public bool Pepperoni { set; get; }
    }
}
