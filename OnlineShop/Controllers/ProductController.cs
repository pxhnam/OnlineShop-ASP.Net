using OnlineShop.Models;
using System.Linq;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class ProductController : Controller
    {
        private DatabaseContext _Context;
        public ProductController()
        {
            _Context = new DatabaseContext();
        }
        // GET: Product
        public ActionResult Details(int? ID)
        {
            var prod = _Context.Products.SingleOrDefault(x => x.ID == ID);
            if (prod == null || ID == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.NameProduct = prod.Name;
            return View(prod);
        }
        public ActionResult Suggest()
        {
            return PartialView("_SuggestPartial", _Context.Products.ToList());
        }
    }
}