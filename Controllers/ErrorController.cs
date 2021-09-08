using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SIP.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View("NotFound");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            var partial = TempData["Partial"];

            if (partial != null)
            {
                return PartialView("_AccessDenied");
            }
            return View("AccessDenied");
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return Redirect("/Identity/Account/Login?returnUrl=" + returnUrl);
        }

        [AllowAnonymous]
        public IActionResult UnderConstruction()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult PageNotFound()
        {
            return View("NotFound");
        }
    }
}