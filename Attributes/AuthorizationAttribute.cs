using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SIP.Helpers;
using System.Linq;

namespace Attributes
{
    public class AuthorizationAttribute : ActionFilterAttribute
    {
        private readonly string _permission;

        public AuthorizationAttribute(string permission)
        {
            _permission = permission;
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            bool isAuthorized = false;

            SessionHandler _session = new SessionHandler();
            var listPermissionString = _session.Get("Permission");

            if (!string.IsNullOrEmpty(listPermissionString))
            {
                var listPermission = listPermissionString.Split("|").ToList();
                var parentPermission = (from a in listPermission
                                        where a.Contains(".")
                                        select a.Split(".")[0]).Distinct().ToList();
                if(parentPermission.Any())
                {
                    listPermission.AddRange(parentPermission);
                }

                isAuthorized = (from a in listPermission
                                where a.Contains(_permission)
                                select a).Any();
            }

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