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
    public class PekerjaanController : Controller
    {
        private readonly DB_NewContext _context;

        public PekerjaanController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "Parameter" })]
        public async Task<IActionResult> Index()
        {
            //Link
            ViewBag.L = Url.Action("Index");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View(await _context.RefPekerjaan.ToListAsync());
        }

        [Auth(new string[] { "Developers", "Parameter" })]
        [AjaxOnly]
        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPekerjaan,Pekerjaan,FlagAktif")] RefPekerjaan refPekerjaan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(refPekerjaan);
                await _context.SaveChangesAsync();
                TempData["status"] = "create";
                string link = Url.Action("Index");
                return Json(new { success = true, url = link });
            }
            return PartialView(refPekerjaan);
        }

        [Auth(new string[] { "Developers", "Parameter" })]
        [AjaxOnly]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refPekerjaan = await _context.RefPekerjaan.FindAsync(id);
            if (refPekerjaan == null)
            {
                return NotFound();
            }
            return PartialView(refPekerjaan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPekerjaan,Pekerjaan,FlagAktif")] RefPekerjaan refPekerjaan)
        {
            if (id != refPekerjaan.IdPekerjaan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(refPekerjaan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RefPekerjaanExists(refPekerjaan.IdPekerjaan))
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
            return View(refPekerjaan);
        }

        [Auth(new string[] { "Developers", "Parameter" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refPekerjaan = await _context.RefPekerjaan
                .FirstOrDefaultAsync(m => m.IdPekerjaan == id);
            if (refPekerjaan == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var refPekerjaan = await _context.RefPekerjaan.FindAsync(id);
            try
            {
                _context.RefPekerjaan.Remove(refPekerjaan);
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

        private bool RefPekerjaanExists(int id)
        {
            return _context.RefPekerjaan.Any(e => e.IdPekerjaan == id);
        }
    }
}
