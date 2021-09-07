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
    public class FiskalController : Controller
    {
        private readonly DB_NewContext _context;

        public FiskalController(DB_NewContext context)
        {
            _context = context;
        }

        [Route("Fiskal/{id}")]
        [Auth(new string[] { "Developers", "Fiskal" })]
        public async Task<IActionResult> Index(Guid? id)
        {
            if(id == null)
            {
                return RedirectToAction("Fiskal", "Laporan");
            }

            //Link
            ViewBag.L = Url.Content("/Fiskal/" + id);
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            ViewBag.IdSubjek = id;
            var fiskal = _context.Fiskal.Where(d => d.IdSubjek == id);
            return View(await fiskal.ToListAsync());
        }

        [Auth(new string[] { "Developers", "Fiskal" })]
        public IActionResult Create(Guid? id)
        {
            var spt = _context.Sptpd.Where(d => d.IdSubjek == id && d.FlagValidasi && d.Keterangan == 0 && d.Sk == null).Count();
            var skpd = _context.Skpd.Where(d => d.IdSptpdNavigation.IdSubjek == id && d.FlagValidasi && d.Keterangan == 0 && d.Sk == null).Count();
            var skpdkb = _context.Skpdkb.Where(d => d.IdSptpdNavigation.IdSubjek == id && d.FlagValidasi && d.Keterangan == 0 && d.Sk == null).Count();
            var skpdkbt = _context.Skpdkbt.Where(d => d.IdSptpdNavigation.IdSubjek == id && d.FlagValidasi && d.Keterangan == 0 && d.Sk == null).Count();
            var stpd = _context.Stpd.Where(d => d.IdSkpdNavigation.IdSptpdNavigation.IdSubjek == id && d.FlagValidasi && d.Keterangan == 0 && d.Sk == null).Count();
            var sppt = _context.Sppt.Where(d => d.IdSpopNavigation.IdSubjek == id && d.FlagValidasi && d.Keterangan == 0).Count();

            if (spt != 0 || skpd != 0 || skpdkb != 0 || skpdkbt != 0 || stpd != 0 || sppt != 0)
            {
                if (spt != 0) { TempData["surat"] = "SPTPD"; }
                else if (skpd != 0) { TempData["surat"] = "SKPD"; }
                else if (skpdkb != 0) { TempData["surat"] = "SKPDKB"; }
                else if (skpdkbt != 0) { TempData["surat"] = "SKPDKBT"; }
                else if (stpd != 0) { TempData["surat"] = "STPD"; }
                else if (sppt != 0) { TempData["surat"] = "PBB"; }
                TempData["status"] = "fiskal";
                return Redirect("/Fiskal/" + id);
            }

            if (id == null)
            {
                return NotFound();
            }
            ViewBag.IdSubjek = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, [Bind("IdFiskal,IdSubjek,Nomor,NoFiskal,Tanggal,Berakhir,Tujuan")] Fiskal fiskal)
        {
            if (ModelState.IsValid)
            {
                fiskal.IdFiskal = Guid.NewGuid();
                fiskal.IdSubjek = id;
                fiskal.Tanggal = DateTime.Now;
                var noSk = _context.Fiskal.Where(d => d.Tanggal.Value.Year == fiskal.Tanggal.Value.Year).Select(e => e.Nomor).Max() ?? 0;
                fiskal.Nomor = noSk + 1;
                fiskal.NoFiskal = string.Format("{0:000000}", fiskal.Nomor) + "/FISKAL/" + string.Format("{0:MM/yyyy}", fiskal.Tanggal);
                _context.Add(fiskal);
                await _context.SaveChangesAsync();
                TempData["status"] = "create";
                return Redirect("/Fiskal/" + id);
            }
            return View(fiskal);
        }

        [Auth(new string[] { "Developers", "Fiskal" })]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fiskal = await _context.Fiskal.FindAsync(id);
            if (fiskal == null)
            {
                return NotFound();
            }
            return View(fiskal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdFiskal,IdSubjek,Nomor,NoFiskal,Tanggal,Berakhir,Tujuan")] Fiskal fiskal)
        {
            if (id != fiskal.IdFiskal)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var old = _context.Fiskal.Find(id);
                try
                {
                    old.Tanggal = fiskal.Tanggal;
                    old.Berakhir = fiskal.Berakhir;
                    old.Tujuan = fiskal.Tujuan;
                    _context.Fiskal.Update(old);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FiskalExists(fiskal.IdFiskal))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("/Fiskal/" + old.IdSubjek);
            }
            return View(fiskal);
        }

        [Auth(new string[] { "Developers", "Fiskal" })]
        public IActionResult Cetak(Guid id)
        {
            return RedirectToAction("Fiskal", "Cetak", new { id });
        }

        [Auth(new string[] { "Developers", "Fiskal" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fiskal = await _context.Fiskal
                .Include(f => f.IdSubjekNavigation)
                .FirstOrDefaultAsync(m => m.IdFiskal == id);
            if (fiskal == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var fiskal = await _context.Fiskal.FindAsync(id);
            try
            {
                _context.Fiskal.Remove(fiskal);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["status"] = "deletefailed";
                string url = Url.Content("/Fiskal/" + fiskal.IdSubjek);
                return Json(new { success = true, url });
            }
            
            TempData["status"] = "delete";
            string link = Url.Content("/Fiskal/" + fiskal.IdSubjek);
            return Json(new { success = true, url = link });
        }

        private bool FiskalExists(Guid id)
        {
            return _context.Fiskal.Any(e => e.IdFiskal == id);
        }
    }
}
