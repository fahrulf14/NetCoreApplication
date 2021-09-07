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
    public class PenerimaanController : Controller
    {
        private readonly DB_NewContext _context;

        public PenerimaanController(DB_NewContext context)
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
                return RedirectToAction("Cetak", new { id = "Tanggal", tanggal1, tanggal2, rincian = lra.Rincian });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Bulanan(PeriodeLra lra)
        {
            if (ModelState.IsValid)
            {
                var tanggal1 = new DateTime(lra.TahunBulan, lra.Bulan, 1);
                var tanggal2 = new DateTime(lra.TahunBulan, lra.Bulan, DateTime.DaysInMonth(lra.TahunBulan, lra.Bulan));
                return RedirectToAction("Cetak", new { id = "Bulan", tanggal1, tanggal2, rincian = lra.Rincian });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Tahunan(PeriodeLra lra)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Cetak", new { id = "Tahun", tanggal1 = new DateTime(lra.Tahun, 1, 1), tanggal2 = new DateTime(lra.Tahun, 12, 31), rincian = lra.Rincian });
            }
            return RedirectToAction("Index");
        }

        [Auth(new string[] { "Developers", "Laporan" })]
        public ActionResult Cetak(string id, DateTime? tanggal1, DateTime? tanggal2, bool rincian)
        {
            return RedirectToAction("Penerimaan", "Cetak", new {id, tanggal1, tanggal2, rincian });
        }
    }
}
