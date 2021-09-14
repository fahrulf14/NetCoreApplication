using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NUNA.Models.BaseApplicationContext;

namespace NUNA.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BaseApplicationContext _appContext;

        public HomeController(ILogger<HomeController> logger, BaseApplicationContext context)
        {
            _logger = logger;
            _appContext = context;
        }

        public IActionResult Index()
        {
            ViewBag.Title = "Home";

            return View();
        }

        public IActionResult DashBoard()
        {
            return View();
        }
    }
}
