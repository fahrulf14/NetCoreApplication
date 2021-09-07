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
    public class PegawaiController : Controller
    {
        private readonly DB_NewContext _context;

        public PegawaiController(DB_NewContext context)
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

            var dB_NewContext = _context.Pegawai.Include(p => p.IdJabatanNavigation).Where(p => p.Nama != "Developers");

            ViewBag.Jabatan = _context.RefJabatan.ToList();

            return View(await dB_NewContext.ToListAsync());
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public IActionResult Create()
        {
            ViewData["IdJabatan"] = new SelectList(_context.RefJabatan, "IdJabatan", "Jabatan");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPegawai,Nama,Nick,Nip,IdJabatan,Active")] Pegawai pegawai)
        {
            if (ModelState.IsValid)
            {
                pegawai.IdPegawai = Guid.NewGuid();
                pegawai.Active = true;
                _context.Add(pegawai);
                await _context.SaveChangesAsync();
                TempData["status"] = "create";
                string link = Url.Action("Index");
                return Json(new { success = true, url = link });
            }
            ViewData["IdJabatan"] = new SelectList(_context.RefJabatan, "IdJabatan", "IdJabatan", pegawai.IdJabatan);
            return PartialView(pegawai);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pegawai = await _context.Pegawai.FindAsync(id);
            if (pegawai == null)
            {
                return NotFound();
            }
            ViewData["IdJabatan"] = new SelectList(_context.RefJabatan, "IdJabatan", "Jabatan", pegawai.IdJabatan);
            return PartialView(pegawai);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdPegawai,Nama,Nick,Nip,IdJabatan,Active")] Pegawai pegawai)
        {
            if (id != pegawai.IdPegawai)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var old = _context.Pegawai.Find(id);
                try
                {
                    old.Active = pegawai.Active;
                    old.IdJabatan = pegawai.IdJabatan;
                    old.Nama = pegawai.Nama;
                    old.Nick = pegawai.Nick;
                    old.Nip = pegawai.Nip;
                    _context.Pegawai.Update(old);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PegawaiExists(pegawai.IdPegawai))
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
            ViewData["IdJabatan"] = new SelectList(_context.RefJabatan, "IdJabatan", "IdJabatan", pegawai.IdJabatan);
            return PartialView(pegawai);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pegawai = await _context.Pegawai
                .Include(p => p.IdJabatanNavigation)
                .FirstOrDefaultAsync(m => m.IdPegawai == id);
            if (pegawai == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var pegawai = await _context.Pegawai.FindAsync(id);

            var user = _context.AspNetUsers.FirstOrDefault(d => d.Email == pegawai.Email);
            if (user != null)
            {
                TempData["status"] = "deletefailed";
                string url = Url.Action("Index");
                return Json(new { success = true, url });
            }

            try
            {
                _context.Pegawai.Remove(pegawai);
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

        private bool PegawaiExists(Guid id)
        {
            return _context.Pegawai.Any(e => e.IdPegawai == id);
        }
    }
}
