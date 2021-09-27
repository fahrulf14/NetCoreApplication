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
            if (context.ViewName.StartsWith("ViewComponents"))
                return new string[] { "/{0}.cshtml" };

            return viewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context) { }
    }
}
