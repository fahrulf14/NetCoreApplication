 using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIP.Models;

namespace SIP.Controllers
{
    public class SKPDController : Controller
    {
        private readonly DB_NewContext _context;

        public SKPDController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "Laporan" })]
        public IActionResult Index()
        {
            return RedirectToAction("SKPD", "Laporan");
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skpd = await _context.Skpd
                .Include(s => s.IdSptpdNavigation)
                .ThenInclude(s => s.IdCoaNavigation)
                .FirstOrDefaultAsync(m => m.IdSkpd == id);
            if (skpd == null)
            {
                return NotFound();
            }

            var coa = skpd.IdSptpdNavigation.IdCoa.Substring(0, 5);
            if (coa == "41104")
            {
                var Reklame = _context.PReklame.Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdTarifNavigation).Include(d => d.IndKelurahan).Include(d => d.IndKecamatan).Where(d => d.IdSptpd == skpd.IdSptpd).ToList();
                ViewBag.Reklame = Reklame;
                var pReklame = Reklame.FirstOrDefault();
                if (pReklame.IdSptpdNavigation.IdCoa == "4110403")
                {
                    ViewBag.Ukuran = "CM";
                    ViewBag.UkuranL = "CM<sup>2</sup>";
                }
                else
                {
                    ViewBag.Ukuran = "M";
                    ViewBag.UkuranL = "M<sup>2</sup>";
                }

                if (pReklame.IdSptpdNavigation.IdCoa == "4110401" || pReklame.IdSptpdNavigation.IdCoa == "4110402" || pReklame.IdSptpdNavigation.IdCoa == "4110405" || pReklame.IdSptpdNavigation.IdCoa == "4110406" || pReklame.IdSptpdNavigation.IdCoa == "4110407")
                {
                    ViewBag.Jumlah = "Buah";
                }
                else if (pReklame.IdSptpdNavigation.IdCoa == "4110408" || pReklame.IdSptpdNavigation.IdCoa == "4110409")
                {
                    ViewBag.Jumlah = "Kali Tayang";
                }
                else if (pReklame.IdSptpdNavigation.IdCoa == "4110411")
                {
                    ViewBag.Jumlah = "Kali Tayang / Hari";
                }
                else if (pReklame.IdSptpdNavigation.IdCoa == "4110403" || pReklame.IdSptpdNavigation.IdCoa == "4110404")
                {
                    ViewBag.Jumlah = "Lembar";
                }
                else
                {
                    ViewBag.Jumlah = "Kali Peragaan";
                }

                if (pReklame.IdSptpdNavigation.IdCoa == "4110401" || pReklame.IdSptpdNavigation.IdCoa == "4110402" || pReklame.IdSptpdNavigation.IdCoa == "4110411" || pReklame.IdSptpdNavigation.IdCoa == "4110405")
                {
                    ViewBag.Hari = "Hari";
                }
                else if (pReklame.IdSptpdNavigation.IdCoa == "4110406" || pReklame.IdSptpdNavigation.IdCoa == "4110407")
                {
                    ViewBag.Hari = "Bulan";
                }
            }
            else if (coa == "41108")
            {
                ViewBag.AirTanah = _context.PAirTanah.Where(d => d.IdSptpd == skpd.IdSptpd).ToList();
            }

            //Link
            ViewBag.L = Url.Action("SKPD", "Penetapan", new { id = skpd.IdSptpd });
            ViewBag.L1 = Url.Action("Details", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            //Status
            if (skpd.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (skpd.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            else if (skpd.Keterangan == 2)
            {
                ViewBag.State = "kt-ribbon--warning";
                ViewBag.Status = "Kurang Bayar";
            }
            else if (skpd.Keterangan == 3)
            {
                ViewBag.State = "kt-ribbon--primary";
                ViewBag.Status = "Lebih Bayar";
            }

            return View(skpd);
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public IActionResult Create(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sptpd = _context.Sptpd.Find(id);
            var coa = sptpd.IdCoa.Substring(0, 5);

            if (coa == "41104")
            {
                return RedirectToAction("SKPD", "PajakReklame", new { id });
            }
            else if (coa == "41108")
            {
                return RedirectToAction("SKPD", "PajakAirTanah", new { id });
            }

            return NotFound();
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skpd = _context.Skpd.Include(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdSkpd == id);
            var coa = skpd.IdSptpdNavigation.IdCoa.Substring(0, 5);

            if (coa == "41104")
            {
                return RedirectToAction("SKPD", "PajakReklame", new { id = skpd.IdSptpd });
            }
            else if (coa == "41108")
            {
                return RedirectToAction("SKPD", "PajakAirTanah", new { id = skpd.IdSptpd });
            }

            return NotFound();
        }

        [Auth(new string[] { "Developers", "Validasi" })]
        public async Task<IActionResult> Validasi(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skpd = await _context.Skpd
                .Include(s => s.IdSptpdNavigation)
                .ThenInclude(s => s.IdCoaNavigation)
                .FirstOrDefaultAsync(m => m.IdSkpd == id);
            if (skpd == null)
            {
                return NotFound();
            }

            var coa = skpd.IdSptpdNavigation.IdCoa.Substring(0, 5);
            if (coa == "41104")
            {
                var Reklame = _context.PReklame.Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdTarifNavigation).Include(d => d.IndKelurahan).Include(d => d.IndKecamatan).Where(d => d.IdSptpd == skpd.IdSptpd).ToList();
                ViewBag.Reklame = Reklame;
                var pReklame = Reklame.FirstOrDefault();
                if (pReklame.IdSptpdNavigation.IdCoa == "4110403")
                {
                    ViewBag.Ukuran = "CM";
                    ViewBag.UkuranL = "CM<sup>2</sup>";
                }
                else
                {
                    ViewBag.Ukuran = "M";
                    ViewBag.UkuranL = "M<sup>2</sup>";
                }

                if (pReklame.IdSptpdNavigation.IdCoa == "4110401" || pReklame.IdSptpdNavigation.IdCoa == "4110402" || pReklame.IdSptpdNavigation.IdCoa == "4110405" || pReklame.IdSptpdNavigation.IdCoa == "4110406" || pReklame.IdSptpdNavigation.IdCoa == "4110407")
                {
                    ViewBag.Jumlah = "Buah";
                }
                else if (pReklame.IdSptpdNavigation.IdCoa == "4110408" || pReklame.IdSptpdNavigation.IdCoa == "4110409")
                {
                    ViewBag.Jumlah = "Kali Tayang";
                }
                else if (pReklame.IdSptpdNavigation.IdCoa == "4110411")
                {
                    ViewBag.Jumlah = "Kali Tayang / Hari";
                }
                else if (pReklame.IdSptpdNavigation.IdCoa == "4110403" || pReklame.IdSptpdNavigation.IdCoa == "4110404")
                {
                    ViewBag.Jumlah = "Lembar";
                }
                else
                {
                    ViewBag.Jumlah = "Kali Peragaan";
                }

                if (pReklame.IdSptpdNavigation.IdCoa == "4110401" || pReklame.IdSptpdNavigation.IdCoa == "4110402" || pReklame.IdSptpdNavigation.IdCoa == "4110411" || pReklame.IdSptpdNavigation.IdCoa == "4110405")
                {
                    ViewBag.Hari = "Hari";
                }
                else if (pReklame.IdSptpdNavigation.IdCoa == "4110406" || pReklame.IdSptpdNavigation.IdCoa == "4110407")
                {
                    ViewBag.Hari = "Bulan";
                }
            }
            else if (coa == "41108")
            {
                ViewBag.AirTanah = _context.PAirTanah.Where(d => d.IdSptpd == skpd.IdSptpd).ToList();
            }

            //Link
            ViewBag.L = Url.Action("SKPD", "Penetapan", new { id = skpd.IdSptpd });
            ViewBag.L1 = Url.Action("Validasi", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            //Status
            if (skpd.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (skpd.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            else if (skpd.Keterangan == 2)
            {
                ViewBag.State = "kt-ribbon--warning";
                ViewBag.Status = "Kurang Bayar";
            }
            else if (skpd.Keterangan == 3)
            {
                ViewBag.State = "kt-ribbon--primary";
                ViewBag.Status = "Lebih Bayar";
            }

            return View(skpd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Validasi(Guid id, Skpd skp)
        {
            if (id != skp.IdSkpd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var skpd = _context.Skpd.FirstOrDefault(d => d.IdSkpd == id);
                if (skpd.FlagValidasi == false)
                {
                    skpd.FlagValidasi = true;
                    skpd.TanggalValidasi = DateTime.Now;
                    TempData["status"] = "valid";
                }
                else
                {
                    skpd.FlagValidasi = false;
                    skpd.TanggalValidasi = null;
                    TempData["status"] = "validbatal";
                }
                skpd.Eu = HttpContext.Session.GetString("User");
                skpd.Ed = DateTime.Now;
                _context.Skpd.Update(skpd);

                await _context.SaveChangesAsync();
                return RedirectToAction("SKPD", "Penetapan", new { id = skpd.IdSptpd });
            }
            return RedirectToAction("Validasi", new { id = skp.IdSkpd });
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public IActionResult Cetak(Guid id)
        {
            return RedirectToAction("SKPD", "Cetak", new { id });
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skpd = await _context.Skpd
                .FirstOrDefaultAsync(m => m.IdSkpd == id);
            if (skpd == null)
            {
                return NotFound();
            }

            if (skpd.FlagValidasi)
            {
                return PartialView("_deletevalid");
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var skpd = await _context.Skpd.FindAsync(id);
            try
            {
                _context.Skpd.Remove(skpd);

                var sptpd = await _context.Sptpd.FirstOrDefaultAsync(d => d.IdSptpd == skpd.IdSptpd);
                sptpd.Sk = null;
                sptpd.Eu = HttpContext.Session.GetString("User");
                sptpd.Ed = DateTime.Now;
                _context.Sptpd.Update(sptpd);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["status"] = "deletefailed";
                string url = Url.Action("SKPD", "Penetapan", new { id = skpd.IdSptpd });
                return Json(new { success = true, url });
            }
            
            TempData["status"] = "delete";
            string link = Url.Action("SKPD", "Penetapan", new { id = skpd.IdSptpd });
            return Json(new { success = true, url = link });
        }

        private bool SkpdExists(Guid id)
        {
            return _context.Skpd.Any(e => e.IdSkpd == id);
        }
    }
}
