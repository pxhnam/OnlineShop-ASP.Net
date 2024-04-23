using OnlineShop.Filters;
using OnlineShop.Models;
using PagedList;
using System.Linq;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    [isUser]
    public class FavouritesController : Controller
    {
        private DatabaseContext _Context;
        public FavouritesController()
        {
            _Context = new DatabaseContext();
        }
        public ActionResult Index(int? page)
        {
            var user = Session["User"] as User;
            int pageSize = 9;
            int pageIndex = page.HasValue ? page.Value : 1;
            var listFavourite = _Context.Favorites.Where(x => x.UserID == user.ID).ToList().ToPagedList(pageIndex, pageSize);
            if (listFavourite.Count == 0)
            {
                ViewBag.Message = "Bạn chưa thích sản phẩm nào...";
            }
            else { ViewBag.Message = ""; }
            return View(listFavourite);
        }
        [HttpPost]
        public JsonResult AddToFavourites(int ID)
        {
            var user = Session["User"] as User;
            var prod = _Context.Products.SingleOrDefault(x => x.ID == ID);
            if (_Context.Favorites.Any(x => x.IDProduct == ID && x.UserID == user.ID))
            {
                return Json(new { success = false, total = _Context.Favorites.Where(x => x.UserID == user.ID).Count() });
            }
            Favorite favourite = new Favorite
            {
                UserID = user.ID,
                IDProduct = prod.ID
            };
            _Context.Favorites.Add(favourite);
            _Context.SaveChanges();
            return Json(new { success = true, total = _Context.Favorites.Where(x => x.UserID == user.ID).Count() });
        }
        public ActionResult FavouritesSum()
        {
            var user = Session["User"] as User;
            ViewBag.Total = _Context.Favorites.Where(x => x.UserID == user.ID).Count();
            return PartialView("_FavouritesPartial");
        }
        public JsonResult Remove(int ID)
        {
            var user = Session["User"] as User;
            var favourite = _Context.Favorites.SingleOrDefault(x => x.ID == ID);
            _Context.Favorites.Remove(favourite);
            _Context.SaveChanges();
            return Json(new { success = true, total = _Context.Favorites.Where(x => x.UserID == user.ID).Count() });
        }
    }
}