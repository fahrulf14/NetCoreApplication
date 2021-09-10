using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Attributes
{
    public class AuthAttribute : ActionFilterAttribute
    {
        private readonly string[] _role;

        public AuthAttribute(string[] role)
        {
            _role = role;
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            bool isAuthorized = _role.Any(filterContext.HttpContext.User.IsInRole);
            if (!isAuthorized)
            {
                filterContext.Result = new RedirectToRouteResult(new { action = "AccessDenied", controller = "Error" });
            }
            else
            {
                base.OnResultExecuting(filterContext);
            }

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(new { action = "Login", controller = "Error", returnUrl = filterContext.HttpContext.Request.Path.ToString() });
            }
        }
    }
}