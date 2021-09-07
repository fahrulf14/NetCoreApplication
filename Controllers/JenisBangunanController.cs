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
    public class JenisBangunanController : Controller
    {
        private readonly DB_NewContext _context;

        public JenisBangunanController(DB_NewContext context)
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

            return View(await _context.RefJenisBangunan.ToListAsync());
        }

        [Auth(new string[] { "Developers", "Parameter" })]
        [AjaxOnly]
        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdJenis,Jenis,FlagAktif")] RefJenisBangunan refJenisBangunan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(refJenisBangunan);
                await _context.SaveChangesAsync();
                TempData["status"] = "create";
                string link = Url.Action("Index");
                return Json(new { success = true, url = link });
            }
            return PartialView(refJenisBangunan);
        }

        [Auth(new string[] { "Developers", "Parameter" })]
        [AjaxOnly]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refJenisBangunan = await _context.RefJenisBangunan.FindAsync(id);
            if (refJenisBangunan == null)
            {
                return NotFound();
            }
            return PartialView(refJenisBangunan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdJenis,Jenis,FlagAktif")] RefJenisBangunan refJenisBangunan)
        {
            if (id != refJenisBangunan.IdJenis)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(refJenisBangunan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RefJenisBangunanExists(refJenisBangunan.IdJenis))
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
            return PartialView(refJenisBangunan);
        }

        [Auth(new string[] { "Developers", "Parameter" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refJenisBangunan = await _context.RefJenisBangunan
                .FirstOrDefaultAsync(m => m.IdJenis == id);
            if (refJenisBangunan == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var refJenisBangunan = await _context.RefJenisBangunan.FindAsync(id);
            try
            {
                _context.RefJenisBangunan.Remove(refJenisBangunan);
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

        private bool RefJenisBangunanExists(int id)
        {
            return _context.RefJenisBangunan.Any(e => e.IdJenis == id);
        }
    }
}
