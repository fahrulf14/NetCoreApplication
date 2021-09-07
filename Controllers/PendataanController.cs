using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIP.Models;
using SIP.ViewModels;
using Attributes;

namespace SIP.Controllers
{
    public class PendataanController : Controller
    {
        private readonly DB_NewContext _context;

        public PendataanController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "Pendataan" })]
        public async Task<IActionResult> Pribadi()
        {
            //Link
            ViewBag.L = Url.Action("Pribadi", null);
            ViewBag.L1 = Url.Action("Pribadi", null);
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            ViewBag.Pekerjaan = _context.RefPekerjaan.ToList();

            return View(await _context.DataSubjek.Include(d => d.IndKecamatan).Include(d => d.IdPekerjaanNavigation).Where(d => d.IdBadanHukum == null && d.Npwpd != null).ToListAsync());
        }

        [Auth(new string[] { "Developers", "Pendataan" })]
        public async Task<IActionResult> Badan()
        {
            //Link
            ViewBag.L = Url.Action("Badan", null);
            ViewBag.L1 = Url.Action("Badan", null);
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            ViewBag.BadanHukum = _context.RefBadanHukum.ToList();

            return View(await _context.DataSubjek.Include(d => d.IndKecamatan).Include(d => d.IdBadanHukumNavigation).Where(d => d.IdBadanHukum != null && d.Npwpd != null).ToListAsync());
        }

        [Auth(new string[] { "Developers", "Pendataan" })]
        public async Task<IActionResult> DataUsaha(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("DataUsaha", new { id });
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            ViewBag.IdSubjek = id;
            var data = _context.DataUsaha.Include(d => d.Sptpd).Include(d => d.IdJenisNavigation).Include(d => d.IndKecamatan).Where(d => d.IdSubjek == id).ToListAsync();

            return View(await data);
        }

        [Auth(new string[] { "Developers", "Retribusi" })]
        public async Task<IActionResult> Retribusi()
        {
            //Link
            ViewBag.L = Url.Action("Retrubusi");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var retribusi = await _context.RefRetribusi
                .Include(d => d.IdCoaNavigation).Where(d => d.FlagAktif).ToListAsync();

            return View(retribusi);
        }

        private bool DataSubjekExists(Guid id)
        {
            return _context.DataSubjek.Any(e => e.IdSubjek == id);
        }
    }
}
