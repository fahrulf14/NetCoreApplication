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

namespace SIP.Controllers
{
    public class SSRDRController : Controller
    {
        private readonly DB_NewContext _context;

        public SSRDRController(DB_NewContext context)
        {
            _context = context;
        }

        [Route("SSRDR/{id}")]
        [Auth(new string[] { "Developers", "Retribusi" })]
        public async Task<IActionResult> Index(string id)
        {
            var dB_NewContext = _context.Ssrdr.Include(s => s.IdBankNavigation)
                .Include(s => s.IdCoaNavigation)
                .Include(s => s.IdSetoranNavigation).Where(d => d.IdCoa ==id);

            var coa = _context.Coa.FirstOrDefault(d => d.IdCoa == id);

            //Link
            ViewBag.Title = "SSRDR " + coa.Uraian;
            ViewBag.S1 = coa.Uraian;
            ViewBag.L1 = Url.Content("/SSRDR/" + id);
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            ViewBag.IdCoa = id;
            return View(await dB_NewContext.ToListAsync());
        }

        [Auth(new string[] { "Developers", "Retribusi" })]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ssrdr = await _context.Ssrdr
                .Include(s => s.IdBankNavigation)
                .Include(s => s.IdCoaNavigation)
                .Include(s => s.IdSetoranNavigation)
                .FirstOrDefaultAsync(m => m.IdSsrdr == id);
            if (ssrdr == null)
            {
                return NotFound();
            }

            ViewBag.Detail = _context.SsrdrDt.Include(d => d.IdTarifNavigation).Where(d => d.IdSsrdr == id).ToList();

            //Link
            ViewBag.L = Url.Content("/SSRDR/" + ssrdr.IdCoa);
            ViewBag.L1 = Url.Action("Details", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            //Status
            if (ssrdr.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Setor";
            }
            else if (ssrdr.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Sudah Setor";
            }

            return View(ssrdr);
        }

        [Auth(new string[] { "Developers", "Retribusi" })]
        public IActionResult Create(string id)
        {
            ViewBag.IdCoa = id;

            ViewData["IdSetoran"] = new SelectList(_context.RefJenisSetoran.Where(d => d.FlagAktif), "IdSetoran", "Jenis");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSsrdr,IdCoa,IdBank,Tanggal,Nomor,NoSsrdr,IdSetoran,Total,TanggalValidasi,Keterangan,Nama,Nik,StatusSetor,Eu,Ed")] Ssrdr ssrdr, string id)
        {
            if (ModelState.IsValid)
            {

                ssrdr.IdSsrdr = Guid.NewGuid();
                ssrdr.IdCoa = id;
                ssrdr.Nama = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ssrdr.Nama);
                ssrdr.Nik = ssrdr.Nik;
                var noSk = _context.Ssrdr.Where(d => d.Tanggal.Value.Year == ssrdr.Tanggal.Value.Year).Select(d => d.Nomor).Max() ?? 0;
                ssrdr.Nomor = noSk + 1;
                ssrdr.NoSsrdr = string.Format("{0:000000}", ssrdr.Nomor) + "/SSRDR/" + string.Format("{0:MM/yyyy}", ssrdr.Tanggal);
                ssrdr.Keterangan = 0;
                ssrdr.Eu = HttpContext.Session.GetString("User");
                ssrdr.Ed = DateTime.Now;
                _context.Add(ssrdr);
                await _context.SaveChangesAsync();
                return RedirectToAction("Data","SSRDR", new { id = ssrdr.IdSsrdr });
            }
            ViewData["IdSetoran"] = new SelectList(_context.RefJenisSetoran.Where(d => d.FlagAktif), "IdSetoran", "Jenis", ssrdr.IdSetoran);
            return View(ssrdr);
        }

        [Auth(new string[] { "Developers", "Retribusi" })]
        public async Task<IActionResult> Data(Guid id)
        {
            var ssrdr = await _context.Ssrdr
                .Include(s => s.IdBankNavigation)
                .Include(s => s.IdCoaNavigation)
                .Include(s => s.IdSetoranNavigation)
                .FirstOrDefaultAsync(m => m.IdSsrdr == id);

            //Status
            if (ssrdr.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Setor";
            }
            else if (ssrdr.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Sudah Setor";
            }

            ViewBag.IdSsrdr = id;
            ViewBag.Detail = _context.SsrdrDt.Where(s => s.IdSsrdr == id)
            .Include (s => s.IdTarifNavigation)
            .Include(s => s.IdSsrdrNavigation)
            .ToList();

            return View(ssrdr);
        }

