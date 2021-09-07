using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Attributes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIP.Models;

namespace SIP.Controllers
{
    public class PemdaController : Controller
    {
        private readonly DB_NewContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public PemdaController(DB_NewContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        [Auth(new string[] { "Developers", "Setting" })]
        public async Task<IActionResult> Index()
        {
            var pemda = await _context.Pemda.Include(p => p.IndKabKota).Include(p => p.IndKecamatan).Include(p => p.IndKelurahan).Include(p => p.IndProvinsi).FirstOrDefaultAsync();

            //Link
            ViewBag.L = Url.Action("Index");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";
            ViewBag.S1 = pemda.NamaPemda;

            return View(pemda);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pemda = await _context.Pemda.FindAsync(id);
            if (pemda == null)
            {
                return NotFound();
            }
            ViewData["IndProvinsiId"] = new SelectList(_context.IndProvinsi, "IndProvinsiId", "Provinsi", pemda.IndProvinsiId);
            ViewData["IndKabKotaId"] = new SelectList(_context.IndKabKota.Where(d => d.IndProvinsiId == pemda.IndProvinsiId), "IndKabKotaId", "KabKota", pemda.IndKabKotaId);
            ViewData["IndKecamatanId"] = new SelectList(_context.IndKecamatan.Where(d => d.IndKabKotaId == pemda.IndKabKotaId), "IndKecamatanId", "Kecamatan", pemda.IndKecamatanId);
            ViewData["IndKelurahanId"] = new SelectList(_context.IndKelurahan.Where(d => d.IndKecamatanId == pemda.IndKecamatanId), "IndKelurahanId", "Kelurahan", pemda.IndKelurahanId);
            return View(pemda);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Pemda pemda, Guid id)
        {
            if (id != pemda.IdPemda)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var files = HttpContext.Request.Form.Files;
                    foreach (var Image in files)
                    {
                        if (Image != null && Image.Length > 0)
                        {
                            var file = Image;
                            var uploads = Path.Combine(_appEnvironment.WebRootPath, "media\\logos");
                            if (file.Length > 0)
                            {
                                var fileName = "logo.png";
                                using var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create);
                                await file.CopyToAsync(fileStream);
                                pemda.Logo = fileName;

                            }
                        }
                    }
                    pemda.NamaPemda = pemda.NamaPemda;
                    pemda.Alamat = pemda.Alamat;
                    pemda.Telp = pemda.Telp;
                    pemda.Telp2 = pemda.Telp2;
                    pemda.IndProvinsiId = pemda.IndProvinsiId;
                    pemda.IndKabKotaId = pemda.IndKabKotaId;
                    pemda.IndKecamatanId = pemda.IndKecamatanId;
                    pemda.IndKelurahanId = pemda.IndKelurahanId;
                    pemda.KodePos = pemda.KodePos;
                    pemda.NamaOpd = pemda.NamaOpd;

                    var kec = _context.IndKecamatan.Find(pemda.IndKecamatanId);
                    var kel = _context.IndKelurahan.Find(pemda.IndKelurahanId);

                    pemda.NoUrutKecamatan = kec.NoUrutKecamatan;
                    pemda.NoUrutKelurahan = kel.NoUrutKelurahan;
                    pemda.Eu = HttpContext.Session.GetString("User");
                    pemda.Ed = DateTime.Now;
                    _context.Update(pemda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PemdaExists(pemda.IdPemda))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IndProvinsiId"] = new SelectList(_context.IndProvinsi, "IndProvinsiId", "Provinsi", pemda.IndProvinsiId);
            ViewData["IndKabKotaId"] = new SelectList(_context.IndKabKota.Where(d => d.IndProvinsiId == pemda.IndProvinsiId), "IndKabKotaId", "KabKota", pemda.IndKabKotaId);
            ViewData["IndKecamatanId"] = new SelectList(_context.IndKecamatan.Where(d => d.IndKabKotaId == pemda.IndKabKotaId), "IndKecamatanId", "Kecamatan", pemda.IndKecamatanId);
            ViewData["IndKelurahanId"] = new SelectList(_context.IndKelurahan.Where(d => d.IndKecamatanId == pemda.IndKecamatanId), "IndKelurahanId", "Kelurahan", pemda.IndKelurahanId);
            return View(pemda);
        }

        private bool PemdaExists(Guid id)
        {
            return _context.Pemda.Any(e => e.IdPemda == id);
        }
    }
}
