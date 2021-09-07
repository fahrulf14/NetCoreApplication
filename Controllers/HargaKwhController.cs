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
    public class HargaKwhController : Controller
    {
        private readonly DB_NewContext _context;

        public HargaKwhController(DB_NewContext context)
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

            return View(await _context.RefHargaKwh.ToListAsync());
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRef,Tarif,TanggalBerlaku,FlagAktif")] RefHargaKwh refHargaKwh)
        {
            if (ModelState.IsValid)
            {
                _context.Add(refHargaKwh);
                await _context.SaveChangesAsync();
                TempData["status"] = "create";
                string link = Url.Action("Index");
                return Json(new { success = true, url = link });
            }
            return PartialView(refHargaKwh);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refHargaKwh = await _context.RefHargaKwh.FindAsync(id);
            if (refHargaKwh == null)
            {
                return NotFound();
            }
            return PartialView(refHargaKwh);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRef,Tarif,TanggalBerlaku,FlagAktif")] RefHargaKwh refHargaKwh)
        {
            if (id != refHargaKwh.IdRef)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(refHargaKwh);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RefHargaKwhExists(refHargaKwh.IdRef))
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
            return PartialView(refHargaKwh);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refHargaKwh = await _context.RefHargaKwh
                .FirstOrDefaultAsync(m => m.IdRef == id);
            if (refHargaKwh == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var refHargaKwh = await _context.RefHargaKwh.FindAsync(id);
            try
            {
                _context.RefHargaKwh.Remove(refHargaKwh);
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

        private bool RefHargaKwhExists(int id)
        {
            return _context.RefHargaKwh.Any(e => e.IdRef == id);
        }
    }
}
