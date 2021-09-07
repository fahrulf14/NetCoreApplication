using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SIP.Models;
using SIP.ViewModels;

namespace SIP.Controllers
{
    public class AjaxDataController : Controller
    {
        private readonly DB_NewContext _context;

        public AjaxDataController(DB_NewContext context)
        {
            _context = context;
        }

        public JsonResult Modal()
        {
            TempData["Partial"] = "True";
            return Json(new { success = true });
        }

        public JsonResult Menu(string id)
        {
            HttpContext.Session.SetString("Menu", id);

            if (id != "head")
            {
                HttpContext.Session.SetString("Button", "kt-aside__brand-aside-toggler--active");
            }
            else
            {
                HttpContext.Session.SetString("Button", "");
            }

            return Json(new { success = true });
        }
    }
}