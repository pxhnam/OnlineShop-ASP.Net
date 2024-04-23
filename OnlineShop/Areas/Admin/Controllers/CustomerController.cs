using OnlineShop.Filters;
using OnlineShop.Models;
using PagedList;
using System.Linq;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    [isAdmin]
    public class CustomerController : Controller
    {
        private DatabaseContext _Context;
        public CustomerController()
        {
            _Context = new DatabaseContext();
        }
        // GET: Admin/Customer
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageIndex = page.HasValue ? page.Value : 1;
            var listUsers = _Context.Users.Where(x => x.Role == false).ToList();
            return View(listUsers.ToPagedList(pageIndex, pageSize));
        }
        [HttpPost]
        public JsonResult Delete(int ID)
        {
            var user = _Context.Users.Find(ID);
            if (user != null)
            {
                _Context.Favorites.RemoveRange(user.Favorites.ToList());
                _Context.InforCustomers.RemoveRange(user.InforCustomers.ToList());
                _Context.Users.Remove(user);
                _Context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        public JsonResult Active(int ID)
        {
            var user = _Context.Users.Find(ID);
            if (user == null)
            {
                return Json(new { success = false });
            }
            user.Active = !user.Active;
            _Context.SaveChanges();
            return Json(new { success = true });
        }
    }
}