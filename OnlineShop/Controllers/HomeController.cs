using OnlineShop.Models;
using PagedList;
using System.Linq;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseContext _Context;
        public HomeController()
        {
            _Context = new DatabaseContext();
        }
        public ActionResult Index(int? page, string search, int? category)
        {
            int pageSize = 9;
            int pageIndex = page.HasValue ? page.Value : 1;
            var list = _Context.Products.Where(x => x.Status == true).ToList();
            if (!string.IsNullOrEmpty(search))
            {
                list = _Context.Products.Where(x => x.Name.Contains(search) && x.Status == true).ToList();
                if (category != null)
                {
                    if (category == 0)
                    {
                        list = _Context.Products.Where(x => x.Name.Contains(search) && x.Status == true).ToList();
                    }
                    else
                    {
                        list = _Context.Products.Where(x => x.Name.Contains(search) && x.CategoryID == category && x.Status == true).ToList();
                    }
                    ViewBag.categoryID = category;
                }

                ViewBag.search = search;
            }
            else
            {
                if (category != null)
                {
                    if (category != 0)
                        list = _Context.Products.Where(x => x.CategoryID == category && x.Status == true).ToList();
                    ViewBag.categoryID = category;
                }
            }

            return View(list.ToPagedList(pageIndex, pageSize));
        }
        [HttpGet]
        public ActionResult getUser()
        {
            var user = Session["User"] as User;
            var getUser = _Context.Users.SingleOrDefault(x => x.ID == user.ID);
            ViewBag.Name = getUser.FullName;
            return PartialView(getUser);
        }
        public ActionResult Search()
        {
            ViewBag.listCategory = _Context.Categories.ToList();
            return PartialView("_SearchPartial");
        }
    }
}