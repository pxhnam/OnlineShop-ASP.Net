using OnlineShop.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class YourOrderController : Controller
    {
        private DatabaseContext _Context;
        public YourOrderController()
        {
            _Context = new DatabaseContext();
        }
        // GET: YourOrder
        public ActionResult Index()
        {
            var ID = Convert.ToInt32(Session["OrderNo"]);
            var getOrder = _Context.Orders.SingleOrDefault(x => x.ID == ID);
            if (getOrder == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(getOrder);
        }
    }
}