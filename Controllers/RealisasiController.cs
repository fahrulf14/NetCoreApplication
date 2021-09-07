using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIP.Models;

namespace SIP.Controllers
{
    public class RealisasiController : Controller
    {
        private readonly DB_NewContext _context;

        public RealisasiController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "Realisasi" })]
        public IActionResult Index()
        {
            //Link
            ViewBag.L = Url.Action("Index");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Lra lra)
        {
            return RedirectToAction("Tahun", new { id = lra.Tahun });
        }

        [Auth(new string[] { "Developers", "Realisasi" })]
        public async Task<IActionResult> Tahun(int id)
        {
            //Link
            ViewBag.L = Url.Action("Tahun", new { id });
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var lra = await _context.Lra.Include(l => l.IdCoaNavigation)
                .Where(d => d.Tahun == id && d.IdCoa.Substring(0, 3) != "411" && d.IdCoa.Substring(0, 3) != "412" && d.IdCoaNavigation.Parent != "41407" && d.IdCoaNavigation.Parent != "41408" && d.IdCoaNavigation.Tingkat == 5)
                .OrderBy(d => d.IdCoa).ToListAsync();

            return View(lra);
        }

    }
}
