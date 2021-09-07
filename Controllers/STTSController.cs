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
    public class STTSController : Controller
    {
        private readonly DB_NewContext _context;

        public STTSController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "PBB" })]
        public async Task<IActionResult> Index()
        {
            var dB_NewContext = _context.Stts.Include(s => s.IdBankNavigation).Include(s => s.IdSetoranNavigation).Include(s => s.IdSpptNavigation).ThenInclude(s => s.IdSpopNavigation);
            return View(await dB_NewContext.ToListAsync());
        }

        [Auth(new string[] { "Developers", "Pembayaran" })]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stts = await _context.Stts
                .Include(s => s.IdBankNavigation).ThenInclude(s => s.IdRefNavigation)
                .Include(s => s.IdSetoranNavigation)
                .Include(s => s.IdSpptNavigation).ThenInclude(s => s.IdSpopNavigation).ThenInclude(s => s.IdSubjekNavigation)
                .Include(s => s.IdSpptNavigation).ThenInclude(s => s.IdSpopNavigation).ThenInclude(s => s.IndKecamatan)
                .Include(s => s.IdSpptNavigation).ThenInclude(s => s.IdSpopNavigation).ThenInclude(s => s.IndKelurahan)
                .Include(s => s.IdSpptNavigation).ThenInclude(s => s.IdSpopNavigation).ThenInclude(s => s.IndKabKota)
                .Include(s => s.IdSpptNavigation).ThenInclude(s => s.IdTarifNavigation)
                .FirstOrDefaultAsync(m => m.IdStts == id);
            if (stts == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("STTS", "PBB", new { id = stts.IdSpptNavigation.IdSpop });
            ViewBag.L1 = Url.Action("Details", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            //Status
            if (stts.IdSpptNavigation.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (stts.IdSpptNavigation.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            ViewBag.Lspop = await _context.Lspop.Where(d => d.IdSpop == stts.IdSpptNavigation.IdSpop).ToListAsync();

            return View(stts);
        }

        [Auth(new string[] { "Developers", "Pembayaran" })]
        public IActionResult Create(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sppt = _context.Sppt.Include(d => d.IdSpopNavigation).FirstOrDefault(d => d.IdSppt == id);

            //Link
            ViewBag.L = Url.Action("STTS", "PBB", new { id = sppt.IdSpop });
            ViewBag.L1 = Url.Action("Create", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            ViewBag.IdSubjek = sppt.IdSpopNavigation.IdSubjek;
            ViewBag.Terhutang = sppt.Terhutang;

            ViewData["IdSetoran"] = new SelectList(_context.RefJenisSetoran, "IdSetoran", "Jenis");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, [Bind("IdStts,IdSppt,TanggalBayar,Denda,TotalBayar,IdSetoran,IdBank,StatusSetor,FlagValidasi,TanggalValidasi,Sn,Eu,Ed")] Stts stts)
        {
            if (ModelState.IsValid)
            {
                var sppt = _context.Sppt.FirstOrDefault(d => d.IdSppt == id);
                sppt.Status = true;

                stts.IdStts = Guid.NewGuid();
                stts.IdSppt = id;
                stts.Eu = HttpContext.Session.GetString("User");
                stts.Ed = DateTime.Now;
                _context.Add(stts);
                _context.Sppt.Update(sppt);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = stts.IdStts });
            }
            return View(stts);
        }

        [Auth(new string[] { "Developers", "Pembayaran" })]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stts = await _context.Stts
                .Include(s => s.IdBankNavigation)
                .Include(s => s.IdSetoranNavigation)
                .Include(s => s.IdSpptNavigation).ThenInclude(d => d.IdSpopNavigation)
                .FirstOrDefaultAsync(m => m.IdStts == id);
            if (stts == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("STTS", "PBB", new { id = stts.IdSpptNavigation.IdSpop });
            ViewBag.L1 = Url.Action("Edit", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            ViewData["IdSetoran"] = new SelectList(_context.RefJenisSetoran, "IdSetoran", "Jenis", stts.IdSetoran);
            var bank = (from b in _context.Bank
                        join r in _context.RefBank on b.IdRef equals r.IdBank
                        join j in _context.RefJenisSetoran on b.IdSetoran equals j.IdSetoran
                        where b.IdSetoran == stts.IdSetoran
                        select new
                        {
                            value = b.IdBank,
                            text = r.NamaBank + " | " + b.NoRek + " | " + b.NamaRek
                        }).ToList();
            ViewData["IdBank"] = new SelectList(bank, "value", "text", stts.IdBank);

            TempData["IdSubjek"] = stts.IdSpptNavigation.IdSpopNavigation.IdSubjek;

            return View(stts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdStts,IdSppt,TanggalBayar,Denda,TotalBayar,IdSetoran,IdBank,StatusSetor,FlagValidasi,TanggalValidasi,Sn,Eu,Ed")] Stts stts)
        {
            if (id != stts.IdStts)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    stts.Eu = HttpContext.Session.GetString("User");
                    stts.Ed = DateTime.Now;
                    _context.Update(stts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SttsExists(stts.IdStts))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "edit";
                return RedirectToAction("STTS", "PBB", new { id = TempData["IdSubjek"] });
            }
            return View(stts);
        }

        [Auth(new string[] { "Developers", "Validasi" })]
        public async Task<IActionResult> Validasi(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stts = await _context.Stts
                .Include(s => s.IdBankNavigation).ThenInclude(s => s.IdRefNavigation)
                .Include(s => s.IdSetoranNavigation)
                .Include(s => s.IdSpptNavigation).ThenInclude(s => s.IdSpopNavigation).ThenInclude(s => s.IdSubjekNavigation)
                .Include(s => s.IdSpptNavigation).ThenInclude(s => s.IdSpopNavigation).ThenInclude(s => s.IndKecamatan)
                .Include(s => s.IdSpptNavigation).ThenInclude(s => s.IdSpopNavigation).ThenInclude(s => s.IndKelurahan)
                .Include(s => s.IdSpptNavigation).ThenInclude(s => s.IdSpopNavigation).ThenInclude(s => s.IndKabKota)
                .Include(s => s.IdSpptNavigation).ThenInclude(s => s.IdTarifNavigation)
                .FirstOrDefaultAsync(m => m.IdStts == id);
            if (stts == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("STTS", "PBB", new { id = stts.IdSpptNavigation.IdSpop });
            ViewBag.L1 = Url.Action("Validasi", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            if (stts.FlagValidasi)
            {
                TempData["status"] = "validbatalerror";
                return RedirectToAction("STTS", "PBB", new { id = stts.IdSpptNavigation.IdSpopNavigation.IdSubjek });
            }

            //Status
            if (stts.IdSpptNavigation.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (stts.IdSpptNavigation.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            ViewBag.Lspop = await _context.Lspop.Where(d => d.IdSpop == stts.IdSpptNavigation.IdSpop).ToListAsync();
            return View(stts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Validasi(Guid id, Stts stts)
        {
            if (id != stts.IdStts)
            {
                return NotFound();
            }

            var old = _context.Stts.Include(d => d.IdSpptNavigation).ThenInclude(d => d.IdSpopNavigation).ThenInclude(d => d.IdCoaNavigation).FirstOrDefault(d => d.IdStts == id);
            if (old.FlagValidasi)
            {
                TempData["status"] = "validbatalerror";
                return RedirectToAction("STTS", "PBB", new { id = old.IdSpptNavigation.IdSpopNavigation.IdSubjek });
            }
            else
            {
                var sppt = _context.Sppt.FirstOrDefault(d => d.IdSppt == old.IdSppt);
                sppt.Kredit = sppt.Terhutang;
                sppt.Keterangan = 1;
                _context.Sppt.Update(sppt);

                old.Eu = HttpContext.Session.GetString("User");
                old.Ed = DateTime.Now;
                old.TanggalValidasi = DateTime.Now;
                old.FlagValidasi = true;
                if (old.IdSetoran == 1)
                {
                    old.StatusSetor = true;
                }
                else
                {
                    old.StatusSetor = false;
                }
                _context.Stts.Update(old);
                await _context.SaveChangesAsync();

                var Tahun = old.TanggalValidasi?.Year;
                var lra = _context.Lra.FirstOrDefault(d => d.IdCoa == old.IdSpptNavigation.IdSpopNavigation.IdCoa && d.Tahun == Tahun);
                if (lra == null)
                {
                    lra = new Lra
                    {
                        IdLra = Guid.NewGuid(),
                        IdCoa = old.IdSpptNavigation.IdSpopNavigation.IdCoa,
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
                    Jumlah = old.IdSpptNavigation.Terhutang,
                    Sumber = old.IdSpptNavigation.IdSpopNavigation.Nop + "-" + old.TanggalBayar.Value.Year,
                    Tanggal = old.TanggalValidasi
                };
                _context.TransaksiLra.Add(transaksi);

                var sum = _context.TransaksiLra.Where(d => d.IdLra == lra.IdLra && d.Tanggal.Value.Year == Tahun).Sum(d => d.Jumlah);
                lra.Realisasi = (sum ?? 0) + (transaksi.Jumlah ?? 0);
                _context.Lra.Update(lra);
                await _context.SaveChangesAsync();

                if (old.Denda != 0)
                {
                    var lraDenda = _context.Lra.FirstOrDefault(d => d.IdCoa == old.IdSpptNavigation.IdSpopNavigation.IdCoaNavigation.Denda && d.Tahun == Tahun);
                    if (lraDenda == null)
                    {
                        lraDenda = new Lra
                        {
                            IdLra = Guid.NewGuid(),
                            IdCoa = old.IdSpptNavigation.IdSpopNavigation.IdCoaNavigation.Denda,
                            Anggaran = 0,
                            Afinal = 0,
                            Tahun = Tahun
                        };
                        _context.Lra.Add(lraDenda);
                        await _context.SaveChangesAsync();
                    }

                    var transaksiDenda = new TransaksiLra
                    {
                        IdTransaksi = Guid.NewGuid(),
                        IdLra = lraDenda.IdLra,
                        Jumlah = old.Denda,
                        Sumber = old.IdSpptNavigation.IdSpopNavigation.Nop + "-" + old.TanggalBayar.Value.Year,
                        Tanggal = old.TanggalValidasi
                    };
                    _context.TransaksiLra.Add(transaksiDenda);

                    var sumDenda = _context.TransaksiLra.Where(d => d.IdLra == lraDenda.IdLra && d.Tanggal.Value.Year == Tahun).Sum(d => d.Jumlah);
                    lraDenda.Realisasi = (sumDenda ?? 0) + (transaksiDenda.Jumlah ?? 0);
                    _context.Lra.Update(lraDenda);
                    await _context.SaveChangesAsync();
                }

                TempData["status"] = "valid";
                return RedirectToAction("STTS", "PBB", new { id = old.IdSpptNavigation.IdSpopNavigation.IdSubjek });
            }
        }

        [Auth(new string[] { "Developers", "Pembayaran" })]
        public ActionResult Cetak(Guid id)
        {
            return RedirectToAction("STTS", "Cetak", new { id });
        }

        [Auth(new string[] { "Developers", "Pembayaran" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stts = await _context.Stts
                .Include(s => s.IdSpptNavigation).ThenInclude(d => d.IdSpopNavigation)
                .FirstOrDefaultAsync(m => m.IdStts == id);
            if (stts == null)
            {
                return NotFound();
            }

            if (stts.FlagValidasi)
            {
                return PartialView("_deletevalid");
            }

            TempData["IdSubjek"] = stts.IdSpptNavigation.IdSpopNavigation.IdSubjek;
            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var stts = await _context.Stts.FindAsync(id);
            var sppt = await _context.Sppt.FirstOrDefaultAsync(d => d.IdSppt == stts.IdSppt);
            try
            {
                sppt.Status = false;
                _context.Stts.Remove(stts);
                _context.Sppt.Update(sppt);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["status"] = "deletefailed";
                string url = Url.Action("STTS", "PBB", new { id = TempData["IdSubjek"] });
                return Json(new { success = true, url });
            }
            
            TempData["status"] = "delete";
            string link = Url.Action("STTS", "PBB", new { id = TempData["IdSubjek"] });
            return Json(new { success = true, url = link });
        }

        private bool SttsExists(Guid id)
        {
            return _context.Stts.Any(e => e.IdStts == id);
        }
    }
}
