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
    public class PajakParkirController : Controller
    {
        private readonly DB_NewContext _context;

        public PajakParkirController(DB_NewContext context)
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

            var pParkir = await _context.PParkir
                .Include(p => p.IdSptpdNavigation).ThenInclude(p => p.IdCoaNavigation).Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdTarifNavigation)
                .FirstOrDefaultAsync(m => m.IdSptpd == id);
            if (pParkir == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.Title = (pParkir.IdSptpdNavigation.Jabatan ? "Detail Nota Jabatan" : "Detail SPTPD");
            ViewBag.SubHeaderTitle = pParkir.IdSptpdNavigation.IdCoaNavigation.Uraian;
            ViewBag.L = Url.Content("/SPTPD/" + id);
            ViewBag.L1 = Url.Action("Details", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            //Status
            if (pParkir.IdSptpdNavigation.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (pParkir.IdSptpdNavigation.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            else if (pParkir.IdSptpdNavigation.Keterangan == 2)
            {
                ViewBag.State = "kt-ribbon--warning";
                ViewBag.Status = "Kurang Bayar";
            }
            else if (pParkir.IdSptpdNavigation.Keterangan == 3)
            {
                ViewBag.State = "kt-ribbon--primary";
                ViewBag.Status = "Lebih Bayar";
            }
            else if (pParkir.IdSptpdNavigation.Keterangan == 4)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPD";
            }
            else if (pParkir.IdSptpdNavigation.Keterangan == 5)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPDKB";
            }
            else if (pParkir.IdSptpdNavigation.Keterangan == 5)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPDKBT";
            }

            return View(pParkir);
        }

        [Auth(new string[] { "Developers", "Pendataan" })]
        public async Task<IActionResult> SPTPD(Guid? id, bool jabatan)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pParkir = await _context.PParkir.Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation).FirstOrDefaultAsync(d => d.IdSptpd == id);
            if (pParkir == null)
            {
                return NotFound();
            }

            if (pParkir.IdSptpdNavigation.Jabatan)
            {
                jabatan = pParkir.IdSptpdNavigation.Jabatan;
            }
            TempData["Jabatan"] = jabatan;
            TempData.Keep("Jabatan");

            //Link
            ViewBag.Title = (jabatan ? "Nota Jabatan " : "SPTPD ") + pParkir.IdSptpdNavigation.IdCoaNavigation.Uraian;
            ViewBag.SubHeaderTitle = pParkir.IdSptpdNavigation.IdCoaNavigation.Uraian;
            ViewBag.L = Url.Content("/SPTPD/" + id);
            ViewBag.L1 = Url.Action("SPTPD", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";
            ViewBag.S1 = (jabatan ? "Nota Jabatan " : "SPTPD ");

            List<SelectListItem> SP = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Komputer", Value = "Komputer" },
                new SelectListItem() { Text = "Manual", Value = "Manual" }
            };
            ViewData["Sistem"] = new SelectList(SP, "Value", "Text");

            return View("Edit", pParkir);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SPTPD(Guid id, [Bind("IdParkir,IdSptpd,RekapKarcis,KapasitasMobil,KapasitasMotor,JumlahMobil,JumlahMotor,MobilJam,MobilNext,MobilMax,MotorJam,MotorNext,MotorMax,TotalMobil,TotalMotor,Sistem,Pendapatan,DPP,PajakTerhutang,eu,ed")] PParkir pParkir, Sptpd sptpd)
        {
            if (id != pParkir.IdSptpd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var pajak = _context.PParkir.FirstOrDefault(d => d.IdSptpd == id);
                    var spt = _context.Sptpd.Find(id);

                    if (pajak != null && spt != null)
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

                        pajak.RekapKarcis = pParkir.RekapKarcis;
                        pajak.KapasitasMobil = pParkir.KapasitasMobil ?? 0;
                        pajak.KapasitasMotor = pParkir.KapasitasMotor ?? 0;
                        pajak.JumlahMobil = pParkir.JumlahMobil ?? 0;
                        pajak.JumlahMotor = pParkir.JumlahMotor ?? 0;
                        pajak.MobilJam = pParkir.MobilJam ?? 0;
                        pajak.MotorJam = pParkir.MotorJam ?? 0;
                        pajak.MobilNext = pParkir.MobilNext ?? 0;
                        pajak.MotorNext = pParkir.MotorNext ?? 0;
                        pajak.MobilMax = pParkir.MobilMax ?? 0;
                        pajak.MotorMax = pParkir.MotorMax ?? 0;
                        pajak.TotalMobil = pParkir.TotalMobil ?? 0;
                        pajak.TotalMotor = pParkir.TotalMotor ?? 0;
                        pajak.Sistem = pParkir.Sistem;
                        pajak.Pendapatan = pajak.TotalMobil + pajak.TotalMotor;
                        pajak.Dpp = pajak.Pendapatan;
                        pajak.PajakTerhutang = pajak.Dpp * (decimal)tarif.Tarif;

                        if (pajak.Dpp == 0)
                        {
                            TempData["status"] = "DPP0";
                            return RedirectToAction("SPTPD", new { id });
                        }

                        pajak.PajakTerhutang = pajak.Dpp * (decimal)tarif.Tarif / 100;

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
                                spt.NoSptpd = string.Format("{0:000000}", spt.Nomor) + "/NotaJabatan/Parkir/" + string.Format("{0:MM/yyyy}", spt.Tanggal);
                            }
                            else
                            {
                                var no = _context.Sptpd.Where(e => e.IdCoa.Substring(0, 5) == spt.IdCoa.Substring(0, 5) && e.Jabatan == false && e.Tahun == spt.Tahun).Select(e => e.Nomor).Max() ?? 0;
                                spt.Nomor = no + 1;
                                spt.NoSptpd = string.Format("{0:000000}", spt.Nomor) + "/SPTPD/Parkir/" + string.Format("{0:MM/yyyy}", spt.Tanggal);
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
                            PParkir newPajak = new PParkir
                            {
                                IdParkir = Guid.NewGuid(),
                                IdSptpd = newSpt.IdSptpd,
                                RekapKarcis = pajak.RekapKarcis,
                                Sistem = pajak.Sistem
                            };

                            _context.Sptpd.Add(newSpt);
                            _context.PParkir.Add(newPajak);
                        }

                        _context.PParkir.Update(pajak);
                        _context.Sptpd.Update(spt);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PParkirExists(pParkir.IdParkir))
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
            return View(pParkir);
        }

        [Auth(new string[] { "Developers", "Validasi" })]
        public async Task<IActionResult> Validasi(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pParkir = await _context.PParkir
                .Include(p => p.IdSptpdNavigation).ThenInclude(p => p.IdCoaNavigation).Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdTarifNavigation)
                .FirstOrDefaultAsync(m => m.IdSptpd == id);
            if (pParkir == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.Title = (pParkir.IdSptpdNavigation.Jabatan ? "Validasi Nota Jabatan" : "Validasi SPTPD");
            ViewBag.SubHeaderTitle = pParkir.IdSptpdNavigation.IdCoaNavigation.Uraian;
            ViewBag.L = Url.Content("/SPTPD/" + id);
            ViewBag.L1 = Url.Action("Validasi", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            //Status
            if (pParkir.IdSptpdNavigation.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (pParkir.IdSptpdNavigation.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            else if (pParkir.IdSptpdNavigation.Keterangan == 2)
            {
                ViewBag.State = "kt-ribbon--warning";
                ViewBag.Status = "Kurang Bayar";
            }
            else if (pParkir.IdSptpdNavigation.Keterangan == 3)
            {
                ViewBag.State = "kt-ribbon--primary";
                ViewBag.Status = "Lebih Bayar";
            }
            else if (pParkir.IdSptpdNavigation.Keterangan == 4)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPD";
            }
            else if (pParkir.IdSptpdNavigation.Keterangan == 5)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPDKB";
            }
            else if (pParkir.IdSptpdNavigation.Keterangan == 5)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPDKBT";
            }

            return View(pParkir);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Validasi(Guid id, PParkir pParkir)
        {
            if (id != pParkir.IdSptpd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var spt = _context.Sptpd.Find(id);

                    if (spt != null)
                    {
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
                        _context.Sptpd.Update(spt);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PParkirExists(pParkir.IdParkir))
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
            return View(pParkir);
        }

        [Auth(new string[] { "Developers", "Pendataan" })]
        public IActionResult Cetak(Guid? id)
        {
            return RedirectToAction("PajakParkir", "Cetak", new { id });
        }
        private bool PParkirExists(Guid id)
        {
            return _context.PParkir.Any(e => e.IdParkir == id);
        }
    }
}
