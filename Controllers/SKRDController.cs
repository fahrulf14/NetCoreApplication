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
    public class SKRDController : Controller
    {
        private readonly DB_NewContext _context;

        public SKRDController(DB_NewContext context)
        {
            _context = context;
        }

        [Route("SKRD/{id}")]
        [Auth(new string[] { "Developers", "Retribusi" })]
        public async Task<IActionResult> Index(string id)
        {
            var dB_NewContext = _context.Skrd
                .Include(s => s.IdCoaNavigation)
                .Include(s => s.IdSubjekNavigation)
                .Include(s => s.Ssrd);

            var coa = _context.Coa.FirstOrDefault(d => d.IdCoa == id);

            //Link
            ViewBag.Title = "SKRD " + coa.Uraian;
            ViewBag.S1 = coa.Uraian;
            ViewBag.L1 = Url.Content("/SKRD/" + id);
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            ViewBag.IdCoa = id;

            return View(await dB_NewContext.ToListAsync());
        }

        [Auth(new string[] { "Developers", "Retribusi" })]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skrd = await _context.Skrd
                .Include(s => s.IdCoaNavigation)
                .Include(s => s.IdSubjekNavigation)
                .FirstOrDefaultAsync(m => m.IdSkrd == id);
            if (skrd == null)
            {
                return NotFound();
            }

            ViewBag.Detail = _context.SkrdDt.Include(d => d.IdTarifNavigation).Where(d => d.IdSkrd == id).ToList();

            //Link
            ViewBag.L = Url.Content("/SKRD/" + skrd.IdCoa);
            ViewBag.L1 = Url.Action("Details", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            //Status
            if (skrd.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (skrd.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            else if (skrd.Keterangan == 2)
            {
                ViewBag.State = "kt-ribbon--warning";
                ViewBag.Status = "Kurang Bayar";
            }
            else if (skrd.Keterangan == 3)
            {
                ViewBag.State = "kt-ribbon--primary";
                ViewBag.Status = "Lebih Bayar";
            }

            return View(skrd);
        }

        [Route("SKRD/Create/{id}/{coa}")]
        [Auth(new string[] { "Developers", "Retribusi" })]
        public IActionResult Create(Guid id, string coa)
        {

            ViewBag.IdSubjek = id;
            ViewBag.IdCoa = coa;
            return View();
        }

        [HttpPost]
        [Route("SKRD/Create/{id}/{coa}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSkrd,IdSubjek,IdCoa,TanggalSkrd,Tahun,Nomor,NoSkrd,MasaRetribusi1,MasaRetribusi2,JatuhTempo,Bunga,Kenaikan,Terhutang,Kredit,FlagValidasi,TanggalValidasi,Keterangan,Sk,Eu,Ed")] Skrd skrd,Guid id, string coa)
        {
            if (ModelState.IsValid)
            {
                skrd.IdSkrd = Guid.NewGuid();
                skrd.IdSubjek  = id;
                skrd.IdCoa = coa;
                skrd.Tahun = skrd.TanggalSkrd.Value.Year.ToString();
                skrd.JatuhTempo = skrd.TanggalSkrd.Value.AddDays(30);
                skrd.Keterangan = 0;
                var noSk = _context.Skrd.Where(d => d.TanggalSkrd.Value.Year == skrd.TanggalSkrd.Value.Year).Select(e => e.Nomor).Max() ?? 0;
                skrd.Nomor = noSk + 1;
                skrd.NoSkrd = string.Format("{0:000000}", skrd.Nomor) + "/SKRD/" + string.Format("{0:MM/yyyy}", skrd.TanggalSkrd);
                skrd.Eu = HttpContext.Session.GetString("User");
                skrd.Ed = DateTime.Now;
                _context.Add(skrd);
                await _context.SaveChangesAsync();
                return RedirectToAction("Data","SKRD", new { id = skrd.IdSkrd });
            }
            return View(skrd);
        }

        [Auth(new string[] { "Developers", "Retribusi" })]
        public async Task<IActionResult> Data(Guid id)
        {
            var skrd = await _context.Skrd
                .Include(s => s.IdCoaNavigation)
                .Include(s => s.IdSubjekNavigation)
                .FirstOrDefaultAsync(s => s.IdSkrd == id);

            //Link
            ViewBag.L = Url.Content("/SKRD/" + skrd.IdCoa);
            ViewBag.L1 = Url.Action("Data", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            ViewBag.Detail = _context.SkrdDt.Include(d => d.IdTarifNavigation).Where(d => d.IdSkrd == id).ToList();

            //Status
            if (skrd.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (skrd.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            else if (skrd.Keterangan == 2)
            {
                ViewBag.State = "kt-ribbon--warning";
                ViewBag.Status = "Kurang Bayar";
            }
            else if (skrd.Keterangan == 3)
            {
                ViewBag.State = "kt-ribbon--primary";
                ViewBag.Status = "Lebih Bayar";
            }
            return View(skrd);
        }

        [Auth(new string[] { "Developers", "Retribusi" })]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skrd = await _context.Skrd.FindAsync(id);
            if (skrd == null)
            {
                return NotFound();
            }
            ViewData["IdCoa"] = new SelectList(_context.Coa, "IdCoa", "IdCoa", skrd.IdCoa);
            ViewData["IdSubjek"] = new SelectList(_context.DataSubjek, "IdSubjek", "IdSubjek", skrd.IdSubjek);
            return View(skrd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdSkrd,IdSubjek,IdCoa,TanggalSkrd,Tahun,Nomor,NoSkrd,MasaRetribusi1,MasaRetribusi2,JatuhTempo,Bunga,Kenaikan,Terhutang,Kredit,FlagValidasi,TanggalValidasi,Keterangan,Sk,Eu,Ed")] Skrd skrd)
        {
            if (id != skrd.IdSkrd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var old = _context.Skrd.Find(id);
                try
                {
                    old.TanggalSkrd = skrd.TanggalSkrd;
                    old.Tahun = old.TanggalSkrd.Value.Year.ToString();
                    old.MasaRetribusi1 = skrd.MasaRetribusi1;
                    old.MasaRetribusi2 = skrd.MasaRetribusi2;
                    old.Eu = HttpContext.Session.GetString("User");
                    old.Ed = DateTime.Now;
                    _context.Skrd.Update(old);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SkrdExists(skrd.IdSkrd))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "edit";
                return Redirect("/SKRD/" + old.IdCoa);
            }
            return View(skrd);
        }

        [Auth(new string[] { "Developers", "Validasi" })]
        public async Task<IActionResult> Validasi(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skrd = await _context.Skrd
                .Include(s => s.IdCoaNavigation)
                .Include(s => s.IdSubjekNavigation)
                .Include(s => s.Ssrd)
                .FirstOrDefaultAsync(m => m.IdSkrd == id);

            if (skrd.Ssrd.FirstOrDefault() != null)
            {
                TempData["status"] = "sudahvalid";
                return Redirect("/SKRD/" + skrd.IdCoa);
            }

            if (skrd == null)
            {
                return NotFound();
            }

            ViewBag.Detail = _context.SkrdDt.Include(d => d.IdTarifNavigation).Where(d => d.IdSkrd == id).ToList();

            //Link
            ViewBag.L = Url.Content("/SKRD/" + skrd.IdCoa);
            ViewBag.L1 = Url.Action("Validasi", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            //Status
            if (skrd.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (skrd.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            else if (skrd.Keterangan == 2)
            {
                ViewBag.State = "kt-ribbon--warning";
                ViewBag.Status = "Kurang Bayar";
            }
            else if (skrd.Keterangan == 3)
            {
                ViewBag.State = "kt-ribbon--primary";
                ViewBag.Status = "Lebih Bayar";
            }

            return View(skrd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Validasi(Guid id, Skrd skrd)
        {
            if (id != skrd.IdSkrd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var old = _context.Skrd.FirstOrDefault(s => s.IdSkrd == id);
                if (old.FlagValidasi == false)
                {
                    old.FlagValidasi = true;
                    old.TanggalValidasi = DateTime.Now;
                    TempData["status"] = "valid";
                }
                else
                {
                    old.FlagValidasi = false;
                    old.TanggalValidasi = null;
                    TempData["status"] = "validbatal";
                }
                old.Eu = HttpContext.Session.GetString("User");
                old.Ed = DateTime.Now;
                _context.Skrd.Update(old);
                await _context.SaveChangesAsync();
                return Redirect("/SKRD/" + old.IdCoa);
            }
            return View(skrd);
        }

        [Auth(new string[] { "Developers", "Retribusi" })]
        public IActionResult Cetak(Guid id)
        {
            return RedirectToAction("SKRD", "Cetak", new { id });
        }

        [Auth(new string[] { "Developers", "Retribusi" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skrd = await _context.Skrd
                .Include(s => s.IdCoaNavigation)
                .Include(s => s.IdSubjekNavigation)
                .FirstOrDefaultAsync(m => m.IdSkrd == id);
            if (skrd == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var detail = _context.SkrdDt.Where(d => d.IdSkrd == id);
            var skrd = await _context.Skrd.FindAsync(id);
            try
            {
                _context.SkrdDt.RemoveRange(detail);
                _context.Skrd.Remove(skrd);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["status"] = "deletefailed";
                string url = Url.Content("/SKRD/" + skrd.IdCoa);
                return Json(new { success = true, url });
            }
            
            TempData["status"] = "delete";
            string link = Url.Content("/SKRD/" + skrd.IdCoa);
            return Json(new { success = true, url = link });
        }

        private bool SkrdExists(Guid id)
        {
            return _context.Skrd.Any(e => e.IdSkrd == id);
        }
    }
}
