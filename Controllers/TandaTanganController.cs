using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace SIP.Models
{
    public class TandaTanganController : Controller
    {
        private readonly DB_NewContext _context;

        public TandaTanganController(DB_NewContext context)
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

            var dB_NewContext = _context.TandaTangan.Include(t => t.IdDokumenNavigation).Include(t => t.IdPegawaiNavigation);
            return View(await dB_NewContext.ToListAsync());
        }

        [Auth(new string[] { "Developers" })]
        [AjaxOnly]
        public IActionResult Create()
        {
            var subselect = _context.TandaTangan.Select(d => d.IdDokumen).ToList();
            var dok = (from d in _context.RefDokumen
                       where !subselect.Contains(d.IdDokumen)
                       select d).ToList();

            ViewData["IdDokumen"] = new SelectList(dok, "IdDokumen", "Dokumen");
            ViewData["IdPegawai"] = new SelectList(_context.Pegawai.Where(d => d.Nama != "Developers"), "IdPegawai", "Nama");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTtd,IdDokumen,IdPegawai")] TandaTangan tandaTangan)
        {
            if (ModelState.IsValid)
            {
                tandaTangan.IdTtd = Guid.NewGuid();
                _context.Add(tandaTangan);
                await _context.SaveChangesAsync();
                TempData["status"] = "create";
                string link = Url.Action("Index");
                return Json(new { success = true, url = link });
            }
            ViewData["IdDokumen"] = new SelectList(_context.RefDokumen, "IdDokumen", "Dokumen", tandaTangan.IdDokumen);
            ViewData["IdPegawai"] = new SelectList(_context.Pegawai, "IdPegawai", "Nama", tandaTangan.IdPegawai);
            return PartialView(tandaTangan);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tandaTangan = await _context.TandaTangan.Include(d => d.IdDokumenNavigation).FirstOrDefaultAsync(d => d.IdTtd == id);
            if (tandaTangan == null)
            {
                return NotFound();
            }
            ViewData["IdPegawai"] = new SelectList(_context.Pegawai.Where(d => d.Nama != "Developers"), "IdPegawai", "Nama", tandaTangan.IdPegawai);
            return PartialView(tandaTangan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdTtd,IdDokumen,IdPegawai")] TandaTangan tandaTangan)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var dok = _context.TandaTangan.Find(id);
                    dok.IdPegawai = tandaTangan.IdPegawai;
                    _context.TandaTangan.Update(dok);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TandaTanganExists(tandaTangan.IdTtd))
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
            ViewData["IdPegawai"] = new SelectList(_context.Pegawai.Where(d => d.Nama != "Developers"), "IdPegawai", "Nama", tandaTangan.IdPegawai);
            return PartialView(tandaTangan);
        }

        private bool TandaTanganExists(Guid id)
        {
            return _context.TandaTangan.Any(e => e.IdTtd == id);
        }
    }
}
