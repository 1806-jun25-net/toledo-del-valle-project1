using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project1.Library;
using Project1.WebApp.Models;

namespace Project1.WebApp.Controllers
{
    public class ManagerController : Controller
    {
        public Project1Repository Repo { get; }

        public ManagerController(Project1Repository repo)
        {
            Repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult ManagerOptions()
        {
            return View();
        }

        public ActionResult DisplayAllUsers()
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

        public ActionResult SearchLocation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchLocation(LocationW locationW)
        {
            try
            {
                var location = Repo.GetLocation(locationW.LocationName);
                if (location == null)
                {
                    ModelState.AddModelError("", "Location doesn't exist");
                    return View();
                }
                else
                {
                    return RedirectToAction(nameof(LocationHistory), new { id = location.Id });
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult LocationHistory(int id)
        {
            var location = Repo.GetLocationById(id);
            var orders = Repo.GetOrdersFromLocation(id);

            var locationOrders = orders.Select(x => new OrderW
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

            TempData["Id"] = "" + id;
            TempData["LocationName"] = location.LocationName;

            return View(locationOrders);
        }

        public ActionResult LocationOrderDetails(int id)
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

        public ActionResult SearchUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchUser(UserW userWeb)
        {
            try
            {
                var user = Repo.GetUser(userWeb.FirstName, userWeb.LastName);
                if (user == null)
                {
                    ModelState.AddModelError("", "User doesn't exist");
                    return View();
                }
                else
                {
                    return RedirectToAction(nameof(UserHistory), new { id = user.Id });
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult UserHistory(int id)
        {
            var user = Repo.GetUserById(id);
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

            TempData["Id"] = "" + id;
            TempData["FirstName"] = user.FirstName;
            TempData["LastName"] = user.LastName;
            
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

        public ActionResult DisplayOrderHistoryByEarliest()
        {
            var orders = Repo.GetAllOrdersEarliest();

            var ordersByEarliest = orders.Select(x => new OrderW
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

            return View(ordersByEarliest);
        }

        public ActionResult OrderDetailsEarliest(int id)
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

        public ActionResult DisplayOrderHistoryByLatest()
        {
            var orders = Repo.GetAllOrdersLatest();

            var ordersByLatest = orders.Select(x => new OrderW
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

            return View(ordersByLatest);
        }

        public ActionResult OrderDetailsLatest(int id)
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

    }
}