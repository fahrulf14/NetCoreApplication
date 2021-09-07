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
    public class SSRDRDetailController : Controller
    {
        private readonly DB_NewContext _context;

        public SSRDRDetailController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "Retribusi" })]
        [AjaxOnly]
        public async Task<IActionResult> Create(Guid id)
        {
            var ssrdr = await _context.Ssrdr.FirstOrDefaultAsync(d => d.IdSsrdr == id);
            ViewData["IdTarif"] = new SelectList(_context.TarifRetribusi.Where(d => d.IdCoa == ssrdr.IdCoa), "IdTarif", "Objek");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSsrdrDt,IdSsrdr,IdTarif,Nomor1,Nomor2,Jumlah,Total")] SsrdrDt ssrdrDt, Guid id)
        {
            
            if (ModelState.IsValid)
            {
                var tarif = _context.TarifRetribusi.Find(ssrdrDt.IdTarif);
                ssrdrDt.IdSsrdrDt = Guid.NewGuid();
                ssrdrDt.IdSsrdr = id;
                ssrdrDt.Total = ssrdrDt.Jumlah * tarif.Tarif;
                _context.Add(ssrdrDt);
                await _context.SaveChangesAsync();

                var ssrdr = _context.Ssrdr.Find(id);
                ssrdr.Total = _context.SsrdrDt.Where(d => d.IdSsrdr == id).Sum(d => d.Total);
                _context.Ssrdr.Update(ssrdr);
                await _context.SaveChangesAsync();

                TempData["status"] = "create";
                string link = Url.Action("Data", "SSRDR", new { id });
                return Json(new { success = true, url = link });
            }

            return PartialView(ssrdrDt);
        }

        [Auth(new string[] { "Developers", "Retribusi" })]
        [AjaxOnly]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ssrdrDt = await _context.SsrdrDt.Include(d => d.IdSsrdrNavigation).FirstOrDefaultAsync(d => d.IdSsrdrDt == id);
            if (ssrdrDt == null)
            {
                return NotFound();
            }

            ViewData["IdTarif"] = new SelectList(_context.TarifRetribusi.Where(d => d.IdCoa == ssrdrDt.IdSsrdrNavigation.IdCoa), "IdTarif", "Objek");
            
            var tarif = _context.TarifRetribusi.FirstOrDefault(d => d.IdTarif == ssrdrDt.IdTarif);
            ViewBag.Satuan = (tarif.Var1 == 1 ? "" + tarif.Satuan1 : tarif.Var1 + "/" + tarif.Satuan1);

            return PartialView(ssrdrDt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("IdSsrdrDt,IdSsrdr,IdTarif,Nomor1,Nomor2,Jumlah,Total")] SsrdrDt ssrdrDt, Guid id)
        {
            if (id != ssrdrDt.IdSsrdrDt)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var detail = _context.SsrdrDt.FirstOrDefault(d => d.IdSsrdrDt == id);
                var tarif = _context.TarifRetribusi.Find(ssrdrDt.IdTarif);
                var ssrdr = _context.Ssrdr.Find(detail.IdSsrdr);
                try
                {
                    detail.IdTarif = ssrdrDt.IdTarif;
                    detail.Jumlah = ssrdrDt.Jumlah;
                    detail.Total = ssrdrDt.Jumlah * tarif.Tarif;
                    _context.SsrdrDt.Update(detail);
                    await _context.SaveChangesAsync();

                    ssrdr.Total = _context.SsrdrDt.Where(d => d.IdSsrdr == detail.IdSsrdr).Sum(d => d.Total);
                    _context.Ssrdr.Update(ssrdr);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SsrdrDtExists(ssrdrDt.IdSsrdrDt))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "edit";
                string link = Url.Action("Data", "SSRDR", new { id = detail.IdSsrdr });
                return Json(new { success = true, url = link });
            }
            return PartialView(ssrdrDt);
        }

        [Auth(new string[] { "Developers", "Retribusi" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ssrdrDt = await _context.SsrdrDt
                .Include(s => s.IdSsrdrNavigation)
                .Include(s => s.IdTarifNavigation)
                .FirstOrDefaultAsync(m => m.IdSsrdrDt == id);
            if (ssrdrDt == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var ssrdrDt = await _context.SsrdrDt.FindAsync(id);
            try
            {
                _context.SsrdrDt.Remove(ssrdrDt);
                await _context.SaveChangesAsync();

                var ssrdr = _context.Ssrdr.Find(id);
                ssrdr.Total = _context.SsrdrDt.Where(d => d.IdSsrdr == ssrdrDt.IdSsrdr).Sum(d => d.Total);
                _context.Ssrdr.Update(ssrdr);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["status"] = "deletefailed";
                string url = Url.Action("Data", "SSRDR", new { id = ssrdrDt.IdSsrdr });
                return Json(new { success = true, url });
            }

            TempData["status"] = "delete";
            string link = Url.Action("Data", "SSRDR", new { id = ssrdrDt.IdSsrdr });
            return Json(new { success = true, url = link });
        }

        private bool SsrdrDtExists(Guid id)
        {
            return _context.SsrdrDt.Any(e => e.IdSsrdrDt == id);
        }
    }
}
