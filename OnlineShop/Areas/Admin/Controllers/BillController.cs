using OnlineShop.Filters;
using OnlineShop.Models;
using PagedList;
using System.Linq;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    [isAdmin]
    public class BillController : Controller
    {
        private DatabaseContext _Context = new DatabaseContext();
        public BillController()
        {
            _Context = new DatabaseContext();
        }
        // GET: Admin/Bill
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageIndex = page.HasValue ? page.Value : 1;
            var listBill = _Context.Orders.ToList();
            return View(listBill.ToPagedList(pageIndex, pageSize));
        }
        [HttpPost]
        public JsonResult Delete(int ID)
        {
            var order = _Context.Orders.Find(ID);
            if (order != null)
            {
                _Context.OrderDetails.RemoveRange(order.OrderDetails.ToList());
                _Context.Orders.Remove(order);
                _Context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        public JsonResult Details(int ID)
        {
            var order = _Context.Orders.Find(ID);
            if (order != null)
            {
                var get = order.OrderDetails.Select(x => new { name = x.InforProduct.Product.Name, size = x.InforProduct.Size.Name, quantity = x.Quantity, price = x.Price }).ToList();
                return Json(new { success = true, get = get, nameCustomer = order.InforCustomer.FullName, total = order.TotalPrice }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }
    }
}