using OnlineShop.Filters;
using OnlineShop.Models;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class AccountController : Controller
    {
        private DatabaseContext _Context;
        public AccountController()
        {
            _Context = new DatabaseContext();
        }
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        //create a string MD5
        public static string getMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
        public ActionResult Login()
        {
            if (Session["User"] != null || Session["Admin"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string Username, string Password)
        {
            if (ModelState.IsValid)
            {
                Password = getMD5(Password);
                var user = _Context.Users.SingleOrDefault(x => x.Username == Username && x.Password.Equals(Password));
                if (user != null)
                {
                    if (user.Active)
                    {
                        Session["User"] = user;
                        if (user.Role)
                        {
                            Session["Admin"] = user;
                            return RedirectToAction("Home", "Admin");
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Tài khoản đã bị khóa!");
                    }
                }
                else
                    ModelState.AddModelError("", "Thông tin tài khoản không chính xác!");
            }
            return View();
        }
        public ActionResult Register()
        {
            if (Session["User"] != null || Session["Admin"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                var customer = _Context.Users.SingleOrDefault(x => x.Username == user.Username);
                var checkMail = _Context.Users.SingleOrDefault(x => x.Gmail == user.Gmail);

                if (customer == null)
                {
                    if (checkMail == null)
                    {
                        user.Password = getMD5(user.Password);
                        _Context.Users.Add(user);
                        _Context.SaveChanges();
                        Session["User"] = user;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Gmail đã có người đăng ký!");
                    }
                }
                else ModelState.AddModelError("", "Tên người dùng đã tồn tại!");
            }
            return View();
        }
        [isUser]
        [HttpGet]
        public ActionResult Edit()
        {
            var user = Session["User"] as User;
            var get = _Context.Users.SingleOrDefault(x => x.ID == user.ID);
            return View(get);
        }
        [isUser]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            var get = _Context.Users.SingleOrDefault(x => x.ID == user.ID);
            user.Password = get.Password;
            user.Role = get.Role;
            _Context.Users.AddOrUpdate(user);
            _Context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}