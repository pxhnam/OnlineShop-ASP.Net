using OnlineShop.Filters;
using OnlineShop.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    [isAdmin]
    public class HomeController : Controller
    {
        private DatabaseContext _Context;
        public HomeController()
        {
            _Context = new DatabaseContext();
        }
        // GET: Admin/Home
        public ActionResult Index()
        {
            var moneyMonth = _Context.Orders.Where(x => x.OrderDate.Month == DateTime.Now.Month && x.OrderDate.Year == DateTime.Now.Year).Sum(x => x.TotalPrice);
            ViewBag.MoneyMonth = String.Format("{0:0,0}", moneyMonth);
            var moneyYear = _Context.Orders.Where(x => x.OrderDate.Year == DateTime.Now.Year).Sum(x => x.TotalPrice);
            ViewBag.MoneyYear = String.Format("{0:0,0}", moneyYear);
            ViewBag.CountOrder = _Context.Orders.Where(x => x.OrderDate.Month == DateTime.Now.Month && x.OrderDate.Year == DateTime.Now.Year).Count();

            var order = _Context.Orders.Where(x => x.OrderDate.Month == DateTime.Now.Month && x.OrderDate.Year == DateTime.Now.Year).ToList();
            int total = 0;
            foreach (var item in order)
            {
                total += item.OrderDetails.Sum(x => x.Quantity);
            }
            ViewBag.CountProduct = total;
            return View();
        }
        public ActionResult Notification()
        {
            //var list = _Context.Orders.ToList();
            return PartialView("_NotificationPartial");
        }
    }
}