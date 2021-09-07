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
    public class SKPDKBController : Controller
    {
        private readonly DB_NewContext _context;

        public SKPDKBController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "Laporan" })]
        public IActionResult Index()
        {
            return RedirectToAction("SKPDKB", "Laporan");
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skpdkb = await _context.Skpdkb
                .Include(s => s.IdSptpdNavigation)
                .ThenInclude(s => s.IdCoaNavigation)
                .FirstOrDefaultAsync(m => m.IdSkpdkb == id);
            if (skpdkb == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("SKPDKB", "Penetapan", new { id = skpdkb.IdSptpd });
            ViewBag.L1 = Url.Action("Details", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            //Status
            if (skpdkb.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (skpdkb.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            else if (skpdkb.Keterangan == 2)
            {
                ViewBag.State = "kt-ribbon--warning";
                ViewBag.Status = "Kurang Bayar";
            }
            else if (skpdkb.Keterangan == 3)
            {
                ViewBag.State = "kt-ribbon--primary";
                ViewBag.Status = "Lebih Bayar";
            }

            return View(skpdkb);
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public IActionResult Create(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("SKPDKB", "Penetapan", new { id });
            ViewBag.L1 = Url.Action("Create", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            ViewBag.IdSptpd = id;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, [Bind("IdSkpdkb,IdSptpd,Nomor,NoSkpdkb,Tanggal,JatuhTempo,PokokPajak,KompKelebihan,Lainnya,KurangBayar,Bunga,Kenaikan,Terhutang,KreditPajak,TanggalDiserahkan,FlagValidasi,TanggalValidasi,Keterangan,Sk,Eu,Ed")] Skpdkb skpd)
        {
            if (ModelState.IsValid)
            {
                var sptpd = _context.Sptpd.Include(d => d.IdCoaNavigation).FirstOrDefault(d => d.IdSptpd == id);
                var coa = _context.Coa.AsNoTracking().FirstOrDefault(d => d.IdCoa == sptpd.IdCoaNavigation.Parent);
                skpd.IdSkpdkb = Guid.NewGuid();
                skpd.IdSptpd = id;
                var noSk = _context.Skpdkb.Where(d => d.Tanggal.Value.Year == skpd.Tanggal.Value.Year).Select(e => e.Nomor).Max() ?? 0;
                skpd.Nomor = noSk + 1;
                skpd.NoSkpdkb = string.Format("{0:000000}", skpd.Nomor) + "/SKPDKB/" + string.Format("{0:MM/yyyy}", skpd.Tanggal);
                skpd.JatuhTempo = skpd.Tanggal.Value.AddDays(30);
                skpd.KompKelebihan ??= 0;
                skpd.Lainnya ??= 0;
                skpd.PokokPajak = sptpd.Terhutang - sptpd.KreditPajak;
                skpd.KurangBayar = skpd.PokokPajak - skpd.KompKelebihan - skpd.Lainnya;

                var JHari = DateTime.Now.Subtract(sptpd.MasaPajak1 ?? DateTime.Now).Days;
                var MaxBulan = 24;
                var JBulan = Convert.ToDecimal(JHari / 30);
                if (JBulan > MaxBulan)
                {
                    JBulan = MaxBulan;
                }
                var Bunga = skpd.KurangBayar * 2 / 100 * Math.Floor(JBulan);
                decimal Kenaikan;
                if (sptpd.Jabatan)
                {
                    Kenaikan = skpd.KurangBayar ?? 0 * (25 / 100);
                }
                else
                {
                    Kenaikan = 0;
                }
                skpd.Bunga = (int)Bunga;
                skpd.Kenaikan = Kenaikan;
                skpd.Terhutang = skpd.KurangBayar + skpd.Bunga + skpd.Kenaikan;
                skpd.KreditPajak = 0;
                skpd.Keterangan = 0;
                skpd.Eu = HttpContext.Session.GetString("User");
                skpd.Ed = DateTime.Now;
                _context.Skpdkb.Add(skpd);

                sptpd.Sk = "SKPDKB";
                //sptpd.Eu = Session["UserID"] as string;
                sptpd.Ed = DateTime.Now;
                _context.Sptpd.Update(sptpd);
                await _context.SaveChangesAsync();

                TempData["status"] = "create";
                return RedirectToAction("Details", new { id = skpd.IdSkpdkb });
            }
            return View(skpd);
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skpdkb = await _context.Skpdkb.FindAsync(id);
            if (skpdkb == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("SKPDKB", "Penetapan", new { id = skpdkb.IdSptpd });
            ViewBag.L1 = Url.Action("Edit", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View(skpdkb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdSkpdkb,IdSptpd,Nomor,NoSkpdkb,Tanggal,JatuhTempo,PokokPajak,KompKelebihan,Lainnya,KurangBayar,Bunga,Kenaikan,Terhutang,KreditPajak,TanggalDiserahkan,FlagValidasi,TanggalValidasi,Keterangan,Sk,Eu,Ed")] Skpdkb skpdkb)
        {
            if (id != skpdkb.IdSkpdkb)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var skpd = _context.Skpdkb.Include(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdSkpdkb == id);
                try
                {
                    skpd.JatuhTempo = skpdkb.Tanggal.Value.AddDays(30);
                    skpd.KompKelebihan = skpdkb.KompKelebihan ?? 0;
                    skpd.Lainnya = skpdkb.Lainnya ?? 0;
                    skpd.PokokPajak = skpd.IdSptpdNavigation.Terhutang - skpd.IdSptpdNavigation.KreditPajak;
                    skpd.KurangBayar = skpd.PokokPajak - skpd.KompKelebihan - skpd.Lainnya;

                    var JHari = DateTime.Now.Subtract(skpd.IdSptpdNavigation.MasaPajak1 ?? DateTime.Now).Days;
                    var MaxBulan = 24;
                    var JBulan = Convert.ToDecimal(JHari / 30);
                    if (JBulan > MaxBulan)
                    {
                        JBulan = MaxBulan;
                    }
                    var Bunga = skpd.KurangBayar * 2 / 100 * Math.Floor(JBulan);
                    decimal Kenaikan;
                    if (skpd.IdSptpdNavigation.Jabatan)
                    {
                        Kenaikan = skpd.KurangBayar ?? 0 * (25 / 100);
                    }
                    else
                    {
                        Kenaikan = 0;
                    }
                    skpd.Bunga = (int)Bunga;
                    skpd.Kenaikan = Kenaikan;
                    skpd.Terhutang = skpd.KurangBayar + skpd.Bunga + skpd.Kenaikan;
                    skpd.KreditPajak = 0;
                    skpd.Keterangan = 0;
                    skpd.Eu = HttpContext.Session.GetString("User");
                    skpd.Ed = DateTime.Now;
                    _context.Skpdkb.Update(skpd);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SkpdkbExists(skpdkb.IdSkpdkb))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "edit";
                return RedirectToAction("SKPDKB", "Penetapan", new { id = skpd.IdSptpd });
            }
            return View(skpdkb);
        }

        [Auth(new string[] { "Developers", "Validasi" })]
        public async Task<IActionResult> Validasi(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skpdkb = await _context.Skpdkb
                .Include(s => s.IdSptpdNavigation)
                .ThenInclude(s => s.IdCoaNavigation)
                .FirstOrDefaultAsync(m => m.IdSkpdkb == id);
            if (skpdkb == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("SKPDKB", "Penetapan", new { id = skpdkb.IdSptpd });
            ViewBag.L1 = Url.Action("Validasi", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            //Status
            if (skpdkb.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (skpdkb.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            else if (skpdkb.Keterangan == 2)
            {
                ViewBag.State = "kt-ribbon--warning";
                ViewBag.Status = "Kurang Bayar";
            }
            else if (skpdkb.Keterangan == 3)
            {
                ViewBag.State = "kt-ribbon--primary";
                ViewBag.Status = "Lebih Bayar";
            }

            return View(skpdkb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Validasi(Guid id, Skpdkb skpdkb)
        {
            if (id != skpdkb.IdSkpdkb)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var skpd = _context.Skpdkb.Include(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdSkpdkb == id);
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
                _context.Skpdkb.Update(skpd);

                await _context.SaveChangesAsync();
                return RedirectToAction("SKPDKB", "Penetapan", new { id = skpd.IdSptpd });
            }
            return View(skpdkb);
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public IActionResult Cetak(Guid id)
        {
            return RedirectToAction("SKPDKB", "Cetak", new { id });
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skpdkb = await _context.Skpdkb
                .Include(s => s.IdSptpdNavigation)
                .FirstOrDefaultAsync(m => m.IdSkpdkb == id);
            if (skpdkb == null)
            {
                return NotFound();
            }

            if (skpdkb.FlagValidasi)
            {
                return PartialView("_deletevalid");
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var skpdkb = await _context.Skpdkb.FindAsync(id);
            try
            {
                _context.Skpdkb.Remove(skpdkb);

                var sptpd = await _context.Sptpd.FirstOrDefaultAsync(d => d.IdSptpd == skpdkb.IdSptpd);
                sptpd.Sk = null;
                sptpd.Eu = HttpContext.Session.GetString("User");
                sptpd.Ed = DateTime.Now;
                _context.Sptpd.Update(sptpd);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["status"] = "deletefailed";
                string url = Url.Action("SKPDKB", "Penetapan", new { id = skpdkb.IdSptpd });
                return Json(new { success = true, url });
            }
            
            TempData["status"] = "delete";
            string link = Url.Action("SKPDKB", "Penetapan", new { id = skpdkb.IdSptpd });
            return Json(new { success = true, url = link });
        }

        private bool SkpdkbExists(Guid id)
        {
            return _context.Skpdkb.Any(e => e.IdSkpdkb == id);
        }
    }
}
