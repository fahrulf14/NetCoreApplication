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
    public class PajakHotelController : Controller
    {
        private readonly DB_NewContext _context;

        public PajakHotelController(DB_NewContext context)
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

            var pHotel = await _context.PHotel
                .Include(p => p.IdSptpdNavigation).ThenInclude(p => p.IdCoaNavigation).Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdTarifNavigation)
                .FirstOrDefaultAsync(m => m.IdSptpd == id);
            if (pHotel == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.Title = (pHotel.IdSptpdNavigation.Jabatan ? "Detail Nota Jabatan" : "Detail SPTPD");
            ViewBag.SubHeaderTitle = pHotel.IdSptpdNavigation.IdCoaNavigation.Uraian;
            ViewBag.L = Url.Content("/SPTPD/" + id);
            ViewBag.L1 = Url.Action("Details", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            //Status
            if (pHotel.IdSptpdNavigation.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (pHotel.IdSptpdNavigation.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            else if (pHotel.IdSptpdNavigation.Keterangan == 2)
            {
                ViewBag.State = "kt-ribbon--warning";
                ViewBag.Status = "Kurang Bayar";
            }
            else if (pHotel.IdSptpdNavigation.Keterangan == 3)
            {
                ViewBag.State = "kt-ribbon--primary";
                ViewBag.Status = "Lebih Bayar";
            }
            else if (pHotel.IdSptpdNavigation.Keterangan == 4)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPD";
            }
            else if (pHotel.IdSptpdNavigation.Keterangan == 5)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPDKB";
            }
            else if (pHotel.IdSptpdNavigation.Keterangan == 5)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPDKBT";
            }

            return View(pHotel);
        }

        [Auth(new string[] { "Developers", "Pendataan" })]
        public async Task<IActionResult> SPTPD(Guid? id, bool jabatan)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pHotel = await _context.PHotel.Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation).FirstOrDefaultAsync(d => d.IdSptpd == id);
            if (pHotel == null)
            {
                return NotFound();
            }

            if (pHotel.IdSptpdNavigation.FlagValidasi)
            {
                TempData["status"] = "sudahvalid";
                RedirectToAction("SPTPD", new { id });
            }

            if (pHotel.IdSptpdNavigation.Jabatan)
            {
                jabatan = pHotel.IdSptpdNavigation.Jabatan;
            }
            TempData["Jabatan"] = jabatan;
            TempData.Keep("Jabatan");

            //Link
            ViewBag.Title = (jabatan ? "Nota Jabatan " : "SPTPD ") + pHotel.IdSptpdNavigation.IdCoaNavigation.Uraian;
            ViewBag.SubHeaderTitle = pHotel.IdSptpdNavigation.IdCoaNavigation.Uraian;
            ViewBag.L = Url.Content("/SPTPD/" + id);
            ViewBag.L1 = Url.Action("SPTPD", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";
            ViewBag.S1 = (jabatan ? "Nota Jabatan " : "SPTPD ");

            ViewData["Fasilitas"] = new SelectList(_context.RefHotel.Where(d => d.Jenis == "Fasilitas Hotel"), "IdRef", "Uraian");
            ViewData["Restoran"] = new SelectList(_context.RefHotel.Where(d => d.Jenis == "Layanan Restoran"), "IdRef", "Uraian");
            ViewData["Layanan"] = new SelectList(_context.RefHotel.Where(d => d.Jenis == "Layanan Lain"), "IdRef", "Uraian");

            return View("Edit", pHotel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SPTPD(Guid id, [Bind("IdHotel,IdSptpd,KasRegister,Pembukuan,Dpp,PajakTerhutang,Eu,Ed")] PHotel pHotel, Sptpd sptpd)
        {
            if (id != pHotel.IdSptpd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var pajak = _context.PHotel.FirstOrDefault(d => d.IdSptpd == id);
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

                        pajak.KasRegister = pHotel.KasRegister;
                        pajak.Pembukuan = pHotel.Pembukuan;

                        var detail = _context.PHotelDt.Where(d => d.IdHotel == pajak.IdHotel).Sum(d => d.Jumlah);
                        var kamar = (from k in _context.PHotelKm
                                     join h in _context.PHotel on k.IdHotel equals h.IdHotel
                                     where k.IdHotel == pajak.IdHotel
                                     select new
                                     {
                                         total = k.Jumlah * k.Tarif
                                     }).Sum(d => d.total);

                        pajak.Dpp = detail + kamar;

                        if(pajak.Dpp == 0)
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
                                spt.NoSptpd = string.Format("{0:000000}", spt.Nomor) + "/NotaJabatan/Hotel/" + string.Format("{0:MM/yyyy}", spt.Tanggal);
                            }
                            else
                            {
                                var no = _context.Sptpd.Where(e => e.IdCoa.Substring(0, 5) == spt.IdCoa.Substring(0, 5) && e.Jabatan == false && e.Tahun == spt.Tahun).Select(e => e.Nomor).Max() ?? 0;
                                spt.Nomor = no + 1;
                                spt.NoSptpd = string.Format("{0:000000}", spt.Nomor) + "/SPTPD/Hotel/" + string.Format("{0:MM/yyyy}", spt.Tanggal);
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
                            PHotel newPajak = new PHotel
                            {
                                IdHotel = Guid.NewGuid(),
                                IdSptpd = newSpt.IdSptpd,
                                KasRegister = pHotel.KasRegister,
                                Pembukuan = pHotel.Pembukuan
                            };

                            _context.Sptpd.Add(newSpt);
                            _context.PHotel.Add(newPajak);
                        }

                        _context.PHotel.Update(pajak);
                        _context.Sptpd.Update(spt);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PHotelExists(pHotel.IdHotel))
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
            return View(pHotel);
        }

        [Auth(new string[] { "Developers", "Validasi" })]
        public async Task<IActionResult> Validasi(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pHotel = await _context.PHotel
                .Include(p => p.IdSptpdNavigation).ThenInclude(p => p.IdCoaNavigation).Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdTarifNavigation)
                .FirstOrDefaultAsync(m => m.IdSptpd == id);
            if (pHotel == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.Title = (pHotel.IdSptpdNavigation.Jabatan ? "Validasi Nota Jabatan" : "Validasi SPTPD");
            ViewBag.SubHeaderTitle = pHotel.IdSptpdNavigation.IdCoaNavigation.Uraian;
            ViewBag.L = Url.Content("/SPTPD/" + id);
            ViewBag.L1 = Url.Action("Validasi", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            //Status
            if (pHotel.IdSptpdNavigation.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (pHotel.IdSptpdNavigation.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            else if (pHotel.IdSptpdNavigation.Keterangan == 2)
            {
                ViewBag.State = "kt-ribbon--warning";
                ViewBag.Status = "Kurang Bayar";
            }
            else if (pHotel.IdSptpdNavigation.Keterangan == 3)
            {
                ViewBag.State = "kt-ribbon--primary";
                ViewBag.Status = "Lebih Bayar";
            }
            else if (pHotel.IdSptpdNavigation.Keterangan == 4)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPD";
            }
            else if (pHotel.IdSptpdNavigation.Keterangan == 5)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPDKB";
            }
            else if (pHotel.IdSptpdNavigation.Keterangan == 5)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPDKBT";
            }

            return View(pHotel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Validasi(Guid id, PHotel pHotel)
        {
            if (id != pHotel.IdSptpd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var pajak = _context.PHotel.FirstOrDefault(d => d.IdSptpd == id);
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

                        var detail = _context.PHotelDt.Where(d => d.IdHotel == pajak.IdHotel).Sum(d => d.Jumlah);
                        var kamar = (from k in _context.PHotelKm
                                     join h in _context.PHotel on k.IdHotel equals h.IdHotel
                                     where k.IdHotel == pajak.IdHotel
                                     select new
                                     {
                                         total = k.Jumlah * k.Tarif
                                     }).Sum(d => d.total);

                        pajak.Dpp = detail + kamar;

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

                        _context.PHotel.Update(pajak);
                        _context.Sptpd.Update(spt);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PHotelExists(pHotel.IdHotel))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("/SPTPD/" + id);
            }
            return View(pHotel);
        }

        [Auth(new string[] { "Developers", "Pendataan" })]
        public IActionResult Cetak(Guid id)
        {
            return RedirectToAction("PajakHotel", "Cetak", new { id });
        }

        [HttpPost]
        public async Task<ActionResult> AddKamar([Bind("IdHotel_Km,IdHotel,NamaKamar,Tarif,Jumlah")]PHotelKm kamar)
        {
            if (ModelState.IsValid)
            {
                kamar.IdHotelKm = Guid.NewGuid();
                _context.Add(kamar);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<ActionResult> RemoveKamar(Guid id)
        {
            var kamar = await _context.PHotelKm.FindAsync(id);
            _context.PHotelKm.Remove(kamar);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        public JsonResult GetKamar(Guid id)
        {
            var data = (from a in _context.PHotel
                      join b in _context.PHotelKm on a.IdHotel equals b.IdHotel
                      where b.IdHotel == id
                      select new
                      {
                          jenis = b.NamaKamar,
                          jumlah = b.Jumlah,
                          tarif = b.Tarif,
                          id = b.IdHotelKm
                      }).ToList();
            return Json(data);
        }

        public JsonResult GetFasilitas(Guid id)
        {
            var data = (from a in _context.PHotel
                        join b in _context.PHotelDt on a.IdHotel equals b.IdHotel
                        join c in _context.RefHotel on b.IdRef equals c.IdRef
                        where b.IdHotel == id && c.Jenis == "Fasilitas Hotel"
                        select new
                        {
                            jenis = c.Uraian,
                            jumlah = b.Jumlah,
                            id = b.IdHotelDt,
                        }).ToList();
            return Json(data);
        }

        public JsonResult GetRestoran(Guid id)
        {
            var data = (from a in _context.PHotel
                        join b in _context.PHotelDt on a.IdHotel equals b.IdHotel
                        join c in _context.RefHotel on b.IdRef equals c.IdRef
                        where b.IdHotel == id && c.Jenis == "Layanan Restoran"
                        select new
                        {
                            jenis = c.Uraian,
                            jumlah = b.Jumlah,
                            id = b.IdHotelDt,
                        }).ToList();
            return Json(data);
        }

        public JsonResult GetLayanan(Guid id)
        {
            var data = (from a in _context.PHotel
                        join b in _context.PHotelDt on a.IdHotel equals b.IdHotel
                        join c in _context.RefHotel on b.IdRef equals c.IdRef
                        where b.IdHotel == id && c.Jenis == "Layanan Lain"
                        select new
                        {
                            jenis = c.Uraian,
                            jumlah = b.Jumlah,
                            id = b.IdHotelDt,
                        }).ToList();
            return Json(data);
        }

        [HttpPost]
        public async Task<ActionResult> AddDetail(DetailHotel detail, Guid id)
        {
            if (ModelState.IsValid)
            {
                var IdRef = 0;
                var Jumlah = 0;
                if(detail.IdFasilitas != 0)
                {
                    IdRef = detail.IdFasilitas;
                    Jumlah = detail.OmzetFasilitas;
                }
                else if(detail.IdRestoran != 0)
                {
                    IdRef = detail.IdRestoran;
                    Jumlah = detail.OmzetRestoran;
                }
                else if (detail.IdLayanan != 0)
                {
                    IdRef = detail.IdLayanan;
                    Jumlah = detail.OmzetLayanan;
                }
                PHotelDt pHotelDt = new PHotelDt
                {
                    IdHotel = id,
                    IdHotelDt = Guid.NewGuid(),
                    IdRef = IdRef,
                    Jumlah = Jumlah
                };
                var data = await _context.RefHotel.FindAsync(IdRef);
                var check = _context.PHotelDt.Where(d => d.IdHotel == id && d.IdRef == IdRef).FirstOrDefault();

                if(check != null)
                {
                    return Json(new { success = false, data.Jenis, data.Uraian, exist = true });
                }

                _context.Add(pHotelDt);
                await _context.SaveChangesAsync();

                return Json(new { success = true, data.Jenis });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<ActionResult> RemoveDetail(Guid id)
        {
            var detail = await _context.PHotelDt.Include(d => d.IdRefNavigation).FirstOrDefaultAsync(d => d.IdHotelDt == id);
            _context.PHotelDt.Remove(detail);
            await _context.SaveChangesAsync();
            return Json(new { success = true, detail.IdRefNavigation.Jenis });
        }

        private bool PHotelExists(Guid id)
        {
            return _context.PHotel.Any(e => e.IdHotel == id);
        }
    }
}
