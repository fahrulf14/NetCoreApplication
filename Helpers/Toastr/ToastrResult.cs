using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NUNA.Helpers
{
    public class ToastrResult : IActionResult
    {
        public IActionResult Result { get; }
        public string Type { get; }
        public string Message { get; }

        public ToastrResult(IActionResult result, string type, string message)
        {
            Result = result;
            Type = type;
            Message = message;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            if (Result is StatusCodeResult || Result is OkObjectResult)
            {
                AddAlertMessageToApiResult(context);
            }
            else
            {
                AddAlertMessageToMvcResult(context);
            }

            await Result.ExecuteResultAsync(context);
        }

        private void AddAlertMessageToApiResult(ActionContext context)
        {
            context.HttpContext.Response.Headers.Add("x-alert-type", Type);
            context.HttpContext.Response.Headers.Add("x-alert-message", Message);
        }

        private void AddAlertMessageToMvcResult(ActionContext context)
        {
            var factory = context.HttpContext.RequestServices.GetService<ITempDataDictionaryFactory>();

            var tempData = factory.GetTempData(context.HttpContext);
            tempData["_toastr"] = Type + "|" + Message;
        }
    }
}