        [Auth(new string[] { "Developers", "Retribusi" })]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ssrdr = await _context.Ssrdr.FindAsync(id);
            if (ssrdr == null)
            {
                return NotFound();
            }
            ViewData["IdSetoran"] = new SelectList(_context.RefJenisSetoran.Where(d => d.FlagAktif), "IdSetoran", "Jenis", ssrdr.IdSetoran);
            return View(ssrdr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdSsrdr,IdCoa,IdBank,Tanggal,Nomor,NoSsrdr,IdSetoran,Total,TanggalValidasi,Keterangan,Nama,Nik,StatusSetor,Eu,Ed")] Ssrdr ssrdr)
        {
            if (id != ssrdr.IdSsrdr)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var old = _context.Ssrdr.Find(id);
                try
                {
                    old.Nama = ssrdr.Nama;
                    old.Nik = ssrdr.Nik;
                    old.Tanggal = ssrdr.Tanggal;
                    old.IdSetoran = ssrdr.IdSetoran;
                    old.Ed = DateTime.Now;
                    old.Eu = HttpContext.Session.GetString("User");
                    old.Ed = DateTime.Now;
                    _context.Ssrdr.Update(old);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SsrdrExists(ssrdr.IdSsrdr))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "edit";
                return Redirect("/SSRDR/" + old.IdCoa);
            }
            
            return View(ssrdr);
        }

        [Auth(new string[] { "Developers", "Validasi" })]
        public async Task<IActionResult> Validasi(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ssrdr = await _context.Ssrdr
               .Include(s => s.IdBankNavigation)
               .Include(s => s.IdCoaNavigation)
               .Include(s => s.IdSetoranNavigation)
               .FirstOrDefaultAsync(m => m.IdSsrdr == id);

            if (ssrdr == null)
            {
                return NotFound();
            }

            if (ssrdr.TanggalValidasi != null)
            {
                TempData["status"] = "sudahvalid";
                return Redirect("/SSRDR/" + ssrdr.IdCoa);
            }

            ViewBag.Detail = _context.SsrdrDt.Include(d => d.IdTarifNavigation).Where(d => d.IdSsrdr == id).ToList();

            //Link
            ViewBag.L = Url.Content("/SSRDR/" + ssrdr.IdCoa);
            ViewBag.L1 = Url.Action("Validasi", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            //Status
            if (ssrdr.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Setor";
            }
            else if (ssrdr.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Sudah Setor";
            }

            return View(ssrdr);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Validasi(Guid id, Ssrdr ssrdr)
        {
            if (id != ssrdr.IdSsrdr)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var old = _context.Ssrdr.Find(id);
                old.TanggalValidasi = DateTime.Now;
                old.Eu = HttpContext.Session.GetString("User");
                old.Ed = DateTime.Now;
                _context.Ssrdr.Update(old);
                await _context.SaveChangesAsync();

                var Tahun = old.TanggalValidasi.Value.Year;
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
                    Sumber = ssrdr.NoSsrdr,
                    Jumlah = ssrdr.Total,
                    Tanggal = old.TanggalValidasi
                };
                _context.TransaksiLra.Add(transaksi);

                var sum = _context.TransaksiLra.Where(d => d.IdLra == lra.IdLra && d.Tanggal.Value.Year == Tahun).Sum(d => d.Jumlah);
                lra.Realisasi = (sum ?? 0) + (transaksi.Jumlah ?? 0);
                _context.Lra.Update(lra);
                await _context.SaveChangesAsync();

                TempData["status"] = "valid";
                return Redirect("/SSRDR/" + old.IdCoa);
            }
            return View(ssrdr);
        }

        [Auth(new string[] { "Developers", "Retribusi" })]
        public IActionResult Cetak(Guid id)
        {
            return RedirectToAction("SSRDR", "Cetak", new { id });
        }

        [Auth(new string[] { "Developers", "Retribusi" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ssrdr = await _context.Ssrdr
                .Include(s => s.IdBankNavigation)
                .Include(s => s.IdCoaNavigation)
                .Include(s => s.IdSetoranNavigation)
                .FirstOrDefaultAsync(m => m.IdSsrdr == id);
            if (ssrdr == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var ssrdr = await _context.Ssrdr.FindAsync(id);
            try
            {
                _context.Ssrdr.Remove(ssrdr);
                await _context.SaveChangesAsync();
                TempData["status"] = "delete";
            }
            catch (DbUpdateException)
            {
                TempData["status"] = "deletefailed";
                string url = Url.Content("/SSRDR/" + ssrdr.IdCoa);
                return Json(new { success = true, url });
            }
            
            string link = Url.Content("/SSRDR/" + ssrdr.IdCoa);
            return Json(new { success = true, url = link });
        }

        private bool SsrdrExists(Guid id)
        {
            return _context.Ssrdr.Any(e => e.IdSsrdr == id);
        }
    }
}
