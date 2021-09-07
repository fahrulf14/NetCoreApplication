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
    public class JabatanController : Controller
    {
        private readonly DB_NewContext _context;

        public JabatanController(DB_NewContext context)
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

            return View(await _context.RefJabatan.ToListAsync());
        }

        [Auth(new string[] { "Developers", "Parameter" })]
        [AjaxOnly]
        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdJabatan,Jabatan,FlagAktif")] RefJabatan refJabatan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(refJabatan);
                await _context.SaveChangesAsync();
                TempData["status"] = "create";
                string link = Url.Action("Index");
                return Json(new { success = true, url = link });
            }
            return PartialView(refJabatan);
        }

        [Auth(new string[] { "Developers", "Parameter" })]
        [AjaxOnly]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refJabatan = await _context.RefJabatan.FindAsync(id);
            if (refJabatan == null)
            {
                return NotFound();
            }
            return PartialView(refJabatan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdJabatan,Jabatan,FlagAktif")] RefJabatan refJabatan)
        {
            if (id != refJabatan.IdJabatan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(refJabatan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RefJabatanExists(refJabatan.IdJabatan))
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
            return PartialView(refJabatan);
        }

        [Auth(new string[] { "Developers", "Parameter" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refJabatan = await _context.RefJabatan
                .FirstOrDefaultAsync(m => m.IdJabatan == id);
            if (refJabatan == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var refJabatan = await _context.RefJabatan.FindAsync(id);
            try
            {
                _context.RefJabatan.Remove(refJabatan);
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

        private bool RefJabatanExists(int id)
        {
            return _context.RefJabatan.Any(e => e.IdJabatan == id);
        }
    }
}
