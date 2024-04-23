using OnlineShop.Filters;
using OnlineShop.Models;
using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    [isAdmin]
    public class SizeController : Controller
    {
        private DatabaseContext _Context;
        public SizeController()
        {
            _Context = new DatabaseContext();
        }
        // GET: Admin/Size
        public ActionResult Index()
        {
            /*
            int? page
            int pageSize = 10;
            int pageIndex = page.HasValue ? page.Value : 1;
            return View(_Context.Sizes.ToList().ToPagedList(pageIndex, pageSize));
            */
            return View();
        }
        [HttpGet]
        public JsonResult listSize(string keySearch, int page)
        {
            try
            {
                var list = _Context.Sizes
                    .Where(x => x.Name.Contains(keySearch))
                    .Select(x => new { ID = x.ID, Name = x.Name }).ToList();
                int pageSize = 10;
                var numPage = list.Count() % pageSize == 0 ? list.Count() / pageSize : list.Count() / pageSize + 1;
                var listSize = list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                return Json(new { success = true, listSize = listSize, numPage = numPage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult getSize(int ID)
        {
            var size = _Context.Sizes.SingleOrDefault(x => x.ID == ID);
            if (size == null)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, size = size.Name }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult updateSize(int ID, string name)
        {
            var size = _Context.Sizes.SingleOrDefault(x => x.ID == ID);
            if (size == null)
            {
                return Json(new { success = false });
            }
            size.Name = name;
            _Context.Sizes.AddOrUpdate(size);
            _Context.SaveChanges();
            return Json(new { success = true });
        }
        public JsonResult createSize(string name)
        {
            Size size = new Size
            {
                Name = name
            };
            var listPord = _Context.Products.ToList();
            foreach (var item in listPord)
            {

                InforProduct infor = new InforProduct()
                {
                    IDProduct = item.ID,
                    IDSize = size.ID,
                    Quantity = 0
                };
                _Context.InforProducts.Add(infor);
            }
            _Context.Sizes.Add(size);
            _Context.SaveChanges();
            return Json(new { success = true });
        }
        [HttpPost]
        public JsonResult Delete(int ID)
        {
            var size = _Context.Sizes.Find(ID);
            if (size != null)
            {
                var list = size.InforProducts.ToList();
                _Context.InforProducts.RemoveRange(list);
                _Context.Sizes.Remove(size);
                _Context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}