using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUNA.Models.BaseApplicationContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NUNA.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BaseApplicationContext _appContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(
            ILogger<HomeController> logger,
            BaseApplicationContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _appContext = context;
            _userManager = userManager;
            _roleManager = roleManager;
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

        public async Task<IActionResult> StartUpApplication()
        {
            var check = (from a in _appContext.AspNetUsers
                         select a).Any();

            if (!check)
            {
                var UserName = "developer.nuha";
                var Email = "developer.nuna@gmail.com";
                var user = new IdentityUser { UserName = UserName, Email = Email };
                var result = await _userManager.CreateAsync(user, "Watashiwafdesu14.");

                if (result.Succeeded)
                {
                    Personals personals = new Personals
                    {
                        UserName = UserName,
                        Gender = "L",
                        IsActive = true,
                        IsVerified = true,
                        Name = "Developer"
                    };

                    _appContext.Add(personals);

                    IdentityResult roleResult = await _roleManager.CreateAsync(new IdentityRole("Developers"));

                    var dataUser = _appContext.AspNetUsers.FirstOrDefault(d => d.Email == Email);
                    dataUser.EmailConfirmed = true;
                    _appContext.AspNetUsers.Update(dataUser);

                    var dataRole = _appContext.AspNetRoles.FirstOrDefault(d => d.Name == "Developers");

                    AspNetUserRoles newRoles = new AspNetUserRoles
                    {
                        RoleId = dataRole.Id,
                        UserId = dataUser.Id
                    };
                    _appContext.Add(newRoles);

                    _appContext.SaveChanges();
                }
            }

            return View("Index");
        }
    }
}
