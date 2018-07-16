using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.WebApp.Models
{
    public class LocationW
    {
        public int Id { get; set; }
        public string LocationName { get; set; }
        public int? DoughQ { get; set; }
        public int? SouceQ { get; set; }
        public int? CheeseQ { get; set; }
        public int? PepperoniQ { get; set; }

        public bool EnoughIngridients(List<Data.Pizza> pizzas)
        {
            int dough = 0;
            int sauce = 0;
            int cheese = 0;
            int pepperoni = 0;
            foreach (var pizza in pizzas)
            {
                dough += pizza.Size;
                if (pizza.Souce == true)
                {
                    sauce += pizza.Size;
                }
                if (pizza.Cheese == true)
                {
                    cheese += pizza.Size;
                }
                if (pizza.ExtraCheese == true)
                {
                    cheese += pizza.Size;
                }
                if (pizza.Pepperoni == true)
                {
                    pepperoni += pizza.Size;
                }
            }
            if (DoughQ - dough >= 0 && SouceQ - sauce >= 0 && CheeseQ - cheese >= 0 && PepperoniQ - pepperoni >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SubstractIngridients(List<Data.Pizza> pizzas)
        {
            foreach (var pizza in pizzas)
            {
                DoughQ -= pizza.Size;
                SouceQ -= pizza.Size;
                CheeseQ -= pizza.Size;
                if(pizza.ExtraCheese == true)
                {
                    CheeseQ -= pizza.Size;
                }
                PepperoniQ -= pizza.Size;
            }
        }

        public static Data.Locations Map(LocationW locationW)
        {
            Data.Locations location = new Data.Locations
            {
                Id = locationW.Id,
                LocationName = locationW.LocationName,
                DoughQ = locationW.DoughQ,
                SouceQ = locationW.SouceQ,
                CheeseQ = locationW.CheeseQ,
                PepperoniQ = locationW.PepperoniQ                
            };
            return location;
        }
    }
}
