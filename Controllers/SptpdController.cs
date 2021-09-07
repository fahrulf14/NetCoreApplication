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
    public class SptpdController : Controller
    {
        private readonly DB_NewContext _context;

        public SptpdController(DB_NewContext context)
        {
            _context = context;
        }

        [Route("SPTPD/{id}")]
        [Auth(new string[] { "Developers", "Pendataan" })]
        public async Task<IActionResult> Index(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = await _context.Sptpd.Include(s => s.IdCoaNavigation).FirstOrDefaultAsync(s => s.IdSptpd == id);
            if (data == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.Title = "Pajak " + data.IdCoaNavigation.Uraian;
            ViewBag.S1 = data.IdCoaNavigation.Uraian;
            ViewBag.L = Url.Content("/DataKewajiban/" + data.IdUsaha);
            ViewBag.L1 = Url.Content("/SPTPD/" + id);
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var dB_NewContext = _context.Sptpd
                .Include(s => s.IdCoaNavigation)
                .Include(s => s.IdSubjekNavigation)
                .Include(s => s.IdTarifNavigation)
                .Include(s => s.IdUsahaNavigation)
                .Where(s => s.IdUsaha == data.IdUsaha && s.IdCoaNavigation.Kdcoa == data.IdCoaNavigation.Kdcoa)
                .OrderBy(s => s.MasaPajak1);

            ViewBag.IdSptpd = dB_NewContext.Where(s => s.FlagKonfirmasi == false).FirstOrDefault().IdSptpd.ToString();

            if (data.IdCoaNavigation.Jenis == "Official Assessment")
            {
                return View("Official", await dB_NewContext.Include(d => d.Skpd).ToListAsync());
            }

            return View(await dB_NewContext.ToListAsync());
        }

        [Auth(new string[] { "Developers", "Pendataan" })]
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sptpd = _context.Sptpd.Find(id);
            var coa = sptpd.IdCoa.Substring(0, 5);

            if (coa == "41101")
            {
                return RedirectToAction("Details", "PajakHotel", new { id });
            }
            else if (coa == "41102")
            {
                return RedirectToAction("Details", "PajakRestoran", new { id });
            }
            else if (coa == "41103")
            {
                return RedirectToAction("Details", "PajakHiburan", new { id });
            }
            else if (coa == "41104")
            {
                return RedirectToAction("Details", "PajakReklame", new { id });
            }
            else if (coa == "41105")
            {
                return RedirectToAction("Details", "PajakPenerangan", new { id });
            }
            else if (coa == "41107")
            {
                return RedirectToAction("Details", "PajakParkir", new { id });
            }
            else if (coa == "41108")
            {
                return RedirectToAction("Details", "PajakAirTanah", new { id });
            }
            else if (coa == "41109")
            {
                return RedirectToAction("Details", "PajakSarangWalet", new { id });
            }
            else if (coa == "41111")
            {
                return RedirectToAction("Details", "PajakMineral", new { id });
            }
            else if (coa == "41113")
            {
                return RedirectToAction("Details", "BPHTB", new { id });
            }

            return View();
        }

        [Auth(new string[] { "Developers", "Pendataan" })]
        public IActionResult Create(Guid? id, bool jabatan)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sptpd = _context.Sptpd.Find(id);
            var coa = sptpd.IdCoa.Substring(0, 5);

            if (coa == "41101")
            {
                return RedirectToAction("SPTPD", "PajakHotel", new { id, jabatan });
            }
            else if (coa == "41102")
            {
                return RedirectToAction("SPTPD", "PajakRestoran", new { id, jabatan });
            }
            else if (coa == "41103")
            {
                return RedirectToAction("SPTPD", "PajakHiburan", new { id, jabatan });
            }
            else if (coa == "41104")
            {
                return RedirectToAction("SPTPD", "PajakReklame", new { id, jabatan });
            }
            else if (coa == "41105")
            {
                return RedirectToAction("SPTPD", "PajakPenerangan", new { id, jabatan });
            }
            else if (coa == "41107")
            {
                return RedirectToAction("SPTPD", "PajakParkir", new { id, jabatan });
            }
            else if (coa == "41108")
            {
                return RedirectToAction("SPTPD", "PajakAirTanah", new { id, jabatan });
            }
            else if (coa == "41109")
            {
                return RedirectToAction("SPTPD", "PajakSarangWalet", new { id, jabatan });
            }
            else if (coa == "41111")
            {
                return RedirectToAction("SPTPD", "PajakMineral", new { id, jabatan });
            }
            else if (coa == "41113")
            {
                return RedirectToAction("SPTPD", "BPHTB", new { id, jabatan });
            }

            return View();
        }

        [Auth(new string[] { "Developers", "Pendataan" })]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sptpd = _context.Sptpd.Find(id);
            var coa = sptpd.IdCoa.Substring(0, 5);

            if (coa == "41101")
            {
                return RedirectToAction("SPTPD", "PajakHotel", new { id });
            }
            else if (coa == "41102")
            {
                return RedirectToAction("SPTPD", "PajakRestoran", new { id });
            }
            else if (coa == "41103")
            {
                return RedirectToAction("SPTPD", "PajakHiburan", new { id });
            }
            else if (coa == "41104")
            {
                return RedirectToAction("SPTPD", "PajakReklame", new { id });
            }
            else if (coa == "41105")
            {
                return RedirectToAction("SPTPD", "PajakPenerangan", new { id });
            }
            else if (coa == "41107")
            {
                return RedirectToAction("SPTPD", "PajakParkir", new { id });
            }
            else if (coa == "41108")
            {
                return RedirectToAction("SPTPD", "PajakAirTanah", new { id });
            }
            else if (coa == "41109")
            {
                return RedirectToAction("SPTPD", "PajakSarangWalet", new { id });
            }
            else if (coa == "41111")
            {
                return RedirectToAction("SPTPD", "PajakMineral", new { id });
            }
            else if (coa == "41113")
            {
                return RedirectToAction("SPTPD", "BPHTB", new { id });
            }

            return View();
        }

        [Auth(new string[] { "Developers", "Validasi" })]
        public IActionResult Validasi(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sptpd = _context.Sptpd.Find(id);
            var coa = sptpd.IdCoa.Substring(0, 5);

            if (coa == "41101")
            {
                return RedirectToAction("Validasi", "PajakHotel", new { id });
            }
            else if (coa == "41102")
            {
                return RedirectToAction("Validasi", "PajakRestoran", new { id });
            }
            else if (coa == "41103")
            {
                return RedirectToAction("Validasi", "PajakHiburan", new { id });
            }
            else if (coa == "41104")
            {
                return RedirectToAction("Validasi", "PajakReklame", new { id });
            }
            else if (coa == "41105")
            {
                return RedirectToAction("Validasi", "PajakPenerangan", new { id });
            }
            else if (coa == "41107")
            {
                return RedirectToAction("Validasi", "PajakParkir", new { id });
            }
            else if (coa == "41108")
            {
                return RedirectToAction("Validasi", "PajakAirTanah", new { id });
            }
            else if (coa == "41109")
            {
                return RedirectToAction("Validasi", "PajakSarangWalet", new { id });
            }
            else if (coa == "41111")
            {
                return RedirectToAction("Validasi", "PajakMineral", new { id });
            }
            else if (coa == "41113")
            {
                return RedirectToAction("Validasi", "BPHTB", new { id });
            }

            return View();
        }

        [Auth(new string[] { "Developers", "Pendataan" })]
        public IActionResult Cetak(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sptpd = _context.Sptpd.Find(id);
            var coa = sptpd.IdCoa.Substring(0, 5);

            if (coa == "41101")
            {
                return RedirectToAction("Cetak", "PajakHotel", new { id });
            }
            else if (coa == "41102")
            {
                return RedirectToAction("Cetak", "PajakRestoran", new { id });
            }
            else if (coa == "41103")
            {
                return RedirectToAction("Cetak", "PajakHiburan", new { id });
            }
            else if (coa == "41104")
            {
                return RedirectToAction("Cetak", "PajakReklame", new { id });
            }
            else if (coa == "41105")
            {
                return RedirectToAction("Cetak", "PajakPenerangan", new { id });
            }
            else if (coa == "41107")
            {
                return RedirectToAction("Cetak", "PajakParkir", new { id });
            }
            else if (coa == "41108")
            {
                return RedirectToAction("Cetak", "PajakAirTanah", new { id });
            }
            else if (coa == "41109")
            {
                return RedirectToAction("Cetak", "PajakSarangWalet", new { id });
            }
            else if (coa == "41111")
            {
                return RedirectToAction("Cetak", "PajakMineral", new { id });
            }
            else if (coa == "41113")
            {
                return RedirectToAction("Cetak", "BPHTB", new { id });
            }

            return View();
        }

        [Auth(new string[] { "Developers", "Pendataan" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sptpd = await _context.Sptpd
                .FirstOrDefaultAsync(m => m.IdSptpd == id);
            if (sptpd == null)
            {
                return NotFound();
            }

            if (sptpd.FlagValidasi)
            {
                return PartialView("_deletevalid");
            }

            return PartialView("_delete", sptpd);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var sptpd = await _context.Sptpd.FindAsync(id);
            var coa = sptpd.IdCoa.Substring(0, 5);
            var spt = await _context.Sptpd.Where(d => d.IdUsaha == sptpd.IdUsaha && d.IdCoa == sptpd.IdCoa && d.FlagKonfirmasi == false).FirstOrDefaultAsync();
            try
            {
                if (coa == "41101")
                {
                    var pajak = await _context.PHotel.FirstOrDefaultAsync(d => d.IdSptpd == id);
                    var detail = _context.PHotelDt.Where(d => d.IdHotel == pajak.IdHotel);
                    var kamar = _context.PHotelKm.Where(d => d.IdHotel == pajak.IdHotel);

                    _context.PHotelKm.RemoveRange(kamar);
                    _context.PHotelDt.RemoveRange(detail);
                    _context.PHotel.Remove(pajak);
                }
                else if (coa == "41102")
                {
                    var pajak = await _context.PRestoran.FirstOrDefaultAsync(d => d.IdSptpd == id);
                    var detail = _context.PRestoranDt.Where(d => d.IdRestoran == pajak.IdRestoran);

                    _context.PRestoranDt.RemoveRange(detail);
                    _context.PRestoran.Remove(pajak);
                }
                else if (coa == "41103")
                {
                    var pajak = await _context.PHiburan.FirstOrDefaultAsync(d => d.IdSptpd == id);
                    var detail = _context.PHiburanDt.Where(d => d.IdHiburan == pajak.IdHiburan);

                    _context.PHiburanDt.RemoveRange(detail);
                    _context.PHiburan.Remove(pajak);
                }
                else if (coa == "41104")
                {
                    var pajak = await _context.PReklame.FirstOrDefaultAsync(d => d.IdSptpd == id);

                    _context.PReklame.Remove(pajak);
                }
                else if (coa == "41105")
                {
                    var pajak = await _context.PPenerangan.FirstOrDefaultAsync(d => d.IdSptpd == id);

                    _context.PPenerangan.Remove(pajak);
                }
                else if (coa == "41107")
                {
                    var pajak = await _context.PParkir.FirstOrDefaultAsync(d => d.IdSptpd == id);

                    _context.PParkir.Remove(pajak);
                }
                else if (coa == "41108")
                {
                    var pajak = await _context.PAirTanah.FirstOrDefaultAsync(d => d.IdSptpd == id);

                    _context.PAirTanah.Remove(pajak);
                }
                else if (coa == "41109")
                {
                    var pajak = await _context.PWalet.FirstOrDefaultAsync(d => d.IdSptpd == id);

                    _context.PWalet.Remove(pajak);
                }
                else if (coa == "41111")
                {
                    var pajak = await _context.PMineral.FirstOrDefaultAsync(d => d.IdSptpd == id);

                    _context.PMineral.Remove(pajak);
                }

                _context.Sptpd.Remove(sptpd);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["status"] = "deletefailed";
                string url = Url.Content("/SPTPD/" + spt.IdSptpd);
                return Json(new { success = true, url });
            }
            
            TempData["status"] = "delete";
            string link = Url.Content("/SPTPD/" + spt.IdSptpd);
            return Json(new { success = true, url = link });
        }
    }
}
