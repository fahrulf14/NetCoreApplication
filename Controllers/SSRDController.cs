using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class SSRDController : Controller
    {
        private readonly DB_NewContext _context;

        public SSRDController(DB_NewContext context)
        {
            _context = context;
        }


        [Auth(new string[] { "Developers", "Pembayaran" })]
        public IActionResult Index()
        {
            //Link
            ViewBag.L = Url.Action("Index");
            ViewBag.L1 = Url.Action("Index");
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View();
        }

        public JsonResult GetSubjek(JQueryDataTableParamModel param)
        {
            var allRecords = _context.DataSubjek.Where(d => d.Npwrd != null);
            IEnumerable<DataSubjek> filteredRecords;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredRecords = allRecords.Where(d => d.Nama.ToLower().Contains(param.sSearch.ToLower())
                || d.Nik.Contains(param.sSearch)
                || d.Npwp.Replace(".", "").Contains(param.sSearch)
                || d.Npwpd.Replace(".", "").Contains(param.sSearch)).Take(100);
            }
            else
            {
                filteredRecords = allRecords;
            }

            if (param.status.HasValue)
            {
                filteredRecords = filteredRecords.Where(d => d.Status == param.status);
            }

            Func<DataSubjek, string> orderingFunction = (c => param.iSortCol_0 == 1 ? c.Nama :
                                                        param.iSortCol_0 == 2 ? c.Npwpd :
                                                        param.iSortCol_0 == 3 ? c.Npwp :
                                                        param.iSortCol_0 == 4 ? c.Nik :
                                                    "");

            if (param.sSortDir_0 == "asc")
                filteredRecords = filteredRecords.OrderBy(orderingFunction);
            else
                filteredRecords = filteredRecords.OrderByDescending(orderingFunction);

            var displayedRecords = filteredRecords.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var data = (from s in displayedRecords
                        join k in _context.IndKecamatan on s.IndKecamatanId equals k.IndKecamatanId
                        join u in _context.Skrd on s.IdSubjek equals u.IdSubjek into d
                        from u in d.DefaultIfEmpty()
                        select new
                        {
                            id = s.IdSubjek,
                            nama = s.Nama,
                            nik = s.Nik ?? "-",
                            npwp = s.Npwp ?? "-",
                            npwpd = s.Npwpd,
                            alamat = s.Alamat,
                            kecamatan = k.Kecamatan,
                            status = (s.Status ? "1" : "0"),
                            skrd = d.Where(d => d.FlagValidasi).FirstOrDefault()?.IdCoa ?? string.Empty
                        }).Distinct().ToArray();
            return Json(new
            {
                param.sEcho,
                iTotalRecords = allRecords.Count(),
                iTotalDisplayRecords = filteredRecords.Count(),
                aaData = data
            });
        }

        [Route("SSRD/{id}")]
        [Route("SSRD/Data/{id}")]
        [Auth(new string[] { "Developers", "Pembayaran" })]
        public async Task<IActionResult> Data(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            //Link
            ViewBag.L = Url.Action("Index");
            ViewBag.L1 = Url.Content("/SSRD/" + id);
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            ViewBag.IdSubjek = id;

            var skrd = _context.Skrd.Include(d => d.IdCoaNavigation).Where(d => d.IdSubjek == id && d.FlagValidasi && d.Keterangan == 0 && d.Sk == null).ToList();
            ViewBag.ListData = skrd;
            ViewBag.List = skrd.Count();

            var data = await _context.Ssrd
                .Where(d => d.IdSubjek == id)
                .Include(s => s.IdSubjekNavigation)
                .Include(s => s.IdCoaNavigation)
                .Include(s => s.IdSetoranNavigation)
                .Include(s => s.IdSkrdNavigation)
                .ToListAsync();
            return View(data);
        }

        [Auth(new string[] { "Developers", "Pembayaran" })]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ssrd = await _context.Ssrd
                .Include(s => s.IdBankNavigation).ThenInclude(d => d.IdRefNavigation)
                .Include(s => s.IdCoaNavigation)
                .Include(s => s.IdSetoranNavigation)
                .Include(s => s.IdSkrdNavigation)
                .Include(s => s.IdSubjekNavigation)
                .FirstOrDefaultAsync(m => m.IdSsrd == id);
            if (ssrd == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Content("/SSRD/" + ssrd.IdSubjek);
            ViewBag.L1 = Url.Action("Details", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View(ssrd);
        }

        [Auth(new string[] { "Developers", "Pembayaran" })]
        public IActionResult Create(Guid id)
        {
            ViewData["IdSetoran"] = new SelectList(_context.RefJenisSetoran.Where(d => d.FlagAktif), "IdSetoran", "Jenis");

            var skrd = _context.Skrd.Find(id);
            ViewBag.IdSkrd = id;
            ViewBag.IdSubjek = skrd.IdSubjek;

            //Link
            ViewBag.L = Url.Content("/SSRD/" + skrd.IdSubjek);
            ViewBag.L1 = Url.Action("Cereate", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, [Bind("IdSsrd,IdSubjek,IdBank,IdCoa,IdSkrd,Nomor,NoSsrd,Tanggal,IdSetoran,JumlahSetoran,FlagBayar,TanggalBayar,StatusSetor,Eu,Ed")] Ssrd ssrd)
        {
            if (ModelState.IsValid)
            {
                var data = _context.Skrd.FirstOrDefault(d => d.IdSkrd == id);

                ssrd.IdSsrd = Guid.NewGuid();
                ssrd.IdSkrd = id;
                ssrd.IdSubjek = data.IdSubjek;
                ssrd.IdCoa = data.IdCoa;
                ssrd.JumlahSetoran = data.Terhutang;
                var noSk = _context.Ssrd.Where(d => d.Tanggal.Value.Year == ssrd.Tanggal.Value.Year).Select(e => e.Nomor).Max() ?? 0;
                ssrd.Nomor = noSk + 1;
                ssrd.NoSsrd = string.Format("{0:000000}", ssrd.Nomor) + "/SSRD/" + string.Format("{0:MM/yyyy}", ssrd.Tanggal);
                _context.Add(ssrd);

                data.Sk = "SSRD";
                data.Eu = HttpContext.Session.GetString("User");
                data.Ed = DateTime.Now;
                _context.Skrd.Update(data);

                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = ssrd.IdSsrd });
            }

            ViewData["IdSetoran"] = new SelectList(_context.RefJenisSetoran.Where(d => d.FlagAktif), "IdSetoran", "Jenis", ssrd.IdSetoran);
            ViewBag.IdSkpd = id;
            return View(ssrd);
        }

        [Auth(new string[] { "Developers", "Pembayaran" })]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ssrd = await _context.Ssrd.FindAsync(id);
            if (ssrd == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Content("/SSRD/" + ssrd.IdSubjek);
            ViewBag.L1 = Url.Action("Edit", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            ViewData["IdSetoran"] = new SelectList(_context.RefJenisSetoran.Where(d => d.FlagAktif), "IdSetoran", "Jenis", ssrd.IdSetoran);

            return View(ssrd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdSsrd,IdSubjek,IdBank,IdCoa,IdSkrd,Nomor,NoSsrd,Tanggal,IdSetoran,JumlahSetoran,FlagBayar,TanggalBayar,StatusSetor,Eu,Ed")] Ssrd ssrd)
        {
            if (id != ssrd.IdSsrd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var old = await _context.Ssrd.FirstOrDefaultAsync(d => d.IdSsrd == id);
                try
                {
                    old.Tanggal = ssrd.Tanggal;
                    old.IdSetoran = ssrd.IdSetoran;
                    _context.Ssrd.Update(old);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SsrdExists(ssrd.IdSsrd))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "edit";
                return Redirect("/SSRD/" + old.IdSubjek);
            }
            ViewData["IdSetoran"] = new SelectList(_context.RefJenisSetoran.Where(d => d.FlagAktif), "IdSetoran", "Jenis", ssrd.IdSetoran);

            return View(ssrd);
        }

        [Auth(new string[] { "Developers", "Validasi" })]
        public async Task<IActionResult> Validasi(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ssrd = await _context.Ssrd
                .Include(s => s.IdBankNavigation)
                .Include(s => s.IdCoaNavigation)
                .Include(s => s.IdSetoranNavigation)
                .Include(s => s.IdSkrdNavigation)
                .Include(s => s.IdSubjekNavigation)
                .FirstOrDefaultAsync(m => m.IdSsrd == id);
            if (ssrd == null)
            {
                return NotFound();
            }

            if (ssrd.FlagBayar)
            {
                TempData["status"] = "sudahvalid";
                return Redirect("/SSRD/" + ssrd.IdSubjek);
            }

            //Link
            ViewBag.L = Url.Content("/SSRD/" + ssrd.IdSubjek);
            ViewBag.L1 = Url.Action("Validasi", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var bank = (from b in _context.Bank
                        join r in _context.RefBank on b.IdRef equals r.IdBank
                        join j in _context.RefJenisSetoran on b.IdSetoran equals j.IdSetoran
                        where b.IdSetoran == ssrd.IdSetoran
                        select new
                        {
                            value = b.IdBank,
                            text = r.NamaBank + " | " + b.NoRek + " | " + b.NamaRek
                        }).ToList();
            ViewData["IdBank"] = new SelectList(bank, "value", "text", ssrd.IdBank);

            return View(ssrd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Validasi(Guid id, Ssrd ssrd)
        {
            if (id != ssrd.IdSsrd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var old = _context.Ssrd.Include(d => d.IdCoaNavigation).FirstOrDefault(d => d.IdSsrd == id);
                old.FlagBayar = true;
                old.TanggalBayar = ssrd.TanggalBayar;
                old.IdBank = ssrd.IdBank;
                if (old.IdSetoran == 1)
                {
                    old.StatusSetor = true;
                }
                old.Eu = HttpContext.Session.GetString("User");
                old.Ed = DateTime.Now;
                _context.Ssrd.Update(old);
                
                var skrd = _context.Skrd.FirstOrDefault(d => d.IdSkrd == old.IdSkrd);
                skrd.Sk = null;
                skrd.Keterangan = 1;
                skrd.Kredit = old.JumlahSetoran;
                await _context.SaveChangesAsync();

                _context.Skrd.Update(skrd);

                var Tahun = old.TanggalBayar.Value.Year;
                var lra = _context.Lra.FirstOrDefault(d => d.IdCoa == old.IdCoa && d.Tahun == Tahun);
                if (lra == null)
                {
                    lra = new Lra
                    {
                        IdLra = Guid.NewGuid(),
                        IdCoa = old.IdCoa,
                        Anggaran = 0,
                        Afinal = 0,
                        Tahun = Tahun
                    };
                    _context.Lra.Add(lra);
                    await _context.SaveChangesAsync();
                }

                var transaksi = new TransaksiLra
                {
                    IdTransaksi = Guid.NewGuid(),
                    IdLra = lra.IdLra,
                    Sumber = skrd.NoSkrd,
                    Jumlah = skrd.Kredit,
                    Tanggal = old.TanggalBayar
                };
                _context.TransaksiLra.Add(transaksi);

                var sum = _context.TransaksiLra.Where(d => d.IdLra == lra.IdLra && d.Tanggal.Value.Year == Tahun).Sum(d => d.Jumlah);
                lra.Realisasi = (sum ?? 0) + (transaksi.Jumlah ?? 0);
                _context.Lra.Update(lra);
                await _context.SaveChangesAsync();

                TempData["status"] = "valid";
                return Redirect("/SSRD/" + old.IdSubjek);
            }
            return View(ssrd);
        }

        [Auth(new string[] { "Developers", "Pembayaran" })]
        public IActionResult Cetak(Guid id)
        {
            return RedirectToAction("SSRD", "Cetak", new { id });
        }

        [Auth(new string[] { "Developers", "Pembayaran" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ssrd = await _context.Ssrd
                .Include(s => s.IdBankNavigation)
                .Include(s => s.IdCoaNavigation)
                .Include(s => s.IdSetoranNavigation)
                .Include(s => s.IdSkrdNavigation)
                .Include(s => s.IdSubjekNavigation)
                .FirstOrDefaultAsync(m => m.IdSsrd == id);
            if (ssrd == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var ssrd = await _context.Ssrd.FindAsync(id);
            try
            {
                _context.Ssrd.Remove(ssrd);

                var skrd = await _context.Skrd.FindAsync(ssrd.IdSkrd);
                skrd.Sk = null;
                _context.Skrd.Update(skrd);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["status"] = "deletefailed";
                string url = Url.Content("/SSRD/" + ssrd.IdSubjek);
                return Json(new { success = true, url });
            }
            
            TempData["status"] = "delete";
            string link = Url.Content("/SSRD/" + ssrd.IdSubjek);
            return Json(new { success = true, url = link });
        }

        private bool SsrdExists(Guid id)
        {
            return _context.Ssrd.Any(e => e.IdSsrd == id);
        }
    }
}
