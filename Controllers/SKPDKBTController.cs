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
    public class SKPDKBTController : Controller
    {
        private readonly DB_NewContext _context;

        public SKPDKBTController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "Laporan" })]
        public IActionResult Index()
        {
            return RedirectToAction("SKPDKBT", "Laporan");
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skpdkbt = await _context.Skpdkbt
                .Include(s => s.IdSptpdNavigation)
                .ThenInclude(s => s.IdCoaNavigation)
                .FirstOrDefaultAsync(m => m.IdSkpdkbt == id);
            if (skpdkbt == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("SKPDKBT", "Penetapan", new { id = skpdkbt.IdSptpd });
            ViewBag.L1 = Url.Action("Details", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            //Status
            if (skpdkbt.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (skpdkbt.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            else if (skpdkbt.Keterangan == 2)
            {
                ViewBag.State = "kt-ribbon--warning";
                ViewBag.Status = "Kurang Bayar";
            }
            else if (skpdkbt.Keterangan == 3)
            {
                ViewBag.State = "kt-ribbon--primary";
                ViewBag.Status = "Lebih Bayar";
            }

            return View(skpdkbt);
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public IActionResult Create(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("SKPDKBT", "Penetapan", new { id });
            ViewBag.L1 = Url.Action("Create", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            ViewBag.IdSptpd = id;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, [Bind("IdSkpdkbt,IdSptpd,Nomor,NoSkpdkbt,Tanggal,JatuhTempo,PokokPajak,KompKelebihan,Lainnya,Tambahan,KurangBayar,Bunga,Kenaikan,Terhutang,KreditPajak,TanggalDiserahkan,FlagValidasi,TanggalValidasi,Keterangan,Sk,Eu,Ed")] Skpdkbt skpd)
        {
            if (ModelState.IsValid)
            {
                var sptpd = _context.Sptpd.Include(d => d.IdCoaNavigation).FirstOrDefault(d => d.IdSptpd == id);
                var coa = _context.Coa.AsNoTracking().FirstOrDefault(d => d.IdCoa == sptpd.IdCoaNavigation.Parent);
                skpd.IdSkpdkbt = Guid.NewGuid();
                skpd.IdSptpd = id;
                var noSk = _context.Skpdkbt.Where(d => d.Tanggal.Value.Year == skpd.Tanggal.Value.Year).Select(e => e.Nomor).Max() ?? 0;
                skpd.Nomor = noSk + 1;
                skpd.NoSkpdkbt = string.Format("{0:000000}", skpd.Nomor) + "/SKPDKBT/" + string.Format("{0:MM/yyyy}", skpd.Tanggal);
                skpd.JatuhTempo = skpd.Tanggal.Value.AddDays(30);
                skpd.KompKelebihan ??= 0;
                skpd.Lainnya ??= 0;
                skpd.Tambahan ??= 0;
                skpd.PokokPajak = sptpd.Terhutang - sptpd.KreditPajak;
                skpd.KurangBayar = skpd.PokokPajak - skpd.KompKelebihan - skpd.Lainnya + skpd.Tambahan;

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
                    Kenaikan = (skpd.KurangBayar + skpd.Tambahan ?? 0) * (25 / 100);
                }
                else
                {
                    Kenaikan = (skpd.Tambahan ?? 0) * (25 / 100);
                }
                skpd.Bunga = (int)Bunga;
                skpd.Kenaikan = Kenaikan;
                skpd.Terhutang = skpd.KurangBayar + skpd.Bunga + skpd.Kenaikan;
                skpd.KreditPajak = 0;
                skpd.Keterangan = 0;
                skpd.Eu = HttpContext.Session.GetString("User");
                skpd.Ed = DateTime.Now;
                _context.Skpdkbt.Add(skpd);

                sptpd.Sk = "SKPDKBT";
                //sptpd.Eu = Session["UserID"] as string;
                sptpd.Ed = DateTime.Now;
                _context.Sptpd.Update(sptpd);
                await _context.SaveChangesAsync();

                TempData["status"] = "create";
                return RedirectToAction("Details", new { id = skpd.IdSkpdkbt });
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

            var skpdkbt = await _context.Skpdkbt.FindAsync(id);
            if (skpdkbt == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("SKPDKBT", "Penetapan", new { id = skpdkbt.IdSptpd });
            ViewBag.L1 = Url.Action("Edit", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View(skpdkbt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdSkpdkbt,IdSptpd,Nomor,NoSkpdkbt,Tanggal,JatuhTempo,PokokPajak,KompKelebihan,Lainnya,Tambahan,KurangBayar,Bunga,Kenaikan,Terhutang,KreditPajak,TanggalDiserahkan,FlagValidasi,TanggalValidasi,Keterangan,Sk,Eu,Ed")] Skpdkbt skpdkbt)
        {
            if (id != skpdkbt.IdSkpdkbt)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var skpd = _context.Skpdkbt.Include(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdSkpdkbt == id);
                try
                {
                    skpd.JatuhTempo = skpdkbt.Tanggal.Value.AddDays(30);
                    skpd.KompKelebihan = skpdkbt.KompKelebihan ?? 0;
                    skpd.Lainnya = skpdkbt.Lainnya ?? 0;
                    skpd.Tambahan = skpdkbt.Tambahan ?? 0;
                    skpd.PokokPajak = skpd.IdSptpdNavigation.Terhutang - skpd.IdSptpdNavigation.KreditPajak;
                    skpd.KurangBayar = skpd.PokokPajak - skpd.KompKelebihan - skpd.Lainnya + skpd.Tambahan;

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
                        Kenaikan = (skpd.KurangBayar + skpd.Tambahan ?? 0) * (25 / 100);
                    }
                    else
                    {
                        Kenaikan = (skpd.Tambahan ?? 0) * (25 / 100);
                    }
                    skpd.Bunga = (int)Bunga;
                    skpd.Kenaikan = Kenaikan;
                    skpd.Terhutang = skpd.KurangBayar + skpd.Bunga + skpd.Kenaikan;
                    skpd.KreditPajak = 0;
                    skpd.Keterangan = 0;
                    skpd.Eu = HttpContext.Session.GetString("User");
                    skpd.Ed = DateTime.Now;
                    _context.Skpdkbt.Update(skpd);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SkpdkbtExists(skpdkbt.IdSkpdkbt))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "edit";
                return RedirectToAction("SKPDKBT", "Penetapan", new { id = skpd.IdSptpd });
            }
            return View(skpdkbt);
        }

        [Auth(new string[] { "Developers", "Validasi" })]
        public async Task<IActionResult> Validasi(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skpdkbt = await _context.Skpdkbt
                .Include(s => s.IdSptpdNavigation)
                .ThenInclude(s => s.IdCoaNavigation)
                .FirstOrDefaultAsync(m => m.IdSkpdkbt == id);
            if (skpdkbt == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("SKPDKBT", "Penetapan", new { id = skpdkbt.IdSptpd });
            ViewBag.L1 = Url.Action("Validasi", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            //Status
            if (skpdkbt.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (skpdkbt.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            else if (skpdkbt.Keterangan == 2)
            {
                ViewBag.State = "kt-ribbon--warning";
                ViewBag.Status = "Kurang Bayar";
            }
            else if (skpdkbt.Keterangan == 3)
            {
                ViewBag.State = "kt-ribbon--primary";
                ViewBag.Status = "Lebih Bayar";
            }

            return View(skpdkbt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Validasi(Guid id, Skpdkbt skpdkbt)
        {
            if (id != skpdkbt.IdSkpdkbt)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var skpd = _context.Skpdkbt.Include(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdSkpdkbt == id);
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
                _context.Skpdkbt.Update(skpd);

                await _context.SaveChangesAsync();
                return RedirectToAction("SKPDKBT", "Penetapan", new { id = skpd.IdSptpd });
            }
            return View(skpdkbt);
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public IActionResult Cetak(Guid id)
        {
            return RedirectToAction("SKPDKBT", "Cetak", new { id });
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skpdkbt = await _context.Skpdkbt
                .Include(s => s.IdSptpdNavigation)
                .FirstOrDefaultAsync(m => m.IdSkpdkbt == id);
            if (skpdkbt == null)
            {
                return NotFound();
            }

            if (skpdkbt.FlagValidasi)
            {
                return PartialView("_deletevalid");
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var skpdkbt = await _context.Skpdkbt.FindAsync(id);
            try
            {
                _context.Skpdkbt.Remove(skpdkbt);

                var sptpd = await _context.Sptpd.FirstOrDefaultAsync(d => d.IdSptpd == skpdkbt.IdSptpd);
                sptpd.Sk = null;
                sptpd.Eu = HttpContext.Session.GetString("User");
                sptpd.Ed = DateTime.Now;
                _context.Sptpd.Update(sptpd);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["status"] = "deletefailed";
                string url = Url.Action("SKPDKBT", "Penetapan", new { id = skpdkbt.IdSptpd });
                return Json(new { success = true, url });
            }

            TempData["status"] = "delete";
            string link = Url.Action("SKPDKBT", "Penetapan", new { id = skpdkbt.IdSptpd });
            return Json(new { success = true, url = link });
        }

        private bool SkpdkbtExists(Guid id)
        {
            return _context.Skpdkbt.Any(e => e.IdSkpdkbt == id);
        }
    }
}
