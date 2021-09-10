using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using System.Linq;

namespace Attributes
{
    public class AjaxOnlyAttribute : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor actionDescriptor)
        {
            if (routeContext.HttpContext.Request.Headers != null &&
              routeContext.HttpContext.Request.Headers.ContainsKey("X-Requested-With") &&
              routeContext.HttpContext.Request.Headers.TryGetValue("X-Requested-With", out StringValues requestedWithHeader))
            {
                if (requestedWithHeader.Contains("XMLHttpRequest"))
                {
                    return true;
                }
            }

            return false;
        }
    }
}