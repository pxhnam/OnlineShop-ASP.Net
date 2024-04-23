using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Filters
{
    public class isAdmin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["Admin"] == null)
            {
                filterContext.Result = new RedirectResult("~/Error");
            }
        }
    }
}