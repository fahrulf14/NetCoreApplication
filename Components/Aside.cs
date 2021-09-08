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
        private readonly DB_NewContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AsideViewComponent(DB_NewContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        public async Task<IViewComponentResult> InvokeAsync(string controller, string action)
        {
            if (HttpContext.Session.GetString("Nama") == null || HttpContext.Session.GetString("Email") == null)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var Personal = await _context.Personal.FirstOrDefaultAsync(d => d.Email == user.Email);

                var Position = _context.RF_Positions.Where(d => d.Id == Personal.PositionId).Select(d => d.Position).FirstOrDefault();

                HttpContext.Session.SetString("Email", Personal.Email);
                HttpContext.Session.SetString("Nama", Personal.Nama);
                HttpContext.Session.SetString("Position", Position);
            }

            var model = _context.Menu.Where(d => d.FlagAktif).OrderBy(d => d.NoUrut).ToList();

            var menu = _context.Menu.Where(d => d.Controller == controller && d.ActionName == action).FirstOrDefault();
            if (menu != null)
            {
                if (menu.ParentId != 0)
                {
                    var child = _context.Menu.Find(menu.ParentId);
                    if (child.ParentId != 0)
                    {
                        var sub = _context.Menu.Find(child.ParentId);
                        if (sub.ParentId != 0)
                        {
                            ViewBag.Parent = sub.ParentId;
                            ViewBag.SubParent = child.ParentId;
                            ViewBag.Sub = menu.ParentId;
                        }
                        else
                        {
                            ViewBag.Parent = child.ParentId;
                            ViewBag.SubParent = menu.ParentId;
                        }
                    }
                    else
                    {
                        ViewBag.Parent = menu.ParentId;
                    }
                }
                else
                {
                    ViewBag.Parent = 0;
                }
            }

            return await Task.FromResult((IViewComponentResult)View("Default", model));
        }
    }
}
