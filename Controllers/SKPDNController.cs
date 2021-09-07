 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIP.Models;

namespace SIP.Controllers
{
    public class SKPDNController : Controller
    {
        private readonly DB_NewContext _context;

        public SKPDNController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "Laporan" })]
        public IActionResult Index()
        {
            return RedirectToAction("SKPDN", "Laporan");
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skpd = await _context.Skpdn
                .Include(s => s.IdSspdNavigation).ThenInclude(s => s.IdSetoranNavigation)
                .Include(s => s.IdSspdNavigation).ThenInclude(s => s.IdBankNavigation)
                .Include(s => s.IdCoaNavigation)
                .FirstOrDefaultAsync(m => m.IdSkpdn == id);
            if (skpd == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("SKPDN", new { id = skpd.IdSptpd });
            ViewBag.L1 = Url.Action("Details", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View(skpd);
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public IActionResult Create(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skpd = _context.Skpdn
                .Include(s => s.IdSspdNavigation).ThenInclude(s => s.IdSetoranNavigation)
                .Include(s => s.IdSspdNavigation).ThenInclude(s => s.IdBankNavigation)
                .Include(s => s.IdCoaNavigation)
                .FirstOrDefault(m => m.IdSkpdn == id);

            return View(skpd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, Skpdn skpd)
        {
            if (id != skpd.IdSkpdn)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var skpdn = _context.Skpdn.Include(d => d.IdCoaNavigation).FirstOrDefault(d => d.IdSkpdn == id);
                var coa = _context.Coa.AsNoTracking().FirstOrDefault(d => d.IdCoa == skpdn.IdCoaNavigation.Parent);
                skpdn.Tanggal = DateTime.Now;
                var noSk = _context.Skpdn.Where(d => d.Tanggal.Value.Year == skpdn.Tanggal.Value.Year).Select(e => e.Nomor).Max() ?? 0;
                skpdn.Nomor = noSk + 1;
                skpdn.NoSkpdn = string.Format("{0:000000}", skpdn.Nomor) + "/SKPDN/" + string.Format("{0:MM/yyyy}", skpdn.Tanggal);
                skpdn.Eu = HttpContext.Session.GetString("User");
                skpdn.Ed = DateTime.Now;
                _context.Skpdn.Update(skpdn);

                await _context.SaveChangesAsync();

                TempData["status"] = "create";
                return RedirectToAction("Details", new { id });
            }
            return View(skpd);
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public IActionResult Cetak(Guid id)
        {
            return RedirectToAction("SKPDN", "Cetak", new { id });
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var skpd = await _context.Skpdn
                .FirstOrDefaultAsync(m => m.IdSkpdn == id);
            if (skpd == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var skpd = await _context.Skpdn.FindAsync(id);
            skpd.Nomor = null;
            skpd.NoSkpdn = null;
            skpd.Tanggal = null;
            _context.Skpdn.Update(skpd);

            await _context.SaveChangesAsync();
            TempData["status"] = "delete";
            string link = Url.Action("SKPDN", "Penetapan", new { id = skpd.IdSptpd });
            return Json(new { success = true, url = link });
        }
    }
}
