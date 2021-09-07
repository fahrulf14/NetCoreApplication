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
    public class PBBController : Controller
    {
        private readonly DB_NewContext _context;

        public PBBController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "PBB" })]
        public IActionResult Index()
        {
            //Link
            ViewBag.L = Url.Action("Index");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var pemda = _context.Pemda.FirstOrDefault();
            ViewBag.Kecamatan = _context.IndKecamatan.Where(d => d.IndKabKotaId == pemda.IndKabKotaId).ToList();

            return View();
        }

        [Auth(new string[] { "Developers", "PBB" })]
        public async Task<IActionResult> SPOP(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "SPOP");
            }

            //Link
            ViewBag.L = Url.Action("SPOP", new { id });
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";
            ViewBag.IdSubjek = id;

            var spop = _context.Spop.Include(s => s.IndKecamatan).Include(s => s.IndKelurahan).Where(s => s.IdSubjek == id);
            return View(await spop.ToListAsync());
        }

        [Auth(new string[] { "Developers", "PBB" })]
        public async Task<IActionResult> LSPOP(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("LSPOP", new { id });
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var lspop = await _context.Lspop.Include(l => l.IdJenisNavigation).Include(l => l.IdSpopNavigation).Where(s => s.IdSpop == id).ToListAsync();
            var spop = await _context.Spop.FirstOrDefaultAsync(d => d.IdSpop == id);
            ViewBag.IdSubjek = spop.IdSubjek;
            ViewBag.IdSpop = id;

            if(lspop.Count() < 1) // Jumlah Bangunan
            {
                ViewBag.Jumlah = lspop.Count();
            }

            return View(lspop);
        }

        [Auth(new string[] { "Developers", "PBB" })]
        public async Task<IActionResult> SPPT(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "SPPT");
            }

            //Link
            ViewBag.L = Url.Action("SPPT", new { id });
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var sppt = await _context.Sppt.Where(d => d.IdSpop == id).ToListAsync();
            var spop = await _context.Spop.FirstOrDefaultAsync(d => d.IdSpop == id);

            ViewBag.IdSubjek = spop.IdSubjek;
            ViewBag.IdSpop = id;

            return View(sppt);
        }

        [Auth(new string[] { "Developers", "Pembayaran" })]
        public async Task<IActionResult> STTS(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "STTS");
            }

            //Link
            ViewBag.L = Url.Action("STTS", new { id });
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";
            ViewBag.IdSubjek = id;

            var stts = await _context.Stts.Include(s => s.IdBankNavigation).Include(s => s.IdSetoranNavigation)
                .Include(s => s.IdSpptNavigation).ThenInclude(s => s.IdSpopNavigation).Where(s => s.IdSpptNavigation.IdSpopNavigation.IdSubjek == id).ToListAsync();

            var sppt = await _context.Sppt.Include(d => d.IdSpopNavigation).Where(d => d.IdSpopNavigation.IdSubjek == id && d.Status == false).ToListAsync();
            ViewBag.Sppt = sppt;
            ViewBag.List = sppt.Count();

            return View(stts);
        }

    }
}
