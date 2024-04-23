using OnlineShop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class CheckOutController : Controller
    {
        private DatabaseContext _Context;
        public CheckOutController()
        {
            _Context = new DatabaseContext();
        }
        // GET: CheckOut
        public ActionResult Index()
        {
            var listShoppingCart = Session["ShoppingCart"] as List<Cart>;
            if (listShoppingCart.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            foreach (var item in listShoppingCart)
            {
                var i = _Context.InforProducts.SingleOrDefault(x => x.ID == item.ID);
                if (item.Quantity > i.Quantity)
                    return RedirectToAction("Index", "Home");
            }
            InformationPayment info = new InformationPayment
            {
                Payments = _Context.Payments.ToList()
            };
            return View(info);
        }
        [HttpPost]
        public ActionResult Index(InformationPayment info)
        {
            var listShoppingCart = Session["ShoppingCart"] as List<Cart>;
            var user = Session["User"] as User;
            if (listShoppingCart.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            InforCustomer customer = new InforCustomer
            {
                FullName = info.FullName,
                Address = info.Address,
                PhoneNumber = info.PhoneNumber
            };
            if (user != null)
                customer.IDUser = user.ID;
            _Context.InforCustomers.Add(customer);
            Order order = new Order
            {
                IDCustomer = customer.ID,
                IDPayment = info.Payment,
                OrderDate = System.DateTime.Now,
                TotalPrice = listShoppingCart.Sum(x => x.Quantity * x.Price),
                Note = info.Notes,
            };
            _Context.Orders.Add(order);
            foreach (var item in listShoppingCart)
            {
                OrderDetail details = new OrderDetail
                {
                    IDProduct = item.ID,
                    IDBill = order.ID,
                    Quantity = item.Quantity,
                    Price = item.Price,
                };
                _Context.OrderDetails.Add(details);
                var product = _Context.InforProducts.SingleOrDefault(x => x.ID == item.ID);
                product.Quantity -= item.Quantity;
            }
            _Context.SaveChanges();
            Session["ShoppingCart"] = null;
            Session["OrderNo"] = order.ID;
            return RedirectToAction("Index", "YourOrder");
        }
    }
}