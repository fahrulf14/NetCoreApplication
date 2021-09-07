using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIP.Controllers.Tags;
using SIP.Models;

namespace SIP.Controllers
{
    public class AnggaranController : Controller
    {
        private readonly DB_NewContext _context;

        public AnggaranController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "Setting" })]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Lra lra)
        {
            return RedirectToAction("Tahun", new { id = lra.Tahun });
        }

        [Auth(new string[] { "Developers", "Setting" })]
        public async Task<IActionResult> Tahun(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("Tahun", "Anggaran", new { id });
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";
            ViewBag.Title = "Anggaran Tahun " + id;
            ViewBag.SubHeaderTitle = "Anggaran Tahun " + id;

            ViewBag.Tahun = id;

            var lra = await _context.Lra.Include(l => l.IdCoaNavigation).Where(l => l.Tahun == id).ToListAsync();

            return View(lra);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lra = await _context.Lra
                .Include(l => l.IdCoaNavigation)
                .FirstOrDefaultAsync(m => m.IdLra == id);
            if (lra == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("Tahun", new { lra.Tahun });
            ViewBag.L1 = Url.Action("Details", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";
            ViewBag.Title = "Detail Anggaran Tahun " + lra.Tahun;
            ViewBag.SubHeaderTitle = "Anggaran Tahun " + lra.Tahun;
            ViewBag.S1 = "Detail Anggaran " + lra.IdCoaNavigation.Uraian;

            return View(lra);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public IActionResult Create(int id)
        {
            var query = from c in _context.Coa
                        where !(from d in _context.Lra where d.Tahun == id select d.IdCoa).Contains(c.IdCoa)
                        where c.Tingkat == 5 && c.Status
                        orderby c.Kdcoa
                        select new
                        {
                            Id = c.IdCoa,
                            Name = c.Uraian
                        };

            ViewData["IdCoa"] = new SelectList(query, "Id", "Name");
            ViewBag.Tahun = id;
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("IdLra,IdCoa,Tahun,Anggaran,Aperubahan,Afinal,Realisasi,Januari,Februari,Maret,April,Mei,Juni,Juli,Agustus,September,Oktober,November,Desember")] Lra lra)
        {
            if (ModelState.IsValid)
            {
                var data = _context.Lra.Include(d => d.IdCoaNavigation).Where(d => d.Tahun == id && d.IdCoa == lra.IdCoa).AsNoTracking().FirstOrDefault();
                if(data != null)
                {
                    TempData["status"] = "LraDuplicate";
                    TempData["uraian"] = data.IdCoaNavigation.Uraian;
                    string url = Url.Action("Index", "Anggaran");
                    return Json(new { success = true, url });
                }
                lra.IdLra = Guid.NewGuid();
                lra.Tahun = id;
                lra.Afinal = lra.Anggaran;
                _context.Add(lra);
                await _context.SaveChangesAsync();
                TempData["status"] = "create";
                string link = Url.Action("Tahun", "Anggaran", new { id });
                return Json(new { success = true, url = link });
            }
            var query = from c in _context.Coa
                        where !(from d in _context.Lra where d.Tahun == id select d.IdCoa).Contains(c.IdCoa)
                        where c.Tingkat == 5 && c.Status
                        orderby c.Kdcoa
                        select new
                        {
                            Id = c.IdCoa,
                            Name = c.Uraian
                        };

            ViewData["IdCoa"] = new SelectList(query, "Id", "Name", lra.IdCoa);
            return PartialView(lra);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        public async Task<IActionResult> Bulanan(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lra = await _context.Lra.Include(d => d.IdCoaNavigation).FirstOrDefaultAsync(d => d.IdLra == id);
            if (lra == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("Tahun", "Anggaran", new { lra.Tahun });
            ViewBag.L1 = Url.Action("Bulanan", "Anggaran", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";
            ViewBag.Title = "Ubah Anggaran Bulanan Tahun " + lra.Tahun;
            ViewBag.SubHeaderTitle = "Anggaran Tahun " + lra.Tahun;
            ViewBag.S1 = "Ubah Anggaran " + lra.IdCoaNavigation.Uraian;

            return View(lra);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Bulanan(Guid id, [Bind("IdLra,IdCoa,Tahun,Anggaran,Aperubahan,Afinal,Januari,Februari,Maret,April,Mei,Juni,Juli,Agustus,September,Oktober,November,Desember")] Lra lra)
        {
            if (id != lra.IdLra)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Lra old = _context.Lra.Include(d => d.IdCoaNavigation).FirstOrDefault(d => d.IdLra == id);
                var bulan = lra.Januari + lra.Februari + lra.Maret + lra.April + lra.Mei + lra.Juni + 
                    lra.Juli + lra.Agustus + lra.September + lra.Oktober + lra.November + lra.Desember;
                if(bulan > old.Afinal)
                {
                    //Link
                    ViewBag.L = Url.Action("Tahun", "Anggaran", new { old.Tahun });
                    ViewBag.L1 = Url.Action("Bulanan", "Anggaran", new { id });
                    ViewBag.L2 = "";
                    ViewBag.L3 = "";
                    ViewBag.Title = "Ubah Anggaran Bulanan Tahun " + old.Tahun;
                    ViewBag.SubHeaderTitle = "Anggaran Tahun " + old.Tahun;
                    ViewBag.S1 = "Ubah Anggaran " + old.IdCoaNavigation.Uraian;

                    TempData["status"] = "AnggaranMax";
                    TempData["jumlah"] = string.Format("{0:C0}", old.Afinal);
                    ViewBag.Uraian = old.IdCoaNavigation.Uraian;
                    ViewBag.Anggaran = old.Anggaran;
                    return View(lra);
                }
                try
                {
                    old.Januari = lra.Januari;
                    old.Februari = lra.Februari;
                    old.Maret = lra.Maret;
                    old.April = lra.April;
                    old.Mei = lra.Mei;
                    old.Juni = lra.Juni;
                    old.Juli = lra.Juli;
                    old.Agustus = lra.Agustus;
                    old.September = lra.September;
                    old.Oktober = lra.Oktober;
                    old.November = lra.November;
                    old.Desember = lra.Desember;
                    _context.Update(old);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LraExists(lra.IdLra))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "edit";
                return RedirectToAction("Tahun", new { id = old.Tahun });
            }
            return View(lra);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        public async Task<IActionResult> Tahunan(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lra = await _context.Lra.Include(d => d.IdCoaNavigation).FirstOrDefaultAsync(d => d.IdLra == id);
            if (lra == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("Tahun", new { lra.Tahun });
            ViewBag.L1 = Url.Action("Tahunan", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";
            ViewBag.Title = "Ubah Anggaran Tahun " + lra.Tahun;
            ViewBag.SubHeaderTitle = "Anggaran Tahun " + lra.Tahun;
            ViewBag.S1 = "Ubah Anggaran " + lra.IdCoaNavigation.Uraian;

            return View(lra);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Tahunan(Guid id, [Bind("IdLra,Anggaran,Aperubahan,Afinal")] Lra lra)
        {
            if (id != lra.IdLra)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Lra old = _context.Lra.Find(id);
                try
                {
                    old.Anggaran = lra.Anggaran;
                    old.Aperubahan = lra.Aperubahan;
                    old.Afinal = lra.Anggaran;
                    if (lra.Aperubahan != null && lra.Aperubahan != 0)
                    {
                        old.Afinal = lra.Aperubahan;
                    }
                    _context.Update(old);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LraExists(lra.IdLra))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "edit";
                return RedirectToAction("Tahun", new { id = old.Tahun });
            }
            return View(lra);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lra = await _context.Lra
                .Include(l => l.IdCoaNavigation)
                .FirstOrDefaultAsync(m => m.IdLra == id);
            if (lra == null)
            {
                return NotFound();
            }

            if (lra.TransaksiLra.FirstOrDefault(d => d.IdLra == lra.IdLra) != null)
            {
                return PartialView("_deletevalid");
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var lra = await _context.Lra.FindAsync(id);
            try
            {
                _context.Lra.Remove(lra);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["status"] = "deletefailed";
                string url = Url.Action("Tahun", new { id = lra.Tahun });
                return Json(new { success = true, url });
            }
            
            TempData["status"] = "delete";
            string link = Url.Action("Tahun", new { id = lra.Tahun });
            return Json(new { success = true, url = link });
        }

        private bool LraExists(Guid id)
        {
            return _context.Lra.Any(e => e.IdLra == id);
        }
    }
}
