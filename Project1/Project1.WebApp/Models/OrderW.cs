using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.WebApp.Models
{
    public class OrderW
    {
        public int Id { get; set; }
        [Display(Name = "Location Name")]
        public string LocationName { get; set; }
        public UserW User { get; set; }
        public List<PizzaW> Pizzas { get; set; }
        [Display(Name = "Order Time")]
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

        public static List<Data.Pizza> Map(List<WebApp.Models.PizzaW> pizzas)
        {
            List<Data.Pizza> pizzaDs = new List<Data.Pizza>();
            foreach (var pizza in pizzas)
            {
                pizzaDs.Add(new Data.Pizza
                {
                    Size = pizza.Size,
                    Souce = pizza.Sauce,
                    Cheese = pizza.Cheese,
                    ExtraCheese = pizza.ExtraCheese,
                    Pepperoni = pizza.Pepperoni
                });
            }
            return pizzaDs;
        }

        public static Data.Orders Map(OrderW orderWeb, int locationId)
        {
            Data.Orders orderD = new Data.Orders
            {
                LocationId = locationId,
                NumberOfPizzas = orderWeb.Pizzas.Count,
                OrderTime = orderWeb.TimeOfOrder,
                UserId = orderWeb.User.Id                
            };
            return orderD;
        }
                
    }    
}
