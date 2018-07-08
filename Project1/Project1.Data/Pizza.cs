using System;
using System.Collections.Generic;

namespace Project1.Data
{
    public partial class Pizza
    {
        public Pizza()
        {
            PizzaOrders = new HashSet<PizzaOrders>();
        }

        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int Size { get; set; }
        public bool? Souce { get; set; }
        public bool? Cheese { get; set; }
        public bool? ExtraCheese { get; set; }
        public bool? Pepperoni { get; set; }

        public Orders Order { get; set; }
        public ICollection<PizzaOrders> PizzaOrders { get; set; }
    }
}
