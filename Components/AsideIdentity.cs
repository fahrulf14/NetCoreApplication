using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUNA.Models.BaseApplicationContext;

namespace NUNA.Components
{
    public class AsideIdentityViewComponent : ViewComponent
    {
        private readonly BaseApplicationContext _appContext;
        private readonly UserManager<IdentityUser> _userManager;

        public AsideIdentityViewComponent(BaseApplicationContext context, UserManager<IdentityUser> userManager)
        {
            _appContext = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
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

            return await Task.FromResult((IViewComponentResult)View("Default", model));
        }
    }
}
