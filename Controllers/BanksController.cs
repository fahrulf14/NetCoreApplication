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
    public class BanksController : Controller
    {
        private readonly DB_NewContext _context;

        public BanksController(DB_NewContext context)
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

            return View(await _context.RefBank.ToListAsync());
        }

        [Auth(new string[] { "Developers", "Parameter" })]
        [AjaxOnly]
        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdBank,NamaBank,Kode,Status")] RefBank refBank)
        {
            if (ModelState.IsValid)
            {
                _context.Add(refBank);
                await _context.SaveChangesAsync();
                TempData["status"] = "create";
                string link = Url.Action("Index");
                return Json(new { success = true, url = link });
            }
            return PartialView(refBank);
        }

        [Auth(new string[] { "Developers", "Parameter" })]
        [AjaxOnly]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refBank = await _context.RefBank.FindAsync(id);
            if (refBank == null)
            {
                return NotFound();
            }
            return PartialView(refBank);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdBank,NamaBank,Kode,Status")] RefBank refBank)
        {
            if (id != refBank.IdBank)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(refBank);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RefBankExists(refBank.IdBank))
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
            return PartialView(refBank);
        }

        [Auth(new string[] { "Developers", "Parameter" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refBank = await _context.RefBank
                .FirstOrDefaultAsync(m => m.IdBank == id);
            if (refBank == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var refBank = await _context.RefBank.FindAsync(id);
            try
            {
                _context.RefBank.Remove(refBank);
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

        private bool RefBankExists(int id)
        {
            return _context.RefBank.Any(e => e.IdBank == id);
        }
    }
}
