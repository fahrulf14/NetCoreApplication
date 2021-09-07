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
    public class UsahaController : Controller
    {
        private readonly DB_NewContext _context;

        public UsahaController(DB_NewContext context)
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

            return View(await _context.RefUsaha.ToListAsync());
        }

        [Auth(new string[] { "Developers", "Parameter" })]
        [AjaxOnly]
        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUsaha,Usaha,FlagAktif")] RefUsaha refUsaha)
        {
            if (ModelState.IsValid)
            {
                _context.Add(refUsaha);
                await _context.SaveChangesAsync();
                TempData["status"] = "create";
                string link = Url.Action("Index");
                return Json(new { success = true, url = link });
            }
            return PartialView(refUsaha);
        }

        [Auth(new string[] { "Developers", "Parameter" })]
        [AjaxOnly]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refUsaha = await _context.RefUsaha.FindAsync(id);
            if (refUsaha == null)
            {
                return NotFound();
            }
            return PartialView(refUsaha);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUsaha,Usaha,FlagAktif")] RefUsaha refUsaha)
        {
            if (id != refUsaha.IdUsaha)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(refUsaha);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RefUsahaExists(refUsaha.IdUsaha))
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
            return PartialView(refUsaha);
        }

        [Auth(new string[] { "Developers", "Parameter" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refUsaha = await _context.RefUsaha
                .FirstOrDefaultAsync(m => m.IdUsaha == id);
            if (refUsaha == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var refUsaha = await _context.RefUsaha.FindAsync(id);
            try
            {
                _context.RefUsaha.Remove(refUsaha);
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

        private bool RefUsahaExists(int id)
        {
            return _context.RefUsaha.Any(e => e.IdUsaha == id);
        }
    }
}
