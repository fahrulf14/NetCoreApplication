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
                var pegawai = await _context.Pegawai.Include(d => d.IdJabatanNavigation).FirstOrDefaultAsync(d => d.Email == user.Email);
                HttpContext.Session.SetString("Email", pegawai.Email);
                HttpContext.Session.SetString("Nama", pegawai.Nama);
                HttpContext.Session.SetString("Jabatan", pegawai.IdJabatanNavigation.Jabatan);
            }

            if (HttpContext.Session.GetString("Pemda") == null || HttpContext.Session.GetString("NamaPemda") == null)
            {
                var pemda = _context.Pemda.Include(d => d.IndKabKota).Include(d => d.IndKecamatan).FirstOrDefault();
                var Pemda = pemda.IndKabKota.KabKota.Replace("Kab. ", "").Replace("Kota ", "");
                var NamaPemda = "PEMERINTAH " + (pemda.IndKabKota.KabKota.Contains("Kab") ? "KABUPATEN " : "KOTA ") + pemda.IndKabKota.KabKota.Replace("Kab. ", "").Replace("Kota ", "").ToUpper();
                var Opd = pemda.NamaOpd.ToUpper();
                var Alamat = pemda.Alamat + " Telp. " + pemda.Telp;
                var IdKabKotaId = pemda.IndKabKotaId;
                var Kecamatan = pemda.IndKecamatan.Kecamatan;

                HttpContext.Session.SetString("Pemda", Pemda);
                HttpContext.Session.SetString("NamaPemda", NamaPemda);
                HttpContext.Session.SetString("Opd", Opd);
                HttpContext.Session.SetString("Alamat", Alamat);
                HttpContext.Session.SetInt32("IdKabKotaId", IdKabKotaId.Value);
                HttpContext.Session.SetString("Kecamatan", Kecamatan);
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
