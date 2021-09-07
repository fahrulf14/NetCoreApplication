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
    public class SKRDDetailController : Controller
    {
        private readonly DB_NewContext _context;

        public SKRDDetailController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "Retribusi" })]
        [AjaxOnly]
        public async Task<IActionResult> Create(Guid id)
        {
            var skrd = await _context.Skrd.FirstOrDefaultAsync(d => d.IdSkrd == id);
            ViewData["IdTarif"] = new SelectList(_context.TarifRetribusi.Where(d => d.IdCoa == skrd.IdCoa), "IdTarif", "Objek");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, [Bind("IdSkrdDt,IdSkrd,IdTarif,Jumlah,Total")] SkrdDt skrdDt)
        {
            if (ModelState.IsValid)
            {
                var tarif = _context.TarifRetribusi.Find(skrdDt.IdTarif);
                skrdDt.IdSkrdDt = Guid.NewGuid();
                skrdDt.IdSkrd = id;
                skrdDt.Total = skrdDt.Jumlah * tarif.Tarif;
                _context.Add(skrdDt);
                await _context.SaveChangesAsync();

                var skrd = _context.Skrd.Find(id);
                skrd.Terhutang = _context.SkrdDt.Where(d => d.IdSkrd == id).Sum(d => d.Total);
                _context.Skrd.Update(skrd);
                await _context.SaveChangesAsync();

                TempData["status"] = "create";
                string link = Url.Action("Data", "SKRD", new { id });
                return Json(new { success = true, url = link });
            }
            return PartialView(skrdDt);
        }

        [Auth(new string[] { "Developers", "Retribusi" })]
        [AjaxOnly]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skrdDt = await _context.SkrdDt.Include(d => d.IdSkrdNavigation).FirstOrDefaultAsync(d => d.IdSkrdDt == id);
            if (skrdDt == null)
            {
                return NotFound();
            }
            ViewData["IdTarif"] = new SelectList(_context.TarifRetribusi.Where(d => d.IdCoa == skrdDt.IdSkrdNavigation.IdCoa), "IdTarif", "Objek", skrdDt.IdTarif);

            var tarif = _context.TarifRetribusi.FirstOrDefault(d => d.IdTarif == skrdDt.IdTarif);
            ViewBag.Satuan = (tarif.Var1 == 1 ? "" + tarif.Satuan1 : tarif.Var1 + "/" + tarif.Satuan1);

            return PartialView(skrdDt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdSkrdDt,IdSkrd,IdTarif,Jumlah,Total")] SkrdDt skrdDt)
        {
            if (id != skrdDt.IdSkrdDt)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var detail = _context.SkrdDt.FirstOrDefault(d => d.IdSkrdDt == id);
                var tarif = _context.TarifRetribusi.Find(skrdDt.IdTarif);
                var skrd = _context.Skrd.Find(detail.IdSkrd);
                try
                {
                    detail.IdTarif = skrdDt.IdTarif;
                    detail.Jumlah = skrdDt.Jumlah;
                    detail.Total = skrdDt.Jumlah * tarif.Tarif;
                    _context.SkrdDt.Update(detail);
                    await _context.SaveChangesAsync();

                    skrd.Terhutang = _context.SkrdDt.Where(d => d.IdSkrd == detail.IdSkrd).Sum(d => d.Total);
                    _context.Skrd.Update(skrd);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SkrdDtExists(skrdDt.IdSkrdDt))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "edit";
                string link = Url.Action("Data", "SKRD", new { id = detail.IdSkrd });
                return Json(new { success = true, url = link });
            }
            return PartialView(skrdDt);
        }

        [Auth(new string[] { "Developers", "Retribusi" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skrdDt = await _context.SkrdDt
                .Include(s => s.IdSkrdNavigation)
                .Include(s => s.IdTarifNavigation)
                .FirstOrDefaultAsync(m => m.IdSkrdDt == id);
            if (skrdDt == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var skrdDt = await _context.SkrdDt.FindAsync(id);
            try
            {
                _context.SkrdDt.Remove(skrdDt);
                await _context.SaveChangesAsync();

                var skrd = _context.Skrd.Find(skrdDt.IdSkrd);
                skrd.Terhutang = _context.SkrdDt.Where(d => d.IdSkrd == skrdDt.IdSkrd).Sum(d => d.Total);
                _context.Skrd.Update(skrd);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["status"] = "deletefailed";
                string url = Url.Action("Data", "SKRD", new { id = skrdDt.IdSkrd });
                return Json(new { success = true, url });
            }

            TempData["status"] = "delete";
            string link = Url.Action("Data", "SKRD", new { id = skrdDt.IdSkrd });
            return Json(new { success = true, url = link });
        }

        private bool SkrdDtExists(Guid id)
        {
            return _context.SkrdDt.Any(e => e.IdSkrdDt == id);
        }
    }
}
