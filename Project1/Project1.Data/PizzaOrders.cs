using System;
using System.Collections.Generic;

namespace Project1.Data
{
    public partial class PizzaOrders
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? PizzaId { get; set; }

        public Orders Order { get; set; }
        public Pizza Pizza { get; set; }
    }
}
