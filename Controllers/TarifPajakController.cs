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
    public class TarifPajakController : Controller
    {
        private readonly DB_NewContext _context;

        public TarifPajakController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "Setting" })]
        public async Task<IActionResult> Index()
        {
            //Link
            ViewBag.L = Url.Action("Index");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var dB_NewContext = _context.TarifPajak.Include(t => t.IdCoaNavigation);
            return View(await dB_NewContext.ToListAsync());
        }


        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public IActionResult Create()
        {
            ViewData["IdCoa"] = new SelectList(_context.Coa.Where(d => d.Kdcoa.Substring(0,3) == "411" && d.Tingkat == 5 && d.Status)
                .OrderBy(d => d.Kdcoa), "IdCoa", "Uraian");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTarif,IdCoa,Tarif,MulaiBerlaku,FlagAktif")] TarifPajak tarifPajak)
        {
            if (ModelState.IsValid)
            {
                tarifPajak.IdTarif = Guid.NewGuid();
                _context.Add(tarifPajak);
                await _context.SaveChangesAsync();
                TempData["status"] = "create";
                string link = Url.Action("Index");
                return Json(new { success = true, url = link });
            }
            ViewData["IdCoa"] = new SelectList(_context.Coa.Where(d => d.Kdcoa.Substring(0, 3) == "411" && d.Tingkat == 5 && d.Status), "IdCoa", "Uraian", tarifPajak.IdCoa);
            return PartialView(tarifPajak);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarifPajak = await _context.TarifPajak.FindAsync(id);
            if (tarifPajak == null)
            {
                return NotFound();
            }
            ViewData["IdCoa"] = new SelectList(_context.Coa.Where(d => d.Kdcoa.Substring(0, 3) == "411" && d.Tingkat == 5 && d.Status)
               .OrderBy(d => d.Kdcoa), "IdCoa", "Uraian", tarifPajak.IdCoa);
            return PartialView(tarifPajak);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdTarif,IdCoa,Tarif,MulaiBerlaku,FlagAktif")] TarifPajak tarifPajak)
        {
            if (id != tarifPajak.IdTarif)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tarifPajak);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TarifPajakExists(tarifPajak.IdTarif))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "edit";
                string link = Url.Action("Index");
                return Json(new { success = true, url = link });
            }
            ViewData["IdCoa"] = new SelectList(_context.Coa.Where(d => d.Kdcoa.Substring(0, 3) == "411" && d.Tingkat == 5 && d.Status), "IdCoa", "Uraian");
            return PartialView(tarifPajak);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarifPajak = await _context.TarifPajak
                .Include(t => t.IdCoaNavigation)
                .FirstOrDefaultAsync(m => m.IdTarif == id);
            if (tarifPajak == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var tarifPajak = await _context.TarifPajak.FindAsync(id);
            try
            {
                _context.TarifPajak.Remove(tarifPajak);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["status"] = "deletefailed";
                string url = Url.Action("Index");
                return Json(new { success = true, url });
            }
            
            TempData["status"] = "delete";
            string link = Url.Action("Index");
            return Json(new { success = true, url = link });
        }

        private bool TarifPajakExists(Guid id)
        {
            return _context.TarifPajak.Any(e => e.IdTarif == id);
        }
    }
}
