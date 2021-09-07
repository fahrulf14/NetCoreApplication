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
    public class RetribusiController : Controller
    {
        private readonly DB_NewContext _context;

        public RetribusiController(DB_NewContext context)
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

            return View(await _context.RefRetribusi.Include(d => d.IdCoaNavigation).ToListAsync());
        }

        [Auth(new string[] { "Developers" })]
        [AjaxOnly]
        public IActionResult Create()
        {
            List<SelectListItem> Dok = new List<SelectListItem>
            {
                new SelectListItem() { Text = "SKRD", Value = "SKRD" },
                new SelectListItem() { Text = "Karcis", Value = "Karcis" }
            };
            ViewBag.Dokumen = new SelectList(Dok, "Value", "Text");

            List<SelectListItem> Jen = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Jasa Umum", Value = "Jasa Umum" },
                new SelectListItem() { Text = "Jasa Usaha", Value = "Jasan Usaha" },
                new SelectListItem() { Text = "Perizinan Tertentu", Value = "Perizina Tertentu" }
            };
            ViewBag.Jenis = new SelectList(Jen, "Value", "Text");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRetribusi,IdCoa,Urutan,Jenis,Uraian,Dok,Icon,Color,Link,FlagAktif")] RefRetribusi refRetribusi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(refRetribusi);
                await _context.SaveChangesAsync();
                TempData["status"] = "create";
                string link = Url.Action("Index");
                return Json(new { success = true, url = link });
            }
            return PartialView(refRetribusi);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refRetribusi = await _context.RefRetribusi.FindAsync(id);
            if (refRetribusi == null)
            {
                return NotFound();
            }
            List<SelectListItem> Dok = new List<SelectListItem>
            {
                new SelectListItem() { Text = "SKRD", Value = "SKRD" },
                new SelectListItem() { Text = "Karcis", Value = "Karcis" }
            };
            ViewBag.Dokumen = new SelectList(Dok, "Value", "Text", refRetribusi.Dok);
            return PartialView(refRetribusi);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRetribusi,IdCoa,Urutan,Jenis,Uraian,Dok,Icon,Color,Link,FlagAktif")] RefRetribusi refRetribusi)
        {
            if (id != refRetribusi.IdRetribusi)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    RefRetribusi retribusi = _context.RefRetribusi.Find(id);
                    retribusi.Dok = refRetribusi.Dok;
                    retribusi.FlagAktif = refRetribusi.FlagAktif;
                    _context.Update(retribusi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RefRetribusiExists(refRetribusi.IdRetribusi))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "edit";
                string link = Url.Action("Index", "Retribusi");
                return Json(new { success = true, url = link });
            }
            return PartialView(refRetribusi);
        }

        [Auth(new string[] { "Developers" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refRetribusi = await _context.RefRetribusi
                .FirstOrDefaultAsync(m => m.IdRetribusi == id);
            if (refRetribusi == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var refRetribusi = await _context.RefRetribusi.FindAsync(id);
            try
            {
                _context.RefRetribusi.Remove(refRetribusi);
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

        private bool RefRetribusiExists(int id)
        {
            return _context.RefRetribusi.Any(e => e.IdRetribusi == id);
        }
    }
}
