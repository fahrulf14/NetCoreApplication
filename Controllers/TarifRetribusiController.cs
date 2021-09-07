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
    public class TarifRetribusiController : Controller
    {
        private readonly DB_NewContext _context;

        public TarifRetribusiController(DB_NewContext context)
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

            ViewBag.Jenis = _context.RefRetribusi.ToList();
            var dB_NewContext = _context.TarifRetribusi.Include(t => t.IdCoaNavigation);
            return View(await dB_NewContext.ToListAsync());
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public IActionResult Create()
        {
            List<SelectListItem> jenis = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Karcis", Value = "Karcis" },
                new SelectListItem() { Text = "SKRD", Value = "SKRD" }
            };
            ViewBag.Jenis = new SelectList(jenis, "Value", "Text");

            ViewData["IdCoa"] = new SelectList(_context.Coa.Where(d => d.Kdcoa.Substring(0, 3) == "412" && d.Tingkat == 5 && d.Status), "IdCoa", "Uraian");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTarif,IdCoa,Objek,Jenis,Var1,Satuan1,Var2,Satuan2,Tarif,FlagAktif")] TarifRetribusi tarifRetribusi)
        {
            if (ModelState.IsValid)
            {
                tarifRetribusi.IdTarif = Guid.NewGuid();
                _context.Add(tarifRetribusi);
                await _context.SaveChangesAsync();
                TempData["status"] = "create";
                string link = Url.Action("Index");
                return Json(new { success = true, url = link });
            }
            ViewData["IdCoa"] = new SelectList(_context.Coa.Where(d => d.Kdcoa.Substring(0, 3) == "412" && d.Tingkat == 5 && d.Status), "IdCoa", "Uraian", tarifRetribusi.IdCoa);
            return PartialView(tarifRetribusi);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarifRetribusi = await _context.TarifRetribusi.FindAsync(id);
            if (tarifRetribusi == null)
            {
                return NotFound();
            }
            List<SelectListItem> jenis = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Karcis", Value = "Karcis" },
                new SelectListItem() { Text = "SKRD", Value = "SKRD" }
            };
            ViewBag.Jenis = new SelectList(jenis, "Value", "Text");

            ViewData["IdCoa"] = new SelectList(_context.Coa.Where(d => d.Kdcoa.Substring(0, 3) == "412" && d.Tingkat == 5 && d.Status), "IdCoa", "Uraian");
            return PartialView(tarifRetribusi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdTarif,IdCoa,Objek,Jenis,Var1,Satuan1,Var2,Satuan2,Tarif,FlagAktif")] TarifRetribusi tarifRetribusi)
        {
            if (id != tarifRetribusi.IdTarif)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tarifRetribusi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TarifRetribusiExists(tarifRetribusi.IdTarif))
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
            ViewData["IdCoa"] = new SelectList(_context.Coa.Where(d => d.Kdcoa.Substring(0, 3) == "412" && d.Tingkat == 5 && d.Status), "IdCoa", "Uraian", tarifRetribusi.IdCoa);
            return PartialView(tarifRetribusi);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarifRetribusi = await _context.TarifRetribusi
                .Include(t => t.IdCoaNavigation)
                .FirstOrDefaultAsync(m => m.IdTarif == id);
            if (tarifRetribusi == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var tarifRetribusi = await _context.TarifRetribusi.FindAsync(id);
            try
            {
                _context.TarifRetribusi.Remove(tarifRetribusi);
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

        private bool TarifRetribusiExists(Guid id)
        {
            return _context.TarifRetribusi.Any(e => e.IdTarif == id);
        }
    }
}
