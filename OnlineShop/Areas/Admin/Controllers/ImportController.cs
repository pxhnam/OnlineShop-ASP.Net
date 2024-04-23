using OnlineShop.Filters;
using OnlineShop.Models;
using PagedList;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    [isAdmin]
    public class ImportController : Controller
    {
        private DatabaseContext _Context;
        public ImportController()
        {
            _Context = new DatabaseContext();
        }
        // GET: Admin/Import
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageIndex = page.HasValue ? page.Value : 1;
            return View(_Context.InforProducts.OrderBy(x => x.IDProduct).ToList().ToPagedList(pageIndex, pageSize));
        }
        [HttpPost]
        public JsonResult Edit(int ID, int num)
        {
            var infor = _Context.InforProducts.Find(ID);
            if (infor != null)
            {
                infor.Quantity = num;
                _Context.InforProducts.AddOrUpdate(infor);
                _Context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        //[HttpGet]
        //public JsonResult Details(int ID)
        //{
        //    var infor = _Context.InforProducts.SingleOrDefault(x => x.ID == ID);
        //    if (infor != null)
        //    {
        //        return Json(new { success = true, infor });
        //    }
        //    return Json(new { success = false });
        //}
        [HttpGet]
        public ActionResult Details(int ID)
        {
            var infor = _Context.InforProducts.SingleOrDefault(x => x.ID == ID);
            return Content(infor.Quantity.ToString());
        }
    }
}