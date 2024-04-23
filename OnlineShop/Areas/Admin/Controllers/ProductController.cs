using OnlineShop.Filters;
using OnlineShop.Models;
using PagedList;
using System;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    [isAdmin]
    public class ProductController : Controller
    {
        private DatabaseContext _Context;
        public ProductController()
        {
            _Context = new DatabaseContext();
        }
        // GET: Admin/Product
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageIndex = page.HasValue ? page.Value : 1;
            return View(_Context.Products.ToList().ToPagedList(pageIndex, pageSize));
        }
        [HttpGet]
        public JsonResult listProduct(string keySearch, int page)
        {
            var getList = _Context.Products
                 .Where(x => x.Name.Contains(keySearch))
                 .Select(x => new
                 {
                     ID = x.ID,
                     Name = x.Name,
                     Picture = x.Picture,
                     Cost = x.Cost,
                     SalePrice = x.SalePrice,
                     Size = x.InforProducts.Where(p => p.Quantity != 0).Select(p => p.Size.Name).ToList(),
                     Sum = x.InforProducts.Sum(p => p.Quantity),
                     DateCreated = x.DateCreated,
                     CategoryName = x.Category.Name,
                     Status = x.Status
                 }).ToList();
            int pageSize = 10;
            var numPage = getList.Count() % pageSize == 0 ? getList.Count() / pageSize : getList.Count() / pageSize + 1;
            var listProduct = getList.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Json(new { success = true, listProduct = listProduct, numPage = numPage }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(_Context.Categories, "ID", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, HttpPostedFileBase Picture)
        {
            var listSize = _Context.Sizes.ToList();
            foreach (var size in listSize)
            {
                InforProduct infor = new InforProduct
                {
                    IDProduct = product.ID,
                    IDSize = size.ID,
                    Quantity = 0
                };

                _Context.InforProducts.Add(infor);
            }
            product.Picture = "null";
            product.DateCreated = DateTime.Now;

            _Context.Products.Add(product);
            _Context.SaveChanges();
            if (Picture != null && Picture.ContentLength > 0)
            {
                string fileName = "";
                int ID = int.Parse(_Context.Products.ToList().Last().ID.ToString());
                int index = Picture.FileName.IndexOf('.');
                fileName = "ImageProduct" + ID.ToString() + "." + Picture.FileName.Substring(index + 1);
                string path = Path.Combine(Server.MapPath("~/Content/img"), fileName);
                Picture.SaveAs(path);
                product = _Context.Products.FirstOrDefault(x => x.ID == ID);
                product.Picture = fileName;
                _Context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int? ID)
        {
            Product prod = _Context.Products.Find(ID);
            if (prod == null || ID == null)
            {
                return RedirectToAction("Index", "Product");
            }
            ViewBag.CategoryID = new SelectList(_Context.Categories, "ID", "Name", prod.CategoryID);
            return View(prod);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, HttpPostedFileBase Picture)
        {
            var find = _Context.Products.FirstOrDefault(p => p.ID == product.ID);
            product.DateCreated = find.DateCreated;
            if (Picture != null && Picture.ContentLength > 0)
            {
                string fileName = "";
                int index = Picture.FileName.IndexOf('.');
                fileName = "ImageProduct" + product.ID.ToString() + "." + Picture.FileName.Substring(index + 1);
                string path = Path.Combine(Server.MapPath("~/Content/img"), fileName);
                Picture.SaveAs(path);
                product.Picture = fileName;
            }
            else
            {
                product.Picture = find.Picture;
            }
            _Context.Products.AddOrUpdate(product);
            _Context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult Delete(int ID)
        {
            var prod = _Context.Products.Find(ID);
            if (prod != null)
            {
                _Context.InforProducts.RemoveRange(prod.InforProducts.ToList());
                _Context.Favorites.RemoveRange(prod.Favorites.ToList());
                _Context.Products.Remove(prod);
                _Context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}