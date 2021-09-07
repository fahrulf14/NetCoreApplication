using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIP.Models;
using SIP.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Attributes;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace SIP.Controllers
{
    public class PendaftaranController : Controller
    {
        private readonly DB_NewContext _context;

        private readonly List<SelectListItem> jk = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Laki-Laki", Value = "L" },
                new SelectListItem() { Text = "Perempuan", Value = "P" }
            };
        private readonly List<SelectListItem> jabatan = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Pemilik", Value = "Pemilik" },
                new SelectListItem() { Text = "Pengelola", Value = "Pengelola" }
            };

        public PendaftaranController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "Pendaftaran" })]
        public IActionResult Pribadi()
        {
            //Link
            ViewBag.L = Url.Action("Pribadi", null);
            ViewBag.L1 = Url.Action("Pribadi", null);
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            ViewBag.Pekerjaan = _context.RefPekerjaan.ToList();

            return View();
        }

        [Auth(new string[] { "Developers", "Pendaftaran" })]
        public IActionResult Badan()
        {
            //Link
            ViewBag.L = Url.Action("Badan", null);
            ViewBag.L1 = Url.Action("Badan", null);
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            ViewBag.BadanHukum = _context.RefBadanHukum.ToList();

            return View();
        }

        [Auth(new string[] { "Developers", "Pendaftaran" })]
        public async Task<IActionResult> DataUsaha(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("DataUsaha", new { id });
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            ViewBag.IdSubjek = id;
            var data = _context.DataUsaha.Include(d => d.IdJenisNavigation).Include(d => d.IndKecamatan).Include(d => d.Sptpd).Where(d => d.IdSubjek == id).ToListAsync();

            return View(await data);
        }

        [Auth(new string[] { "Developers", "Pendaftaran" })]
        [AjaxOnly]
        public async Task<IActionResult> NPWPD(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = await _context.DataSubjek.FindAsync(id);
            if (data == null)
            {
                return NotFound();
            }

            return PartialView(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NPWPD(Guid id, DataSubjek dataSubjek)
        {
            if (id != dataSubjek.IdSubjek)
            {
                return NotFound();
            }

            var old = _context.DataSubjek.Find(id);
            if (old.Npwpd != null)
            {
                old.Npwrd = old.Npwpd.Replace("P", "R");
            }
            if (old.Npwrd != null)
            {
                old.Npwpd = old.Npwrd.Replace("R", "P");
            }
            old.Eu = HttpContext.Session.GetString("User");
            old.Ed = DateTime.Now;
            _context.DataSubjek.Update(old);
            await _context.SaveChangesAsync();
            TempData["status"] = "edit";
            string link = Url.Action("DataUsaha", new { id });
            return Json(new { success = true, url = link });
        }

        [Auth(new string[] { "Developers", "Pendaftaran" })]
        public IActionResult Create(string id, string jenis)
        {
            //Link
            ViewBag.Title = "Pendaftaran Wajib " + jenis + " " + id;
            ViewBag.L = Url.Action("Index", "Pendaftaran", null);
            ViewBag.L1 = Url.Action(id, "Pendaftaran", null);
            ViewBag.L2 = Url.Action("Create", "Pendaftaran", new { id, jenis });
            ViewBag.L3 = "";

            ViewData["IndProvinsiId"] = new SelectList(_context.IndProvinsi, "IndProvinsiId", "Provinsi");
            ViewBag.Jenis = jenis;

            if (id == "Pribadi")
            {
                ViewData["IdPekerjaan"] = new SelectList(_context.RefPekerjaan, "IdPekerjaan", "Pekerjaan");
                ViewData["Kelamin"] = new SelectList(jk, "Value", "Text");

                return View("FormPribadi");
            }
            else if (id == "Badan")
            {
                ViewData["IdBadanHukum"] = new SelectList(_context.RefBadanHukum, "IdBadanHukum", "BadanHukum"); 

                return View("FormBadan");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string id, string jenis, [Bind("IdSubjek,Nama,Alamat,Rtrw,IndProvinsiId,IndKabKotaId,IndKecamatanId,IndKelurahanId,NoTelp,KodePos,Email,Nik,Npwp,Kelamin,TglLahir,IdPekerjaan,PekerjaanLain,NamaInstansi,IdBadanHukum,TglDaftar,NoPokok,Npwpd,Npwrd,Status,Eu,Ed")] DataSubjek dataSubjek)
        {
            if (ModelState.IsValid)
            {
                var npwp = _context.DataSubjek.Where(d => d.Npwp == dataSubjek.Npwp && dataSubjek.Npwp != null).Count();
                var nik = _context.DataSubjek.Where(d => d.Nik == dataSubjek.Nik && dataSubjek.Nik != null).Count();

                if (npwp != 0 || nik != 0)
                {
                    //Link
                    ViewBag.Title = "Pendaftaran Wajib " + jenis + " " + id;
                    ViewBag.L = Url.Action("Index", "Pendaftaran", null);
                    ViewBag.L1 = Url.Action(id, "Pendaftaran", null);
                    ViewBag.L2 = Url.Action("Create", "Pendaftaran", new { id, jenis });
                    ViewBag.L3 = "";

                    if (npwp != 0) { TempData["status"] = "npwp"; }
                    if (nik != 0) { TempData["status"] = "nik"; }

                    ViewData["IndProvinsiId"] = new SelectList(_context.IndProvinsi, "IndProvinsiId", "Provinsi", dataSubjek.IndProvinsiId);
                    ViewData["IndKabKotaId"] = new SelectList(_context.IndKabKota.Where(d => d.IndProvinsiId == dataSubjek.IndProvinsiId), "IndKabKotaId", "KabKota", dataSubjek.IndKabKotaId);
                    ViewData["IndKecamatanId"] = new SelectList(_context.IndKecamatan.Where(d => d.IndKabKotaId == dataSubjek.IndKabKotaId), "IndKecamatanId", "Kecamatan", dataSubjek.IndKecamatanId);
                    ViewData["IndKelurahanId"] = new SelectList(_context.IndKelurahan.Where(d => d.IndKecamatanId == dataSubjek.IndKecamatanId), "IndKelurahanId", "Kelurahan", dataSubjek.IndKelurahanId);

                    ViewBag.Jenis = jenis;
                    ViewBag.Id = id;
                    ViewBag.Act = "Create";

                    if (id == "Pribadi")
                    {
                        ViewData["IdPekerjaan"] = new SelectList(_context.RefPekerjaan, "IdPekerjaan", "Pekerjaan", dataSubjek.IdPekerjaan);
                        ViewData["Kelamin"] = new SelectList(jk, "Value", "Text", dataSubjek.Kelamin);
                        return View("EditPribadi", dataSubjek);
                    }
                    else if (id == "Badan")
                    {
                        ViewData["IdBadanHukum"] = new SelectList(_context.RefBadanHukum, "IdBadanHukum", "BadanHukum", dataSubjek.IdBadanHukum);
                        return View("EditBadan", dataSubjek);
                    }
                    return View(dataSubjek);
                }

                var pemda = _context.Pemda.First();

                var Jenis = "";
                if (id == "Pribadi") { Jenis = "1"; }
                else if (id == "Badan") { Jenis = "2"; }

                dataSubjek.IdSubjek = Guid.NewGuid();
                dataSubjek.Status = true;
                var No = _context.DataSubjek.Select(e => e.NoPokok).Max() ?? 0;
                dataSubjek.NoPokok = No + 1;

                if (jenis == "Pajak")
                {
                    dataSubjek.Npwpd = "P." + Jenis + "." + string.Format("{0:000000}", dataSubjek.NoPokok) + "." + pemda.NoUrutKecamatan + "." + pemda.NoUrutKelurahan;
                }
                else
                {
                    dataSubjek.Npwrd = "R." + Jenis + "." + string.Format("{0:000000}", dataSubjek.NoPokok) + "." + pemda.NoUrutKecamatan + "." + pemda.NoUrutKelurahan;
                }
                _context.Add(dataSubjek);
                await _context.SaveChangesAsync();

                if (id == "Pribadi")
                {
                    TempData["status"] = "create";
                    return RedirectToAction("DataUsaha", new { id = dataSubjek.IdSubjek });
                }
                else
                {
                    DataBadan dataBadan = new DataBadan
                    {
                        IdBadan = Guid.NewGuid(),
                        IdSubjek = dataSubjek.IdSubjek
                    };
                    _context.Add(dataBadan);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("DataTambahan", new { id = dataBadan.IdBadan });
                }
            }

            //Link
            ViewBag.Title = "Pendaftaran Wajib " + jenis + " " + id;
            ViewBag.L = Url.Action("Index", "Pendaftaran", null);
            ViewBag.L1 = Url.Action(id, "Pendaftaran", null);
            ViewBag.L2 = Url.Action("Create", "Pendaftaran", new { id, jenis });
            ViewBag.L3 = "";

            ViewData["IndProvinsiId"] = new SelectList(_context.IndProvinsi, "IndProvinsiId", "Provinsi", dataSubjek.IndProvinsiId);
            ViewData["IndKabKotaId"] = new SelectList(_context.IndKabKota.Where(d => d.IndProvinsiId == dataSubjek.IndProvinsiId), "IndKabKotaId", "KabKota", dataSubjek.IndKabKotaId);
            ViewData["IndKecamatanId"] = new SelectList(_context.IndKecamatan.Where(d => d.IndKabKotaId == dataSubjek.IndKabKotaId), "IndKecamatanId", "Kecamatan", dataSubjek.IndKecamatanId);
            ViewData["IndKelurahanId"] = new SelectList(_context.IndKelurahan.Where(d => d.IndKecamatanId == dataSubjek.IndKecamatanId), "IndKelurahanId", "Kelurahan", dataSubjek.IndKelurahanId);

            ViewBag.Jenis = jenis;
            ViewBag.Id = id;
            ViewBag.Act = "Create";

            if (id == "Pribadi")
            {
                ViewData["IdPekerjaan"] = new SelectList(_context.RefPekerjaan, "IdPekerjaan", "Pekerjaan", dataSubjek.IdPekerjaan);
                ViewData["Kelamin"] = new SelectList(jk, "Value", "Text", dataSubjek.Kelamin);
                return View("EditPribadi", dataSubjek);
            }
            else
            {
                ViewData["IdBadanHukum"] = new SelectList(_context.RefBadanHukum, "IdBadanHukum", "BadanHukum", dataSubjek.IdBadanHukum);
                return View("EditBadan", dataSubjek);
            }
        }

        [Auth(new string[] { "Developers", "Pendaftaran" })]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataSubjek = await _context.DataSubjek.FindAsync(id);
            if (dataSubjek == null)
            {
                return NotFound();
            }

            var subjek = (dataSubjek.IdBadanHukum == null) ? "Pribadi" : "Badan";
            var jenis = (dataSubjek.Npwpd != null) ? "Pajak" : "Retribusi";

            //Link
            ViewBag.Title = "Edit Wajib " + jenis + " " + subjek;
            ViewBag.L = Url.Action("Index", "Pendaftaran", null);
            ViewBag.L1 = Url.Action(subjek, "Pendaftaran", null);
            ViewBag.L2 = Url.Action("Edit", "Pendaftaran", new { id });
            ViewBag.L3 = "";

            ViewData["IndProvinsiId"] = new SelectList(_context.IndProvinsi, "IndProvinsiId", "Provinsi", dataSubjek.IndProvinsiId);
            ViewData["IndKabKotaId"] = new SelectList(_context.IndKabKota.Where(d => d.IndProvinsiId == dataSubjek.IndProvinsiId), "IndKabKotaId", "KabKota", dataSubjek.IndKabKotaId);
            ViewData["IndKecamatanId"] = new SelectList(_context.IndKecamatan.Where(d => d.IndKabKotaId == dataSubjek.IndKabKotaId), "IndKecamatanId", "Kecamatan", dataSubjek.IndKecamatanId);
            ViewData["IndKelurahanId"] = new SelectList(_context.IndKelurahan.Where(d => d.IndKecamatanId == dataSubjek.IndKecamatanId), "IndKelurahanId", "Kelurahan", dataSubjek.IndKelurahanId);

            TempData["NoPokok"] = dataSubjek.NoPokok;
            TempData["Npwpd"] = dataSubjek.Npwpd;
            TempData["Npwrd"] = dataSubjek.Npwrd;
            TempData["Npwp"] = dataSubjek.Npwp;
            TempData["Nik"] = dataSubjek.Nik;

            ViewBag.Id = id;
            ViewBag.Act = "Edit";

            if (subjek == "Pribadi")
            {
                ViewData["IdPekerjaan"] = new SelectList(_context.RefPekerjaan, "IdPekerjaan", "Pekerjaan", dataSubjek.IdPekerjaan);
                ViewData["Kelamin"] = new SelectList(jk, "Value", "Text", dataSubjek.Kelamin);

                return View("EditPribadi", dataSubjek);
            }
            else if (subjek == "Badan")
            {
                ViewData["IdBadanHukum"] = new SelectList(_context.RefBadanHukum, "IdBadanHukum", "BadanHukum", dataSubjek.IdBadanHukum);

                return View("EditBadan", dataSubjek);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdSubjek,Nama,Alamat,Rtrw,IndProvinsiId,IndKabKotaId,IndKecamatanId,IndKelurahanId,NoTelp,KodePos,Email,Nik,Npwp,Kelamin,TglLahir,IdPekerjaan,PekerjaanLain,NamaInstansi,IdBadanHukum,TglDaftar,Status,Eu,Ed")] DataSubjek dataSubjek)
        {
            if (id != dataSubjek.IdSubjek)
            {
                return NotFound();
            }

            var subjek = (dataSubjek.IdBadanHukum == null) ? "Pribadi" : "Badan";
            var jenis = (TempData["Npwpd"] != null) ? "Pajak" : "Retribusi";

            if (ModelState.IsValid)
            {
                var npwp = _context.DataSubjek.Where(d => d.Npwp == dataSubjek.Npwp && dataSubjek.Npwp != (string)TempData["Npwp"]).Count();
                var nik = _context.DataSubjek.Where(d => d.Nik == dataSubjek.Nik && dataSubjek.Nik != (string)TempData["Nik"]).Count();

                if (npwp != 0 || nik != 0)
                {
                    //Link
                    ViewBag.Title = "Pendaftaran Wajib " + jenis + " " + subjek;
                    ViewBag.L = Url.Action("Index", "Pendaftaran", null);
                    ViewBag.L1 = Url.Action(subjek, "Pendaftaran", null);
                    ViewBag.L2 = Url.Action("Edit", "Pendaftaran", new { id });
                    ViewBag.L3 = "";

                    if (npwp != 0) { TempData["status"] = "npwp"; }
                    if (nik != 0) { TempData["status"] = "nik"; }

                    ViewData["IndProvinsiId"] = new SelectList(_context.IndProvinsi, "IndProvinsiId", "Provinsi", dataSubjek.IndProvinsiId);
                    ViewData["IndKabKotaId"] = new SelectList(_context.IndKabKota.Where(d => d.IndProvinsiId == dataSubjek.IndProvinsiId), "IndKabKotaId", "KabKota", dataSubjek.IndKabKotaId);
                    ViewData["IndKecamatanId"] = new SelectList(_context.IndKecamatan.Where(d => d.IndKabKotaId == dataSubjek.IndKabKotaId), "IndKecamatanId", "Kecamatan", dataSubjek.IndKecamatanId);
                    ViewData["IndKelurahanId"] = new SelectList(_context.IndKelurahan.Where(d => d.IndKecamatanId == dataSubjek.IndKecamatanId), "IndKelurahanId", "Kelurahan", dataSubjek.IndKelurahanId);

                    ViewBag.Id = id;
                    ViewBag.Act = "Edit";

                    if (subjek == "Pribadi")
                    {
                        ViewData["IdPekerjaan"] = new SelectList(_context.RefPekerjaan, "IdPekerjaan", "Pekerjaan", dataSubjek.IdPekerjaan);
                        ViewData["Kelamin"] = new SelectList(jk, "Value", "Text", dataSubjek.Kelamin);
                        return View("EditPribadi", dataSubjek);
                    }
                    else if (subjek == "Badan")
                    {
                        ViewData["IdBadanHukum"] = new SelectList(_context.RefBadanHukum, "IdBadanHukum", "BadanHukum", dataSubjek.IdBadanHukum);
                        return View("EditBadan", dataSubjek);
                    }
                }
                try
                {
                    dataSubjek.NoPokok = (int)TempData["NoPokok"];
                    dataSubjek.Npwpd = (string)TempData["Npwpd"];
                    dataSubjek.Npwrd = (string)TempData["Npwrd"];
                    dataSubjek.Eu = HttpContext.Session.GetString("User");
                    dataSubjek.Ed = DateTime.Now;
                    _context.Update(dataSubjek);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataSubjekExists(dataSubjek.IdSubjek))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "edit";
                return RedirectToAction("DataUsaha", new { id = dataSubjek.IdSubjek });
            }

            //Link
            ViewBag.Title = "Pendaftaran Wajib " + jenis + " " + subjek;
            ViewBag.L = Url.Action("Index", "Pendaftaran", null);
            ViewBag.L1 = Url.Action(subjek, "Pendaftaran", null);
            ViewBag.L2 = Url.Action("Edit", "Pendaftaran", new { id });
            ViewBag.L3 = "";

            ViewData["IndProvinsiId"] = new SelectList(_context.IndProvinsi, "IndProvinsiId", "Provinsi", dataSubjek.IndProvinsiId);
            ViewData["IndKabKotaId"] = new SelectList(_context.IndKabKota.Where(d => d.IndProvinsiId == dataSubjek.IndProvinsiId), "IndKabKotaId", "KabKota", dataSubjek.IndKabKotaId);
            ViewData["IndKecamatanId"] = new SelectList(_context.IndKecamatan.Where(d => d.IndKabKotaId == dataSubjek.IndKabKotaId), "IndKecamatanId", "Kecamatan", dataSubjek.IndKecamatanId);
            ViewData["IndKelurahanId"] = new SelectList(_context.IndKelurahan.Where(d => d.IndKecamatanId == dataSubjek.IndKecamatanId), "IndKelurahanId", "Kelurahan", dataSubjek.IndKelurahanId);

            ViewBag.Id = id;
            ViewBag.Act = "Edit";

            if (subjek == "Pribadi")
            {
                ViewData["IdPekerjaan"] = new SelectList(_context.RefPekerjaan, "IdPekerjaan", "Pekerjaan", dataSubjek.IdPekerjaan);
                ViewData["Kelamin"] = new SelectList(jk, "Value", "Text", dataSubjek.Kelamin);
                return View("EditPribadi", dataSubjek);
            }
            else if (subjek == "Badan")
            {
                ViewData["IdBadanHukum"] = new SelectList(_context.RefBadanHukum, "IdBadanHukum", "BadanHukum", dataSubjek.IdBadanHukum);
                return View("EditBadan", dataSubjek);
            }
            return View(dataSubjek);
        }

        [Auth(new string[] { "Developers", "Pendaftaran" })]
        public async Task<IActionResult> DataTambahan(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataBadan = await _context.DataBadan.FindAsync(id);
            if (dataBadan == null)
            {
                return NotFound();
            }

            var dataSubjek = _context.DataSubjek.Find(dataBadan.IdSubjek);

            var subjek = (dataSubjek.IdBadanHukum == null) ? "Pribadi" : "Badan";
            var jenis = (dataSubjek.Npwpd != null) ? "Pajak" : "Retribusi";

            //Link
            ViewBag.Title = "Pendaftaran Wajib " + jenis + " " + subjek;
            ViewBag.L = Url.Action("Index", "Pendaftaran", null);
            ViewBag.L1 = Url.Action(subjek, "Pendaftaran", null);
            ViewBag.L2 = Url.Action("DataTambahan", "Pendaftaran", new { id });
            ViewBag.L3 = "";

            ViewData["IndProvinsiId"] = new SelectList(_context.IndProvinsi, "IndProvinsiId", "Provinsi", dataBadan.IndProvinsiId);
            ViewData["IndKabKotaId"] = new SelectList(_context.IndKabKota.Where(d => d.IndProvinsiId == dataBadan.IndProvinsiId), "IndKabKotaId", "KabKota", dataBadan.IndKabKotaId);
            ViewData["IndKecamatanId"] = new SelectList(_context.IndKecamatan.Where(d => d.IndKabKotaId == dataBadan.IndKabKotaId), "IndKecamatanId", "Kecamatan", dataBadan.IndKecamatanId);
            ViewData["IndKelurahanId"] = new SelectList(_context.IndKelurahan.Where(d => d.IndKecamatanId == dataBadan.IndKecamatanId), "IndKelurahanId", "Kelurahan", dataBadan.IndKelurahanId);

            ViewData["Kelamin"] = new SelectList(jk, "Value", "Text", dataBadan.Kelamin);
            ViewData["Jabatan"] = new SelectList(jabatan, "Value", "Text", dataBadan.Jabatan);

            TempData["IdSubjek"] = dataBadan.IdSubjek;

            return View("FormTambahan", dataBadan);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DataTambahan(Guid id, [Bind("IdBadan,IdSubjek,Nama,Alamat,Rtrw,IndProvinsiId,IndKabKotaId,IndKecamatanId,IndKelurahanId,NoTelp,KodePos,Email,Nik,Npwp,Kelamin,TglLahir,Jabatan,PekerjaanLain,Eu,Ed")] DataBadan dataBadan)
        {
            if (id != dataBadan.IdBadan)
            {
                return NotFound();
            }

            var IdSubjek = new Guid(TempData["IdSubjek"].ToString().Replace("-", string.Empty));

            var data = await _context.DataBadan.Include(d => d.IdSubjekNavigation).AsNoTracking().FirstOrDefaultAsync(d => d.IdBadan == id);
            var a = data;
            var subjek = (data.IdSubjekNavigation.IdBadanHukum == null) ? "Pribadi" : "Badan";
            var jenis = (data.IdSubjekNavigation.Npwpd != null) ? "Pajak" : "Retribusi";

            if (ModelState.IsValid)
            {
                try
                {
                    dataBadan.IdSubjek = IdSubjek;
                    dataBadan.Eu = HttpContext.Session.GetString("User");
                    dataBadan.Ed = DateTime.Now;
                    _context.Update(dataBadan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataSubjekExists(dataBadan.IdBadan))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "edit";
                return RedirectToAction("DataUsaha", new { id = IdSubjek });
            }

            //Link
            ViewBag.Title = "Pendaftaran Wajib " + jenis + " " + subjek;
            ViewBag.L = Url.Action("Index", "Pendaftaran", null);
            ViewBag.L1 = Url.Action(subjek, "Pendaftaran", null);
            ViewBag.L2 = Url.Action("DataTambahan", "Pendaftaran", new { id });
            ViewBag.L3 = "";

            ViewData["IndProvinsiId"] = new SelectList(_context.IndProvinsi, "IndProvinsiId", "Provinsi", dataBadan.IndProvinsiId);
            ViewData["IndKabKotaId"] = new SelectList(_context.IndKabKota.Where(d => d.IndProvinsiId == dataBadan.IndProvinsiId), "IndKabKotaId", "KabKota", dataBadan.IndKabKotaId);
            ViewData["IndKecamatanId"] = new SelectList(_context.IndKecamatan.Where(d => d.IndKabKotaId == dataBadan.IndKabKotaId), "IndKecamatanId", "Kecamatan", dataBadan.IndKecamatanId);
            ViewData["IndKelurahanId"] = new SelectList(_context.IndKelurahan.Where(d => d.IndKecamatanId == dataBadan.IndKecamatanId), "IndKelurahanId", "Kelurahan", dataBadan.IndKelurahanId);

            ViewData["Kelamin"] = new SelectList(jk, "Value", "Text", dataBadan.Kelamin);
            ViewData["Jabatan"] = new SelectList(jabatan, "Value", "Text", dataBadan.Jabatan);


            return View(dataBadan);
        }

        [Auth(new string[] { "Developers", "Pendaftaran" })]
        public IActionResult Cetak(Guid? id)
        {
            return RedirectToAction("NPWPD", "Cetak", new { id });
        }

        [Auth(new string[] { "Developers", "Pendaftaran" })]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataSubjek = await _context.DataSubjek
                .FirstOrDefaultAsync(m => m.IdSubjek == id);
            if (dataSubjek == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var dataSubjek = _context.DataSubjek.Find(id);

            var dataBadan = _context.DataBadan.FirstOrDefault(d => d.IdSubjek == id);
            if (dataBadan != null)
            {
                _context.DataBadan.Remove(dataBadan);
            }

            _context.DataSubjek.Remove(dataSubjek);
            _context.SaveChanges();
            TempData["status"] = "delete";
            string link = "";
            if(dataSubjek.IdBadanHukum != null)
            {
                link = Url.Action("Badan");
            }
            if (dataSubjek.IdBadanHukum != null)
            {
                link = Url.Action("Pribadi");
            }

            return Json(new { success = true, url = link });
        }

        private bool DataSubjekExists(Guid id)
        {
            return _context.DataSubjek.Any(e => e.IdSubjek == id);
        }
    }
}
