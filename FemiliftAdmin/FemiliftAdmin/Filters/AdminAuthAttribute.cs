using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FemiliftAdmin.Filters
{
    public class AdminAuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["AdminUserId"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        { "controller", "Account" },
                        { "action", "Login" },
                        { "area", "Admin" }
                    });
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
