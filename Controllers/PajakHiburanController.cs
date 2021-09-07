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
    public class PajakHiburanController : Controller
    {
        private readonly DB_NewContext _context;

        public PajakHiburanController(DB_NewContext context)
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

            var pHiburan = await _context.PHiburan
                .Include(p => p.IdSptpdNavigation).ThenInclude(p => p.IdCoaNavigation).Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdTarifNavigation)
                .FirstOrDefaultAsync(m => m.IdSptpd == id);
            if (pHiburan == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.Title = (pHiburan.IdSptpdNavigation.Jabatan ? "Detail Nota Jabatan" : "Detail SPTPD");
            ViewBag.SubHeaderTitle = pHiburan.IdSptpdNavigation.IdCoaNavigation.Uraian;
            ViewBag.L = Url.Content("/SPTPD/" + id);
            ViewBag.L1 = Url.Action("Details", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            //Status
            if (pHiburan.IdSptpdNavigation.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (pHiburan.IdSptpdNavigation.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            else if (pHiburan.IdSptpdNavigation.Keterangan == 2)
            {
                ViewBag.State = "kt-ribbon--warning";
                ViewBag.Status = "Kurang Bayar";
            }
            else if (pHiburan.IdSptpdNavigation.Keterangan == 3)
            {
                ViewBag.State = "kt-ribbon--primary";
                ViewBag.Status = "Lebih Bayar";
            }
            else if (pHiburan.IdSptpdNavigation.Keterangan == 4)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPD";
            }
            else if (pHiburan.IdSptpdNavigation.Keterangan == 5)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPDKB";
            }
            else if (pHiburan.IdSptpdNavigation.Keterangan == 5)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPDKBT";
            }

            return View(pHiburan);
        }

        [Auth(new string[] { "Developers", "Pendataan" })]
        public async Task<IActionResult> SPTPD(Guid? id, bool jabatan)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pHiburan = await _context.PHiburan.Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation).FirstOrDefaultAsync(d => d.IdSptpd == id);
            if (pHiburan == null)
            {
                return NotFound();
            }

            if (pHiburan.IdSptpdNavigation.Jabatan)
            {
                jabatan = pHiburan.IdSptpdNavigation.Jabatan;
            }
            TempData["Jabatan"] = jabatan;
            TempData.Keep("Jabatan");

            //Link
            ViewBag.Title = (jabatan ? "Nota Jabatan " : "SPTPD ") + pHiburan.IdSptpdNavigation.IdCoaNavigation.Uraian;
            ViewBag.SubHeaderTitle = pHiburan.IdSptpdNavigation.IdCoaNavigation.Uraian;
            ViewBag.L = Url.Content("/SPTPD/" + id);
            ViewBag.L1 = Url.Action("SPTPD", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";
            ViewBag.S1 = (jabatan ? "Nota Jabatan " : "SPTPD ");

           
            return View("Edit", pHiburan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SPTPD(Guid id, [Bind("IdHiburan,IdSptpd,Pembukuan,KasRegister,Dpp,PajakTerhutang,Eu,Ed")] PHiburan pHiburan, Sptpd sptpd)
        {
            if (id != pHiburan.IdSptpd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var pajak = _context.PHiburan.FirstOrDefault(d => d.IdSptpd == id);
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

                        pajak.KasRegister = pHiburan.KasRegister;
                        pajak.Pembukuan = pHiburan.Pembukuan;
                       
                        pajak.Dpp = (from k in _context.PHiburanDt
                                     join h in _context.PHiburan on k.IdHiburan equals h.IdHiburan
                                     where k.IdHiburan == pajak.IdHiburan
                                     select new
                                     {
                                         total = k.Jumlah * k.Tarif
                                     }).Sum(d => d.total);

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
                                spt.NoSptpd = string.Format("{0:000000}", spt.Nomor) + "/NotaJabatan/Hiburan/" + string.Format("{0:MM/yyyy}", spt.Tanggal);
                            }
                            else
                            {
                                var no = _context.Sptpd.Where(e => e.IdCoa.Substring(0, 5) == spt.IdCoa.Substring(0, 5) && e.Jabatan == false && e.Tahun == spt.Tahun).Select(e => e.Nomor).Max() ?? 0;
                                spt.Nomor = no + 1;
                                spt.NoSptpd = string.Format("{0:000000}", spt.Nomor) + "/SPTPD/Hiburan/" + string.Format("{0:MM/yyyy}", spt.Tanggal);
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
                            PHiburan newPajak = new PHiburan
                            {
                                IdHiburan = Guid.NewGuid(),
                                IdSptpd = newSpt.IdSptpd,
                                KasRegister = pHiburan.KasRegister,
                                Pembukuan = pHiburan.Pembukuan
                            };

                            _context.Sptpd.Add(newSpt);
                            _context.PHiburan.Add(newPajak);
                        }

                        _context.PHiburan.Update(pajak);
                        _context.Sptpd.Update(spt);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PHiburanExists(pHiburan.IdHiburan))
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
            return View(pHiburan);
        }

        [Auth(new string[] { "Developers", "Validasi" })]
        public async Task<IActionResult> Validasi(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pHiburan = await _context.PHiburan
                .Include(p => p.IdSptpdNavigation).ThenInclude(p => p.IdCoaNavigation).Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdTarifNavigation)
                .FirstOrDefaultAsync(m => m.IdSptpd == id);
            if (pHiburan == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.Title = (pHiburan.IdSptpdNavigation.Jabatan ? "Validasi Nota Jabatan" : "Validasi SPTPD");
            ViewBag.SubHeaderTitle = pHiburan.IdSptpdNavigation.IdCoaNavigation.Uraian;
            ViewBag.L = Url.Content("/SPTPD/" + id);
            ViewBag.L1 = Url.Action("Validasi", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            //Status
            if (pHiburan.IdSptpdNavigation.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (pHiburan.IdSptpdNavigation.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            else if (pHiburan.IdSptpdNavigation.Keterangan == 2)
            {
                ViewBag.State = "kt-ribbon--warning";
                ViewBag.Status = "Kurang Bayar";
            }
            else if (pHiburan.IdSptpdNavigation.Keterangan == 3)
            {
                ViewBag.State = "kt-ribbon--primary";
                ViewBag.Status = "Lebih Bayar";
            }
            else if (pHiburan.IdSptpdNavigation.Keterangan == 4)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPD";
            }
            else if (pHiburan.IdSptpdNavigation.Keterangan == 5)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPDKB";
            }
            else if (pHiburan.IdSptpdNavigation.Keterangan == 5)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPDKBT";
            }

            return View(pHiburan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Validasi(Guid id, PHiburan pHiburan)
        {
            if (id != pHiburan.IdSptpd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var pajak = _context.PHiburan.FirstOrDefault(d => d.IdSptpd == id);
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

                        pajak.Dpp = (from k in _context.PHiburanDt
                                     join h in _context.PHiburan on k.IdHiburan equals h.IdHiburan
                                     where k.IdHiburan == pajak.IdHiburan
                                     select new
                                     {
                                         total = k.Jumlah * k.Tarif
                                     }).Sum(d => d.total);

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

                        _context.PHiburan.Update(pajak);
                        _context.Sptpd.Update(spt);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PHiburanExists(pHiburan.IdHiburan))
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
            return View(pHiburan);
        }

        [Auth(new string[] { "Developers", "Pendataan" })]
        public IActionResult Cetak(Guid? id)
        {
            return RedirectToAction("PajakHiburan", "Cetak", new { id });
        }

        [HttpPost]
        public async Task<ActionResult> AddDetail([Bind("IdHiburanDt,IdHiburan,Jenis,Tarif,Jumlah,Satuan")] PHiburanDt pHiburanDt, Guid id)
        {
            if (ModelState.IsValid)
            {
              
                PHiburanDt pHiburan = new PHiburanDt
                {
                    IdHiburan = id,
                    IdHiburanDt = Guid.NewGuid(),
                    Tarif = pHiburanDt.Tarif,
                    Satuan = pHiburanDt.Satuan,
                    Jumlah = pHiburanDt.Jumlah,
                    Jenis = pHiburanDt.Jenis
                };

                _context.Add(pHiburan);
                await _context.SaveChangesAsync();
                return Json(new { success = true});
            }
            return Json(new { success = false });
        }

        public JsonResult GetDetail(Guid id)
        {
            var data = (from a in _context.PHiburan
                        join b in _context.PHiburanDt on a.IdHiburan equals b.IdHiburan
                        where b.IdHiburan == id
                        select new
                        {
                            jenis = b.Jenis,
                            jumlah = b.Jumlah,
                            tarif = b.Tarif,
                            satuan = b.Satuan,
                            id = b.IdHiburanDt
                        }).ToList();
            return Json(data);
        }

        [HttpPost]
        public async Task<ActionResult> RemoveDetail(Guid id)
        {
            var detail = await _context.PHiburanDt.FirstOrDefaultAsync(d => d.IdHiburanDt == id);
            _context.PHiburanDt.Remove(detail);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        private bool PHiburanExists(Guid id)
        {
            return _context.PHiburan.Any(e => e.IdHiburan == id);
        }
    }
}
