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
    public class DataIzinController : Controller
    {
        private readonly DB_NewContext _context;

        public DataIzinController(DB_NewContext context)
        {
            _context = context;
        }

        [Route("DataIzin/{id}")]
        [Auth(new string[] { "Developers", "Pendaftaran" })]
        public async Task<IActionResult> Index(Guid? id) 
        {
            var dB_NewContext = await _context.DataIzin.Include(d => d.IdSuratIzinNavigation).Include(d => d.IdUsahaNavigation).Where(d => d.IdUsaha == id).ToListAsync();
            var subjek = _context.DataUsaha.FirstOrDefault(d => d.IdUsaha == id);
            ViewBag.IdUsaha = id;
            ViewBag.IdSubjek = subjek.IdSubjek;

            //Link
            ViewBag.L = Url.Content("/DataIzin/" + id);
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View(dB_NewContext);
            
        }

        [Auth(new string[] { "Developers", "Pendaftaran" })]
        public IActionResult Create(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Content("/DataIzin/" + id);
            ViewBag.L1 = Url.Action("Create", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            ViewBag.IdUsaha = id;
            ViewData["IdSuratIzin"] = new SelectList(_context.RefSuratIzin.Where(d => d.FlagAktif), "IdSuratIzin", "NamaSuratIzin");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, [Bind("IdIzin,IdUsaha,IdSuratIzin,Nomor,Tanggal,MasaBerlaku,PemberiIzin,Eu,Ed")] DataIzin dataIzin)
        {
            if (ModelState.IsValid)
            {
                dataIzin.IdIzin = Guid.NewGuid();
                dataIzin.IdUsaha = id;
                _context.Add(dataIzin);
                await _context.SaveChangesAsync();
                TempData["status"] = "create";
                return Redirect("/DataIzin/" + id);
            }
            ViewData["IdSuratIzin"] = new SelectList(_context.RefSuratIzin.Where(d => d.FlagAktif), "IdSuratIzin", "NamaSuratIzin", dataIzin.IdSuratIzin);
            return View(dataIzin);
        }

        [Auth(new string[] { "Developers", "Pendaftaran" })]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataIzin = await _context.DataIzin.FindAsync(id);
            if (dataIzin == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Content("/DataIzin/" + dataIzin.IdUsaha);
            ViewBag.L1 = Url.Action("Edit", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            ViewData["IdSuratIzin"] = new SelectList(_context.RefSuratIzin.Where(d => d.FlagAktif), "IdSuratIzin", "NamaSuratIzin", dataIzin.IdSuratIzin);
            TempData["IdUsaha"] = dataIzin.IdUsaha;

            return View(dataIzin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdIzin,IdUsaha,IdSuratIzin,Nomor,Tanggal,MasaBerlaku,PemberiIzin,Eu,Ed")] DataIzin dataIzin)
        {
            if (id != dataIzin.IdIzin)
            {
                return NotFound();
            }

            var IdUsaha = new Guid(TempData["IdUsaha"].ToString().Replace("-", string.Empty));
            if (ModelState.IsValid)
            {
                try
                {
                    dataIzin.IdUsaha = IdUsaha;
                    dataIzin.Eu = HttpContext.Session.GetString("User");
                    dataIzin.Ed = DateTime.Now;
                    _context.Update(dataIzin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataIzinExists(dataIzin.IdIzin))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "edit";
                return Redirect("/DataIzin/" + IdUsaha);
            }
            ViewData["IdSuratIzin"] = new SelectList(_context.RefSuratIzin.Where(d => d.FlagAktif), "IdSuratIzin", "NamaSuratIzin", dataIzin.IdSuratIzin);
            return View(dataIzin);
        }

        [Auth(new string[] { "Developers", "Pendaftaran" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataIzin = await _context.DataIzin
                .Include(d => d.IdSuratIzinNavigation)
                .Include(d => d.IdUsahaNavigation)
                .FirstOrDefaultAsync(m => m.IdIzin == id);
            if (dataIzin == null)
            {
                return NotFound();
            }
            TempData["IdUsaha"] = dataIzin.IdUsaha;

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var IdUsaha = new Guid(TempData["IdUsaha"].ToString().Replace("-", string.Empty));
            var dataIzin = await _context.DataIzin.FindAsync(id);
            try
            {
                _context.DataIzin.Remove(dataIzin);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["status"] = "deletefailed";
                string url = Url.Content("/DataIzin/" + IdUsaha);
                return Json(new { success = true, url });
            }
            
            TempData["status"] = "delete";
            string link = Url.Content("/DataIzin/" + IdUsaha);
            return Json(new { success = true, url = link });
        }

        private bool DataIzinExists(Guid id)
        {
            return _context.DataIzin.Any(e => e.IdIzin == id);
        }
    }
}
