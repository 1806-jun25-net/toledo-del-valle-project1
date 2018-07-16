﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using Project1.Data;
using Project1.Library;
using Project1.WebApp.Models;

namespace Project1.WebApp.Controllers
{
    public class OrderController : Controller
    {
        public Project1Repository Repo { get; }

        public OrderController(Project1Repository repo)
        {
            Repo = repo;
        }

        // GET: User
        public ActionResult Index([FromQuery]string search = "")
        {
            var users = Repo.GetUsersWithLocationName();
            var webUser = users.Select(x => new UserW
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                DefaultLocation = x.Location.LocationName
            });
            return View(webUser);
        }

        // GET: Order/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Order/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult MakeOrder()
        {
            UserW webUser = new UserW();
            return View(webUser);
        }
                
        // POST: Order/MakeOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MakeOrder(UserW webUser)
        {
            try
            {
                // TODO: Add insert logic here
                var user = Repo.GetUser(webUser.FirstName, webUser.LastName);
                if(user == null)
                {
                    ModelState.AddModelError("", "User does not exist");
                    return View();
                }
                else
                {
                    OrderW orderWeb = new OrderW
                    {
                        LocationName = user.Location.LocationName,
                        User = new UserW
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            DefaultLocation = user.Location.LocationName
                        },
                        Pizzas = new List<PizzaW>()
                    };
                    string orderName = orderWeb.User.Id + "newOrder";
                    TempData.Put("orderName", orderName);
                    TempData.Put(orderName, orderWeb);
                    return RedirectToAction(nameof(UserOptions), new { newOrder = orderWeb.User.Id + "newOrder" });
                }
                //return RedirectToAction(nameof(Index)); 
            }
            catch
            {
                return View();
            }
        }

        public ActionResult UserOptions(string newOrder)
        {
            var orderWeb = TempData.Get<OrderW>(newOrder);
            TempData.Put(newOrder, orderWeb);
            return View(orderWeb);
        }

        public ActionResult UserHistory(int id)
        {
            var orders = Repo.GetUserOrders(id);

            var UserOrders = orders.Select(x => new OrderW
            {
                Id = x.Id, 
                LocationName = x.Location.LocationName,
                User = new UserW
                {
                    Id = x.User.Id,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName
                },
                Pizzas = OrderW.Map(Repo.GetPizzasFromOder(x.Id)),
                TimeOfOrder = x.OrderTime
            });
            return View(UserOrders);
        }

        public ActionResult OrderDetails(int id)
        {
            var order = Repo.GetOrderById(id);
            OrderW orderWeb = new OrderW
            {
                Id = order.Id,
                LocationName = order.Location.LocationName,
                User = new UserW
                {
                    Id = order.User.Id,
                    FirstName = order.User.FirstName,
                    LastName = order.User.LastName
                },
                Pizzas = OrderW.Map(Repo.GetPizzasFromOder(order.Id)),
                TimeOfOrder = order.OrderTime,
                Price = Location.OrderPrice(Repo.GetPizzasFromOder(order.Id))
            };
            return View(orderWeb);
        }

        public ActionResult NewOrder(string newOrder)
        {
            var orderWeb = TempData.Get<OrderW>(newOrder);
            TempData.Put(newOrder, orderWeb);
            return View(orderWeb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewOrder(OrderW webOrder, IFormCollection collection)
        {
            // get all of the data from TempData and collection
            var location = collection["Location"];
            var orderName = TempData.Get<String>("orderName");
            var orderWeb = TempData.Get<OrderW>(orderName);
            List<Data.Pizza> pizzas = OrderW.Map(orderWeb.Pizzas);
            // Assign values to the location and time of the order
            orderWeb.LocationName = location;
            orderWeb.TimeOfOrder = DateTime.Now;
            // Get the id of the location 
            int locationId = Repo.GetLocationId(location);
            // Make an Data.Orders object to insert into the db
            Data.Orders orderD = OrderW.Map(orderWeb, locationId);
            // Add order to the db
            Repo.AddOrder(orderD);
            // save changes in the db
            Repo.Save();
            // Get the id of the order
            int orderId = Repo.GetOrderId(orderD);
            // Get the id of all the pizzas in the order (create a new pizza entry in the db if it doesn't exist)
            List<int> pizzaIds = Repo.AddPizzas(pizzas); 
            // Add data to the junction table
            foreach(var item in pizzaIds)
            {
                Repo.AddPizzaOrders(orderId, item);
            }
            // Save changes to the db
            Repo.Save();
            // Get the price of the order
            decimal price = Location.OrderPrice(pizzas);
            // Place the id and price in the orderWeb obj
            orderWeb.Id = orderId;
            orderWeb.Price = price;            
            // Insert the data into TempData to get it in the OrderReview
            TempData.Put("orderName", orderName);
            TempData.Put(orderName, orderWeb);
            return RedirectToAction(nameof(OrderReview), new { id = orderWeb.User.Id });
        }
        
        public ActionResult NewPizza()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewPizza(PizzaW pizza)
        {
            var orderName = TempData.Get<String>("orderName");
            var orderWeb = TempData.Get<OrderW>(orderName);
            orderWeb.Pizzas.Add(pizza);
            TempData.Put("orderName", orderName);
            TempData.Put(orderName, orderWeb);
            return RedirectToAction(nameof(NewOrder), new { newOrder = orderName });
        }

        public ActionResult OrderReview(int id)
        {
            var orderName = TempData.Get<string>("orderName");
            var orderMade = TempData.Get<OrderW>(orderName);
            orderMade.User.Id = id;
            //TempData.Put("orderName", orderName);
            //TempData.Put(orderName, orderMade);
            return View(orderMade);
        }
    }// end of Order Controller

    // Used to store our objects in TempData
    public static class TempDataExtensions
    {
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonConvert.SerializeObject(value);
        }

        public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            object o;
            tempData.TryGetValue(key, out o);
            return o == null ? null : JsonConvert.DeserializeObject<T>((string)o);
        }
    }
}