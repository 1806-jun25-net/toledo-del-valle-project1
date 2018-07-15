using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.WebApp.Models
{
    public class OrderW
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public UserW User { get; set; }
        public List<PizzaW> Pizzas { get; set; }
        public DateTime TimeOfOrder { get; set; }
        public Decimal Price { get; set; }

        public static List<WebApp.Models.PizzaW> Map(List<Data.Pizza> pizzas)
        {
            List<WebApp.Models.PizzaW> pizzaWs = new List<WebApp.Models.PizzaW>();
            foreach (var pizza in pizzas)
            {
                pizzaWs.Add(new WebApp.Models.PizzaW
                {
                    Id = pizza.Id,
                    Size = pizza.Size,
                    Sauce = (bool)pizza.Souce,
                    Cheese = (bool)pizza.Cheese,
                    ExtraCheese = (bool)pizza.ExtraCheese,
                    Pepperoni = (bool)pizza.Pepperoni
                });
            }
            return pizzaWs;
        }
    }    
}
