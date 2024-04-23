using OnlineShop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private DatabaseContext _Context;
        public ShoppingCartController()
        {
            _Context = new DatabaseContext();
        }
        public List<Cart> GetShoppingCartFromSession()
        {
            var listShoppingCart = Session["ShoppingCart"] as List<Cart>;
            if (listShoppingCart == null)
            {
                listShoppingCart = new List<Cart>();
                Session["ShoppingCart"] = listShoppingCart;
            }
            return listShoppingCart;
        }
        // GET: ShoppingCart
        public ActionResult Index()
        {
            List<Cart> ShoppingCart = GetShoppingCartFromSession();
            if (ShoppingCart.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Total = ShoppingCart.Sum(x => x.Quantity * x.Price);
            return View(ShoppingCart);
        }
        [HttpPost]
        public JsonResult AddToCart(int ID, int quantity)
        {
            List<Cart> ShoppingCart = GetShoppingCartFromSession();
            var item = ShoppingCart.FirstOrDefault(x => x.ID == ID);
            if (item == null)
            {
                var i = _Context.InforProducts.FirstOrDefault(x => x.ID == ID);
                if (i.Quantity < quantity)
                {
                    return Json(new { success = false });
                }
                Cart cart = new Cart()
                {
                    ID = i.ID,
                    IDProduct = i.IDProduct,
                    Name = i.Product.Name,
                    Size = i.Size.Name,
                    Price = i.Product.SalePrice,
                    Picture = i.Product.Picture,
                    Quantity = quantity
                };
                ShoppingCart.Add(cart);
            }
            else
                item.Quantity += quantity;
            return Json(new { success = true, total = GetShoppingCartFromSession().Count() });
        }
        [HttpGet]
        public ActionResult getInStock(int ID)
        {
            var infor = _Context.InforProducts.FirstOrDefault(x => x.ID == ID);
            return Content(infor.Quantity.ToString());
        }
        public ActionResult CartSummary()
        {
            ViewBag.Total = GetShoppingCartFromSession().Count();
            return PartialView("_CartPartial");
        }
        public JsonResult Remove(int ID)
        {
            var item = GetShoppingCartFromSession().FirstOrDefault(x => x.ID == ID);
            if (item == null)
                return Json(new { success = false, msg = "Sản phẩm không tồn tại." });
            GetShoppingCartFromSession().Remove(item);
            var totalCount = GetShoppingCartFromSession().Count();
            var totalPrice = GetShoppingCartFromSession().Sum(x => x.Quantity * x.Price);
            return Json(new { success = true, msg = "Đã xóa sản phẩm khỏi giỏ hàng.", totalCount = totalCount, totalPrice = totalPrice });
        }
        public ActionResult RemoveAll()
        {
            GetShoppingCartFromSession().Clear();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult updateQuantity(int ID, int num)
        {
            var item = GetShoppingCartFromSession().FirstOrDefault(x => x.ID == ID);
            if (item != null)
                item.Quantity = num;
            var totalCount = GetShoppingCartFromSession().Count();
            var totalPrice = GetShoppingCartFromSession().Sum(x => x.Quantity * x.Price);
            return Json(new { success = true, totalCount = totalCount, totalPrice = totalPrice });
        }
        public JsonResult CheckVaild()
        {
            var listCart = GetShoppingCartFromSession().ToList();
            foreach (var item in listCart)
            {
                var info = _Context.InforProducts.SingleOrDefault(x => x.ID == item.ID);
                if (item.Quantity > info.Quantity)
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}