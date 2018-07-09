using System;
using System.Collections.Generic;

namespace Project1.Data
{
    public partial class Orders
    {
        public Orders()
        {
            Pizza = new HashSet<Pizza>();
            PizzaOrders = new HashSet<PizzaOrders>();
        }

        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? LocationId { get; set; }
        public int NumberOfPizzas { get; set; }
        public DateTime OrderTime { get; set; }

        public Locations Location { get; set; }
        public Users User { get; set; }
        public ICollection<Pizza> Pizza { get; set; }
        public ICollection<PizzaOrders> PizzaOrders { get; set; }
    }
}
