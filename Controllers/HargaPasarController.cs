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
    public class HargaPasarController : Controller
    {
        private readonly DB_NewContext _context;

        public HargaPasarController(DB_NewContext context)
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

            var dB_NewContext = _context.RefHargaPasar.Include(p => p.IdCoaNavigation);

            return View(await dB_NewContext.ToListAsync());
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public IActionResult Create()
        {
            var subselect = _context.RefHargaPasar.Select(d => d.IdCoa).ToList();
            var coa = (from c in _context.Coa
                         where !subselect.Contains(c.IdCoa) && c.Parent == "41111" select c).ToList();

            ViewData["IdCoa"] = new SelectList(coa, "IdCoa", "Uraian");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdHarga,IdCoa,Harga,FlagAktif")] RefHargaPasar refHargaPasar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(refHargaPasar);
                await _context.SaveChangesAsync();
                TempData["status"] = "create";
                string link = Url.Action("Index");
                return Json(new { success = true, url = link });
            }
            return PartialView(refHargaPasar);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refHargaPasar = await _context.RefHargaPasar.FindAsync(id);
            if (refHargaPasar == null)
            {
                return NotFound();
            }
            ViewData["IdCoa"] = new SelectList(_context.Coa.Where(d => d.Parent == "41111"), "IdCoa", "Uraian", refHargaPasar.IdCoa);
            return PartialView(refHargaPasar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdHarga,IdCoa,Harga,FlagAktif")] RefHargaPasar refHargaPasar)
        {
            if (id != refHargaPasar.IdHarga)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var coa = _context.RefHargaPasar.Include(d => d.IdCoaNavigation).AsNoTracking().FirstOrDefault(d => d.IdCoa == refHargaPasar.IdCoa);
                    if(coa != null && coa.IdHarga != id)
                    {
                        TempData["status"] = "HargaDuplicate";
                        TempData["uraian"] = coa.IdCoaNavigation.Uraian;
                        string url = Url.Action("Index");
                        return Json(new { success = true, url });
                    }
                    _context.Update(refHargaPasar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RefHargaPasarExists(refHargaPasar.IdHarga))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "edit";
                string link = Url.Action("Index", "HargaPasar");
                return Json(new { success = true, url = link });
            }
            ViewData["IdCoa"] = new SelectList(_context.Coa.Where(d => d.Parent == "41111"), "IdCoa", "Uraian", refHargaPasar.IdCoa);
            return PartialView(refHargaPasar);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refHargaPasar = await _context.RefHargaPasar.Include(d => d.IdCoaNavigation)
                .FirstOrDefaultAsync(m => m.IdHarga == id);
            if (refHargaPasar == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var refHargaPasar = await _context.RefHargaPasar.FindAsync(id);
            try
            {
                _context.RefHargaPasar.Remove(refHargaPasar);
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

        private bool RefHargaPasarExists(int id)
        {
            return _context.RefHargaPasar.Any(e => e.IdHarga == id);
        }
    }
}
