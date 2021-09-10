using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIP.Models;

namespace SIP.Components
{
    public class AsideViewComponent : ViewComponent
    {
        private readonly BaseApplicationContext _appContext;
        private readonly UserManager<IdentityUser> _userManager;

        public AsideViewComponent(BaseApplicationContext context, UserManager<IdentityUser> userManager)
        {
            _appContext = context;
            _userManager = userManager;

        }

        public async Task<IViewComponentResult> InvokeAsync(string controller, string action)
        {
            if (HttpContext.Session.GetString("Nama") == null || HttpContext.Session.GetString("Email") == null)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var Personal = await _appContext.Personal.FirstOrDefaultAsync(d => d.Email == user.Email);

                var Position = _appContext.RF_Positions.Where(d => d.Id == Personal.PositionId).Select(d => d.Position).FirstOrDefault();

                HttpContext.Session.SetString("Email", Personal.Email);
                HttpContext.Session.SetString("Nama", Personal.Nama);
                HttpContext.Session.SetString("Position", Position);
            }

            var model = _appContext.Menu.Where(d => d.IsActive).OrderBy(d => d.NoUrut).ToList();

            var menu = _appContext.Menu.Where(d => d.Controller == controller && d.ActionName == action).FirstOrDefault();
            if (menu != null)
            {
                if (menu.Parent != "0")
                {
                    var child = _appContext.Menu.Where(d => d.Code == menu.Parent).FirstOrDefault();
                    if (child.Parent != "0")
                    {
                        var sub = _appContext.Menu.Where(d => d.Code == child.Parent).FirstOrDefault();
                        if (sub.Parent != "0")
                        {
                            ViewBag.Parent = sub.Parent;
                            ViewBag.SubParent = child.Parent;
                            ViewBag.Sub = menu.Parent;
                        }
                        else
                        {
                            ViewBag.Parent = child.Parent;
                            ViewBag.SubParent = menu.Parent;
                        }
                    }
                    else
                    {
                        ViewBag.Parent = menu.Parent;
                    }
                }
                else
                {
                    ViewBag.Parent = "0";
                }
            }

            return await Task.FromResult((IViewComponentResult)View("Default", model));
        }
    }
}
