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
    public class PajakRestoranController : Controller
    {
        private readonly DB_NewContext _context;

        public PajakRestoranController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "Pendataan" })]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pRestoran = await _context.PRestoran
                .Include(p => p.IdSptpdNavigation).ThenInclude(p => p.IdCoaNavigation).Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdTarifNavigation)
                .FirstOrDefaultAsync(m => m.IdSptpd == id);
            if (pRestoran == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.Title = (pRestoran.IdSptpdNavigation.Jabatan ? "Detail Nota Jabatan" : "Detail SPTPD");
            ViewBag.SubHeaderTitle = pRestoran.IdSptpdNavigation.IdCoaNavigation.Uraian;
            ViewBag.L = Url.Content("/SPTPD/" + id);
            ViewBag.L1 = Url.Action("Details", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            //Status
            if (pRestoran.IdSptpdNavigation.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (pRestoran.IdSptpdNavigation.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            else if (pRestoran.IdSptpdNavigation.Keterangan == 2)
            {
                ViewBag.State = "kt-ribbon--warning";
                ViewBag.Status = "Kurang Bayar";
            }
            else if (pRestoran.IdSptpdNavigation.Keterangan == 3)
            {
                ViewBag.State = "kt-ribbon--primary";
                ViewBag.Status = "Lebih Bayar";
            }
            else if (pRestoran.IdSptpdNavigation.Keterangan == 4)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPD";
            }
            else if (pRestoran.IdSptpdNavigation.Keterangan == 5)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPDKB";
            }
            else if (pRestoran.IdSptpdNavigation.Keterangan == 5)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPDKBT";
            }

            return View(pRestoran);
        }

        [Auth(new string[] { "Developers", "Pendataan" })]
        public async Task<IActionResult> SPTPD(Guid? id, bool jabatan)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pRestoran = await _context.PRestoran.Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation).FirstOrDefaultAsync(d => d.IdSptpd == id);
            if (pRestoran == null)
            {
                return NotFound();
            }

            if (pRestoran.IdSptpdNavigation.FlagValidasi)
            {
                TempData["status"] = "sudahvalid";
                RedirectToAction("SPTPD", new { id });
            }

            if (pRestoran.IdSptpdNavigation.Jabatan)
            {
                jabatan = pRestoran.IdSptpdNavigation.Jabatan;
            }
            TempData["Jabatan"] = jabatan;
            TempData.Keep("Jabatan");

            //Link
            ViewBag.Title = (jabatan ? "Nota Jabatan " : "SPTPD ") + pRestoran.IdSptpdNavigation.IdCoaNavigation.Uraian;
            ViewBag.SubHeaderTitle = pRestoran.IdSptpdNavigation.IdCoaNavigation.Uraian;
            ViewBag.L = Url.Content("/SPTPD/" + id);
            ViewBag.L1 = Url.Action("SPTPD", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";
            ViewBag.S1 = (jabatan ? "Nota Jabatan " : "SPTPD ");

            ViewData["Layanan"] = new SelectList(_context.RefRestoran, "IdRef", "Uraian");

            return View("Edit", pRestoran);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SPTPD(Guid id, [Bind("IdRestoran,IdSptpd,KasRegister,Pembukuan,Dpp,PajakTerhutang,Eu,Ed")] PRestoran pRestoran, Sptpd sptpd)
        {
            if (id != pRestoran.IdSptpd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var pajak = _context.PRestoran.FirstOrDefault(d => d.IdSptpd == id);
                    var spt = _context.Sptpd.Find(id);

                    if(pajak != null && spt != null)
                    {
                        //TARIF PAJAK
                        var tarif = _context.TarifPajak.Where(d => d.IdCoa == spt.IdCoa)
                            .Where(d => d.MulaiBerlaku <= sptpd.MasaPajak1)
                            .OrderByDescending(d => d.MulaiBerlaku).AsNoTracking().FirstOrDefault();

                        if (tarif == null)
                        {
                            var coa = _context.Coa.Find(spt.IdCoa);
                            TempData["Coa"] = coa.Uraian;
                            TempData["status"] = "TarifNull";
                            return RedirectToAction("Index", "TarifPajak", null);
                        }

                        pajak.KasRegister = pRestoran.KasRegister;
                        pajak.Pembukuan = pRestoran.Pembukuan;

                        pajak.Dpp = _context.PRestoranDt.Where(d => d.IdRestoran == pajak.IdRestoran).Sum(d => d.Jumlah);

                        if (pajak.Dpp == 0)
                        {
                            TempData["status"] = "DPP0";
                            return RedirectToAction("SPTPD", new { id });
                        }

                        pajak.PajakTerhutang = pajak.Dpp * (decimal)tarif.Tarif/100;

                        spt.MasaPajak1 = sptpd.MasaPajak1;
                        spt.MasaPajak2 = sptpd.MasaPajak2;
                        spt.Tahun = sptpd.Tanggal?.Year.ToString();
                        spt.Tanggal = sptpd.Tanggal;
                        spt.IdTarif = tarif.IdTarif;
                        spt.Jabatan = bool.Parse(TempData["Jabatan"].ToString());
                        spt.Terhutang = pajak.PajakTerhutang;
                        spt.Eu = HttpContext.Session.GetString("User");
                        spt.Ed = DateTime.Now;

                        if (spt.FlagKonfirmasi == false)
                        {
                            if (spt.Jabatan)
                            {
                                var no = _context.Sptpd.Where(e => e.IdCoa.Substring(0, 5) == spt.IdCoa.Substring(0, 5) && e.Jabatan == true && e.Tahun == spt.Tahun).Select(e => e.Nomor).Max() ?? 0;
                                spt.Nomor = no + 1;
                                spt.NoSptpd = string.Format("{0:000000}", spt.Nomor) + "/NotaJabatan/Restoran/" + string.Format("{0:MM/yyyy}", spt.Tanggal);
                            }
                            else
                            {
                                var no = _context.Sptpd.Where(e => e.IdCoa.Substring(0, 5) == spt.IdCoa.Substring(0, 5) && e.Jabatan == false && e.Tahun == spt.Tahun).Select(e => e.Nomor).Max() ?? 0;
                                spt.Nomor = no + 1;
                                spt.NoSptpd = string.Format("{0:000000}", spt.Nomor) + "/SPTPD/Restoran/" + string.Format("{0:MM/yyyy}", spt.Tanggal);
                            }
                            spt.FlagKonfirmasi = true;
                            spt.TanggalKonfirmasi = DateTime.Now;

                            //GET NEXT MASA PAJAK
                            DateTime masapajak = spt.MasaPajak1 ?? DateTime.Now;
                            DateTime firstDay = masapajak.AddDays(-masapajak.Day + 1).AddMonths(1);
                            DateTime lastDay = new DateTime(masapajak.Year, masapajak.Month, 1).AddMonths(2).AddDays(-1);

                            //CLONE SPTPD
                            Sptpd newSpt = new Sptpd
                            {
                                IdSptpd = Guid.NewGuid(),
                                IdUsaha = spt.IdUsaha,
                                IdSubjek = spt.IdSubjek,
                                IdCoa = spt.IdCoa,
                                KreditPajak = 0,
                                Keterangan = 0,
                                MasaPajak1 = firstDay,
                                MasaPajak2 = lastDay,
                                Tahun = firstDay.Year.ToString()
                            };

                            //CLONE DATA PAJAK
                            PRestoran newPajak = new PRestoran
                            {
                                IdRestoran = Guid.NewGuid(),
                                IdSptpd = newSpt.IdSptpd,
                                KasRegister = pRestoran.KasRegister,
                                Pembukuan = pRestoran.Pembukuan
                            };

                            _context.Sptpd.Add(newSpt);
                            _context.PRestoran.Add(newPajak);
                        }

                        _context.PRestoran.Update(pajak);
                        _context.Sptpd.Update(spt);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PRestoranExists(pRestoran.IdRestoran))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id });
            }
            return View(pRestoran);
        }

        [Auth(new string[] { "Developers", "Validasi" })]
        public async Task<IActionResult> Validasi(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pRestoran = await _context.PRestoran
                .Include(p => p.IdSptpdNavigation).ThenInclude(p => p.IdCoaNavigation).Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdTarifNavigation)
                .FirstOrDefaultAsync(m => m.IdSptpd == id);
            if (pRestoran == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.Title = (pRestoran.IdSptpdNavigation.Jabatan ? "Validasi Nota Jabatan" : "Validasi SPTPD");
            ViewBag.SubHeaderTitle = pRestoran.IdSptpdNavigation.IdCoaNavigation.Uraian;
            ViewBag.L = Url.Content("/SPTPD/" + id);
            ViewBag.L1 = Url.Action("Validasi", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            //Status
            if (pRestoran.IdSptpdNavigation.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (pRestoran.IdSptpdNavigation.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            else if (pRestoran.IdSptpdNavigation.Keterangan == 2)
            {
                ViewBag.State = "kt-ribbon--warning";
                ViewBag.Status = "Kurang Bayar";
            }
            else if (pRestoran.IdSptpdNavigation.Keterangan == 3)
            {
                ViewBag.State = "kt-ribbon--primary";
                ViewBag.Status = "Lebih Bayar";
            }
            else if (pRestoran.IdSptpdNavigation.Keterangan == 4)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPD";
            }
            else if (pRestoran.IdSptpdNavigation.Keterangan == 5)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPDKB";
            }
            else if (pRestoran.IdSptpdNavigation.Keterangan == 5)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPDKBT";
            }

            return View(pRestoran);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Validasi(Guid id, PRestoran pRestoran)
        {
            if (id != pRestoran.IdSptpd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var pajak = _context.PRestoran.FirstOrDefault(d => d.IdSptpd == id);
                    var spt = _context.Sptpd.Find(id);

                    if (pajak != null && spt != null)
                    {
                        //TARIF PAJAK
                        var tarif = _context.TarifPajak.Where(d => d.IdCoa == spt.IdCoa)
                            .Where(d => d.MulaiBerlaku <= spt.MasaPajak1)
                            .OrderByDescending(d => d.MulaiBerlaku).AsNoTracking().FirstOrDefault();

                        if (tarif == null)
                        {
                            var coa = _context.Coa.Find(spt.IdCoa);
                            TempData["Coa"] = coa.Uraian;
                            TempData["status"] = "TarifNull";
                            return RedirectToAction("Index", "TarifPajak", null);
                        }
                       
                        pajak.Dpp = _context.PRestoranDt.Where(d => d.IdRestoran == pajak.IdRestoran).Sum(d => d.Jumlah);

                        if (pajak.Dpp == 0)
                        {
                            TempData["status"] = "DPP0";
                            return RedirectToAction("SPTPD", new { id });
                        }

                        pajak.PajakTerhutang = pajak.Dpp * (decimal)tarif.Tarif / 100;

                        spt.IdTarif = tarif.IdTarif;
                        spt.Terhutang = pajak.PajakTerhutang;

                        if (spt.FlagValidasi)
                        {
                            spt.FlagValidasi = false;
                            TempData["status"] = "validbatal";
                        }
                        else
                        {
                            spt.FlagValidasi = true;
                            TempData["status"] = "valid";
                        }

                        spt.Eu = HttpContext.Session.GetString("User");
                        spt.Ed = DateTime.Now;

                        _context.PRestoran.Update(pajak);
                        _context.Sptpd.Update(spt);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PRestoranExists(pRestoran.IdRestoran))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "valid";
                return Redirect("/SPTPD/" + id);
            }
            return View(pRestoran);
        }

        [Auth(new string[] { "Developers", "Pendataan" })]
        public IActionResult Cetak(Guid? id)
        {
            return RedirectToAction("PajakRestoran", "Cetak", new { id });
        }

        public JsonResult GetDetail(Guid id)
        {
            var data = (from a in _context.PRestoran
                        join b in _context.PRestoranDt on a.IdRestoran equals b.IdRestoran
                        join c in _context.RefRestoran on b.IdRef equals c.IdRef
                        where b.IdRestoran == id
                        select new
                        {
                            jenis = c.Uraian,
                            jumlah = b.Jumlah,
                            id = b.IdRestoranDt,
                        }).ToList();
            return Json(data);
        }

        [HttpPost]
        public async Task<ActionResult> AddDetail([Bind("IdRestoranDt,IdRestoran,IdRef,Jumlah")] PRestoranDt detail, Guid id)
        {
            if (ModelState.IsValid)
            {
                PRestoranDt pRestoranDt = new PRestoranDt
                {
                    IdRestoran = id,
                    IdRestoranDt = Guid.NewGuid(),
                    IdRef = detail.IdRef,
                    Jumlah = detail.Jumlah
                };
                var data = await _context.RefRestoran.FindAsync(detail.IdRef);
                var check = _context.PRestoranDt.Where(d => d.IdRestoran == id && d.IdRef == detail.IdRef).FirstOrDefault();

                if(check != null)
                {
                    return Json(new { success = false, data.Uraian, exist = true });
                }

                _context.Add(pRestoranDt);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<ActionResult> RemoveDetail(Guid id)
        {
            var detail = await _context.PRestoranDt.Include(d => d.IdRefNavigation).FirstOrDefaultAsync(d => d.IdRestoranDt == id);
            _context.PRestoranDt.Remove(detail);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        private bool PRestoranExists(Guid id)
        {
            return _context.PRestoran.Any(e => e.IdRestoran == id);
        }
    }
}
