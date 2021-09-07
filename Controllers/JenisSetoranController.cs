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
    public class JenisSetoranController : Controller
    {
        private readonly DB_NewContext _context;

        public JenisSetoranController(DB_NewContext context)
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

            return View(await _context.RefJenisSetoran.ToListAsync());
        }

        [Auth(new string[] { "Developers" })]
        [AjaxOnly]
        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSetoran,Kode,Jenis,FlagAktif")] RefJenisSetoran refJenisSetoran)
        {
            if (ModelState.IsValid)
            {
                _context.Add(refJenisSetoran);
                await _context.SaveChangesAsync();
                TempData["status"] = "create";
                string link = Url.Action("Index");
                return Json(new { success = true, url = link });
            }
            return PartialView(refJenisSetoran);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refJenisSetoran = await _context.RefJenisSetoran.FindAsync(id);
            if (refJenisSetoran == null)
            {
                return NotFound();
            }
            return PartialView(refJenisSetoran);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSetoran,Kode,Jenis,FlagAktif")] RefJenisSetoran refJenisSetoran)
        {
            if (id != refJenisSetoran.IdSetoran)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var jenis = _context.RefJenisSetoran.Find(id);
                    jenis.FlagAktif = refJenisSetoran.FlagAktif;
                    _context.RefJenisSetoran.Update(jenis);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RefJenisSetoranExists(refJenisSetoran.IdSetoran))
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
            return PartialView(refJenisSetoran);
        }

        [Auth(new string[] { "Developers" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refJenisSetoran = await _context.RefJenisSetoran
                .FirstOrDefaultAsync(m => m.IdSetoran == id);
            if (refJenisSetoran == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var refJenisSetoran = await _context.RefJenisSetoran.FindAsync(id);
            try
            {
                _context.RefJenisSetoran.Remove(refJenisSetoran);
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

        private bool RefJenisSetoranExists(int id)
        {
            return _context.RefJenisSetoran.Any(e => e.IdSetoran == id);
        }
    }
}
