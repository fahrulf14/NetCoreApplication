using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIP.Models
{
    public static class MenuActive
    {
        public static string IsSelected(this IHtmlHelper htmlherper, string controllers, string actions, string css = " kt-menu__item--active")
        {
            string currentAction = htmlherper.ViewContext.RouteData.Values["action"] as string;
            string currentController = htmlherper.ViewContext.RouteData.Values["controller"] as string;

            IEnumerable<string> acceptedActions = (actions ?? currentAction).Split(',');
            IEnumerable<string> acceptedControllers = (controllers ?? currentController).Split(',');

            return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ? css : String.Empty;
        }
    }
}
