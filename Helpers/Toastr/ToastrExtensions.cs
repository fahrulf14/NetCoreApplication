using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace NUNA.Helpers
{
    public static class ToastrExtensions
    {
        public static IActionResult WithSuccess(this IActionResult result, string message)
        {
            return Alert(result, "success", message);
        }

        public static IActionResult WithInfo(this IActionResult result, string message)
        {
            return Alert(result, "info", message);
        }

        public static IActionResult WithWarning(this IActionResult result, string message)
        {
            return Alert(result, "warning", message);
        }

        public static IActionResult WithError(this IActionResult result, string message)
        {
            return Alert(result, "error", message);
        }

        private static IActionResult Alert(IActionResult result, string type, string message)
        {
            return new ToastrResult(result, type, message);
        }
    }
}
