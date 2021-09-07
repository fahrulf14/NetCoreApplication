using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIP.Models;
using SIP.ViewModels;

namespace SIP.Controllers
{
    public class LraController : Controller
    {
        private readonly DB_NewContext _context;

        public LraController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "Laporan" })]
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

        [Auth(new string[] { "Developers", "Laporan" })]
        public async Task<IActionResult> Tahun(int id)
        {
            //Link
            ViewBag.Title = "LRA Tahun " + id;
            ViewBag.L = Url.Action("Tahun", new { id });
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";
            ViewBag.Tahun = id;

            var lra = await _context.Lra.Include(l => l.IdCoaNavigation).Where(l => l.Tahun == id).OrderBy(d => d.IdCoa).ToListAsync();
            return View(lra);
        }

        [Auth(new string[] { "Developers", "Laporan" })]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lra = await _context.Lra.Include(d => d.IdCoaNavigation).Include(d => d.TransaksiLra).FirstOrDefaultAsync(s => s.IdLra == id);

            ViewBag.Title = "LRA " + lra.IdCoaNavigation.Uraian;
            ViewBag.L = Url.Action("Tahun", new { id = lra.Tahun });
            ViewBag.L1 = Url.Action("Details", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View(lra);
        }

        [Route("Lra/Periode/{id}")]
        [Auth(new string[] { "Developers", "Laporan" })]
        public IActionResult Periode(string id)
        {
            //Link
            ViewBag.L = Url.Action("Tahun", new { id });
            ViewBag.L1 = Url.Action("Periode", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            List<SelectListItem> SP = new List<SelectListItem>
            {
              new SelectListItem() { Text = "Januari", Value = "1" },
                new SelectListItem() { Text = "Februari", Value = "2" },
                new SelectListItem() { Text = "Maret", Value = "3" },
                new SelectListItem() { Text = "April", Value = "4" },
                new SelectListItem() { Text = "Mei", Value = "5" },
                new SelectListItem() { Text = "Juni", Value = "6" },
                new SelectListItem() { Text = "Juli", Value = "7" },
                new SelectListItem() { Text = "Agustus", Value = "8" },
                new SelectListItem() { Text = "September", Value = "9" },
                new SelectListItem() { Text = "Oktober", Value = "10" },
                new SelectListItem() { Text = "November", Value = "11" },
                new SelectListItem() { Text = "Desember", Value = "12" }
            };
            ViewData["Bulan"] = new SelectList(SP, "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Tanggal(PeriodeLra lra)
        {
            if (ModelState.IsValid)
            {
                var tanggal1 = lra.Tanggal1;
                var tanggal2 = lra.Tanggal2;
                return RedirectToAction("Cetak", new { id = "Tanggal", tanggal1, tanggal2 });
            }
            return RedirectToAction("Periode");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Bulanan(PeriodeLra lra)
        {
            if (ModelState.IsValid)
            {
                var tanggal1 = new DateTime(lra.TahunBulan, lra.Bulan, 1);
                var tanggal2 = new DateTime(lra.TahunBulan, lra.Bulan, DateTime.DaysInMonth(lra.TahunBulan, lra.Bulan));
                return RedirectToAction("Cetak", new { id = "Bulan", tanggal1, tanggal2 });
            }
            return RedirectToAction("Periode");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Tahunan(PeriodeLra lra)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Cetak", new { id = "Tahun", tanggal1 = new DateTime(lra.Tahun, 1, 1), tanggal2 = new DateTime(lra.Tahun, 12, 31) });
            }
            return RedirectToAction("Periode");
        }

        [Auth(new string[] { "Developers", "Laporan" })]
        public ActionResult Cetak(string id, DateTime? tanggal1, DateTime? tanggal2)
        {
            if (tanggal1 != null && tanggal2 != null)
            {
                return RedirectToAction("LRACetak", "Cetak", new { id, tanggal1, tanggal2 });
            }
            else
            {
                return RedirectToAction("Periode");
            }
        }
    }
}
