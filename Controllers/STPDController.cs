 using System;
using System.Collections.Generic;
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
    public class STPDController : Controller
    {
        private readonly DB_NewContext _context;

        public STPDController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "Laporan" })]
        public IActionResult Index()
        {
            return RedirectToAction("STPD", "Laporan");
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stpd = await _context.Stpd
                .Include(d => d.IdSkpdNavigation).ThenInclude(s => s.IdSptpdNavigation)
                .ThenInclude(s => s.IdCoaNavigation)
                .FirstOrDefaultAsync(m => m.IdStpd == id);
            if (stpd == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("STPD", "Penetapan", new { id = stpd.IdSkpdNavigation.IdSptpd });
            ViewBag.L1 = Url.Action("Details", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            //Status
            if (stpd.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (stpd.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            else if (stpd.Keterangan == 2)
            {
                ViewBag.State = "kt-ribbon--warning";
                ViewBag.Status = "Kurang Bayar";
            }
            else if (stpd.Keterangan == 3)
            {
                ViewBag.State = "kt-ribbon--primary";
                ViewBag.Status = "Lebih Bayar";
            }

            return View(stpd);
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public async Task<IActionResult> Create(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skpd = await _context.Skpd
                .Include(s => s.IdSptpdNavigation)
                .ThenInclude(s => s.IdCoaNavigation)
                .FirstOrDefaultAsync(s => s.IdSkpd == id);
            if (skpd == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("STPD", "Penetapan", new { id = skpd.IdSptpd });
            ViewBag.L1 = Url.Action("Create", new { id });
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
        public async Task<IActionResult> Create(Guid id, Stpd stpd)
        {
            if (ModelState.IsValid)
            {
                var skpd = _context.Skpd.Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation).FirstOrDefault(d => d.IdSkpd == id);
                var coa = _context.Coa.AsNoTracking().FirstOrDefault(d => d.IdCoa == skpd.IdSptpdNavigation.IdCoaNavigation.Parent);
                stpd.IdStpd = Guid.NewGuid();
                stpd.IdSkpd = id;
                stpd.Tanggal = DateTime.Now;
                var noSk = _context.Stpd.Where(d => d.Tanggal.Value.Year == stpd.Tanggal.Value.Year).Select(e => e.Nomor).Max() ?? 0;
                stpd.Nomor = noSk + 1;
                stpd.NoStpd = string.Format("{0:000000}", stpd.Nomor) + "/STPD/" + string.Format("{0:MM/yyyy}", stpd.Tanggal);
                stpd.JatuhTempo = stpd.Tanggal.Value.AddDays(30);
                stpd.KurangBayar = skpd.Terhutang - skpd.KreditPajak;

                var JHari = DateTime.Now.Subtract(skpd.IdSptpdNavigation.MasaPajak1 ?? DateTime.Now).Days;
                var MaxBulan = 24;
                var JBulan = Convert.ToDecimal(JHari / 30);
                if (JBulan > MaxBulan)
                {
                    JBulan = MaxBulan;
                }
                var Bunga = stpd.KurangBayar * 2 / 100 * Math.Floor(JBulan);
                stpd.Bunga = (int)Bunga;
                stpd.Terhutang = stpd.KurangBayar + stpd.Bunga;
                stpd.KreditPajak = 0;
                stpd.Keterangan = 0;
                stpd.Eu = HttpContext.Session.GetString("User");
                stpd.Ed = DateTime.Now;
                _context.Stpd.Add(stpd);

                skpd.Sk = "STPD";
                skpd.Eu = HttpContext.Session.GetString("User");
                skpd.Ed = DateTime.Now;
                _context.Skpd.Update(skpd);
                await _context.SaveChangesAsync();

                TempData["status"] = "create";
                return RedirectToAction("Details", new { id = stpd.IdStpd });
            }
            return RedirectToAction("Create", new { id });
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stpd = await _context.Stpd
                .Include(s => s.IdSkpdNavigation)
                .ThenInclude(s => s.IdSptpdNavigation)
                .ThenInclude(s => s.IdCoaNavigation)
                .FirstOrDefaultAsync(s => s.IdStpd == id);
            if (stpd == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("STPD", "Penetapan", new { id = stpd.IdSkpdNavigation.IdSptpd });
            ViewBag.L1 = Url.Action("Edit", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            //Status
            if (stpd.IdSkpdNavigation.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (stpd.IdSkpdNavigation.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            else if (stpd.IdSkpdNavigation.Keterangan == 2)
            {
                ViewBag.State = "kt-ribbon--warning";
                ViewBag.Status = "Kurang Bayar";
            }
            else if (stpd.IdSkpdNavigation.Keterangan == 3)
            {
                ViewBag.State = "kt-ribbon--primary";
                ViewBag.Status = "Lebih Bayar";
            }

            return View(stpd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Stpd stpd)
        {
            if (id != stpd.IdStpd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var skpd = _context.Stpd.Include(d => d.IdSkpdNavigation).ThenInclude(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdStpd == id);
                try
                {
                    skpd.Tanggal = DateTime.Now;
                    skpd.JatuhTempo = skpd.Tanggal.Value.AddDays(30);
                    skpd.KurangBayar = skpd.IdSkpdNavigation.Terhutang - skpd.IdSkpdNavigation.KreditPajak;

                    var JHari = DateTime.Now.Subtract(skpd.IdSkpdNavigation.IdSptpdNavigation.MasaPajak1 ?? DateTime.Now).Days;
                    var MaxBulan = 24;
                    var JBulan = Convert.ToDecimal(JHari / 30);
                    if (JBulan > MaxBulan)
                    {
                        JBulan = MaxBulan;
                    }
                    var Bunga = skpd.KurangBayar * 2 / 100 * Math.Floor(JBulan);
                    skpd.Bunga = (int)Bunga;
                    skpd.Terhutang = skpd.KurangBayar + skpd.Bunga;
                    skpd.KreditPajak = 0;
                    skpd.Keterangan = 0;
                    skpd.Eu = HttpContext.Session.GetString("User");
                    skpd.Ed = DateTime.Now;
                    _context.Stpd.Update(skpd);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StpdExists(stpd.IdStpd))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "edit";
                return RedirectToAction("STPD", "Penetapan", new { id = skpd.IdSkpdNavigation.IdSptpd });
            }
            return View(stpd);
        }

        [Auth(new string[] { "Developers", "Validasi" })]
        public async Task<IActionResult> Validasi(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stpd = await _context.Stpd
                .Include(d => d.IdSkpdNavigation).ThenInclude(s => s.IdSptpdNavigation)
                .ThenInclude(s => s.IdCoaNavigation)
                .FirstOrDefaultAsync(m => m.IdStpd == id);
            if (stpd == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("STPD", "Penetapan", new { id = stpd.IdSkpdNavigation.IdSptpd });
            ViewBag.L1 = Url.Action("Validasi", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            //Status
            if (stpd.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (stpd.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            else if (stpd.Keterangan == 2)
            {
                ViewBag.State = "kt-ribbon--warning";
                ViewBag.Status = "Kurang Bayar";
            }
            else if (stpd.Keterangan == 3)
            {
                ViewBag.State = "kt-ribbon--primary";
                ViewBag.Status = "Lebih Bayar";
            }

            return View(stpd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Validasi(Guid id, Stpd stpd)
        {
            if (id != stpd.IdStpd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var skpd = _context.Stpd.Include(d => d.IdSkpdNavigation).ThenInclude(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdStpd == id);
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
                _context.Stpd.Update(skpd);

                await _context.SaveChangesAsync();
                return RedirectToAction("STPD", "Penetapan", new { id = skpd.IdSkpdNavigation.IdSptpd });
            }
            return View(stpd);
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public ActionResult Cetak(Guid? id)
        {
            return RedirectToAction("STPD", "Cetak", new { id });
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stpd = await _context.Stpd
                .Include(d => d.IdSkpdNavigation).ThenInclude(s => s.IdSptpdNavigation)
                .FirstOrDefaultAsync(m => m.IdStpd == id);
            if (stpd == null)
            {
                return NotFound();
            }

            if (stpd.FlagValidasi)
            {
                return PartialView("_deletevalid");
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var stpd = await _context.Stpd.FindAsync(id);
            var skpd = await _context.Skpd.FirstOrDefaultAsync(d => d.IdSkpd == stpd.IdSkpd);
            try
            {
                _context.Stpd.Remove(stpd);

                skpd.Sk = null;
                skpd.Eu = HttpContext.Session.GetString("User");
                skpd.Ed = DateTime.Now;
                _context.Skpd.Update(skpd);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["status"] = "deletefailed";
                string url = Url.Action("STPD", "Penetapan", new { id = skpd.IdSptpd });
                return Json(new { success = true, url });
            }
            
            TempData["status"] = "delete";
            string link = Url.Action("STPD", "Penetapan", new { id = skpd.IdSptpd });
            return Json(new { success = true, url = link });
        }

        private bool StpdExists(Guid id)
        {
            return _context.Stpd.Any(e => e.IdStpd == id);
        }
    }
}
