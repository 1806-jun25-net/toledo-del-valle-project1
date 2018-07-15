using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                    //return RedirectToAction("UserOptions", "Order", new { id = user.Id });
                    return RedirectToAction(nameof(UserOptions), new { id = user.Id });
                }
                //return RedirectToAction(nameof(Index)); 
            }
            catch
            {
                return View();
            }
        }

        public ActionResult UserOptions(int id)
        {
            var user = Repo.GetUserById(id);
            UserW userW = new UserW
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DefaultLocation = user.Location.LocationName
            };
            return View(userW);
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
                TimeOfOrder = order.OrderTime
            };
            return View(orderWeb);
        }

        public ActionResult NewOrder(int id)
        {
            return View();
        }
    }
}