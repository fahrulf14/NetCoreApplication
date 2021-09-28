using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NUNA.Helpers
{
    public class ComponentViewLocationExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (context.ViewName.StartsWith("Components"))
                return new string[] { "ViewComponents/{0}" + RazorViewEngine.ViewExtension };

            return viewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context) { }
    }
}
