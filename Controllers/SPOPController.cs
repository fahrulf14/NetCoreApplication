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
using SIP.ViewModels;

namespace SIP.Controllers
{
    public class SpopController : Controller
    {
        private readonly DB_NewContext _context;

        public SpopController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "PBB" })]
        public IActionResult Index()
        {
            //Link
            ViewBag.L = Url.Action("Index");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var pemda = _context.Pemda.FirstOrDefault();
            ViewBag.Kecamatan = _context.IndKecamatan.Where(d => d.IndKabKotaId == pemda.IndKabKotaId).ToList();

            return View();
        }

        public JsonResult GetSPOP(JQueryDataTableParamModel param)
        {
            var allRecords = _context.Spop.Include(d => d.IdSubjekNavigation); //All records
            IEnumerable<Spop> filteredRecords;

            //Check whether the users should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //filteredRecords = repository.Labs
                filteredRecords = allRecords.Where(d => d.IdSubjekNavigation.Nama.ToLower().Contains(param.sSearch.ToLower())
                || d.IdSubjekNavigation.Npwpd.Replace(".", "").Contains(param.sSearch)
                || d.Nop.Replace(".", "").Contains(param.sSearch));
            }
            else
            {
                filteredRecords = allRecords;
            }

            if (param.status.HasValue)
            {
                filteredRecords = filteredRecords.Where(d => d.FlagValidasi == param.status);
            }

            if (!string.IsNullOrEmpty(param.tipe))
            {
                filteredRecords = filteredRecords.Where(d => d.IndKecamatanId.ToString() == param.tipe);
            }

            if (!string.IsNullOrEmpty(param.tipe2))
            {
                filteredRecords = filteredRecords.Where(d => d.IndKelurahanId.ToString() == param.tipe2);
            }

            Func<Spop, string> orderingFunction = (c => param.iSortCol_0 == 1 ? c.IdSubjekNavigation.Nama :
                                                        param.iSortCol_0 == 2 ? c.IdSubjekNavigation.Npwpd :
                                                        param.iSortCol_0 == 3 ? c.Nop :
                                                        param.iSortCol_0 == 4 ? c.Tahun.ToString() :
                                                    "");

            if (param.sSortDir_0 == "asc")
                filteredRecords = filteredRecords.OrderBy(orderingFunction);
            else
                filteredRecords = filteredRecords.OrderByDescending(orderingFunction);

            var displayedRecords = filteredRecords.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var data = (from o in displayedRecords
                        join k in _context.IndKecamatan on o.IndKecamatanId equals k.IndKecamatanId
                        join l in _context.IndKelurahan on o.IndKelurahanId equals l.IndKelurahanId
                        join s in _context.DataSubjek on o.IdSubjek equals s.IdSubjek
                        join p in _context.Sppt on o.IdSpop equals p.IdSpop into spop
                        from p in spop.DefaultIfEmpty()
                        select new
                        {
                            id = o.IdSpop,
                            ids = s.IdSubjek,
                            nop = o.Nop,
                            nama = s.Nama,
                            npwpd = s.Npwpd,
                            lokasi = o.NamaJalan,
                            kecamatan = k.Kecamatan,
                            kelurahan = l.Kelurahan,
                            status = (o.FlagValidasi ? "1" : "0"),
                            tahun = DateTime.Now.Year.ToString(),
                            sppt = spop.OrderByDescending(d => d.Tahun).FirstOrDefault()?.Tahun.ToString() ?? "-"
                        }).Distinct().ToArray();
            return Json(new
            {
                param.sEcho,
                iTotalRecords = allRecords.Count(),
                iTotalDisplayRecords = filteredRecords.Count(),
                aaData = data
            });
        }

        [Auth(new string[] { "Developers", "PBB" })]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spop = await _context.Spop
                .Include(s => s.IdSubjekNavigation).ThenInclude(s => s.IndKabKota)
                .Include(s => s.IdSubjekNavigation).ThenInclude(s => s.IndKecamatan)
                .Include(s => s.IdSubjekNavigation).ThenInclude(s => s.IndKelurahan)
                .Include(s => s.IndKabKota)
                .Include(s => s.IndKecamatan)
                .Include(s => s.IndKelurahan)
                .FirstOrDefaultAsync(m => m.IdSpop == id);
            if (spop == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("SPOP", "PBB", new { id = spop.IdSubjek });
            ViewBag.L1 = Url.Action("Details", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View(spop);
        }

        [Auth(new string[] { "Developers", "PBB" })]
        public IActionResult Create(Guid? id)
        {
            //Link
            ViewBag.L = Url.Action("SPOP", "PBB", new { id });
            ViewBag.L1 = Url.Action("Create", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var pemda = _context.Pemda.First();

            List<SelectListItem> JT = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Tanah + Bangunan", Value = "Tanah + Bangunan" },
                new SelectListItem() { Text = "Kavling Siap Bangun", Value = "Kavling Siap Bangun" },
                new SelectListItem() { Text = "Tanah Kosong", Value = "Tanah Kosong" },
                new SelectListItem() { Text = "Fasilitas Umum", Value = "Fasilitas Umum" },
                new SelectListItem() { Text = "Tanah Perairan", Value = "Tanah Perairan" }
            };
            ViewBag.JenisTanah = new SelectList(JT, "Value", "Text");

            ViewBag.IdSubjek = id;
            ViewData["IndKecamatanId"] = new SelectList(_context.IndKecamatan.Where(d => d.IndKabKotaId == pemda.IndKabKotaId), "IndKecamatanId", "Kecamatan");
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, [Bind("IdSpop,IdSubjek,IdCoa,Tanggal,JatuhTempo,Tahun,Nomor,NoFormulir,Nop,IndKabKotaId,IndKecamatanId,IndKelurahanId,NamaJalan,Rt,Rw,BlokKavNo,KodePos,LuasTanah,JenisTanah,Zona,JumlahBangunan,Njop,FlagValidasi,Eu,Ed")] Spop spop)
        {
            var pemda = _context.Pemda.AsNoTracking().FirstOrDefault();
            if (ModelState.IsValid)
            {
                spop.IdSpop = Guid.NewGuid();
                spop.IdSubjek = id;
                spop.IndKabKotaId = pemda.IndKabKotaId;
                spop.IdCoa = "4111201";
                spop.Tanggal = DateTime.Now;
                spop.Tahun = DateTime.Now.Year;
                spop.Eu = HttpContext.Session.GetString("User");
                spop.Ed = DateTime.Now;
                _context.Add(spop);
                if (spop.JumlahBangunan == 0)
                {
                    Lspop lspop0 = new Lspop
                    {
                        IdLspop = Guid.NewGuid(),
                        IdSpop = spop.IdSpop,
                        IdJenis = 10,
                        Luas = 0,
                        JumlahLantai = 0,
                        Njop = 0,
                        Zona = ""
                    };
                    _context.Lspop.Add(lspop0);
                }
                else
                {
                    Lspop lspop = new Lspop
                    {
                        IdLspop = Guid.NewGuid(),
                        IdSpop = spop.IdSpop
                    };
                    _context.Lspop.Add(lspop);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("NOP", new { id = spop.IdSpop, redirect = "LSPOP" });
            }
            List<SelectListItem> JT = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Tanah + Bangunan", Value = "Tanah + Bangunan" },
                new SelectListItem() { Text = "Kavling Siap Bangun", Value = "Kavling Siap Bangun" },
                new SelectListItem() { Text = "Tanah Kosong", Value = "Tanah Kosong" },
                new SelectListItem() { Text = "Fasilitas Umum", Value = "Fasilitas Umum" },
                new SelectListItem() { Text = "Tanah Perairan", Value = "Tanah Perairan" }
            };
            ViewBag.JenisTanah = new SelectList(JT, "Value", "Text", spop.JenisTanah);

            ViewData["IndKecamatanId"] = new SelectList(_context.IndKecamatan.Where(d => d.IndKabKotaId == pemda.IndKabKotaId), "IndKecamatanId", "Kecamatan", spop.IndKecamatanId);
            ViewData["IndKelurahanId"] = new SelectList(_context.IndKelurahan.Where(d => d.IndKecamatanId == spop.IndKecamatanId), "IndKelurahanId", "Kelurahan", spop.IndKelurahanId);
            return View(spop);
        }

        [Auth(new string[] { "Developers", "PBB" })]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spop = await _context.Spop.FindAsync(id);
            if (spop == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("SPOP", "PBB", new { id = spop.IdSubjek });
            ViewBag.L1 = Url.Action("Edit", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            List<SelectListItem> JT = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Tanah + Bangunan", Value = "Tanah + Bangunan" },
                new SelectListItem() { Text = "Kavling Siap Bangun", Value = "Kavling Siap Bangun" },
                new SelectListItem() { Text = "Tanah Kosong", Value = "Tanah Kosong" },
                new SelectListItem() { Text = "Fasilitas Umum", Value = "Fasilitas Umum" },
                new SelectListItem() { Text = "Tanah Perairan", Value = "Tanah Perairan" }
            };
            ViewBag.JenisTanah = new SelectList(JT, "Value", "Text", spop.JenisTanah);

            ViewData["IndKecamatanId"] = new SelectList(_context.IndKecamatan.Where(d => d.IndKabKotaId == spop.IndKabKotaId), "IndKecamatanId", "Kecamatan", spop.IndKecamatan);
            ViewData["IndKelurahanId"] = new SelectList(_context.IndKelurahan.Where(d => d.IndKecamatanId == spop.IndKecamatanId), "IndKelurahanId", "Kelurahan", spop.IndKelurahan);
            return View(spop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdSpop,IdSubjek,IdCoa,Tanggal,JatuhTempo,Tahun,Nomor,NoFormulir,Nop,IndKabKotaId,IndKecamatanId,IndKelurahanId,NamaJalan,Rt,Rw,BlokKavNo,KodePos,LuasTanah,JenisTanah,Zona,JumlahBangunan,Njop,FlagValidasi,Eu,Ed")] Spop spop)
        {
            if (id != spop.IdSpop)
            {
                return NotFound();
            }

            var old = await _context.Spop.FirstOrDefaultAsync(d => d.IdSpop == id);
            bool change = false;

            if (ModelState.IsValid)
            {
                try
                {
                    if (old.IndKecamatanId != spop.IndKecamatanId || old.IndKelurahanId != spop.IndKelurahanId)
                    {
                        change = true;
                        old.Nop = null;
                    }
                    old.NoFormulir = spop.NoFormulir;
                    old.NamaJalan = spop.NamaJalan;
                    old.Rt = spop.Rt;
                    old.Rw = spop.Rw;
                    old.IndKecamatanId = spop.IndKecamatanId;
                    old.IndKelurahanId = spop.IndKelurahanId;
                    old.LuasTanah = spop.LuasTanah;
                    old.JumlahBangunan = spop.JumlahBangunan;
                    old.JenisTanah = spop.JenisTanah;
                    old.KodePos = spop.KodePos;
                    old.Njop = spop.Njop;
                    old.Zona = spop.Zona;

                    old.Eu = HttpContext.Session.GetString("User");
                    old.Ed = DateTime.Now;
                    _context.Spop.Update(old);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SPOPExists(spop.IdSpop))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (change)
                {
                    return RedirectToAction("NOP", new { id });
                }
                return RedirectToAction("SPOP", "PBB", new { id = old.IdSubjek });
            }
            List<SelectListItem> JT = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Tanah + Bangunan", Value = "Tanah + Bangunan" },
                new SelectListItem() { Text = "Kavling Siap Bangun", Value = "Kavling Siap Bangun" },
                new SelectListItem() { Text = "Tanah Kosong", Value = "Tanah Kosong" },
                new SelectListItem() { Text = "Fasilitas Umum", Value = "Fasilitas Umum" },
                new SelectListItem() { Text = "Tanah Perairan", Value = "Tanah Perairan" }
            };
            ViewBag.JenisTanah = new SelectList(JT, "Value", "Text", spop.JenisTanah);

            ViewData["IndKecamatanId"] = new SelectList(_context.IndKecamatan.Where(d => d.IndKabKotaId == old.IndKabKotaId), "IndKecamatanId", "Kecamatan", spop.IndKecamatan);
            ViewData["IndKelurahanId"] = new SelectList(_context.IndKelurahan.Where(d => d.IndKecamatanId == spop.IndKecamatanId), "IndKelurahanId", "Kelurahan", spop.IndKelurahan);
            return View(spop);
        }

        [Auth(new string[] { "Developers", "PBB" })]
        public async Task<IActionResult> NOP(Guid? id, string redirect)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spop = await _context.Spop.Include(s => s.IndKelurahan).FirstOrDefaultAsync(s => s.IdSpop == id);

            if (spop == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("SPOP", "PBB", new { id = spop.IdSubjek });
            ViewBag.L1 = Url.Action("NOP", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var prov = spop.IndKelurahan.KodeKelurahan.Substring(0, 2);
            var kab = spop.IndKelurahan.KodeKelurahan.Substring(2, 2);
            var kec = "0" + spop.IndKelurahan.KodeKelurahan.Substring(4, 2);
            var kel = spop.IndKelurahan.KodeKelurahan.Substring(7, 3);

            ViewBag.NOP = prov + "." + kab + "." + kec + "." + kel + ".";
            ViewBag.Current = spop.Nop;

            TempData["redirect"] = redirect;

            return View(spop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NOP(Guid id, [Bind("IdSpop,Nop,Eu,Ed")] Spop spop)
        {
            if (id != spop.IdSpop)
            {
                return NotFound();
            }

            var nop = _context.Spop.AsNoTracking().FirstOrDefault(d => d.Nop == spop.Nop);
            var old = _context.Spop.FirstOrDefault(d => d.IdSpop == id);

            if(nop != null && spop.Nop != old.Nop)
            {
                TempData["status"] = "nop";
                return RedirectToAction("NOP", new { id });
            }

            if (spop.Nop.Length < 24)
            {
                TempData["status"] = "nopsalah";
                return RedirectToAction("NOP", new { id });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    old.Nop = spop.Nop;
                    old.Eu = HttpContext.Session.GetString("User");
                    old.Ed = DateTime.Now;
                    _context.Update(old);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SPOPExists(spop.IdSpop))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var redirect = TempData["redirect"]?.ToString();
                if (redirect != null)
                {
                    var lspop = _context.Lspop.FirstOrDefault(d => d.IdSpop == id);
                    return RedirectToAction("Edit", redirect, new { id = lspop.IdLspop });
                }
                TempData["status"] = "edit";
                return RedirectToAction("SPOP", "PBB", new { id = old.IdSubjek });
            }
            return View(spop);
        }

        [Auth(new string[] { "Developers", "PBB" })]
        public async Task<IActionResult> NJOP(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spop = await _context.Spop.FirstOrDefaultAsync(s => s.IdSpop == id);

            //Link
            ViewBag.L = Url.Action("SPOP", "PBB", new { id = spop.IdSubjek });
            ViewBag.L1 = Url.Action("NJOP", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            if (spop == null)
            {
                return NotFound();
            }

            return View(spop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NJOP(Guid id, [Bind("IdSpop,Njop,Eu,Ed")] Spop spop)
        {
            if (id != spop.IdSpop)
            {
                return NotFound();
            }

            var old = _context.Spop.FirstOrDefault(d => d.IdSpop == id);

            if (ModelState.IsValid)
            {
                try
                {
                    old.Eu = HttpContext.Session.GetString("User");
                    old.Ed = DateTime.Now;
                    old.Ed = DateTime.Now;
                    _context.Update(old);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SPOPExists(spop.IdSpop))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "edit";
                return RedirectToAction("SPOP", "PBB", new { id = old.IdSubjek });
            }
            return View(spop);
        }

        [Auth(new string[] { "Developers", "Validasi" })]
        public async Task<IActionResult> Validasi(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spop = await _context.Spop
                .Include(s => s.IdSubjekNavigation).ThenInclude(s => s.IndKabKota)
                .Include(s => s.IdSubjekNavigation).ThenInclude(s => s.IndKecamatan)
                .Include(s => s.IdSubjekNavigation).ThenInclude(s => s.IndKelurahan)
                .Include(s => s.IndKabKota)
                .Include(s => s.IndKecamatan)
                .Include(s => s.IndKelurahan)
                .FirstOrDefaultAsync(m => m.IdSpop == id);
            if (spop == null)
            {
                return NotFound();
            }

            var lspop = _context.Lspop.Where(d => d.IdSpop == id && d.FlagValidasi == false).Count();
            if (lspop > 0)
            {
                TempData["status"] = "validlspoperror";
                return RedirectToAction("SPOP", "PBB", new { id = spop.IdSubjek });
            }

            var sppt = _context.Sppt.FirstOrDefault(d => d.IdSpop == id);
            if (sppt != null && spop.FlagValidasi)
            {
                TempData["status"] = "validbatalerror";
                return RedirectToAction("SPOP", "PBB", new { id = spop.IdSubjek });
            }

            //Link
            ViewBag.L = Url.Action("SPOP", "PBB", new { id = spop.IdSubjek });
            ViewBag.L1 = Url.Action("Validasi", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View(spop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Validasi(Guid id, Spop spop)
        {
            if (id != spop.IdSpop)
            {
                return NotFound();
            }

            var old = _context.Spop.FirstOrDefault(d => d.IdSpop == id);

            old.Eu = HttpContext.Session.GetString("User");
            old.Ed = DateTime.Now;

            if (old.FlagValidasi)
            {
                old.FlagValidasi = false;
                _context.Update(old);
                await _context.SaveChangesAsync();
                TempData["status"] = "validbatal";
                return RedirectToAction("SPOP", "PBB", new { id = old.IdSubjek });
            }
            else
            {
                old.FlagValidasi = true;
                _context.Update(old);
                await _context.SaveChangesAsync();
                TempData["status"] = "valid";
                return RedirectToAction("SPOP", "PBB", new { id = old.IdSubjek });
            }
        }

        [Auth(new string[] { "Developers", "PBB" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spop = await _context.Spop
                .FirstOrDefaultAsync(m => m.IdSpop == id);
            if (spop == null)
            {
                return NotFound();
            }

            if (spop.FlagValidasi)
            {
                return PartialView("_deletevalid");
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var spop = await _context.Spop.FindAsync(id);
            try
            {
                _context.Spop.Remove(spop);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["status"] = "deletefailed";
                string url = Url.Action("SPOP", "PBB", new { id = spop.IdSubjek });
                return Json(new { success = true, url });
            }
            
            TempData["status"] = "delete";
            string link = Url.Action("SPOP", "PBB", new { id = spop.IdSubjek });
            return Json(new { success = true, url = link });
        }

        private bool SPOPExists(Guid id)
        {
            return _context.Spop.Any(e => e.IdSpop == id);
        }

    }
}
