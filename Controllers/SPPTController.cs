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
    public class SPPTController : Controller
    {
        private readonly DB_NewContext _context;

        public SPPTController(DB_NewContext context)
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

        public JsonResult GetSPPT(JQueryDataTableParamModel param)
        {
            var allRecords = _context.Sppt.Include(d => d.IdSpopNavigation).ThenInclude(d => d.IdSubjekNavigation); //All records
            IEnumerable<Sppt> filteredRecords;

            //Check whether the users should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //filteredRecords = repository.Labs
                filteredRecords = allRecords.Where(d => d.IdSpopNavigation.IdSubjekNavigation.Nama.ToLower().Contains(param.sSearch.ToLower())
                || d.IdSpopNavigation.Nop.Replace(".", "").Contains(param.sSearch));
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
                filteredRecords = filteredRecords.Where(d => d.IdSpopNavigation.IndKecamatanId.ToString() == param.tipe);
            }

            if (!string.IsNullOrEmpty(param.tipe2))
            {
                filteredRecords = filteredRecords.Where(d => d.IdSpopNavigation.IndKelurahanId.ToString() == param.tipe2);
            }

            Func<Sppt, string> orderingFunction = (c => param.iSortCol_0 == 1 ? c.IdSpopNavigation.IdSubjekNavigation.Nama :
                                                        param.iSortCol_0 == 2 ? c.IdSpopNavigation.Nop :
                                                        param.iSortCol_0 == 3 ? c.Tahun.ToString() :
                                                    "");

            if (param.sSortDir_0 == "asc")
                filteredRecords = filteredRecords.OrderBy(orderingFunction);
            else
                filteredRecords = filteredRecords.OrderByDescending(orderingFunction);

            var displayedRecords = filteredRecords.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var data = (from p in displayedRecords
                        join o in _context.Spop on p.IdSpop equals o.IdSpop
                        join k in _context.IndKecamatan on o.IndKecamatanId equals k.IndKecamatanId
                        join l in _context.IndKelurahan on o.IndKelurahanId equals l.IndKelurahanId
                        join s in _context.DataSubjek on o.IdSubjek equals s.IdSubjek
                        select new
                        {
                            id = p.IdSppt,
                            ido = o.IdSpop,
                            ids = o.IdSubjek,
                            nop = o.Nop,
                            nama = s.Nama,
                            tahun = p.Tahun,
                            njop = string.Format("{0:C0}", p.Njophitung),
                            hutang = string.Format("{0:C0}", p.Terhutang),
                            lokasi = o.NamaJalan,
                            kecamatan = k.Kecamatan,
                            kelurahan = l.Kelurahan,
                            status = (o.FlagValidasi ? "1" : "0")
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

            var sppt = await _context.Sppt
                .Include(s => s.IdSpopNavigation).ThenInclude(s => s.IdSubjekNavigation)
                .Include(s => s.IdSpopNavigation).ThenInclude(s => s.IndKecamatan)
                .Include(s => s.IdSpopNavigation).ThenInclude(s => s.IndKelurahan)
                .Include(s => s.IdTarifNavigation)
                .FirstOrDefaultAsync(m => m.IdSppt == id);
            if (sppt == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("SPPT", "PBB", new { id = sppt.IdSpop });
            ViewBag.L1 = Url.Action("Details", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            //Status
            if (sppt.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (sppt.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            ViewBag.Lspop = await _context.Lspop.Where(d => d.IdSpop == sppt.IdSpop).ToListAsync();
            return View(sppt);
        }

        [Auth(new string[] { "Developers", "PBB" })]
        public async Task<IActionResult> Create(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spop = await _context.Spop.FirstOrDefaultAsync(d => d.IdSpop == id);
            var lspop = await _context.Lspop.FirstOrDefaultAsync(d => d.IdSpop == id);
            var sppt = await _context.Sppt.Where(d => d.IdSpop == id).ToListAsync();

            if (spop == null || lspop == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("SPPT", "PBB", new { id });
            ViewBag.L1 = Url.Action("Create", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var njop = (from n in _context.RefNjoptkp select n)
                       .AsEnumerable().OrderBy(k => k.Tahun).Select(w => new
                       {
                           Text = string.Format("{0:C0}", w.Njoptkp) + " (Tahun " + w.Tahun + ")",
                           Value = w.Njoptkp
                       }).ToList();

            ViewData["NJOPTKP"] = new SelectList(njop, "Value", "Text");

            //var njopBangunan = new decimal();
            //var luasBanguan = new double();
            //foreach(var n in lspop)
            //{
            //    njopBangunan += njopBangunan + (decimal )n.Njop;
            //    luasBanguan += luasBanguan + (double)n.Luas;
            //}

            ViewBag.IdSpop = spop.IdSpop;
            ViewBag.NOP = spop.Nop;
            ViewBag.LuasTanah = spop.LuasTanah;
            ViewBag.ZonaTanah = spop.Zona;
            ViewBag.LuasBangunan = lspop.Luas;
            ViewBag.KelasBangunan = lspop.Zona;
            ViewBag.NJOPTanah = spop.Njop;
            ViewBag.NJOPBangunan = lspop.Njop;
            ViewBag.TotalTanah = spop.LuasTanah * (double)spop.Njop;
            ViewBag.TotalBangunan = lspop.Luas * (double)lspop.Njop;

            var year = DateTime.Now.Year;
            List<SelectListItem> tahunList = new List<SelectListItem>();
            int tahun = year - 3;
            for(var i = 0; i < 6; i++)
            {
                var th = tahun + i;
                if (sppt.FirstOrDefault(d => d.Tahun == th) == null) {
                    tahunList.Add(new SelectListItem() { Text = th.ToString(), Value = th.ToString() });
                }
            }
            ViewBag.Tahun = new SelectList(tahunList, "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, [Bind("IdSppt,IdSpop,Tanggal,JatuhTempo,Tahun,Njoptkp,Eu,Ed")] Sppt sppt)
        {
            if (ModelState.IsValid)
            {
                var spop = _context.Spop.AsNoTracking().FirstOrDefault(d => d.IdSpop == id);
                var lspop = _context.Lspop.AsNoTracking().FirstOrDefault(d => d.IdSpop == id);

                //TARIF PAJAK
                var tarif = _context.TarifPajak.Where(d => d.IdCoa == spop.IdCoa)
                    .Where(d => d.MulaiBerlaku <= sppt.Tanggal)
                    .OrderByDescending(d => d.MulaiBerlaku).AsNoTracking().FirstOrDefault();

                if (tarif == null)
                {
                    var coa = _context.Coa.Find(spop.IdCoa);
                    TempData["Coa"] = coa.Uraian;
                    TempData["status"] = "TarifNull";
                    return RedirectToAction("Index", "TarifPajak", null);
                }

                sppt.IdSppt = Guid.NewGuid();
                sppt.IdSpop = id;
                sppt.IdTarif = tarif.IdTarif;
                sppt.Tanggal = sppt.Tanggal;
                sppt.JatuhTempo = sppt.JatuhTempo;
                sppt.Tahun = sppt.Tahun;
                sppt.Njopbumi = spop.Njop;
                sppt.Njopbangunan = lspop.Njop;
                sppt.TotalBumi = sppt.Njopbumi * (decimal)spop.LuasTanah;
                sppt.TotalBangunan = sppt.Njopbangunan * (decimal)lspop.Luas;
                sppt.DasarPengenaan = sppt.TotalBumi + sppt.TotalBangunan;
                sppt.Njoptkp ??= 0;
                sppt.Njophitung = sppt.DasarPengenaan - (sppt.Njoptkp ?? 0);
                sppt.Terhutang = sppt.Njophitung * (decimal)tarif.Tarif / 100;
                sppt.Kredit = 0;
                sppt.Keterangan = 0;
                sppt.Eu = HttpContext.Session.GetString("User");
                sppt.Ed = DateTime.Now;
                _context.Add(sppt);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = sppt.IdSppt });
            }
            return View(sppt);
        }

        [Auth(new string[] { "Developers", "PBB" })]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sppt = await _context.Sppt.Include(d => d.IdSpopNavigation).FirstOrDefaultAsync(d => d.IdSppt == id);
            var lspop = await _context.Lspop.FirstOrDefaultAsync(d => d.IdSpop == sppt.IdSpop);
            if (sppt == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("SPPT", "PBB", new { id = sppt.IdSpop });
            ViewBag.L1 = Url.Action("Edit", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var njop = (from n in _context.RefNjoptkp select n)
                       .AsEnumerable().OrderBy(k => k.Tahun).Select(w => new
                       {
                           Text = string.Format("{0:C0}", w.Njoptkp) + " (Tahun " + w.Tahun + ")",
                           Value = w.Njoptkp
                       }).ToList();

            ViewData["NJOPTKP"] = new SelectList(njop, "Value", "Text");

            ViewBag.NOP = sppt.IdSpopNavigation.Nop;
            ViewBag.LuasTanah = sppt.IdSpopNavigation.LuasTanah;
            ViewBag.ZonaTanah = sppt.IdSpopNavigation.Zona;
            ViewBag.LuasBangunan = lspop.Luas;
            ViewBag.KelasBangunan = lspop.Zona;
            ViewBag.NJOPTanah = sppt.IdSpopNavigation.Njop;
            ViewBag.NJOPBangunan = lspop.Njop;
            ViewBag.TotalTanah = sppt.IdSpopNavigation.LuasTanah * (double)sppt.IdSpopNavigation.Njop;
            ViewBag.TotalBangunan = lspop.Luas * (double)lspop.Njop;

            var spptList = _context.Sppt.Where(d => d.IdSpop == sppt.IdSpop && d.IdSppt != id).ToList(); 
            var year = DateTime.Now.Year;
            List<SelectListItem> tahunList = new List<SelectListItem>();
            int tahun = year - 3;
            for (var i = 0; i < 6; i++)
            {
                var th = tahun + i;
                if (spptList.FirstOrDefault(d => d.Tahun == th) == null)
                {
                    tahunList.Add(new SelectListItem() { Text = th.ToString(), Value = th.ToString() });
                }
            }
            ViewBag.Tahun = new SelectList(tahunList, "Value", "Text");
            return View(sppt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdSppt,Tanggal,JatuhTempo,Tahun,Njoptkp,Eu,Ed")] Sppt sppt)
        {
            if (id != sppt.IdSppt)
            {
                return NotFound();
            }

            var old = _context.Sppt.Find(id);
            var spop = _context.Spop.AsNoTracking().FirstOrDefault(d => d.IdSpop == old.IdSpop);
            var lspop = _context.Lspop.AsNoTracking().FirstOrDefault(d => d.IdSpop == old.IdSpop);

            if (ModelState.IsValid)
            {
                try
                {
                    //TARIF PAJAK
                    var tarif = _context.TarifPajak.Where(d => d.IdCoa == spop.IdCoa)
                        .Where(d => d.MulaiBerlaku <= sppt.Tanggal)
                        .OrderByDescending(d => d.MulaiBerlaku).AsNoTracking().FirstOrDefault();

                    if (tarif == null)
                    {
                        var coa = _context.Coa.Find(spop.IdCoa);
                        TempData["Coa"] = coa.Uraian;
                        TempData["status"] = "TarifNull";
                        return RedirectToAction("Index", "TarifPajak", null);
                    }

                    old.IdTarif = tarif.IdTarif;
                    old.Tanggal = sppt.Tanggal;
                    old.JatuhTempo = sppt.JatuhTempo;
                    old.Tahun = sppt.Tahun;
                    old.Njopbumi = spop.Njop;
                    old.Njopbangunan = lspop.Njop;
                    old.TotalBumi = old.Njopbumi * (decimal)spop.LuasTanah;
                    old.TotalBangunan = old.Njopbangunan * (decimal)lspop.Luas;
                    old.DasarPengenaan = old.TotalBumi + old.TotalBangunan;
                    old.Njoptkp = sppt.Njoptkp ?? 0;
                    old.Njophitung = old.DasarPengenaan - (old.Njoptkp ?? 0);
                    old.Terhutang = old.Njophitung * (decimal)tarif.Tarif / 100;
                    old.Kredit = 0;
                    old.Keterangan = 0;
                    old.Eu = HttpContext.Session.GetString("User");
                    old.Ed = DateTime.Now;
                    _context.Sppt.Update(old);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpptExists(sppt.IdSppt))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "edit";
                return RedirectToAction("SPPT", "PBB", new { id = spop.IdSpop });
            }
            return View(sppt);
        }

        [Auth(new string[] { "Developers", "Validasi" })]
        public async Task<IActionResult> Validasi(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sppt = await _context.Sppt
                .Include(s => s.IdSpopNavigation).ThenInclude(s => s.IdSubjekNavigation)
                .Include(s => s.IdSpopNavigation).ThenInclude(s => s.IndKecamatan)
                .Include(s => s.IdSpopNavigation).ThenInclude(s => s.IndKelurahan)
                .Include(s => s.IdTarifNavigation)
                .FirstOrDefaultAsync(m => m.IdSppt == id);
            if (sppt == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("SPPT", "PBB", new { id = sppt.IdSpop });
            ViewBag.L1 = Url.Action("Validasi", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var stts = _context.Stts.FirstOrDefault(d => d.IdSppt == id);
            if (stts != null && sppt.FlagValidasi)
            {
                TempData["status"] = "validbatalerror";
                return RedirectToAction("SPPT", "PBB", new { id = sppt.IdSpop });
            }

            //Status
            if (sppt.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (sppt.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            ViewBag.Lspop = await _context.Lspop.Where(d => d.IdSpop == sppt.IdSpop).ToListAsync();
            return View(sppt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Validasi(Guid id, Sppt sppt)
        {
            if (id != sppt.IdSppt)
            {
                return NotFound();
            }

            var old = _context.Sppt.FirstOrDefault(d => d.IdSppt == id);

            old.Eu = HttpContext.Session.GetString("User");
            old.Ed = DateTime.Now;

            if (old.FlagValidasi)
            {
                old.FlagValidasi = false;
                _context.Update(old);
                await _context.SaveChangesAsync();
                TempData["status"] = "validbatal";
                return RedirectToAction("SPPT", "PBB", new { id = old.IdSpop });
            }
            else
            {
                old.FlagValidasi = true;
                _context.Update(old);
                await _context.SaveChangesAsync();
                TempData["status"] = "valid";
                return RedirectToAction("SPPT", "PBB", new { id = old.IdSpop });
            }
        }

        [Auth(new string[] { "Developers", "PBB" })]
        public ActionResult Cetak(Guid id)
        {
            return RedirectToAction("SPPT", "Cetak", new { id });
        }

        [Auth(new string[] { "Developers", "PBB" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sppt = await _context.Sppt
                .FirstOrDefaultAsync(m => m.IdSppt == id);
            if (sppt == null)
            {
                return NotFound();
            }

            var stts = _context.Stts.FirstOrDefault(d => d.IdSppt == id);

            if (sppt.FlagValidasi || stts != null)
            {
                return PartialView("_deletevalid");
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var sppt = await _context.Sppt.FindAsync(id);
            try
            {
                _context.Sppt.Remove(sppt);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["status"] = "deletefailed";
                string url = Url.Action("SPPT", "PBB", new { id = sppt.IdSpop });
                return Json(new { success = true, url });
            }
            
            TempData["status"] = "delete";
            string link = Url.Action("SPPT", "PBB", new { id = sppt.IdSpop });
            return Json(new { success = true, url = link });
        }

        private bool SpptExists(Guid id)
        {
            return _context.Sppt.Any(e => e.IdSppt == id);
        }
    }
}
