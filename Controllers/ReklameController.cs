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
    public class ReklameController : Controller
    {
        private readonly DB_NewContext _context;

        public ReklameController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers" })]
        public async Task<IActionResult> Index()
        {
            //Link
            ViewBag.L = Url.Action("Index");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View(await _context.RefReklame.ToListAsync());
        }

        [Auth(new string[] { "Developers" })]
        [AjaxOnly]
        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRef,Uraian,Tambahan,FlagAktif")] RefReklame refReklame)
        {
            if (ModelState.IsValid)
            {
                _context.Add(refReklame);
                await _context.SaveChangesAsync();
                TempData["status"] = "create";
                string link = Url.Action("Index");
                return Json(new { success = true, url = link });
            }
            return PartialView(refReklame);
        }

        [Auth(new string[] { "Developers" })]
        [AjaxOnly]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refReklame = await _context.RefReklame.FindAsync(id);
            if (refReklame == null)
            {
                return NotFound();
            }
            return PartialView(refReklame);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRef,Uraian,Tambahan,FlagAktif")] RefReklame refReklame)
        {
            if (id != refReklame.IdRef)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(refReklame);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RefReklameExists(refReklame.IdRef))
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
            return PartialView(refReklame);
        }

        [Auth(new string[] { "Developers" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refReklame = await _context.RefReklame
                .FirstOrDefaultAsync(m => m.IdRef == id);
            if (refReklame == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var refReklame = await _context.RefReklame.FindAsync(id);
            try
            {
                _context.RefReklame.Remove(refReklame);
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

        private bool RefReklameExists(int id)
        {
            return _context.RefReklame.Any(e => e.IdRef == id);
        }
    }
}
