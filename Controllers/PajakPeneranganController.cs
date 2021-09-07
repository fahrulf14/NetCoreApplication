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
    public class PajakPeneranganController : Controller
    {
        private readonly DB_NewContext _context;

        public PajakPeneranganController(DB_NewContext context)
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

            var pPenerangan = await _context.PPenerangan
                .Include(p => p.IdSptpdNavigation).ThenInclude(p => p.IdCoaNavigation).Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdTarifNavigation)
                .FirstOrDefaultAsync(m => m.IdSptpd == id);
            if (pPenerangan == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.Title = (pPenerangan.IdSptpdNavigation.Jabatan ? "Detail Nota Jabatan" : "Detail SPTPD");
            ViewBag.SubHeaderTitle = pPenerangan.IdSptpdNavigation.IdCoaNavigation.Uraian;
            ViewBag.L = Url.Content("/SPTPD/" + id);
            ViewBag.L1 = Url.Action("Details", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            //Status
            if (pPenerangan.IdSptpdNavigation.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (pPenerangan.IdSptpdNavigation.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            else if (pPenerangan.IdSptpdNavigation.Keterangan == 2)
            {
                ViewBag.State = "kt-ribbon--warning";
                ViewBag.Status = "Kurang Bayar";
            }
            else if (pPenerangan.IdSptpdNavigation.Keterangan == 3)
            {
                ViewBag.State = "kt-ribbon--primary";
                ViewBag.Status = "Lebih Bayar";
            }
            else if (pPenerangan.IdSptpdNavigation.Keterangan == 4)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPD";
            }
            else if (pPenerangan.IdSptpdNavigation.Keterangan == 5)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPDKB";
            }
            else if (pPenerangan.IdSptpdNavigation.Keterangan == 5)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPDKBT";
            }

            return View(pPenerangan);
        }

        [Auth(new string[] { "Developers", "Pendataan" })]
        public async Task<IActionResult> SPTPD(Guid? id, bool jabatan)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pPenerangan = await _context.PPenerangan.Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation).FirstOrDefaultAsync(d => d.IdSptpd == id);
            if (pPenerangan == null)
            {
                return NotFound();
            }

            if (pPenerangan.IdSptpdNavigation.Jabatan)
            {
                jabatan = pPenerangan.IdSptpdNavigation.Jabatan;
            }
            TempData["Jabatan"] = jabatan;
            TempData.Keep("Jabatan");

            //Link
            ViewBag.Title = (jabatan ? "Nota Jabatan " : "SPTPD ") + pPenerangan.IdSptpdNavigation.IdCoaNavigation.Uraian;
            ViewBag.SubHeaderTitle = pPenerangan.IdSptpdNavigation.IdCoaNavigation.Uraian;
            ViewBag.L = Url.Content("/SPTPD/" + id);
            ViewBag.L1 = Url.Action("SPTPD", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";
            ViewBag.S1 = (jabatan ? "Nota Jabatan " : "SPTPD ");

            var date = string.Format("{0:dd/MM/yyyy}", pPenerangan.Bulan?.Date);
            List <SelectListItem> SP = new List<SelectListItem>
            {
              new SelectListItem() { Text = "Januari", Value = "01/01/" + (pPenerangan.Bulan?.Year ?? DateTime.Now.Year) },
                new SelectListItem() { Text = "Februari", Value = "01/02/" + (pPenerangan.Bulan?.Year ?? DateTime.Now.Year) },
                new SelectListItem() { Text = "Maret", Value = "01/03/" + (pPenerangan.Bulan?.Year ?? DateTime.Now.Year) },
                new SelectListItem() { Text = "April", Value = "01/04/" + (pPenerangan.Bulan?.Year ?? DateTime.Now.Year) },
                new SelectListItem() { Text = "Mei", Value = "01/05/"  + (pPenerangan.Bulan?.Year ?? DateTime.Now.Year) },
                new SelectListItem() { Text = "Juni", Value = "01/06/" + (pPenerangan.Bulan?.Year ?? DateTime.Now.Year) },
                new SelectListItem() { Text = "Juli", Value = "01/07/" + (pPenerangan.Bulan?.Year ?? DateTime.Now.Year) },
                new SelectListItem() { Text = "Agustus", Value = "01/08/" + (pPenerangan.Bulan?.Year ?? DateTime.Now.Year) },
                new SelectListItem() { Text = "September", Value = "01/09/" + (pPenerangan.Bulan?.Year ?? DateTime.Now.Year) },
                new SelectListItem() { Text = "Oktober", Value = "01/10/" + (pPenerangan.Bulan?.Year ?? DateTime.Now.Year) },
                new SelectListItem() { Text = "November", Value = "01/11/" + (pPenerangan.Bulan?.Year ?? DateTime.Now.Year) },
                new SelectListItem() { Text = "Desember", Value = "01/12/" + (pPenerangan.Bulan?.Year ?? DateTime.Now.Year) }
            };
            ViewData["Bulan"] = new SelectList(SP, "Value", "Text");
            ViewBag.HargaKwh = _context.RefHargaKwh.Where(d => d.FlagAktif).OrderByDescending(d => d.TanggalBerlaku).FirstOrDefault().Tarif;
            ViewBag.Selected = date;

            return View("Edit", pPenerangan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SPTPD(Guid id, [Bind("IdPenerangan,IdSptpd,Sumber,Golongan,Bulan,JumlahKwh,TarifListrik,Tagihan,Rekap,Dpp,PajakTerhutang,Eu,Ed")] PPenerangan pPenerangan, Sptpd sptpd)
        {
            if (id != pPenerangan.IdSptpd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var pajak = _context.PPenerangan.FirstOrDefault(d => d.IdSptpd == id);
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

                        pajak.Sumber = pPenerangan.Sumber;
                        pajak.Golongan = pPenerangan.Golongan;
                        pajak.Bulan = pPenerangan.Bulan;
                        pajak.JumlahKwh = pPenerangan.JumlahKwh;
                        pajak.TarifListrik = pPenerangan.TarifListrik;
                        pajak.Tagihan = pPenerangan.Tagihan;
                        pajak.Rekap = pPenerangan.Rekap;
                        pajak.Dpp = (decimal)pPenerangan.JumlahKwh * pPenerangan.TarifListrik;
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
                                spt.NoSptpd = string.Format("{0:000000}", spt.Nomor) + "/NotaJabatan/Penerangan/" + string.Format("{0:MM/yyyy}", spt.Tanggal);
                            }
                            else
                            {
                                var no = _context.Sptpd.Where(e => e.IdCoa.Substring(0, 5) == spt.IdCoa.Substring(0, 5) && e.Jabatan == false && e.Tahun == spt.Tahun).Select(e => e.Nomor).Max() ?? 0;
                                spt.Nomor = no + 1;
                                spt.NoSptpd = string.Format("{0:000000}", spt.Nomor) + "/SPTPD/Penerangan/" + string.Format("{0:MM/yyyy}", spt.Tanggal);
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
                            PPenerangan newPajak = new PPenerangan
                            {
                                IdPenerangan = Guid.NewGuid(),
                                IdSptpd = newSpt.IdSptpd,
                                Rekap = pPenerangan.Rekap
                            };

                            _context.Sptpd.Add(newSpt);
                            _context.PPenerangan.Add(newPajak);
                        }

                        _context.PPenerangan.Update(pajak);
                        _context.Sptpd.Update(spt);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PPeneranganExists(pPenerangan.IdPenerangan))
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
            return View("Edit", pPenerangan);
        }

        [Auth(new string[] { "Developers", "Validasi" })]
        public async Task<IActionResult> Validasi(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pPenerangan = await _context.PPenerangan
                .Include(p => p.IdSptpdNavigation).ThenInclude(p => p.IdCoaNavigation).Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdTarifNavigation)
                .FirstOrDefaultAsync(m => m.IdSptpd == id);
            if (pPenerangan == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.Title = (pPenerangan.IdSptpdNavigation.Jabatan ? "Validasi Nota Jabatan" : "Validasi SPTPD");
            ViewBag.SubHeaderTitle = pPenerangan.IdSptpdNavigation.IdCoaNavigation.Uraian;
            ViewBag.L = Url.Content("/SPTPD/" + id);
            ViewBag.L1 = Url.Action("Validasi", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            //Status
            if (pPenerangan.IdSptpdNavigation.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (pPenerangan.IdSptpdNavigation.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            else if (pPenerangan.IdSptpdNavigation.Keterangan == 2)
            {
                ViewBag.State = "kt-ribbon--warning";
                ViewBag.Status = "Kurang Bayar";
            }
            else if (pPenerangan.IdSptpdNavigation.Keterangan == 3)
            {
                ViewBag.State = "kt-ribbon--primary";
                ViewBag.Status = "Lebih Bayar";
            }
            else if (pPenerangan.IdSptpdNavigation.Keterangan == 4)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPD";
            }
            else if (pPenerangan.IdSptpdNavigation.Keterangan == 5)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPDKB";
            }
            else if (pPenerangan.IdSptpdNavigation.Keterangan == 5)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPDKBT";
            }

            return View(pPenerangan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Validasi(Guid id, PPenerangan pPenerangan)
        {
            if (id != pPenerangan.IdSptpd)
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
                    if (!PPeneranganExists(pPenerangan.IdPenerangan))
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
            return View(pPenerangan);
        }

        [Auth(new string[] { "Developers", "Pendataan" })]
        public IActionResult Cetak(Guid? id)
        {
            return RedirectToAction("PajakPenerangan", "Cetak", new { id });
        }

        private bool PPeneranganExists(Guid id)
        {
            return _context.PPenerangan.Any(e => e.IdPenerangan == id);
        }
    }
}
