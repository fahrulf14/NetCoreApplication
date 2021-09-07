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
    public class SSPDController : Controller
    {
        private readonly DB_NewContext _context;

        public SSPDController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "Pembayaran" })]
        public IActionResult Index()
        {
            //Link
            ViewBag.L = Url.Action("Index", null);
            ViewBag.L1 = Url.Action("Index", null);
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View();
        }

        public JsonResult GetSubjek(JQueryDataTableParamModel param)
        {
            var allRecords = _context.DataSubjek.Where(d => d.Npwpd != null);
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
                        join u in _context.Sptpd on s.IdSubjek equals u.IdSubjek into d
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
                            sptpd = d.FirstOrDefault()?.IdCoa ?? string.Empty
                        }).Distinct().ToArray();
            return Json(new
            {
                param.sEcho,
                iTotalRecords = allRecords.Count(),
                iTotalDisplayRecords = filteredRecords.Count(),
                aaData = data
            });
        }

        [Auth(new string[] { "Developers", "Pembayaran" })]
        public async Task<IActionResult> DataUsaha(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("Index");
            ViewBag.L1 = Url.Action("DataUsaha", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            ViewBag.IdSubjek = id;
            var data = await _context.DataUsaha.Include(d => d.Sptpd).Include(d => d.IdJenisNavigation).Include(d => d.IndKecamatan).Where(d => d.IdSubjek == id).ToListAsync();

            return View(data);
        }

        [Route("SSPD/{id}")]
        [Route("SSPD/Data/{id}")]
        [Auth(new string[] { "Developers", "Pembayaran" })]
        public async Task<IActionResult> Data(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var usaha = _context.DataUsaha.FirstOrDefault(d => d.IdUsaha == id);

            //Link
            ViewBag.L = Url.Action("DataUsaha", new { id = usaha.IdSubjek });
            ViewBag.L1 = Url.Content("/SSPD/" + id);
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            ViewBag.IdUsaha = id;
            ViewBag.IdSubjek = usaha.IdSubjek;

            var spt = _context.Sptpd.Include(d => d.IdCoaNavigation).Where(d => d.IdUsaha == id && d.FlagValidasi && d.Keterangan == 0 && d.Sk == null && d.IdCoaNavigation.Jenis != "Official Assessment")
                .Select(x => new ListData { Id = x.IdSptpd, Coa = x.IdCoaNavigation.Uraian, Jenis = "SPTPD", Hutang = string.Format("{0:C0}", x.Terhutang), Masa = string.Format("{0:MMMM yyyy}", x.MasaPajak1), Nomor = x.NoSptpd }).ToList();

            var skpd = _context.Skpd.Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation).Where(d => d.IdSptpdNavigation.IdUsaha == id && d.FlagValidasi && d.Keterangan == 0 && d.Sk == null)
                .Select(x => new ListData { Id = x.IdSkpd, Coa = x.IdSptpdNavigation.IdCoaNavigation.Uraian, Jenis = "SKPD", Hutang = string.Format("{0:C0}", x.Terhutang), Masa = string.Format("{0:MMMM yyyy}", x.IdSptpdNavigation.MasaPajak1), Nomor = x.NoSkpd }).ToList();

            var skpdkb = _context.Skpdkb.Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation).Where(d => d.IdSptpdNavigation.IdUsaha == id && d.FlagValidasi && d.Keterangan == 0 && d.Sk == null)
                .Select(x => new ListData { Id = x.IdSkpdkb, Coa = x.IdSptpdNavigation.IdCoaNavigation.Uraian, Jenis = "SKPDKB", Hutang = string.Format("{0:C0}", x.Terhutang), Masa = string.Format("{0:MMMM yyyy}", x.IdSptpdNavigation.MasaPajak1), Nomor = x.NoSkpdkb }).ToList();

            var skpdkbt = _context.Skpdkbt.Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation).Where(d => d.IdSptpdNavigation.IdUsaha == id && d.FlagValidasi && d.Keterangan == 0 && d.Sk == null)
                .Select(x => new ListData { Id = x.IdSkpdkbt, Coa = x.IdSptpdNavigation.IdCoaNavigation.Uraian, Jenis = "SKPDKBT", Hutang = string.Format("{0:C0}", x.Terhutang), Masa = string.Format("{0:MMMM yyyy}", x.IdSptpdNavigation.MasaPajak1), Nomor = x.NoSkpdkbt }).ToList();

            var stpd = _context.Stpd.Include(d => d.IdSkpdNavigation).ThenInclude(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation).Where(d => d.IdSkpdNavigation.IdSptpdNavigation.IdUsaha == id && d.FlagValidasi && d.Keterangan == 0 && d.Sk == null)
                .Select(x => new ListData { Id = x.IdStpd, Coa = x.IdSkpdNavigation.IdSptpdNavigation.IdCoaNavigation.Uraian, Jenis = "SKPD", Hutang = string.Format("{0:C0}", x.Terhutang), Masa = string.Format("{0:MMMM yyyy}", x.IdSkpdNavigation.IdSptpdNavigation.MasaPajak1), Nomor = x.NoStpd }).ToList();

            var result = spt.Union(skpd).Union(skpdkb).Union(skpdkbt).Union(stpd);

            List<ListData> ListDokumen = new List<ListData>();

            foreach (var item in result)
            {
                ListDokumen.Add(new ListData
                {
                    Id = item.Id,
                    Coa = item.Coa,
                    Jenis = item.Jenis,
                    Nomor = item.Nomor,
                    Masa = item.Masa,
                    Hutang = item.Hutang
                });

            }

            ViewBag.ListData = ListDokumen;
            ViewBag.List = ListDokumen.Count();

            var data = await _context.Sspd
                .Where(d => d.IdUsaha == id)
                .Include(s => s.IdSptpdNavigation)
                .Include(s => s.IdSkpdNavigation).ThenInclude(s => s.IdSptpdNavigation)
                .Include(s => s.IdSkpdkbNavigation).ThenInclude(s => s.IdSptpdNavigation)
                .Include(s => s.IdSkpdkbtNavigation).ThenInclude(s => s.IdSptpdNavigation)
                .Include(s => s.IdStpdNavigation).ThenInclude(s => s.IdSkpdNavigation).ThenInclude(s => s.IdSptpdNavigation)
                .Include(s => s.IdBankNavigation).ThenInclude(s => s.IdRefNavigation)
                .Include(s => s.IdCoaNavigation)
                .Include(s => s.IdSetoranNavigation)
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

            var sspd = await _context.Sspd
                .Include(s => s.IdBankNavigation).ThenInclude(s => s.IdRefNavigation)
                .Include(s => s.IdCoaNavigation)
                .Include(s => s.IdSetoranNavigation)
                .Include(s => s.IdSubjekNavigation)
                .Include(s => s.IdUsahaNavigation).ThenInclude(s => s.IdJenisNavigation)
                .Include(s => s.IdUsahaNavigation).ThenInclude(s => s.IndKelurahan)
                .Include(s => s.IdUsahaNavigation).ThenInclude(s => s.IndKecamatan)
                .FirstOrDefaultAsync(m => m.IdSspd == id);
            if (sspd == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Content("/SSPD/" + sspd.IdUsaha);
            ViewBag.L1 = Url.Action("Details", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View(sspd);
        }

        [Route("SSPD/Create/{id}/{jenis}")]
        [Auth(new string[] { "Developers", "Pembayaran" })]
        public IActionResult Create(Guid id, string jenis)
        {
            ViewData["IdSetoran"] = new SelectList(_context.RefJenisSetoran, "IdSetoran", "Jenis");

            if(jenis == "SPTPD")
            {
                var data = _context.Sptpd.FirstOrDefault(d => d.IdSptpd == id);
                ViewBag.IdUsaha = data.IdUsaha;
            }
            else if (jenis == "SKPD")
            {
                var data = _context.Skpd.Include(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdSkpd == id);
                ViewBag.IdUsaha = data.IdSptpdNavigation.IdUsaha;
            }
            else if (jenis == "SKPDKB")
            {
                var data = _context.Skpdkb.Include(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdSkpdkb == id);
                ViewBag.IdUsaha = data.IdSptpdNavigation.IdUsaha;
            }
            else if (jenis == "SKPDKBT")
            {
                var data = _context.Skpdkbt.Include(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdSkpdkbt == id);
                ViewBag.IdUsaha = data.IdSptpdNavigation.IdUsaha;
            }
            else if (jenis == "STPD")
            {
                var data = _context.Stpd.Include(d => d.IdSkpdNavigation).ThenInclude(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdStpd == id);
                ViewBag.IdUsaha = data.IdSkpdNavigation.IdSptpdNavigation.IdUsaha;
            }

            //Link
            ViewBag.L = Url.Content("/SSPD/" + ViewBag.IdUsaha);
            ViewBag.L1 = Url.Content("/SSPD/Create/" + id + "/" + jenis);
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            ViewBag.IdSurat = id;
            ViewBag.Jenis = jenis;

            return View();
        }

        [Route("SSPD/Create/{id}/{jenis}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, string jenis, [Bind("IdSspd,IdSubjek,IdUsaha,IdBank,IdCoa,IdSptpd,IdSkpd,IdSkpdkb,IdSkpdkbt,IdStpd,Tahun,Nomor,NoSspd,Tanggal,IdSetoran,JumlahSetoran,FlagBayar,TanggalBayar,StatusSetor,NoValidasi,Eu,Ed")] Sspd sspd)
        {
            if (ModelState.IsValid)
            {
                sspd.IdSspd = Guid.NewGuid();
                if (jenis == "SPTPD")
                {
                    var data = _context.Sptpd.FirstOrDefault(d => d.IdSptpd == id);
                    sspd.IdSptpd = id;
                    sspd.IdUsaha = data.IdUsaha;
                    sspd.IdSubjek = data.IdSubjek;
                    sspd.IdCoa = data.IdCoa;
                    sspd.JumlahSetoran = data.Terhutang;

                    data.Sk = "SSPD";
                    _context.Sptpd.Update(data);
                }
                else if (jenis == "SKPD")
                {
                    var data = _context.Skpd.Include(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdSkpd == id);
                    sspd.IdSkpd = id;
                    sspd.IdUsaha = data.IdSptpdNavigation.IdUsaha;
                    sspd.IdSubjek = data.IdSptpdNavigation.IdSubjek;
                    sspd.IdCoa = data.IdSptpdNavigation.IdCoa;
                    sspd.JumlahSetoran = data.Terhutang;

                    data.Sk = "SSPD";
                    _context.Skpd.Update(data);
                }
                else if (jenis == "SKPDKB")
                {
                    var data = _context.Skpdkb.Include(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdSkpdkb == id);
                    sspd.IdSkpdkb = id;
                    sspd.IdUsaha = data.IdSptpdNavigation.IdUsaha;
                    sspd.IdSubjek = data.IdSptpdNavigation.IdSubjek;
                    sspd.IdCoa = data.IdSptpdNavigation.IdCoa;
                    sspd.JumlahSetoran = data.Terhutang;

                    data.Sk = "SSPD";
                    _context.Skpdkb.Update(data);
                }
                else if (jenis == "SKPDKBT")
                {
                    var data = _context.Skpdkbt.Include(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdSkpdkbt == id);
                    sspd.IdSkpdkbt = id;
                    sspd.IdUsaha = data.IdSptpdNavigation.IdUsaha;
                    sspd.IdSubjek = data.IdSptpdNavigation.IdSubjek;
                    sspd.IdCoa = data.IdSptpdNavigation.IdCoa;
                    sspd.JumlahSetoran = data.Terhutang;

                    data.Sk = "SSPD";
                    _context.Skpdkbt.Update(data);
                }
                else if (jenis == "STPD")
                {
                    var data = _context.Stpd.Include(d => d.IdSkpdNavigation).ThenInclude(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdStpd == id);
                    sspd.IdStpd = id;
                    sspd.IdUsaha = data.IdSkpdNavigation.IdSptpdNavigation.IdUsaha;
                    sspd.IdSubjek = data.IdSkpdNavigation.IdSptpdNavigation.IdSubjek;
                    sspd.IdCoa = data.IdSkpdNavigation.IdSptpdNavigation.IdCoa;
                    sspd.JumlahSetoran = data.Terhutang;

                    data.Sk = "SSPD";
                    _context.Stpd.Update(data);
                }
                sspd.Tahun = sspd.Tanggal.Value.Year.ToString();
                var noSk = _context.Sspd.Where(d => d.Tahun == sspd.Tahun).Select(e => e.Nomor).Max() ?? 0;
                sspd.Nomor = noSk + 1;
                sspd.NoSspd = string.Format("{0:000000}", sspd.Nomor) + "/SSPD/" + string.Format("{0:MM/yyyy}", sspd.Tanggal);
                sspd.Eu = HttpContext.Session.GetString("User");
                sspd.Ed = DateTime.Now;
                _context.Add(sspd);

                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = sspd.IdSspd });
            }
            ViewData["IdSetoran"] = new SelectList(_context.RefJenisSetoran, "IdSetoran", "Jenis", sspd.IdSetoran);
            return View(sspd);
        }

        [Auth(new string[] { "Developers", "Pembayaran" })]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sspd = await _context.Sspd.FindAsync(id);
            if (sspd == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Content("/SSPD/" + sspd.IdUsaha);
            ViewBag.L1 = Url.Action("Edit", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            ViewData["IdSetoran"] = new SelectList(_context.RefJenisSetoran, "IdSetoran", "Jenis", sspd.IdSetoran);
            return View(sspd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdSspd,IdSubjek,IdUsaha,IdBank,IdCoa,IdSptpd,IdSkpd,IdSkpdkb,IdSkpdkbt,IdStpd,Tahun,Nomor,NoSspd,Tanggal,IdSetoran,JumlahSetoran,FlagBayar,TanggalBayar,StatusSetor,NoValidasi,Eu,Ed")] Sspd sspd)
        {
            if (id != sspd.IdSspd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var old = await _context.Sspd.FirstOrDefaultAsync(d => d.IdSspd == id);
                try
                {
                    old.Tanggal = sspd.Tanggal;
                    old.Tahun = sspd.Tanggal.Value.Year.ToString();
                    old.IdSetoran = sspd.IdSetoran;
                    old.Eu = HttpContext.Session.GetString("User");
                    old.Ed = DateTime.Now;

                    _context.Sspd.Update(old);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SspdExists(sspd.IdSspd))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "edit";
                return Redirect("/SSPD/" + old.IdUsaha);
            }
            ViewData["IdSetoran"] = new SelectList(_context.RefJenisSetoran, "IdSetoran", "Jenis", sspd.IdSetoran);
            return View(sspd);
        }

        [Auth(new string[] { "Developers", "Validasi" })]
        //[Auth(new string[] { "Developers" })]
        public async Task<IActionResult> Validasi(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sspd = await _context.Sspd
                .Include(s => s.IdBankNavigation).ThenInclude(s => s.IdRefNavigation)
                .Include(s => s.IdCoaNavigation)
                .Include(s => s.IdSetoranNavigation)
                .Include(s => s.IdSubjekNavigation)
                .Include(s => s.IdUsahaNavigation).ThenInclude(s => s.IdJenisNavigation)
                .Include(s => s.IdUsahaNavigation).ThenInclude(s => s.IndKelurahan)
                .Include(s => s.IdUsahaNavigation).ThenInclude(s => s.IndKecamatan)
                .FirstOrDefaultAsync(m => m.IdSspd == id);
            if (sspd == null)
            {
                return NotFound();
            }

            if (sspd.FlagBayar)
            {
                TempData["status"] = "sudahvalid";
                return Redirect("/SSPD/" + sspd.IdUsaha);
            }

            //Link
            ViewBag.L = Url.Content("/SSPD/" + sspd.IdUsaha);
            ViewBag.L1 = Url.Action("Validasi", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var bank = (from b in _context.Bank
                        join r in _context.RefBank on b.IdRef equals r.IdBank
                        join j in _context.RefJenisSetoran on b.IdSetoran equals j.IdSetoran
                        where b.IdSetoran == sspd.IdSetoran
                        select new
                        {
                            value = b.IdBank,
                            text = r.NamaBank + " | " + b.NoRek + " | " + b.NamaRek
                        }).ToList();
            ViewData["IdBank"] = new SelectList(bank, "value", "text", sspd.IdBank);

            return View(sspd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Validasi(Guid id, Sspd sspd)
        {
            if (id != sspd.IdSspd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var old = _context.Sspd.Include(d => d.IdCoaNavigation).FirstOrDefault(d => d.IdSspd == id);

                if (old.FlagBayar)
                {
                    var Tahun = old.TanggalBayar.Value.Year;
                    bool Denda = false;

                    old.FlagBayar = false;
                    old.TanggalBayar = null;
                    old.IdBank = null;
                    old.StatusSetor = false;
                    old.Eu = HttpContext.Session.GetString("User");
                    old.Ed = DateTime.Now;

                    var skpdnEx = _context.Skpdn.FirstOrDefault(d => d.IdSspd == old.IdSspd);
                    _context.Skpdn.Remove(skpdnEx);
                    _context.Sspd.Update(old);

                    var Sumber = "";
                    //SPTPD
                    if (old.IdSptpd != null)
                    {
                        var data = _context.Sptpd.FirstOrDefault(d => d.IdSptpd == old.IdSptpd);
                        data.Sk = "SSPD";
                        data.Keterangan = 0;
                        data.KreditPajak = 0;
                        Sumber = data.NoSptpd;

                        _context.Sptpd.Update(data);
                    }
                    else if (old.IdSkpd != null)
                    {
                        var data = _context.Skpd.Include(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdSkpd == old.IdSkpd);
                        data.Sk = "SSPD";
                        data.Keterangan = 0;
                        data.KreditPajak = 0;
                        Sumber = data.NoSkpd;

                        if (data.Terhutang > (data.IdSptpdNavigation.Terhutang ?? 0))
                        {
                            Denda = true;
                        }

                        _context.Skpd.Update(data);
                    }
                    else if (old.IdSkpdkb != null)
                    {
                        var data = _context.Skpdkb.Include(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdSkpdkb == old.IdSkpdkb);
                        data.Sk = "SSPD";
                        data.Keterangan = 0;
                        data.KreditPajak = 0;
                        Sumber = data.NoSkpdkb;

                        if (data.Terhutang > (data.IdSptpdNavigation.Terhutang ?? 0))
                        {
                            Denda = true;
                        }

                        _context.Skpdkb.Update(data);
                    }
                    else if (old.IdSkpdkbt != null)
                    {
                        var data = _context.Skpdkbt.Include(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdSkpdkbt == old.IdSkpdkbt);
                        data.Sk = "SSPD";
                        data.Keterangan = 0;
                        data.KreditPajak = 0;
                        Sumber = data.NoSkpdkbt;

                        if (data.Terhutang > (data.IdSptpdNavigation.Terhutang ?? 0))
                        {
                            Denda = true;
                        }

                        _context.Skpdkbt.Update(data);
                    }
                    else if (old.IdStpd != null)
                    {
                        var data = _context.Stpd.Include(d => d.IdSkpdNavigation).ThenInclude(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdStpd == old.IdStpd);
                        data.Sk = "SSPD";
                        data.Keterangan = 0;
                        data.KreditPajak = 0;
                        Sumber = data.NoStpd;

                        if (data.Terhutang > (data.IdSkpdNavigation.IdSptpdNavigation.Terhutang ?? 0))
                        {
                            Denda = true;
                        }

                        _context.Stpd.Update(data);
                    }

                    var lra = _context.Lra.FirstOrDefault(d => d.IdCoa == old.IdCoa && d.Tahun == Tahun);

                    var transaksi = _context.TransaksiLra.Where(d => d.IdLra == lra.IdLra && d.Sumber == Sumber).Single();
                    _context.TransaksiLra.Remove(transaksi);

                    var sum = _context.TransaksiLra.Where(d => d.IdLra == lra.IdLra && d.Tanggal.Value.Year == Tahun).Sum(d => d.Jumlah);
                    lra.Realisasi = sum ?? 0;
                    _context.Lra.Update(lra);

                    if (Denda)
                    {
                        var lraDenda = _context.Lra.FirstOrDefault(d => d.IdCoa == old.IdCoaNavigation.Denda && d.Tahun == Tahun);

                        var transaksiDenda = _context.TransaksiLra.Where(d => d.IdLra == lraDenda.IdLra && d.Sumber == Sumber).Single();
                        _context.TransaksiLra.Remove(transaksi);

                        var sumDenda = _context.TransaksiLra.Where(d => d.IdLra == lraDenda.IdLra && d.Tanggal.Value.Year == Tahun).Sum(d => d.Jumlah);
                        lraDenda.Realisasi = sumDenda ?? 0;
                        _context.Lra.Update(lraDenda);
                    }

                    await _context.SaveChangesAsync();
                    TempData["status"] = "validbatal";
                }
                else
                {
                    old.FlagBayar = true;
                    old.TanggalBayar = sspd.TanggalBayar;
                    old.IdBank = sspd.IdBank;
                    if (old.IdSetoran == 1)
                    {
                        old.StatusSetor = true;
                    }
                    old.Eu = HttpContext.Session.GetString("User");
                    old.Ed = DateTime.Now;

                    Skpdn skpdn = new Skpdn
                    {
                        IdSkpdn = Guid.NewGuid(),
                        IdSspd = old.IdSspd,
                        IdCoa = old.IdCoa,
                        KompKelebihan = 0,
                        Lainnya = 0,
                        Tambahan = 0,
                        Bunga = 0,
                        Kenaikan = 0
                    };

                    var Hutang = Convert.ToDecimal(0);
                    var Denda = Convert.ToDecimal(0);
                    var Sumber = "";
                    //SPTPD
                    if (old.IdSptpd != null)
                    {
                        var data = _context.Sptpd.FirstOrDefault(d => d.IdSptpd == old.IdSptpd);
                        data.Sk = null;
                        data.Keterangan = 1;
                        data.KreditPajak = old.JumlahSetoran;

                        _context.Sptpd.Update(data);

                        Sumber = data.NoSptpd;
                        Hutang = data.Terhutang ?? 0;

                        skpdn.MasaPajak1 = data.MasaPajak1;
                        skpdn.MasaPajak2 = data.MasaPajak2;
                        skpdn.Jenis = "SPTPD";
                        skpdn.NoSurat = data.NoSptpd;
                        skpdn.TanggalSurat = data.Tanggal;
                        skpdn.PokokPajak = data.Terhutang;
                        skpdn.Terhutang = data.Terhutang;
                        skpdn.KreditPajak = data.KreditPajak;
                        skpdn.IdSptpd = data.IdSptpd;
                    }
                    else if (old.IdSkpd != null)
                    {
                        var data = _context.Skpd.Include(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdSkpd == old.IdSkpd);
                        data.Sk = null;
                        data.Keterangan = 1;
                        data.KreditPajak = old.JumlahSetoran;

                        _context.Skpd.Update(data);

                        Sumber = data.NoSkpd;
                        Hutang = data.IdSptpdNavigation.Terhutang ?? 0;
                        if (data.Terhutang > Hutang)
                        {
                            Denda = Decimal.Subtract((decimal)data.Terhutang, (decimal)data.IdSptpdNavigation.Terhutang);
                        }

                        skpdn.MasaPajak1 = data.IdSptpdNavigation.MasaPajak1;
                        skpdn.MasaPajak2 = data.IdSptpdNavigation.MasaPajak2;
                        skpdn.Jenis = "SKPD";
                        skpdn.NoSurat = data.NoSkpd;
                        skpdn.TanggalSurat = data.Tanggal;
                        skpdn.PokokPajak = data.IdSptpdNavigation.Terhutang;
                        skpdn.Terhutang = data.Terhutang;
                        skpdn.Bunga = data.Bunga;
                        skpdn.Kenaikan = data.Kenaikan;
                        skpdn.KreditPajak = data.KreditPajak;
                        skpdn.IdSptpd = data.IdSptpd;
                    }
                    else if (old.IdSkpdkb != null)
                    {
                        var data = _context.Skpdkb.Include(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdSkpdkb == old.IdSkpdkb);
                        data.Sk = null;
                        data.Keterangan = 1;
                        data.KreditPajak = old.JumlahSetoran;

                        _context.Skpdkb.Update(data);

                        Sumber = data.NoSkpdkb;
                        Hutang = data.PokokPajak ?? 0;
                        if (data.Terhutang > Hutang)
                        {
                            Denda = Decimal.Subtract((decimal)data.Terhutang, (decimal)data.IdSptpdNavigation.Terhutang);
                        }

                        skpdn.MasaPajak1 = data.IdSptpdNavigation.MasaPajak1;
                        skpdn.MasaPajak2 = data.IdSptpdNavigation.MasaPajak2;
                        skpdn.Jenis = "SKPDKB";
                        skpdn.NoSurat = data.NoSkpdkb;
                        skpdn.TanggalSurat = data.Tanggal;
                        skpdn.Terhutang = data.Terhutang;
                        skpdn.Bunga = data.Bunga;
                        skpdn.Kenaikan = data.Kenaikan;
                        skpdn.Lainnya = data.Lainnya;
                        skpdn.PokokPajak = data.PokokPajak;
                        skpdn.KompKelebihan = data.KompKelebihan;
                        skpdn.KreditPajak = data.KreditPajak;
                        skpdn.IdSptpd = data.IdSptpd;
                    }
                    else if (old.IdSkpdkbt != null)
                    {
                        var data = _context.Skpdkbt.Include(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdSkpdkbt == old.IdSkpdkbt);
                        data.Sk = null;
                        data.Keterangan = 1;
                        data.KreditPajak = old.JumlahSetoran;

                        _context.Skpdkbt.Update(data);

                        Sumber = data.NoSkpdkbt;
                        Hutang = data.PokokPajak ?? 0;
                        if (data.Terhutang > Hutang)
                        {
                            Denda = Decimal.Subtract((decimal)data.Terhutang, (decimal)data.IdSptpdNavigation.Terhutang);
                        }

                        skpdn.MasaPajak1 = data.IdSptpdNavigation.MasaPajak1;
                        skpdn.MasaPajak2 = data.IdSptpdNavigation.MasaPajak2;
                        skpdn.Jenis = "SKPDKBT";
                        skpdn.NoSurat = data.NoSkpdkbt;
                        skpdn.TanggalSurat = data.Tanggal;
                        skpdn.Terhutang = data.Terhutang;
                        skpdn.Bunga = data.Bunga;
                        skpdn.Kenaikan = data.Kenaikan;
                        skpdn.Lainnya = data.Lainnya;
                        skpdn.PokokPajak = data.PokokPajak;
                        skpdn.KompKelebihan = data.KompKelebihan;
                        skpdn.Tambahan = data.Tambahan;
                        skpdn.KreditPajak = data.KreditPajak;
                        skpdn.IdSptpd = data.IdSptpd;
                    }
                    else if (old.IdStpd != null)
                    {
                        var data = _context.Stpd.Include(d => d.IdSkpdNavigation).ThenInclude(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdStpd == old.IdStpd);
                        data.Sk = null;
                        data.Keterangan = 1;
                        data.KreditPajak = old.JumlahSetoran;

                        _context.Stpd.Update(data);

                        Sumber = data.NoStpd;
                        Hutang = data.IdSkpdNavigation.IdSptpdNavigation.Terhutang ?? 0;
                        if (data.Terhutang > Hutang)
                        {
                            Denda = Decimal.Subtract((decimal)data.Terhutang, (decimal)data.IdSkpdNavigation.IdSptpdNavigation.Terhutang);
                        }

                        skpdn.MasaPajak1 = data.IdSkpdNavigation.IdSptpdNavigation.MasaPajak1;
                        skpdn.MasaPajak2 = data.IdSkpdNavigation.IdSptpdNavigation.MasaPajak2;
                        skpdn.Jenis = "STPD";
                        skpdn.NoSurat = data.NoStpd;
                        skpdn.TanggalSurat = data.Tanggal;
                        skpdn.PokokPajak = data.IdSkpdNavigation.IdSptpdNavigation.Terhutang;
                        skpdn.Terhutang = data.Terhutang;
                        skpdn.Bunga = data.Bunga;
                        skpdn.KreditPajak = data.KreditPajak;
                        skpdn.IdSptpd = data.IdSkpdNavigation.IdSptpd;
                    }

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
                        Sumber = Sumber,
                        Jumlah = Hutang,
                        Tanggal = old.TanggalBayar
                    };
                    _context.TransaksiLra.Add(transaksi);

                    var sum = _context.TransaksiLra.Where(d => d.IdLra == lra.IdLra && d.Tanggal.Value.Year == Tahun).Sum(d => d.Jumlah);
                    lra.Realisasi = (sum ?? 0) + (transaksi.Jumlah ?? 0);
                    _context.Lra.Update(lra);

                    if (Denda != 0)
                    {
                        var lraDenda = _context.Lra.FirstOrDefault(d => d.IdCoa == old.IdCoaNavigation.Denda && d.Tahun == Tahun);
                        if (lraDenda == null)
                        {
                            lraDenda = new Lra
                            {
                                IdLra = Guid.NewGuid(),
                                IdCoa = old.IdCoaNavigation.Denda,
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
                            Sumber = Sumber,
                            Jumlah = Denda,
                            Tanggal = old.TanggalBayar
                        };
                        _context.TransaksiLra.Add(transaksiDenda);

                        var sumDenda = _context.TransaksiLra.Where(d => d.IdLra == lraDenda.IdLra && d.Tanggal.Value.Year == Tahun).Sum(d => d.Jumlah);
                        lraDenda.Realisasi = (sumDenda ?? 0) + (transaksiDenda.Jumlah ?? 0);
                        _context.Lra.Update(lraDenda);
                    }
                    _context.Sspd.Update(old);
                    _context.Skpdn.Add(skpdn);
                    await _context.SaveChangesAsync();
                    TempData["status"] = "valid";
                }
                
                return Redirect("/SSPD/" + old.IdUsaha);
            }
            return View(sspd);
        }

        [Auth(new string[] { "Developers", "Pembayaran" })]
        public IActionResult Cetak(Guid id)
        {
            return RedirectToAction("SSPD", "Cetak", new { id });
        }

        [Auth(new string[] { "Developers", "Pembayaran" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sspd = await _context.Sspd
                .FirstOrDefaultAsync(m => m.IdSspd == id);
            if (sspd == null)
            {
                return NotFound();
            }

            if (sspd.StatusSetor)
            {
                return PartialView("_deletevalid");
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var sspd = await _context.Sspd.FindAsync(id);
            try
            {
                _context.Sspd.Remove(sspd);

                if (sspd.IdSptpd != null)
                {
                    var data = _context.Sptpd.FirstOrDefault(d => d.IdSptpd == sspd.IdSptpd);
                    data.Sk = null;
                    _context.Sptpd.Update(data);
                }
                else if (sspd.IdSkpd != null)
                {
                    var data = _context.Skpd.FirstOrDefault(d => d.IdSkpd == sspd.IdSkpd);
                    data.Sk = null;
                    _context.Skpd.Update(data);
                }
                else if (sspd.IdSkpdkb != null)
                {
                    var data = _context.Skpdkb.FirstOrDefault(d => d.IdSkpdkb == sspd.IdSkpdkb);
                    data.Sk = null;
                    _context.Skpdkb.Update(data);
                }
                else if (sspd.IdSkpdkbt != null)
                {
                    var data = _context.Skpdkbt.FirstOrDefault(d => d.IdSkpdkbt == sspd.IdSkpdkbt);
                    data.Sk = null;
                    _context.Skpdkbt.Update(data);
                }
                else if (sspd.IdStpd != null)
                {
                    var data = _context.Stpd.FirstOrDefault(d => d.IdStpd == sspd.IdStpd);
                    data.Sk = null;
                    _context.Stpd.Update(data);
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["status"] = "deletefailed";
                string url = Url.Content("/SSPD/" + sspd.IdUsaha);
                return Json(new { success = true, url });
            }
            
            TempData["status"] = "delete";
            string link = Url.Content("/SSPD/" + sspd.IdUsaha);
            return Json(new { success = true, url = link });
        }

        private bool SspdExists(Guid id)
        {
            return _context.Sspd.Any(e => e.IdSspd == id);
        }
    }
}
