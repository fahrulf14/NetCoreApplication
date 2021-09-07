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
    public class KodeAkunController : Controller
    {
        private readonly DB_NewContext _context;

        public KodeAkunController(DB_NewContext context)
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

            ViewBag.Coa = _context.Coa.Where(d => d.Tingkat == 4).ToList();

            return View(await _context.Coa.ToListAsync());
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public IActionResult Create(string id)
        {
            var kd = 0;
            var coa = _context.Coa.Where(d => d.Parent == id).OrderByDescending(d => d.IdCoa).FirstOrDefault();
            if (coa != null)
            {
                kd = int.Parse(coa.IdCoa.Substring(5, 2));
            }
            var kode = kd + 1;
            ViewBag.Kode = id + string.Format("{0:00}", kode);

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string id, [Bind("IdCoa,Kdcoa,Uraian,Jenis,Parent,Tingkat,Denda,Status")] Coa coa)
        {
            if (ModelState.IsValid)
            {
                var parent = _context.Coa.AsNoTracking().Where(d => d.Parent == id).OrderByDescending(d => d.IdCoa).FirstOrDefault();
                var data = _context.Coa.AsNoTracking().FirstOrDefault(d => d.IdCoa == id);
                var kd = 0;
                if (parent != null)
                {
                    kd = int.Parse(parent.IdCoa.Substring(5, 2));
                    coa.Parent = parent.Kdcoa;
                    coa.Jenis = parent.Jenis;
                    coa.Denda = parent.Denda;
                }
                else
                {
                    coa.Parent = data.IdCoa;
                    coa.Jenis = data.Jenis;
                    coa.Denda = data.Denda;
                }
                var kode = kd + 1;
                coa.IdCoa = id + string.Format("{0:00}", kode);
                coa.Kdcoa = id + string.Format("{0:00}", kode);
                coa.Tingkat = 5;
                coa.Status = true;

                _context.Coa.Add(coa);
                await _context.SaveChangesAsync();
                TempData["status"] = "create";
                string link = Url.Action("Index");
                return Json(new { success = true, url = link });
            }
            return PartialView(coa);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coa = await _context.Coa.FindAsync(id);
            if (coa == null)
            {
                return NotFound();
            }
            return PartialView(coa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdCoa,Kdcoa,Uraian,Jenis,Parent,Tingkat,Denda,Status")] Coa coa)
        {
            if (id != coa.IdCoa)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var old = _context.Coa.Find(id);
                    old.Uraian = coa.Uraian;
                    _context.Coa.Update(old);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoaExists(coa.IdCoa))
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
            return PartialView(coa);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coa = await _context.Coa
                .FirstOrDefaultAsync(m => m.IdCoa == id);
            if (coa == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var coa = await _context.Coa.FindAsync(id);
            try
            {
                _context.Coa.Remove(coa);
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

        private bool CoaExists(string id)
        {
            return _context.Coa.Any(e => e.IdCoa == id);
        }
    }
}
