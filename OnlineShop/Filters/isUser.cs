using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Filters
{
    public class isUser : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["User"] == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
            }
        }
    }
}