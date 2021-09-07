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
    public class NsrLainController : Controller
    {
        private readonly DB_NewContext _context;

        public NsrLainController(DB_NewContext context)
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

            return View(await _context.NsrLain.ToListAsync());
        }

        [Auth(new string[] { "Developers" })]
        [AjaxOnly]
        public IActionResult Create()
        {
            ViewBag.Jenis = new SelectList(_context.Coa.Where(d => d.Kdcoa.Contains("41104")).Where(d => d.Tingkat == 5 && d.Status == true)
               .OrderBy(d => d.Kdcoa), "Uraian", "Uraian");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdNsr,Jenis,Ukuran,SatuanUkuran,Waktu,SatuanWaktu,Nsr,FlagAktif")] NsrLain nsrLain)
        {
            if (ModelState.IsValid)
            {
                var idn = _context.NsrLain.Select(d => d.IdNsr).Max();
                nsrLain.IdNsr = idn + 1;
                _context.Add(nsrLain);
                await _context.SaveChangesAsync();
                TempData["status"] = "create";
                string link = Url.Action("Index");
                return Json(new { success = true, url = link });
            }
            return PartialView(nsrLain);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nsrLain = await _context.NsrLain.FindAsync(id);
            if (nsrLain == null)
            {
                return NotFound();
            }
            ViewBag.Jenis = new SelectList(_context.Coa.Where(d => d.Kdcoa.Contains("41104")).Where(d => d.Tingkat == 5 && d.Status == true)
               .OrderBy(d => d.Kdcoa), "Uraian", "Uraian");
            return PartialView(nsrLain);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdNsr,Jenis,Ukuran,SatuanUkuran,Waktu,SatuanWaktu,Nsr,FlagAktif")] NsrLain nsrLain)
        {
            if (id != nsrLain.IdNsr)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nsrLain);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NsrLainExists(nsrLain.IdNsr))
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
            ViewBag.Jenis = new SelectList(_context.Coa.Where(d => d.Kdcoa.Contains("41104")).Where(d => d.Tingkat == 5 && d.Status == true)
               .OrderBy(d => d.Kdcoa), "Uraian", "Uraian");
            return PartialView(nsrLain);
        }

        [Auth(new string[] { "Developers" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nsrLain = await _context.NsrLain
                .FirstOrDefaultAsync(m => m.IdNsr == id);
            if (nsrLain == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nsrLain = await _context.NsrLain.FindAsync(id);
            try
            {
                _context.NsrLain.Remove(nsrLain);
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

        private bool NsrLainExists(int id)
        {
            return _context.NsrLain.Any(e => e.IdNsr == id);
        }
    }
}
