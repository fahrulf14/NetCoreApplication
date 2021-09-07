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
    public class SuratIzinController : Controller
    {
        private readonly DB_NewContext _context;

        public SuratIzinController(DB_NewContext context)
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

            return View(await _context.RefSuratIzin.ToListAsync());
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSuratIzin,NamaSuratIzin,FlagAktif")] RefSuratIzin refSuratIzin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(refSuratIzin);
                await _context.SaveChangesAsync();
                TempData["status"] = "create";
                string link = Url.Action("Index");
                return Json(new { success = true, url = link });
            }
            return PartialView(refSuratIzin);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refSuratIzin = await _context.RefSuratIzin.FindAsync(id);
            if (refSuratIzin == null)
            {
                return NotFound();
            }
            return PartialView(refSuratIzin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSuratIzin,NamaSuratIzin,FlagAktif")] RefSuratIzin refSuratIzin)
        {
            if (id != refSuratIzin.IdSuratIzin)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(refSuratIzin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RefSuratIzinExists(refSuratIzin.IdSuratIzin))
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
            return PartialView(refSuratIzin);
        }


        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refSuratIzin = await _context.RefSuratIzin
                .FirstOrDefaultAsync(m => m.IdSuratIzin == id);
            if (refSuratIzin == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var refSuratIzin = await _context.RefSuratIzin.FindAsync(id);
            try
            {
                _context.RefSuratIzin.Remove(refSuratIzin);
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

        private bool RefSuratIzinExists(int id)
        {
            return _context.RefSuratIzin.Any(e => e.IdSuratIzin == id);
        }
    }
}
