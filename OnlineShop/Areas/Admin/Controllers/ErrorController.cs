using OnlineShop.Filters;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    [isAdmin]
    public class ErrorController : Controller
    {
        // GET: Admin/Error
        public ActionResult Index()
        {
            return View();
        }
    }
}