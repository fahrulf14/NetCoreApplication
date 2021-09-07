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
    public class STSController : Controller
    {
        private readonly DB_NewContext _context;

        public STSController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "Penyetoran" })]
        public IActionResult Index()
        {
            //Link
            ViewBag.L = Url.Action("Index");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var sts = _context.Sts
                .Include(s => s.IdBankNavigation).ToList();

            return View(sts);

        }

        [Auth(new string[] { "Developers", "Penyetoran" })]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sts = await _context.Sts
                .Include(s => s.IdBankNavigation)
                .FirstOrDefaultAsync(m => m.IdSts == id);
            if (sts == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("Index");
            ViewBag.L1 = Url.Action("Details", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View(sts);
        }

        [Auth(new string[] { "Developers", "Penyetoran" })]
        public async Task<IActionResult> Data(Guid id)
        {
            //Link
            ViewBag.L = Url.Action("Index");
            ViewBag.L1 = Url.Action("Data", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var sts = await _context.Sts.FirstOrDefaultAsync(d => d.IdSts == id);

            if (sts == null)
            {
                return NotFound();
            }

            sts.FlagKonfirmasi = false;
            _context.Update(sts);
            await _context.SaveChangesAsync();

            return View(sts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Data(Guid id, Sts sts)
        {
            if (id != sts.IdSts)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var old = await _context.Sts.FindAsync(id);
                    var detail = _context.StsDt.Where(d => d.IdSts == id);
                    if(old.Keterangan == "SSPD")
                    {
                        old.JumlahSetoran = detail.Sum(d => d.IdSspdNavigation.JumlahSetoran);
                    }
                    else if (old.Keterangan == "STTS")
                    {
                        old.JumlahSetoran = detail.Sum(d => d.IdSttsNavigation.TotalBayar);
                    }
                    else if (old.Keterangan == "SSRD")
                    {
                        old.JumlahSetoran = detail.Sum(d => d.IdSsrdNavigation.JumlahSetoran);
                    }
                    else if (old.Keterangan == "SSRDR")
                    {
                        old.JumlahSetoran = detail.Sum(d => d.IdSsrdrNavigation.Total);
                    }
                    old.FlagKonfirmasi = true;
                    old.Eu = HttpContext.Session.GetString("User");
                    old.Ed = DateTime.Now;
                    _context.Update(old);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StsExists(sts.IdSts))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "edit";
                return RedirectToAction(nameof(Index));
            }
            return View(sts);
        }

        [Auth(new string[] { "Developers", "Penyetoran" })]
        public IActionResult Create(string id)
        {
            //Link
            ViewBag.L = Url.Action("Index");
            ViewBag.L1 = Url.Action("Create", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var bank = (from b in _context.Bank
                        join r in _context.RefBank on b.IdRef equals r.IdBank
                        join j in _context.RefJenisSetoran on b.IdSetoran equals j.IdSetoran
                        where b.IdSetoran == 1
                        select new
                        {
                            value = b.IdBank,
                            text = r.NamaBank + " | " + b.NoRek + " | " + b.NamaRek
                        }).ToList();
            ViewData["IdBank"] = new SelectList(bank, "value", "text");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string id, [Bind("IdSts,IdBank,Tahun,Nomor,NoSts,Tanggal,JumlahSetoran,Keterangan,FlagKonfirmasi,FlagValidasi,TanggalValidasi,Eu,Ed")] Sts sts)
        {
            if (ModelState.IsValid)
            {
                sts.IdSts = Guid.NewGuid();
                sts.Tahun = sts.Tanggal.Value.Year.ToString();
                var noSk = _context.Sts.Where(d => d.Tahun == sts.Tahun).Select(e => e.Nomor).Max() ?? 0;
                sts.Nomor = noSk + 1;
                sts.NoSts = string.Format("{0:000000}", sts.Nomor) + "/STS/" + string.Format("{0:MM/yyyy}", sts.Tanggal);
                sts.Keterangan = id;
                sts.JumlahSetoran = 0;
                sts.Eu = HttpContext.Session.GetString("User");
                sts.Ed = DateTime.Now;
                _context.Add(sts);
                await _context.SaveChangesAsync();
                return RedirectToAction("Data", new { id = sts.IdSts });
            }
            var bank = (from b in _context.Bank
                        join r in _context.RefBank on b.IdRef equals r.IdBank
                        join j in _context.RefJenisSetoran on b.IdSetoran equals j.IdSetoran
                        where b.IdSetoran == 1
                        select new
                        {
                            value = b.IdBank,
                            text = r.NamaBank + " | " + b.NoRek + " | " + b.NamaRek
                        }).ToList();
            ViewData["IdBank"] = new SelectList(bank, "value", "text");
            return View(sts);
        }

        [Auth(new string[] { "Developers", "Penyetoran" })]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sts = await _context.Sts.FindAsync(id);
            if (sts == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("Index");
            ViewBag.L1 = Url.Action("Edit", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var bank = (from b in _context.Bank
                        join r in _context.RefBank on b.IdRef equals r.IdBank
                        join j in _context.RefJenisSetoran on b.IdSetoran equals j.IdSetoran
                        where b.IdSetoran == 1
                        select new
                        {
                            value = b.IdBank,
                            text = r.NamaBank + " | " + b.NoRek + " | " + b.NamaRek
                        }).ToList();
            ViewData["IdBank"] = new SelectList(bank, "value", "text", sts.IdBank);
            return View(sts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdSts,IdBank,Tanggal,Eu,Ed")] Sts sts)
        {
            if (id != sts.IdSts)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var old = await _context.Sts.FindAsync(id);
                    old.IdBank = sts.IdBank;
                    old.Tanggal = sts.Tanggal;
                    old.Tahun = sts.Tanggal.Value.Year.ToString();
                    old.Eu = HttpContext.Session.GetString("User");
                    old.Ed = DateTime.Now;
                    _context.Update(old);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StsExists(sts.IdSts))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "edit";
                return RedirectToAction(nameof(Index));
            }
            var bank = (from b in _context.Bank
                        join r in _context.RefBank on b.IdRef equals r.IdBank
                        join j in _context.RefJenisSetoran on b.IdSetoran equals j.IdSetoran
                        where b.IdSetoran == 1
                        select new
                        {
                            value = b.IdBank,
                            text = r.NamaBank + " | " + b.NoRek + " | " + b.NamaRek
                        }).ToList();
            ViewData["IdBank"] = new SelectList(bank, "value", "text", sts.IdBank);
            return View(sts);
        }

        [Auth(new string[] { "Developers", "Validasi" })]
        //[Auth(new string[] { "Developers" })]
        public async Task<IActionResult> Validasi(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sts = await _context.Sts
                .Include(s => s.IdBankNavigation)
                .FirstOrDefaultAsync(m => m.IdSts == id);
            if (sts == null)
            {
                return NotFound();
            }

            if (sts.FlagValidasi)
            {
                TempData["status"] = "sudahvalid";
                return RedirectToAction(nameof(Index));
            }

            //Link
            ViewBag.L = Url.Action("Index");
            ViewBag.L1 = Url.Action("Validasi", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View(sts);
        }

        //Post: Validasi STS
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Validasi(Sts sts, Guid id)
        {
            Sts old = _context.Sts.Find(id);

            if (old.FlagValidasi)
            {
                old.FlagValidasi = false;
                old.TanggalValidasi = null;
                old.Eu = HttpContext.Session.GetString("User");
                old.Ed = DateTime.Now;
                _context.Update(old);

                var detail = _context.StsDt.Where(d => d.IdSts == id).ToList();

                foreach (var item in detail)
                {
                    if (item.IdSspd != null)
                    {
                        Sspd dok = _context.Sspd.Find(item.IdSspd);
                        if (dok != null)
                        {
                            dok.StatusSetor = false;
                            _context.Update(dok);
                        }
                    }
                    else if (item.IdStts != null)
                    {
                        Stts dok = _context.Stts.Find(item.IdStts);
                        if (dok != null)
                        {
                            dok.StatusSetor = false;
                            _context.Update(dok);
                        }
                    }
                    else if (item.IdSsrd != null)
                    {
                        Ssrd dok = _context.Ssrd.Find(item.IdSsrd);
                        if (dok != null)
                        {
                            dok.StatusSetor = false;
                            _context.Update(dok);
                        }
                    }
                    else if (item.IdSsrdr != null)
                    {
                        Ssrdr dok = _context.Ssrdr.Find(item.IdSsrdr);
                        if (dok != null)
                        {
                            dok.StatusSetor = false;
                            _context.Update(dok);
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                TempData["status"] = "validbatal";
            }
            else
            {
                old.FlagValidasi = true;
                old.TanggalValidasi = sts.TanggalValidasi;
                old.Eu = HttpContext.Session.GetString("User");
                old.Ed = DateTime.Now;
                _context.Update(old);

                var detail = _context.StsDt.Where(d => d.IdSts == id).ToList();

                foreach (var item in detail)
                {
                    if (item.IdSspd != null)
                    {
                        Sspd dok = _context.Sspd.Find(item.IdSspd);
                        if (dok != null)
                        {
                            dok.StatusSetor = true;
                            _context.Update(dok);
                        }
                    }
                    else if (item.IdStts != null)
                    {
                        Stts dok = _context.Stts.Find(item.IdStts);
                        if (dok != null)
                        {
                            dok.StatusSetor = true;
                            _context.Update(dok);
                        }
                    }
                    else if (item.IdSsrd != null)
                    {
                        Ssrd dok = _context.Ssrd.Find(item.IdSsrd);
                        if (dok != null)
                        {
                            dok.StatusSetor = true;
                            _context.Update(dok);
                        }
                    }
                    else if (item.IdSsrdr != null)
                    {
                        Ssrdr dok = _context.Ssrdr.Find(item.IdSsrdr);
                        if (dok != null)
                        {
                            dok.StatusSetor = true;
                            _context.Update(dok);
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                TempData["status"] = "valid";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult AddDetail(Guid id, Guid sts, string jenis)
        {
            if(id != null && sts != null && jenis != null)
            {
                StsDt detail = new StsDt
                {
                    IdDetailSts = Guid.NewGuid(),
                    IdSts = sts
                };
                if (jenis == "SSPD")
                {
                    var check = _context.StsDt.FirstOrDefault(d => d.IdSspd == id);
                    if (check != null)
                    {
                        return Json(new { success = false, exist = true });
                    }
                    detail.IdSspd = id;
                }
                else if (jenis == "STTS")
                {
                    var check = _context.StsDt.FirstOrDefault(d => d.IdStts == id);
                    if (check != null)
                    {
                        return Json(new { success = false, exist = true });
                    }
                    detail.IdStts = id;
                }
                else if (jenis == "SSRD")
                {
                    detail.IdSsrd = id;
                }
                else if (jenis == "SSRDR")
                {
                    detail.IdSsrdr = id;
                }
                _context.StsDt.Add(detail);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            
            return Json(new { success = false });
        }

        // LOAD SSPD BELUM SETOR
        public JsonResult GetSSPD()
        {
            var subselect = (from d in _context.StsDt select d.IdSspd).ToList();
            var data = (from s in _context.Sspd
                        where !subselect.Contains(s.IdSspd) && s.StatusSetor == false && s.FlagBayar
                        select new
                        {
                            id = s.IdSspd,
                            no = s.NoSspd,
                            tanggal = string.Format("{0:dd MMMM yyyy}", s.Tanggal),
                            jumlah = string.Format("{0:C0}", s.JumlahSetoran)
                        }).ToList();
            return Json(data);
        }

        // LOAD STTS BELUM SETOR
        public JsonResult GetSTTS()
        {
            var subselect = (from d in _context.StsDt select d.IdStts).ToList();
            var data = (from s in _context.Stts
                        join p in _context.Sppt on s.IdSppt equals p.IdSppt
                        join o in _context.Spop on p.IdSpop equals o.IdSpop
                        where !subselect.Contains(s.IdStts) && s.StatusSetor == false && s.FlagValidasi
                        select new
                        {
                            id = s.IdStts,
                            no = o.Nop,
                            tanggal = string.Format("{0:dd MMMM yyyy}", s.TanggalBayar),
                            jumlah = string.Format("{0:C0}", s.TotalBayar)
                        }).ToList();
            return Json(data);
        }

        // LOAD SSRD BELUM SETOR
        public JsonResult GetSSRD()
        {
            var subselect = (from d in _context.StsDt select d.IdSsrd).ToList();
            var data = (from s in _context.Ssrd
                        where !subselect.Contains(s.IdSsrd) && s.StatusSetor == false && s.FlagBayar
                        select new
                        {
                            id = s.IdSsrd,
                            no = s.NoSsrd,
                            tanggal = string.Format("{0:dd MMMM yyyy}", s.Tanggal),
                            jumlah = string.Format("{0:C0}", s.JumlahSetoran)
                        }).ToList();
            return Json(data);
        }

        // LOAD SSRDR BELUM SETOR
        public JsonResult GetSSRDR()
        {

            var subselect = (from d in _context.StsDt select d.IdSsrdr).ToList();
            var data = (from s in _context.Ssrdr
                        where !subselect.Contains(s.IdSsrdr) && s.StatusSetor == false && s.TanggalValidasi != null
                        select new
                        {
                            id = s.IdSsrdr,
                            no = s.NoSsrdr,
                            tanggal = string.Format("{0:dd MMMM yyyy}", s.Tanggal),
                            jumlah = string.Format("{0:C0}", s.Total)
                        }).ToList();
            return Json(data);
        }

        public JsonResult GetSTS(Guid id, string jenis)
        {
            var detail = _context.StsDt.Where(d => d.IdSts == id);

            if (jenis == "SSPD")
            {
                var data = (from s in _context.StsDt
                          join d in _context.Sspd on s.IdSspd equals d.IdSspd
                          where s.IdSts == id
                          select new
                          {
                              id = s.IdDetailSts,
                              no = d.NoSspd,
                              tanggal = string.Format("{0:dd MMMM yyyy}", d.TanggalBayar),
                              jumlah = string.Format("{0:C0}", d.JumlahSetoran)
                          }).ToList();
                return Json(data);
            }
            else if (jenis == "STTS")
            {
                var data = (from s in _context.StsDt
                            join d in _context.Stts on s.IdStts equals d.IdStts
                            join p in _context.Sppt on d.IdSppt equals p.IdSppt
                            join o in _context.Spop on p.IdSpop equals o.IdSpop
                            where s.IdSts == id
                            select new
                            {
                                id = s.IdDetailSts,
                                no = o.Nop,
                                tanggal = string.Format("{0:dd MMMM yyyy}", d.TanggalBayar),
                                jumlah = string.Format("{0:C0}", d.TotalBayar)
                            }).ToList();
                return Json(data);
            }
            else if (jenis == "SSRD")
            {
                var data = (from s in _context.StsDt
                            join d in _context.Ssrd on s.IdSsrd equals d.IdSsrd
                            where s.IdSts == id
                            select new
                            {
                                id = s.IdDetailSts,
                                no = d.NoSsrd,
                                tanggal = string.Format("{0:dd MMMM yyyy}", d.TanggalBayar),
                                jumlah = string.Format("{0:C0}", d.JumlahSetoran)
                            }).ToList();
                return Json(data);
            }
            else if (jenis == "SSRDR")
            {
                var data = (from s in _context.StsDt
                            join d in _context.Ssrdr on s.IdSsrdr equals d.IdSsrdr
                            where s.IdSts == id
                            select new
                            {
                                id = s.IdDetailSts,
                                no = d.NoSsrdr,
                                tanggal = string.Format("{0:dd MMMM yyyy}", d.Tanggal),
                                jumlah = string.Format("{0:C0}", d.Total)
                            }).ToList();
                return Json(data);
            }
            
            return Json(new { success = false });
        }

        public JsonResult GetSTSDetail(Guid id, string jenis)
        {
            var detail = _context.StsDt.Where(d => d.IdSts == id);

            if (jenis == "SSPD")
            {
                var data = (from s in _context.StsDt
                            join d in _context.Sspd on s.IdSspd equals d.IdSspd
                            join c in _context.Coa on d.IdCoa equals c.IdCoa
                            where s.IdSts == id
                            select new
                            {
                                id = s.IdDetailSts,
                                no = d.NoSspd,
                                tanggal = string.Format("{0:dd MMMM yyyy}", d.TanggalBayar),
                                jumlah = string.Format("{0:C0}", d.JumlahSetoran),
                                kode = c.IdCoa,
                                uraian = c.Uraian
                            }).ToList();
                return Json(data);
            }
            else if (jenis == "STTS")
            {
                var data = (from s in _context.StsDt
                            join d in _context.Stts on s.IdStts equals d.IdStts
                            join p in _context.Sppt on d.IdSppt equals p.IdSppt
                            join o in _context.Spop on p.IdSpop equals o.IdSpop
                            join c in _context.Coa on o.IdCoa equals c.IdCoa
                            where s.IdSts == id
                            select new
                            {
                                id = s.IdDetailSts,
                                no = o.Nop,
                                tanggal = string.Format("{0:dd MMMM yyyy}", d.TanggalBayar),
                                jumlah = string.Format("{0:C0}", d.TotalBayar),
                                kode = c.IdCoa,
                                uraian = c.Uraian
                            }).ToList();
                return Json(data);
            }
            else if (jenis == "SSRD")
            {
                var data = (from s in _context.StsDt
                            join d in _context.Ssrd on s.IdSsrd equals d.IdSsrd
                            join c in _context.Coa on d.IdCoa equals c.IdCoa
                            where s.IdSts == id
                            select new
                            {
                                id = s.IdDetailSts,
                                no = d.NoSsrd,
                                tanggal = string.Format("{0:dd MMMM yyyy}", d.TanggalBayar),
                                jumlah = string.Format("{0:C0}", d.JumlahSetoran),
                                kode = c.IdCoa,
                                uraian = c.Uraian
                            }).ToList();
                return Json(data);
            }
            else if (jenis == "SSRDR")
            {
                var data = (from s in _context.StsDt
                            join d in _context.Ssrdr on s.IdSsrdr equals d.IdSsrdr
                            join c in _context.Coa on d.IdCoa equals c.IdCoa
                            where s.IdSts == id
                            select new
                            {
                                id = s.IdDetailSts,
                                no = d.NoSsrdr,
                                tanggal = string.Format("{0:dd MMMM yyyy}", d.Tanggal),
                                jumlah = string.Format("{0:C0}", d.Total),
                                kode = c.IdCoa,
                                uraian = c.Uraian
                            }).ToList();
                return Json(data);
            }

            return Json(new { success = false });
        }
        // DEL DETAIL TO STS
        [HttpPost]
        public JsonResult RemoveSTS(Guid id)
        {
            StsDt detail = _context.StsDt.Find(id);
            _context.StsDt.Remove(detail);
            _context.SaveChanges();
            return Json(new { success = true });
        }

        [Auth(new string[] { "Developers", "Penyetoran" })]
        public IActionResult Cetak(Guid id)
        {
            return RedirectToAction("STS", "Cetak", new { id });
        }

        [Auth(new string[] { "Developers", "Laporan" })]
        public IActionResult Register()
        {
            //Link
            ViewBag.L = Url.Action("Index");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            List<SelectListItem> SP = new List<SelectListItem>
            {
              new SelectListItem() { Text = "Januari", Value = "1" },
                new SelectListItem() { Text = "Februari", Value = "2" },
                new SelectListItem() { Text = "Maret", Value = "3" },
                new SelectListItem() { Text = "April", Value = "4" },
                new SelectListItem() { Text = "Mei", Value = "5" },
                new SelectListItem() { Text = "Juni", Value = "6" },
                new SelectListItem() { Text = "Juli", Value = "7" },
                new SelectListItem() { Text = "Agustus", Value = "8" },
                new SelectListItem() { Text = "September", Value = "9" },
                new SelectListItem() { Text = "Oktober", Value = "10" },
                new SelectListItem() { Text = "November", Value = "11" },
                new SelectListItem() { Text = "Desember", Value = "12" }
            };
            ViewData["Bulan"] = new SelectList(SP, "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Tanggal(PeriodeLra lra)
        {
            if (ModelState.IsValid)
            {
                var tanggal1 = lra.Tanggal1;
                var tanggal2 = lra.Tanggal2;
                return RedirectToAction("RegisterSTS", "Cetak", new { id = "Tanggal", tanggal1, tanggal2 });
            }
            return RedirectToAction("Register");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Bulanan(PeriodeLra lra)
        {
            if (ModelState.IsValid)
            {
                var tanggal1 = new DateTime(lra.TahunBulan, lra.Bulan, 1);
                var tanggal2 = new DateTime(lra.TahunBulan, lra.Bulan, DateTime.DaysInMonth(lra.TahunBulan, lra.Bulan));
                return RedirectToAction("RegisterSTS", "Cetak", new { id = "Bulan", tanggal1, tanggal2 });
            }
            return RedirectToAction("Register");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Tahunan(PeriodeLra lra)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("RegisterSTS", "Cetak", new { id = "Tahun", tanggal1 = new DateTime(lra.Tahun, 1, 1), tanggal2 = new DateTime(lra.Tahun, 12, 31) });
            }
            return RedirectToAction("Register");
        }

        [Auth(new string[] { "Developers", "Penyetoran" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sts = await _context.Sts
                .Include(s => s.IdBankNavigation)
                .FirstOrDefaultAsync(m => m.IdSts == id);
            if (sts == null)
            {
                return NotFound();
            }

            if (sts.FlagValidasi)
            {
                return PartialView("_deletevalid");
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var sts = await _context.Sts.FindAsync(id);
            var detail = _context.StsDt.Where(d => d.IdSts == id);
            try
            {
                _context.StsDt.RemoveRange(detail);
                _context.Sts.Remove(sts);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["status"] = "deletefailed";
                string url = Url.Action("Index");
                return Json(new { success = true, url });
            }

            TempData["status"] = "delete";
            string link = Url.Action("Index");
            return Json(new { success = true, url = link });
        }

        private bool StsExists(Guid id)
        {
            return _context.Sts.Any(e => e.IdSts == id);
        }
    }
}
