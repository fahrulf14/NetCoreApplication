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
    public class TransaksiLraController : Controller
    {
        private readonly DB_NewContext _context;

        public TransaksiLraController(DB_NewContext context)
        {
            _context = context;
        }

        [Route("TransaksiLra/{id}")]
        [Auth(new string[] { "Developers", "Realiasi" })]
        public async Task<IActionResult> Index(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lra = await _context.Lra.Include(d => d.IdCoaNavigation).Include(d => d.TransaksiLra).FirstOrDefaultAsync(s => s.IdLra == id);

            if (lra.IdCoaNavigation.Denda != null)
            {
                return NotFound();
            }

            ViewBag.Title = "Realisasi " + lra.IdCoaNavigation.Uraian;
            ViewBag.L = Url.Action("Tahun", "Realisasi", new { id = lra.Tahun });
            ViewBag.L1 = Url.Content("/TransaksiLra/" + id);
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View(lra);
        }

        [Auth(new string[] { "Developers", "Realiasi" })]
        [AjaxOnly]
        public IActionResult Create(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, [Bind("IdTransaksi,IdLra,Tanggal,Jumlah")] TransaksiLra transaksiLra)
        {
            if (ModelState.IsValid)
            {
                transaksiLra.IdTransaksi = Guid.NewGuid();
                transaksiLra.IdLra = id;
                _context.Add(transaksiLra);
                await _context.SaveChangesAsync();

                var lra = _context.Lra.Find(id);
                var transaksi = _context.TransaksiLra.Where(d => d.IdLra == id).Sum(d => d.Jumlah);
                lra.Realisasi = transaksi ?? 0;
                _context.Lra.Update(lra);
                await _context.SaveChangesAsync();

                TempData["status"] = "create";
                string link = Url.Content("/TransaksiLra/" + id);
                return Json(new { success = true, url = link });
            }
            return PartialView(transaksiLra);
        }

        [Auth(new string[] { "Developers", "Realiasi" })]
        [AjaxOnly]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaksiLra = await _context.TransaksiLra.FindAsync(id);
            if (transaksiLra == null)
            {
                return NotFound();
            }
            return PartialView(transaksiLra);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdTransaksi,IdLra,Tanggal,Jumlah")] TransaksiLra transaksiLra)
        {
            if (id != transaksiLra.IdTransaksi)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaksiLra);
                    await _context.SaveChangesAsync();

                    var lra = _context.Lra.Find(transaksiLra.IdLra);
                    var transaksi = _context.TransaksiLra.Where(d => d.IdLra == transaksiLra.IdLra).Sum(d => d.Jumlah);
                    lra.Realisasi = transaksi ?? 0;
                    _context.Lra.Update(lra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransaksiLraExists(transaksiLra.IdTransaksi))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "edit";
                string link = Url.Content("/TransaksiLra/" + transaksiLra.IdLra);
                return Json(new { success = true, url = link });
            }
            return PartialView(transaksiLra);
        }

        [Auth(new string[] { "Developers", "Realiasi" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaksiLra = await _context.TransaksiLra
                .Include(t => t.IdLraNavigation)
                .FirstOrDefaultAsync(m => m.IdTransaksi == id);
            if (transaksiLra == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var transaksiLra = await _context.TransaksiLra.FindAsync(id);
            try
            {
                _context.TransaksiLra.Remove(transaksiLra);
                await _context.SaveChangesAsync();

                var lra = _context.Lra.Find(transaksiLra.IdLra);
                var transaksi = _context.TransaksiLra.Where(d => d.IdLra == transaksiLra.IdLra).Sum(d => d.Jumlah);
                lra.Realisasi = transaksi ?? 0;
                _context.Lra.Update(lra);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["status"] = "deletefailed";
                string url = Url.Content("/TransaksiLra/" + transaksiLra.IdLra);
                return Json(new { success = true, url });
            }

            TempData["status"] = "delete";
            string link = Url.Content("/TransaksiLra/" + transaksiLra.IdLra);
            return Json(new { success = true, url = link });
        }

        private bool TransaksiLraExists(Guid id)
        {
            return _context.TransaksiLra.Any(e => e.IdTransaksi == id);
        }
    }
}
