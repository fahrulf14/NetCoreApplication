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
    public class AsideIdentityViewComponent : ViewComponent
    {
        private readonly DB_NewContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AsideIdentityViewComponent(DB_NewContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (HttpContext.Session.GetString("Nama") == null || HttpContext.Session.GetString("Email") == null)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var pegawai = await _context.Pegawai.Include(d => d.IdJabatanNavigation).FirstOrDefaultAsync(d => d.Email == user.Email);
                HttpContext.Session.SetString("Email", pegawai.Email);
                HttpContext.Session.SetString("Nama", pegawai.Nama);
                HttpContext.Session.SetString("Jabatan", pegawai.IdJabatanNavigation.Jabatan);
            }

            var model = _context.Menu.Where(d => d.FlagAktif).OrderBy(d => d.NoUrut).ToList();

            return await Task.FromResult((IViewComponentResult)View("Default", model));
        }
    }
}
