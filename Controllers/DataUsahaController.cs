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
    public class DataUsahaController : Controller
    {
        private readonly DB_NewContext _context;

        public DataUsahaController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "Pendaftaran" })]
        public IActionResult Create(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("DataUsaha", "Pendaftaran", new { id });
            ViewBag.L1 = Url.Action("Create", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var pemda = _context.Pemda.First();
            ViewBag.IdSubjek = id;
            ViewData["IdJenis"] = new SelectList(_context.RefUsaha, "IdUsaha", "Usaha");
            ViewData["IndKecamatanId"] = new SelectList(_context.IndKecamatan.Where(d => d.IndKabKotaId == pemda.IndKabKotaId), "IndKecamatanId", "Kecamatan");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, [Bind("IdUsaha,IdSubjek,IdJenis,NamaUsaha,AlamatUsaha,Rtrw,IndKecamatanId,IndKelurahanId,NoTelp,KodePos,Status,Eu,Ed")] DataUsaha dataUsaha)
        {
            var pemda = _context.Pemda.First();
            if (ModelState.IsValid)
            {
                dataUsaha.IdUsaha = Guid.NewGuid();
                dataUsaha.IdSubjek = id;
                dataUsaha.IndProvinsiId = pemda.IndProvinsiId;
                dataUsaha.IndKabKotaId = pemda.IndKabKotaId;
                _context.Add(dataUsaha);
                await _context.SaveChangesAsync();
                TempData["status"] = "create";
                return RedirectToAction("DataUsaha", "Pendaftaran", new { id });
            }

            //Link
            ViewBag.L = Url.Action("DataUsaha", "Pendaftaran", new { id });
            ViewBag.L1 = Url.Action("Create", "DataUsaha", null);
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            ViewData["IdJenis"] = new SelectList(_context.RefUsaha, "IdUsaha", "Usaha", dataUsaha.IdJenis);
            ViewData["IndKecamatanId"] = new SelectList(_context.IndKecamatan.Where(d => d.IndKabKotaId == pemda.IndKabKotaId), "IndKecamatanId", "Kecamatan", dataUsaha.IndKecamatanId);
            ViewData["IndKelurahanId"] = new SelectList(_context.IndKelurahan.Where(d => d.IndKecamatanId == dataUsaha.IndKecamatanId), "IndKelurahanId", "Kelurahan", dataUsaha.IndKelurahanId);
            return View(dataUsaha);
        }

        [Auth(new string[] { "Developers", "Pendaftaran" })]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataUsaha = await _context.DataUsaha.FindAsync(id);
            if (dataUsaha == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("DataUsaha", "Pendaftaran", new { id = dataUsaha.IdSubjek });
            ViewBag.L1 = Url.Action("Edit", "DataUsaha", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var pemda = _context.Pemda.First();

            ViewBag.IdSubjek = dataUsaha.IdSubjek;
            ViewData["IdJenis"] = new SelectList(_context.RefUsaha, "IdUsaha", "Usaha", dataUsaha.IdJenis);
            ViewData["IndKecamatanId"] = new SelectList(_context.IndKecamatan.Where(d => d.IndKabKotaId == pemda.IndKabKotaId), "IndKecamatanId", "Kecamatan", dataUsaha.IndKecamatanId);
            ViewData["IndKelurahanId"] = new SelectList(_context.IndKelurahan.Where(d => d.IndKecamatanId == dataUsaha.IndKecamatanId), "IndKelurahanId", "Kelurahan", dataUsaha.IndKelurahanId);

            return View(dataUsaha);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdUsaha,IdJenis,NamaUsaha,AlamatUsaha,Rtrw,IndKecamatanId,IndKelurahanId,NoTelp,KodePos,Status,Eu,Ed")] DataUsaha dataUsaha)
        {
            if (id != dataUsaha.IdUsaha)
            {
                return NotFound();
            }

            DataUsaha old = _context.DataUsaha.Find(id);
            if (ModelState.IsValid)
            {
                try
                {
                    old.IdJenis = dataUsaha.IdJenis;
                    old.NamaUsaha = dataUsaha.NamaUsaha;
                    old.AlamatUsaha = dataUsaha.AlamatUsaha;
                    old.Rtrw = dataUsaha.Rtrw;
                    old.IndKecamatanId = dataUsaha.IndKecamatanId;
                    old.IndKelurahanId = dataUsaha.IndKelurahanId;
                    old.NoTelp = dataUsaha.NoTelp;
                    old.KodePos = dataUsaha.KodePos;
                    old.Status = dataUsaha.Status;
                    old.Eu = HttpContext.Session.GetString("User");
                    old.Ed = DateTime.Now;
                    _context.Update(old);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataUsahaExists(dataUsaha.IdUsaha))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "edit";
                return RedirectToAction("DataUsaha", "Pendaftaran", new { id = old.IdSubjek });
            }

            //Link
            ViewBag.L = Url.Action("DataUsaha", "Pendaftaran", new { id = dataUsaha.IdSubjek });
            ViewBag.L1 = Url.Action("Edit", "DataUsaha", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            ViewData["IdJenis"] = new SelectList(_context.RefUsaha, "IdUsaha", "Usaha", dataUsaha.IdJenis);
            ViewData["IndKecamatanId"] = new SelectList(_context.IndKecamatan, "IndKecamatanId", "Kecamatan", dataUsaha.IndKecamatanId);
            ViewData["IndKelurahanId"] = new SelectList(_context.IndKelurahan, "IndKelurahanId", "Kelurahan", dataUsaha.IndKelurahanId);
            return View(dataUsaha);
        }

        [Auth(new string[] { "Developers", "Pendaftaran" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataUsaha = await _context.DataUsaha
                .Include(d => d.IdJenisNavigation)
                .Include(d => d.IdSubjekNavigation)
                .Include(d => d.IndKabKota)
                .Include(d => d.IndKecamatan)
                .Include(d => d.IndKelurahan)
                .Include(d => d.IndProvinsi)
                .FirstOrDefaultAsync(m => m.IdUsaha == id);
            if (dataUsaha == null)
            {
                return NotFound();
            }
            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var dataUsaha = await _context.DataUsaha.FindAsync(id);
            try
            {
                _context.DataUsaha.Remove(dataUsaha);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["status"] = "deletefailed";
                string url = Url.Action("DataUsaha", "Pendaftaran", new { id = dataUsaha.IdSubjek });
                return Json(new { success = true, url });
            }
            TempData["status"] = "delete";
            string link = Url.Action("DataUsaha", "Pendaftaran", new { id = dataUsaha.IdSubjek });
            return Json(new { success = true, url = link });
        }

        private bool DataUsahaExists(Guid id)
        {
            return _context.DataUsaha.Any(e => e.IdUsaha == id);
        }
    }
}
