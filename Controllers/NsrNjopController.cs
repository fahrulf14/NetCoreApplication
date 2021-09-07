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
    public class NsrNjopController : Controller
    {
        private readonly DB_NewContext _context;

        public NsrNjopController(DB_NewContext context)
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

            var dB_NewContext = _context.NsrNjop.Include(n => n.IdCoaNavigation).OrderBy(d => d.IdCoa);
            return View(await dB_NewContext.ToListAsync());
        }

        [Auth(new string[] { "Developers" })]
        [AjaxOnly]
        public IActionResult Create()
        {
            ViewData["IdCoa"] = new SelectList(_context.Coa.Where(d => d.Kdcoa.Contains("41104")).Where(d => d.Tingkat == 5 && d.Status == true), "IdCoa", "Uraian");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdNsrNjop,IdCoa,Jenis,SubJenis,Njop,Strategis1,NonStrategis1,Strategis2,NonStrategis2,Strategis3,NonStrategis3,Satuan")] NsrNjop nsrNjop)
        {
            if (ModelState.IsValid)
            {
                nsrNjop.IdNsrNjop = Guid.NewGuid();
                _context.Add(nsrNjop);
                await _context.SaveChangesAsync();
                TempData["status"] = "create";
                string link = Url.Action("Index");
                return Json(new { success = true, url = link });
            }
            ViewData["IdCoa"] = new SelectList(_context.Coa.Where(d => d.Kdcoa.Contains("41104")).Where(d => d.Tingkat == 5 && d.Status == true), "IdCoa", "Uraian", nsrNjop.IdCoa);
            return PartialView(nsrNjop);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nsrNjop = await _context.NsrNjop.FindAsync(id);
            if (nsrNjop == null)
            {
                return NotFound();
            }
            ViewData["IdCoa"] = new SelectList(_context.Coa.Where(d => d.Kdcoa.Contains("41104")).Where(d => d.Tingkat == 5 && d.Status == true), "IdCoa", "Uraian", nsrNjop.IdCoa);
            return PartialView(nsrNjop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdNsrNjop,IdCoa,Jenis,SubJenis,Njop,Strategis1,NonStrategis1,Strategis2,NonStrategis2,Strategis3,NonStrategis3,Satuan")] NsrNjop nsrNjop)
        {
            if (id != nsrNjop.IdNsrNjop)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nsrNjop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NsrNjopExists(nsrNjop.IdNsrNjop))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "Edit";
                string link = Url.Action("Index");
                return Json(new { success = true, url = link });
            }
            ViewData["IdCoa"] = new SelectList(_context.Coa.Where(d => d.Kdcoa.Contains("41104")).Where(d => d.Tingkat == 5 && d.Status == true), "IdCoa", "Uraian", nsrNjop.IdCoa);
            return PartialView(nsrNjop);
        }

        [Auth(new string[] { "Developers" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nsrNjop = await _context.NsrNjop
                .Include(n => n.IdCoaNavigation)
                .FirstOrDefaultAsync(m => m.IdNsrNjop == id);
            if (nsrNjop == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var nsrNjop = await _context.NsrNjop.FindAsync(id);
            try
            {
                _context.NsrNjop.Remove(nsrNjop);
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

        private bool NsrNjopExists(Guid id)
        {
            return _context.NsrNjop.Any(e => e.IdNsrNjop == id);
        }
    }
}
