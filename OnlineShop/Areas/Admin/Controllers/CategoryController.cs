using OnlineShop.Filters;
using OnlineShop.Models;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    [isAdmin]
    public class CategoryController : Controller
    {
        private DatabaseContext _Context;
        public CategoryController()
        {
            _Context = new DatabaseContext();
        }
        // GET: Admin/Category
        public ActionResult Index()
        {
            /*
            int? page
            int pageSize = 10;
            int pageIndex = page.HasValue ? page.Value : 1;
            return View(_Context.Categories.ToList().ToPagedList(pageIndex, pageSize));
            */
            return View();
        }
        [HttpGet]
        public JsonResult listCategory(string keySearch, int page)
        {
            var getList = _Context.Categories
                 .Where(x => x.Name.Contains(keySearch))
                 .Select(x => new { ID = x.ID, Name = x.Name }).ToList();
            int pageSize = 10;
            var numPage = getList.Count() % pageSize == 0 ? getList.Count() / pageSize : getList.Count() / pageSize + 1;
            var listCategory = getList.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Json(new { success = true, listCategory = listCategory, numPage = numPage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(int ID)
        {
            var category = _Context.Categories.Find(ID);
            if (category != null)
            {
                _Context.Categories.Remove(category);
                _Context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        public JsonResult getCategory(int ID)
        {
            var category = _Context.Categories.SingleOrDefault(x => x.ID == ID);
            if (category == null)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, category = category.Name }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updateCategory(int ID, string name)
        {
            var category = _Context.Categories.SingleOrDefault(x => x.ID == ID);
            if (category == null)
            {
                return Json(new { success = false });
            }
            category.Name = name;
            _Context.Categories.AddOrUpdate(category);
            _Context.SaveChanges();
            return Json(new { success = true });
        }
        public JsonResult createCategory(string name)
        {
            Category category = new Category
            {
                Name = name
            };
            _Context.Categories.Add(category);
            _Context.SaveChanges();
            return Json(new { success = true });
        }
    }
}