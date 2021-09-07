using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIP.Models;
using SIP.ViewModels;

namespace SIP.Controllers
{
    public class LaporanController : Controller
    {

        private readonly DB_NewContext _context;

        public LaporanController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "Laporan" })]
        public IActionResult WajibPajak(string id)
        {
            //Link
            ViewBag.L = Url.Action("WajibPajak");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var pemda = _context.Pemda.FirstOrDefault();
            ViewBag.Kecamatan = _context.IndKecamatan.Where(d => d.IndKabKotaId == pemda.IndKabKotaId).ToList();

            if (id == "Pribadi")
            {
                ViewBag.Pekerjaan = _context.RefPekerjaan.ToList();
                return View("Pribadi");
            }
            else if (id == "Badan")
            {
                ViewBag.BadanHukum = _context.RefBadanHukum.ToList();
                return View("Badan");
            }
            else
            {
                return View();
            }
        }

        public JsonResult GetSubjekAll(JQueryDataTableParamModel param)
        {
            var allRecords = _context.DataSubjek;
            IEnumerable<DataSubjek> filteredRecords;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredRecords = allRecords.Where(d => d.Nama.ToLower().Contains(param.sSearch.ToLower())
                || d.Nik.Contains(param.sSearch)
                || d.Npwpd.Replace(".", "").Contains(param.sSearch)
                || d.Npwp.Replace(".", "").Contains(param.sSearch));
            }
            else
            {
                filteredRecords = allRecords;
            }

            if (param.status.HasValue)
            {
                filteredRecords = filteredRecords.Where(d => d.Status == param.status);
            }

            if (!string.IsNullOrEmpty(param.tipe))
            {
                filteredRecords = filteredRecords.Where(d => d.IndKecamatanId.ToString() == param.tipe);
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
                        join u in _context.DataUsaha on s.IdSubjek equals u.IdSubjek into d
                        from u in d.DefaultIfEmpty()
                        select new
                        {
                            id = s.IdSubjek,
                            nama = s.Nama,
                            npwpd = s.Npwpd,
                            nik = s.Nik,
                            npwp = s.Npwp,
                            alamat = s.Alamat,
                            kecamatan = k.Kecamatan,
                            tanggal = string.Format("{0:dd MMMM yyyy}", s.TglDaftar),
                            status = (s.Status ? "1" : "0"),
                            usaha = d.Count()
                        }).Distinct().ToArray();
            return Json(new
            {
                param.sEcho,
                iTotalRecords = allRecords.Count(),
                iTotalDisplayRecords = filteredRecords.Count(),
                aaData = data
            });
        }

        public JsonResult GetSubjekPribadi(JQueryDataTableParamModel param)
        {
            var allRecords = _context.DataSubjek.Where(d => d.IdBadanHukum == null); //All records
            IEnumerable<DataSubjek> filteredRecords;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredRecords = allRecords.Where(d => d.Nama.ToLower().Contains(param.sSearch.ToLower())
                || d.Nik.Contains(param.sSearch)
                || d.Npwpd.Replace(".", "").Contains(param.sSearch));
            }
            else
            {
                filteredRecords = allRecords;
            }

            if (param.status.HasValue)
            {
                filteredRecords = filteredRecords.Where(d => d.Status == param.status);
            }

            if (!string.IsNullOrEmpty(param.tipe))
            {
                filteredRecords = filteredRecords.Where(d => d.IndKecamatanId.ToString() == param.tipe);
            }

            Func<DataSubjek, string> orderingFunction = (c => param.iSortCol_0 == 1 ? c.Nama :
                                                        param.iSortCol_0 == 2 ? c.Npwpd :
                                                        param.iSortCol_0 == 3 ? c.Nik :
                                                    "");

            if (param.sSortDir_0 == "asc")
                filteredRecords = filteredRecords.OrderBy(orderingFunction);
            else
                filteredRecords = filteredRecords.OrderByDescending(orderingFunction);

            var displayedRecords = filteredRecords.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var data = (from s in displayedRecords
                        join p in _context.RefPekerjaan on s.IdPekerjaan equals p.IdPekerjaan
                        join k in _context.IndKecamatan on s.IndKecamatanId equals k.IndKecamatanId
                        join u in _context.DataUsaha on s.IdSubjek equals u.IdSubjek into d
                        from u in d.DefaultIfEmpty()
                        select new
                        {
                            id = s.IdSubjek,
                            nama = s.Nama,
                            npwpd = s.Npwpd,
                            nik = s.Nik,
                            pekerjaan = p.Pekerjaan,
                            alamat = s.Alamat,
                            kecamatan = k.Kecamatan,
                            tanggal = string.Format("{0:dd MMMM yyyy}", s.TglDaftar),
                            status = (s.Status ? "1" : "0"),
                            usaha = d.Count()
                        }).Distinct().ToArray();
            return Json(new
            {
                param.sEcho,
                iTotalRecords = allRecords.Count(),
                iTotalDisplayRecords = filteredRecords.Count(),
                aaData = data
            });
        }

        public JsonResult GetSubjekBadan(JQueryDataTableParamModel param)
        {
            var allRecords = _context.DataSubjek.Where(d => d.IdBadanHukum != null); //All records
            IEnumerable<DataSubjek> filteredRecords;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredRecords = allRecords.Where(d => d.Nama.ToLower().Contains(param.sSearch.ToLower())
                || d.Npwp.Replace(".", "").Contains(param.sSearch)
                || d.Npwpd.Replace(".", "").Contains(param.sSearch));
            }
            else
            {
                filteredRecords = allRecords;
            }

            if (param.status.HasValue)
            {
                filteredRecords = filteredRecords.Where(d => d.Status == param.status);
            }

            if (!string.IsNullOrEmpty(param.tipe))
            {
                filteredRecords = filteredRecords.Where(d => d.IndKecamatanId.ToString() == param.tipe);
            }

            Func<DataSubjek, string> orderingFunction = (c => param.iSortCol_0 == 1 ? c.Nama :
                                                        param.iSortCol_0 == 2 ? c.Npwpd :
                                                        param.iSortCol_0 == 3 ? c.Npwp :
                                                    "");

            if (param.sSortDir_0 == "asc")
                filteredRecords = filteredRecords.OrderBy(orderingFunction);
            else
                filteredRecords = filteredRecords.OrderByDescending(orderingFunction);

            var displayedRecords = filteredRecords.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var data = (from s in displayedRecords
                        join b in _context.RefBadanHukum on s.IdBadanHukum equals b.IdBadanHukum
                        join k in _context.IndKecamatan on s.IndKecamatanId equals k.IndKecamatanId
                        join u in _context.DataUsaha on s.IdSubjek equals u.IdSubjek into d
                        from u in d.DefaultIfEmpty()
                        select new
                        {
                            id = s.IdSubjek,
                            nama = s.Nama,
                            npwpd = s.Npwpd,
                            npwp = s.Npwp,
                            badan = b.BadanHukum,
                            alamat = s.Alamat,
                            kecamatan = k.Kecamatan,
                            tanggal = string.Format("{0:dd MMMM yyyy}", s.TglDaftar),
                            status = (s.Status ? "1" : "0"),
                            usaha = d.Count()
                        }).Distinct().ToArray();
            return Json(new
            {
                param.sEcho,
                iTotalRecords = allRecords.Count(),
                iTotalDisplayRecords = filteredRecords.Count(),
                aaData = data
            });
        }


        [Auth(new string[] { "Developers", "Laporan" })]
        public IActionResult SPTPD (string id)
        {
            //Link
            ViewBag.L = Url.Action("SPTPD", new { id });
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            if (id == "Lapor")
            {
                var tgl = DateTime.Now;
                var tanggal = new DateTime(tgl.Year, tgl.Month, DateTime.DaysInMonth(tgl.Year, tgl.Month));
                var query = (from c in _context.Coa
                             where (from d in _context.Sptpd where d.FlagKonfirmasi == false && d.MasaPajak1 != null && d.MasaPajak2 <= tanggal select d.IdCoa).Contains(c.IdCoa)
                             select c).ToList();
                ViewBag.Jenis = query;
                ViewBag.Tanggal = tanggal.DayOfYear;

                var lapor = _context.Sptpd
                    .Include(d => d.IdSubjekNavigation)
                    .Where(d => d.FlagKonfirmasi == false && d.MasaPajak1 != null && d.MasaPajak2 <= tanggal).OrderByDescending(d => d.MasaPajak1).ToList();
                return View("SPTPDLapor", lapor);
            }

            else if (id == null)
            {
                var query = (from c in _context.Coa
                             where (from d in _context.Sptpd where d.FlagKonfirmasi select d.IdCoa).Contains(c.IdCoa)
                             select c).ToList();
                ViewBag.Jenis = query;

                return View();
            }
            else
            {
                return NotFound();
            }
        }

        public JsonResult GetSPTPD(JQueryDataTableParamModel param)
        {
            var allRecords = _context.Sptpd.Include(d => d.IdSubjekNavigation).Include(d => d.IdCoaNavigation).Where(d => d.FlagKonfirmasi); //All records
            IEnumerable<Sptpd> filteredRecords;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredRecords = allRecords.Where(d => d.IdSubjekNavigation.Nama.ToLower().Contains(param.sSearch.ToLower())
                || d.IdSubjekNavigation.Npwpd.Replace(".", "").Contains(param.sSearch)
                || d.NoSptpd.Replace("/", "").Contains(param.sSearch));
            }
            else
            {
                filteredRecords = allRecords;
            }

            if (!string.IsNullOrEmpty(param.tipe))
            {
                if(int.Parse(param.tipe) >= 3 )
                {
                    var Sk = "SKPD";
                    if(param.tipe == "5") { Sk = "SKPDKB"; }
                    else if(param.tipe == "6") { Sk = "SKPDKBT"; }
                    filteredRecords = filteredRecords.Where(d => d.Sk == Sk);
                }
                else
                {
                    filteredRecords = filteredRecords.Where(d => d.Keterangan.ToString() == param.tipe && d.Sk == null);
                }
            }

            if (!string.IsNullOrEmpty(param.tipe2))
            {
                filteredRecords = filteredRecords.Where(d => d.IdCoaNavigation.Jenis == param.tipe2);
            }

            if (!string.IsNullOrEmpty(param.tipe3))
            {
                filteredRecords = filteredRecords.Where(d => d.IdCoa == param.tipe3);
            }

            if (param.min != null && param.max != null)
            {
                filteredRecords = filteredRecords.Where(d => d.Tanggal >= param.min && d.Tanggal <= param.max);
            }

            Func<Sptpd, string> orderingFunction = (c => param.iSortCol_0 == 1 ? c.IdSubjekNavigation.Nama :
                                                        param.iSortCol_0 == 2 ? c.IdSubjekNavigation.Npwpd :
                                                        param.iSortCol_0 == 3 ? c.NoSptpd :
                                                    "");

            if (param.sSortDir_0 == "asc")
                filteredRecords = filteredRecords.OrderBy(orderingFunction);
            else
                filteredRecords = filteredRecords.OrderByDescending(orderingFunction);

            var displayedRecords = filteredRecords.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var data = (from s in displayedRecords
                        join w in _context.DataSubjek on s.IdSubjek equals w.IdSubjek
                        join c in _context.Coa on s.IdCoa equals c.IdCoa
                        select new
                        {
                            id = s.IdSptpd,
                            nama = w.Nama,
                            npwpd = w.Npwpd,
                            nomor = s.NoSptpd,
                            jenis = c.Jenis,
                            masa = string.Format("{0:MMM yyyy}", s.MasaPajak1),
                            tanggal = string.Format("{0:dd MMMM yyyy}", s.Tanggal),
                            terhutang = string.Format("{0:C0}", s.Terhutang),
                            uraian = c.Uraian,
                            status = (s.Sk == "SKPD" ? "4" : s.Sk == "SKPDKB" ? "5" : s.Sk == "SKPDKBT" ? "6" : s.Keterangan.ToString())
                        }).ToArray();
            return Json(new
            {
                param.sEcho,
                iTotalRecords = allRecords.Count(),
                iTotalDisplayRecords = filteredRecords.Count(),
                aaData = data
            });
        }


        [Auth(new string[] { "Developers", "Laporan" })]
        public IActionResult SKPD()
        {
            //Link
            ViewBag.L = Url.Action("SKPD");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var query = (from c in _context.Coa
                         where (from d in _context.Sptpd where d.FlagKonfirmasi && c.Jenis == "Official Assessment" select d.IdCoa).Contains(c.IdCoa)
                         select c).ToList();
            ViewBag.Jenis = query;

            return View();
        }

        public JsonResult GetSKPD(JQueryDataTableParamModel param)
        {
            var allRecords = _context.Skpd.Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdSubjekNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation);
            IEnumerable<Skpd> filteredRecords;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredRecords = allRecords.Where(d => d.IdSptpdNavigation.IdSubjekNavigation.Nama.ToLower().Contains(param.sSearch.ToLower())
                || d.IdSptpdNavigation.IdSubjekNavigation.Npwpd.Replace(".", "").Contains(param.sSearch)
                || d.NoSkpd.Replace("/", "").Contains(param.sSearch));
            }
            else
            {
                filteredRecords = allRecords;
            }

            if (!string.IsNullOrEmpty(param.tipe))
            {
                if (int.Parse(param.tipe) >= 3)
                {
                    var Sk = "STPD";
                    filteredRecords = filteredRecords.Where(d => d.Sk == Sk);
                }
                else
                {
                    filteredRecords = filteredRecords.Where(d => d.Keterangan.ToString() == param.tipe && d.Sk == null);
                }
            }

            if (!string.IsNullOrEmpty(param.tipe2))
            {
                filteredRecords = filteredRecords.Where(d => d.IdSptpdNavigation.IdCoa == param.tipe2);
            }

            if (param.min != null && param.max != null)
            {
                filteredRecords = filteredRecords.Where(d => d.Tanggal >= param.min && d.Tanggal <= param.max);
            }

            Func<Skpd, string> orderingFunction = (c => param.iSortCol_0 == 1 ? c.IdSptpdNavigation.IdSubjekNavigation.Nama :
                                                        param.iSortCol_0 == 2 ? c.IdSptpdNavigation.IdSubjekNavigation.Npwpd :
                                                        param.iSortCol_0 == 3 ? c.NoSkpd :
                                                    "");

            if (param.sSortDir_0 == "asc")
                filteredRecords = filteredRecords.OrderBy(orderingFunction);
            else
                filteredRecords = filteredRecords.OrderByDescending(orderingFunction);

            var displayedRecords = filteredRecords.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var data = (from skpd in displayedRecords
                        join sptpd in _context.Sptpd on skpd.IdSptpd equals sptpd.IdSptpd
                        join usaha in _context.DataSubjek on sptpd.IdSubjek equals usaha.IdSubjek
                        join coa in _context.Coa on sptpd.IdCoa equals coa.IdCoa
                        select new
                        {
                            id = skpd.IdSptpd,
                            nama = usaha.Nama,
                            npwpd = usaha.Npwpd,
                            nomor = skpd.NoSkpd,
                            masa = string.Format("{0:MMM yyyy}", sptpd.MasaPajak1),
                            tanggal = string.Format("{0:dd MMMM yyyy}", skpd.Tanggal),
                            terhutang = string.Format("{0:C0}", skpd.Terhutang),
                            uraian = coa.Uraian,
                            status = (skpd.Sk == "STPD" ? "7" : skpd.Keterangan.ToString())
                        }).ToArray();
            return Json(new
            {
                param.sEcho,
                iTotalRecords = allRecords.Count(),
                iTotalDisplayRecords = filteredRecords.Count(),
                aaData = data
            });
        }


        [Auth(new string[] { "Developers", "Laporan" })]
        public IActionResult SKPDKB()
        {
            //Link
            ViewBag.L = Url.Action("SKPDKB");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var query = (from c in _context.Coa
                         where (from d in _context.Sptpd where d.FlagKonfirmasi && c.Jenis == "Self Assessment" select d.IdCoa).Contains(c.IdCoa)
                         select c).ToList();
            ViewBag.Jenis = query;

            return View();
        }

        public JsonResult GetSKPDKB(JQueryDataTableParamModel param)
        {
            var allRecords = _context.Skpdkb.Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdSubjekNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation);
            IEnumerable<Skpdkb> filteredRecords;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredRecords = allRecords.Where(d => d.IdSptpdNavigation.IdSubjekNavigation.Nama.ToLower().Contains(param.sSearch.ToLower())
                || d.IdSptpdNavigation.IdSubjekNavigation.Npwpd.Replace(".", "").Contains(param.sSearch)
                || d.NoSkpdkb.Replace("/", "").Contains(param.sSearch));
            }
            else
            {
                filteredRecords = allRecords;
            }

            if (!string.IsNullOrEmpty(param.tipe))
            {
                filteredRecords = filteredRecords.Where(d => d.Keterangan.ToString() == param.tipe && d.Sk == null);
            }

            if (!string.IsNullOrEmpty(param.tipe2))
            {
                filteredRecords = filteredRecords.Where(d => d.IdSptpdNavigation.IdCoa == param.tipe2);
            }

            if (param.min != null && param.max != null)
            {
                filteredRecords = filteredRecords.Where(d => d.Tanggal >= param.min && d.Tanggal <= param.max);
            }

            Func<Skpdkb, string> orderingFunction = (c => param.iSortCol_0 == 1 ? c.IdSptpdNavigation.IdSubjekNavigation.Nama :
                                                        param.iSortCol_0 == 2 ? c.IdSptpdNavigation.IdSubjekNavigation.Npwpd :
                                                        param.iSortCol_0 == 3 ? c.NoSkpdkb :
                                                    "");

            if (param.sSortDir_0 == "asc")
                filteredRecords = filteredRecords.OrderBy(orderingFunction);
            else
                filteredRecords = filteredRecords.OrderByDescending(orderingFunction);

            var displayedRecords = filteredRecords.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var data = (from skpdkb in displayedRecords
                        join sptpd in _context.Sptpd on skpdkb.IdSptpd equals sptpd.IdSptpd
                        join usaha in _context.DataSubjek on sptpd.IdSubjek equals usaha.IdSubjek
                        join coa in _context.Coa on sptpd.IdCoa equals coa.IdCoa
                        select new
                        {
                            id = skpdkb.IdSptpd,
                            nama = usaha.Nama,
                            npwpd = usaha.Npwpd,
                            nomor = skpdkb.NoSkpdkb,
                            masa = string.Format("{0:MMM yyyy}", sptpd.MasaPajak1),
                            tanggal = string.Format("{0:dd MMMM yyyy}", skpdkb.Tanggal),
                            terhutang = string.Format("{0:C0}", skpdkb.Terhutang),
                            uraian = coa.Uraian,
                            status = skpdkb.Keterangan.ToString()
                        }).ToArray();
            return Json(new
            {
                param.sEcho,
                iTotalRecords = allRecords.Count(),
                iTotalDisplayRecords = filteredRecords.Count(),
                aaData = data
            });
        }


        [Auth(new string[] { "Developers", "Laporan" })]
        public IActionResult SKPDKBT()
        {
            //Link
            ViewBag.L = Url.Action("SKPDKBT");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var query = (from c in _context.Coa
                         where (from d in _context.Sptpd where d.FlagKonfirmasi && c.Jenis == "Self Assessment" select d.IdCoa).Contains(c.IdCoa)
                         select c).ToList();
            ViewBag.Jenis = query;

            return View();
        }

        public JsonResult GetSKPDKBT(JQueryDataTableParamModel param)
        {
            var allRecords = _context.Skpdkbt.Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdSubjekNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation);
            IEnumerable<Skpdkbt> filteredRecords;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredRecords = allRecords.Where(d => d.IdSptpdNavigation.IdSubjekNavigation.Nama.ToLower().Contains(param.sSearch.ToLower())
                || d.IdSptpdNavigation.IdSubjekNavigation.Npwpd.Replace(".", "").Contains(param.sSearch)
                || d.NoSkpdkbt.Replace("/", "").Contains(param.sSearch));
            }
            else
            {
                filteredRecords = allRecords;
            }

            if (!string.IsNullOrEmpty(param.tipe))
            {
                filteredRecords = filteredRecords.Where(d => d.Keterangan.ToString() == param.tipe && d.Sk == null);
            }

            if (!string.IsNullOrEmpty(param.tipe2))
            {
                filteredRecords = filteredRecords.Where(d => d.IdSptpdNavigation.IdCoa == param.tipe2);
            }

            if (param.min != null && param.max != null)
            {
                filteredRecords = filteredRecords.Where(d => d.Tanggal >= param.min && d.Tanggal <= param.max);
            }

            Func<Skpdkbt, string> orderingFunction = (c => param.iSortCol_0 == 1 ? c.IdSptpdNavigation.IdSubjekNavigation.Nama :
                                                        param.iSortCol_0 == 2 ? c.IdSptpdNavigation.IdSubjekNavigation.Npwpd :
                                                        param.iSortCol_0 == 3 ? c.NoSkpdkbt :
                                                    "");

            if (param.sSortDir_0 == "asc")
                filteredRecords = filteredRecords.OrderBy(orderingFunction);
            else
                filteredRecords = filteredRecords.OrderByDescending(orderingFunction);

            var displayedRecords = filteredRecords.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var data = (from skpdkbt in displayedRecords
                        join sptpd in _context.Sptpd on skpdkbt.IdSptpd equals sptpd.IdSptpd
                        join usaha in _context.DataSubjek on sptpd.IdSubjek equals usaha.IdSubjek
                        join coa in _context.Coa on sptpd.IdCoa equals coa.IdCoa
                        select new
                        {
                            id = skpdkbt.IdSptpd,
                            nama = usaha.Nama,
                            npwpd = usaha.Npwpd,
                            nomor = skpdkbt.NoSkpdkbt,
                            jenis = coa.Jenis,
                            masa = string.Format("{0:MMM yyyy}", sptpd.MasaPajak1),
                            tanggal = string.Format("{0:dd MMMM yyyy}", skpdkbt.Tanggal),
                            terhutang = string.Format("{0:C0}", skpdkbt.Terhutang),
                            uraian = coa.Uraian,
                            status = skpdkbt.Keterangan.ToString()
                        }).ToArray();
            return Json(new
            {
                param.sEcho,
                iTotalRecords = allRecords.Count(),
                iTotalDisplayRecords = filteredRecords.Count(),
                aaData = data
            });
        }

        [Auth(new string[] { "Developers", "Laporan" })]
        public IActionResult SKPDN()
        {
            //Link
            ViewBag.L = Url.Action("SKPDN");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var query = (from c in _context.Coa
                         where (from d in _context.Sptpd where d.FlagKonfirmasi select d.IdCoa).Contains(c.IdCoa)
                         select c).ToList();
            ViewBag.Jenis = query;

            return View();
        }

        public JsonResult GetSKPDN(JQueryDataTableParamModel param)
        {
            var allRecords = _context.Skpdn.Include(d => d.IdSspdNavigation).ThenInclude(d => d.IdSubjekNavigation)
                .Include(d => d.IdCoaNavigation).Where(d => d.NoSkpdn != null);
            IEnumerable<Skpdn> filteredRecords;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredRecords = allRecords.Where(d => d.IdSspdNavigation.IdSubjekNavigation.Nama.ToLower().Contains(param.sSearch.ToLower())
                || d.IdSspdNavigation.IdSubjekNavigation.Npwpd.Replace(".", "").Contains(param.sSearch)
                || d.NoSkpdn.Replace("/", "").Contains(param.sSearch));
            }
            else
            {
                filteredRecords = allRecords;
            }

            if (!string.IsNullOrEmpty(param.tipe2))
            {
                filteredRecords = filteredRecords.Where(d => d.IdCoa == param.tipe2);
            }

            if (param.min != null && param.max != null)
            {
                filteredRecords = filteredRecords.Where(d => d.Tanggal >= param.min && d.Tanggal <= param.max);
            }

            Func<Skpdn, string> orderingFunction = (c => param.iSortCol_0 == 1 ? c.IdSspdNavigation.IdSubjekNavigation.Nama :
                                                        param.iSortCol_0 == 2 ? c.IdSspdNavigation.IdSubjekNavigation.Npwpd :
                                                        param.iSortCol_0 == 3 ? c.NoSkpdn :
                                                    "");

            if (param.sSortDir_0 == "asc")
                filteredRecords = filteredRecords.OrderBy(orderingFunction);
            else
                filteredRecords = filteredRecords.OrderByDescending(orderingFunction);

            var displayedRecords = filteredRecords.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var data = (from skpdn in displayedRecords
                        join sspd in _context.Sspd on skpdn.IdSspd equals sspd.IdSspd
                        join usaha in _context.DataSubjek on sspd.IdSubjek equals usaha.IdSubjek
                        join coa in _context.Coa on skpdn.IdCoa equals coa.IdCoa
                        select new
                        {
                            id = skpdn.IdSptpd,
                            nama = usaha.Nama,
                            npwpd = usaha.Npwpd,
                            nomor = skpdn.NoSkpdn,
                            jenis = coa.Jenis,
                            masa = string.Format("{0:MMM yyyy}", skpdn.MasaPajak1),
                            tanggal = string.Format("{0:dd MMMM yyyy}", skpdn.Tanggal),
                            terhutang = string.Format("{0:C0}", skpdn.Terhutang),
                            uraian = coa.Uraian
                        }).ToArray();
            return Json(new
            {
                param.sEcho,
                iTotalRecords = allRecords.Count(),
                iTotalDisplayRecords = filteredRecords.Count(),
                aaData = data
            });
        }


        [Auth(new string[] { "Developers", "Laporan" })]
        public IActionResult STPD()
        {
            //Link
            ViewBag.L = Url.Action("STPD");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var query = (from c in _context.Coa
                         where (from d in _context.Sptpd where d.FlagKonfirmasi && c.Jenis == "Official Assessment" select d.IdCoa).Contains(c.IdCoa)
                         select c).ToList();
            ViewBag.Jenis = query;

            return View();
        }

        public JsonResult GetSTPD(JQueryDataTableParamModel param)
        {
            var allRecords = _context.Stpd.Include(d => d.IdSkpdNavigation).ThenInclude(d => d.IdSptpdNavigation).ThenInclude(d => d.IdSubjekNavigation)
                .Include(d => d.IdSkpdNavigation).ThenInclude(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation);
            IEnumerable<Stpd> filteredRecords;

            //Check whether the users should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //filteredRecords = repository.Labs
                filteredRecords = allRecords.Where(d => d.IdSkpdNavigation.IdSptpdNavigation.IdSubjekNavigation.Nama.ToLower().Contains(param.sSearch.ToLower())
                || d.IdSkpdNavigation.IdSptpdNavigation.IdSubjekNavigation.Npwpd.Replace(".", "").Contains(param.sSearch)
                || d.NoStpd.Replace("/", "").Contains(param.sSearch));
            }
            else
            {
                filteredRecords = allRecords;
            }

            if (!string.IsNullOrEmpty(param.tipe))
            {
                filteredRecords = filteredRecords.Where(d => d.Keterangan.ToString() == param.tipe && d.Sk == null);
            }

            if (!string.IsNullOrEmpty(param.tipe2))
            {
                filteredRecords = filteredRecords.Where(d => d.IdSkpdNavigation.IdSptpdNavigation.IdCoa == param.tipe2);
            }

            if (param.min != null && param.max != null)
            {
                filteredRecords = filteredRecords.Where(d => d.Tanggal >= param.min && d.Tanggal <= param.max);
            }

            Func<Stpd, string> orderingFunction = (c => param.iSortCol_0 == 1 ? c.IdSkpdNavigation.IdSptpdNavigation.IdSubjekNavigation.Nama :
                                                        param.iSortCol_0 == 2 ? c.IdSkpdNavigation.IdSptpdNavigation.IdSubjekNavigation.Npwpd :
                                                        param.iSortCol_0 == 3 ? c.NoStpd :
                                                    "");

            if (param.sSortDir_0 == "asc")
                filteredRecords = filteredRecords.OrderBy(orderingFunction);
            else
                filteredRecords = filteredRecords.OrderByDescending(orderingFunction);

            var displayedRecords = filteredRecords.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var data = (from stpd in displayedRecords
                        join skpd in _context.Skpd on stpd.IdSkpd equals skpd.IdSkpd
                        join sptpd in _context.Sptpd on skpd.IdSptpd equals sptpd.IdSptpd
                        join usaha in _context.DataSubjek on sptpd.IdSubjek equals usaha.IdSubjek
                        join coa in _context.Coa on sptpd.IdCoa equals coa.IdCoa
                        select new
                        {
                            id = skpd.IdSptpd,
                            nama = usaha.Nama,
                            npwpd = usaha.Npwpd,
                            nomor = stpd.NoStpd,
                            jenis = coa.Jenis,
                            masa = string.Format("{0:MMM yyyy}", sptpd.MasaPajak1),
                            tanggal = string.Format("{0:dd MMMM yyyy}", stpd.Tanggal),
                            terhutang = string.Format("{0:C0}", stpd.Terhutang),
                            uraian = coa.Uraian,
                            status = stpd.Keterangan.ToString()
                        }).ToArray();
            return Json(new
            {
                param.sEcho,
                iTotalRecords = allRecords.Count(),
                iTotalDisplayRecords = filteredRecords.Count(),
                aaData = data
            });
        }


        [Auth(new string[] { "Developers", "Laporan" })]
        public IActionResult SSPD()
        {
            //Link
            ViewBag.L = Url.Action("SSPD");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View();
        }

        public JsonResult GetSSPD(JQueryDataTableParamModel param)
        {
            var allRecords = _context.Sspd.Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdSubjekNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation);
            IEnumerable<Sspd> filteredRecords;

            //Check whether the users should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //filteredRecords = repository.Labs
                filteredRecords = allRecords.Where(d => d.IdSptpdNavigation.IdSubjekNavigation.Nama.ToLower().Contains(param.sSearch.ToLower())
                || d.IdSptpdNavigation.IdSubjekNavigation.Npwpd.Replace(".", "").Contains(param.sSearch)
                || d.NoSspd.Replace("/", "").Contains(param.sSearch));
            }
            else
            {
                filteredRecords = allRecords;
            }

            if (!string.IsNullOrEmpty(param.tipe))
            {
                var status = bool.Parse(param.tipe);
                filteredRecords = filteredRecords.Where(d => d.FlagBayar == status);
            }

            if (!string.IsNullOrEmpty(param.tipe2))
            {
                var status = bool.Parse(param.tipe2);
                filteredRecords = filteredRecords.Where(d => d.StatusSetor == status);
            }

            if (param.min != null && param.max != null)
            {
                filteredRecords = filteredRecords.Where(d => d.Tanggal >= param.min && d.Tanggal <= param.max);
            }

            Func<Sspd, string> orderingFunction = (c => param.iSortCol_0 == 1 ? c.IdSptpdNavigation.IdSubjekNavigation.Nama :
                                                        param.iSortCol_0 == 2 ? c.IdSptpdNavigation.IdSubjekNavigation.Npwpd :
                                                        param.iSortCol_0 == 3 ? c.NoSspd :
                                                    "");

            if (param.sSortDir_0 == "asc")
                filteredRecords = filteredRecords.OrderBy(orderingFunction);
            else
                filteredRecords = filteredRecords.OrderByDescending(orderingFunction);

            var displayedRecords = filteredRecords.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var data = (from sspd in displayedRecords
                        join usaha in _context.DataSubjek on sspd.IdSubjek equals usaha.IdSubjek
                        join coa in _context.Coa on sspd.IdCoa equals coa.IdCoa
                        select new
                        {
                            id = sspd.IdUsaha,
                            nama = usaha.Nama,
                            npwpd = usaha.Npwpd,
                            nomor = sspd.NoSspd,
                            jenis = coa.Jenis,
                            tanggal = string.Format("{0:dd MMMM yyyy}", sspd.Tanggal),
                            terhutang = string.Format("{0:C0}", sspd.JumlahSetoran),
                            uraian = coa.Uraian,
                            status = (sspd.StatusSetor ? "1" : "0"),
                            valid = (sspd.FlagBayar ? "1" : "0")
                        }).ToArray();
            return Json(new
            {
                param.sEcho,
                iTotalRecords = allRecords.Count(),
                iTotalDisplayRecords = filteredRecords.Count(),
                aaData = data
            });
        }


        [Auth(new string[] { "Developers", "Laporan" })]
        public IActionResult STS()
        {
            //Link
            ViewBag.L = Url.Action("STS");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";


            return View();
        }

        public JsonResult GetSTS(JQueryDataTableParamModel param)
        {
            var allRecords = _context.Sts.Include(d => d.IdBankNavigation);
            IEnumerable<Sts> filteredRecords;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredRecords = allRecords.Where(d => d.NoSts.Replace("/", "").Contains(param.sSearch));
            }
            else
            {
                filteredRecords = allRecords;
            }

            if (!string.IsNullOrEmpty(param.tipe))
            {
                var Valid = bool.Parse(param.tipe);
                filteredRecords = filteredRecords.Where(d => d.FlagValidasi == Valid);
            }

            if (!string.IsNullOrEmpty(param.tipe2))
            {
                filteredRecords = filteredRecords.Where(d => d.Keterangan == param.tipe2);
            }

            if (param.min != null && param.max != null)
            {
                filteredRecords = filteredRecords.Where(d => d.Tanggal >= param.min && d.Tanggal <= param.max);
            }

            Func<Sts, string> orderingFunction = (c => param.iSortCol_0 == 3 ? c.NoSts :
                                                    "");

            if (param.sSortDir_0 == "asc")
                filteredRecords = filteredRecords.OrderBy(orderingFunction);
            else
                filteredRecords = filteredRecords.OrderByDescending(orderingFunction);

            var displayedRecords = filteredRecords.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var data = (from sts in displayedRecords
                        join bank in _context.Bank on sts.IdBank equals bank.IdBank
                        select new
                        {
                            id = sts.IdSts,
                            nomor = sts.NoSts,
                            tanggal = string.Format("{0:MMM yyyy}", sts.Tanggal),
                            jumlahsetoran = string.Format("{0:C0}", sts.JumlahSetoran),
                            jenis = sts.Keterangan,
                            status = (sts.FlagValidasi ? "1" : "0")
                        }).ToArray();
            return Json(new
            {
                param.sEcho,
                iTotalRecords = allRecords.Count(),
                iTotalDisplayRecords = filteredRecords.Count(),
                aaData = data
            });
        }


        [Auth(new string[] { "Developers", "Laporan" })]
        public IActionResult KartuData (Guid? id)
        {
            if (id != null)
            {
                //Link
                ViewBag.L = Url.Action("KartuData", new { id });
                ViewBag.L1 = "";
                ViewBag.L2 = "";
                ViewBag.L3 = "";

                var data = _context.DataSubjek.FirstOrDefault(d => d.IdSubjek == id);

                return View("ListSurat", data);
            }
            //Link
            ViewBag.L = Url.Action("KartuData");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var pemda = _context.Pemda.FirstOrDefault();
            ViewBag.Kecamatan = _context.IndKecamatan.Where(d => d.IndKabKotaId == pemda.IndKabKotaId).ToList();

            return View();
        }

        [Route("Laporan/GetKartuData/{id}")]
        public JsonResult GetKartuData(JQueryDataTableParamModel param, Guid? id)
        {
            var spt = _context.Sptpd.Include(d => d.IdCoaNavigation).Where(d => d.IdSubjek == id && d.FlagKonfirmasi)
                .Select(x => new ListSurat {
                    Id = x.IdSptpd,
                    Coa = x.IdCoaNavigation.Uraian,
                    Link = "/SPTPD/" + x.IdSptpd.ToString().ToLower(),
                    Tanggal = x.Tanggal,
                    Hutang = x.Terhutang,
                    Masa = string.Format("{0:MMMM yyyy}", x.MasaPajak1),
                    Nomor = x.NoSptpd,
                    Status = (x.Sk == "SKPD" ? "4" : x.Sk == "SKPDKB" ? "5" : x.Sk == "SKPDKBT" ? "6" : x.Keterangan.ToString())
                }).ToList();

            var skpd = _context.Skpd.Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation).Where(d => d.IdSptpdNavigation.IdSubjek == id)
                .Select(x => new ListSurat
                {
                    Id = x.IdSkpd,
                    Coa = x.IdSptpdNavigation.IdCoaNavigation.Uraian,
                    Link = "/Penetapan/SKPD/" + x.IdSptpd.ToString().ToLower(),
                    Tanggal = x.Tanggal,
                    Hutang = x.Terhutang,
                    Masa = string.Format("{0:MMMM yyyy}", x.IdSptpdNavigation.MasaPajak1),
                    Nomor = x.NoSkpd,
                    Status = (x.Sk == "STPD" ? "7" : x.Keterangan.ToString())
                }).ToList();

            var skpdkb = _context.Skpdkb.Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation).Where(d => d.IdSptpdNavigation.IdSubjek == id)
                .Select(x => new ListSurat
                {
                    Id = x.IdSkpdkb,
                    Coa = x.IdSptpdNavigation.IdCoaNavigation.Uraian,
                    Link = "/Penetapan/SKPDKB/" + x.IdSptpd.ToString().ToLower(),
                    Tanggal = x.Tanggal,
                    Hutang = x.Terhutang,
                    Masa = string.Format("{0:MMMM yyyy}", x.IdSptpdNavigation.MasaPajak1),
                    Nomor = x.NoSkpdkb,
                    Status = x.Keterangan.ToString()
                }).ToList();

            var skpdkbt = _context.Skpdkbt.Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation).Where(d => d.IdSptpdNavigation.IdSubjek == id)
                .Select(x => new ListSurat
                {
                    Id = x.IdSkpdkbt,
                    Coa = x.IdSptpdNavigation.IdCoaNavigation.Uraian,
                    Link = "/Penetapan/SKPDKBT/" + x.IdSptpd.ToString().ToLower(),
                    Tanggal = x.Tanggal,
                    Hutang = x.Terhutang,
                    Masa = string.Format("{0:MMMM yyyy}", x.IdSptpdNavigation.MasaPajak1),
                    Nomor = x.NoSkpdkbt,
                    Status = x.Keterangan.ToString()
                }).ToList();

            var stpd = _context.Stpd.Include(d => d.IdSkpdNavigation).ThenInclude(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation).Where(d => d.IdSkpdNavigation.IdSptpdNavigation.IdSubjek == id)
                .Select(x => new ListSurat
                {
                    Id = x.IdStpd,
                    Coa = x.IdSkpdNavigation.IdSptpdNavigation.IdCoaNavigation.Uraian,
                    Link = "/Penetapan/STPD/" + x.IdSkpdNavigation.IdSptpd.ToString().ToLower(),
                    Tanggal = x.Tanggal,
                    Hutang = x.Terhutang,
                    Masa = string.Format("{0:MMMM yyyy}", x.IdSkpdNavigation.IdSptpdNavigation.MasaPajak1),
                    Nomor = x.NoStpd,
                    Status = x.Keterangan.ToString()
                }).ToList();

            var sppt = _context.Sppt.Include(d => d.IdSpopNavigation).ThenInclude(d => d.IdSubjekNavigation).Include(d => d.IdSpopNavigation).ThenInclude(d => d.IdCoaNavigation)
                .Where(d => d.IdSpopNavigation.IdSubjek == id)
                .Select(x => new ListSurat
                {
                    Id = x.IdSppt,
                    Coa = x.IdSpopNavigation.IdCoaNavigation.Uraian,
                    Link = "/PBB/SPOP/" + x.IdSpopNavigation.IdSubjek.ToString().ToLower(),
                    Tanggal = x.Tanggal,
                    Hutang = x.Terhutang,
                    Masa = x.Tahun.ToString(),
                    Nomor = x.IdSpopNavigation.Nop,
                    Status = x.Keterangan.ToString()
                }).ToList();

            var result = spt.Union(skpd).Union(skpdkb).Union(skpdkbt).Union(stpd).Union(sppt);

            List<ListSurat> listSurat = new List<ListSurat>();

            foreach (var item in result)
            {
                listSurat.Add(new ListSurat
                {
                    Id = item.Id,
                    Coa = item.Coa,
                    Link = item.Link,
                    Tanggal = item.Tanggal,
                    Hutang = item.Hutang,
                    Masa = item.Masa,
                    Nomor = item.Nomor,
                    Status = item.Status
                });

            }

            var allRecords = listSurat;
            IEnumerable<ListSurat> filteredRecords;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredRecords = allRecords.Where(d => d.Coa.ToLower().Contains(param.sSearch.ToLower())
                || d.Nomor.Replace("/", "").Contains(param.sSearch));
            }
            else
            {
                filteredRecords = allRecords;
            }

            if (!string.IsNullOrEmpty(param.tipe))
            {
                filteredRecords = filteredRecords.Where(d => d.Status == param.tipe);
            }

            if (param.min != null && param.max != null)
            {
                filteredRecords = filteredRecords.Where(d => d.Tanggal >= param.min && d.Tanggal <= param.max);
            }

            var total = filteredRecords.Sum(d => d.Hutang);

            Func<ListSurat, string> orderingFunction = (c => param.iSortCol_0 == 1 ? c.Nomor :
                                                        param.iSortCol_0 == 2 ? c.Tanggal.ToString() :
                                                        param.iSortCol_0 == 3 ? c.Coa :
                                                        param.iSortCol_0 == 4 ? c.Masa :
                                                    "");

            if (param.sSortDir_0 == "asc")
                filteredRecords = filteredRecords.OrderBy(orderingFunction);
            else
                filteredRecords = filteredRecords.OrderByDescending(orderingFunction);

            var displayedRecords = filteredRecords.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var data = (from item in displayedRecords
                        select new
                        {
                            id = item.Id,
                            coa = item.Coa,
                            link = item.Link,
                            tanggal = string.Format("{0:dd MMMM yyyy}", item.Tanggal),
                            hutang = string.Format("{0:C0}", item.Hutang),
                            masa = item.Masa,
                            nomor = item.Nomor,
                            status = item.Status
                        }).ToArray();
            return Json(new
            {
                param.sEcho,
                iTotalRecords = allRecords.Count(),
                iTotalDisplayRecords = filteredRecords.Count(),
                total = string.Format("{0:C0}", total),
                aaData = data
            });
        }


        [Auth(new string[] { "Developers", "Fiskal" })]
        public IActionResult Fiskal()
        {
            //Link
            ViewBag.L = Url.Action("Fiskal");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var pemda = _context.Pemda.FirstOrDefault();
            ViewBag.Kecamatan = _context.IndKecamatan.Where(d => d.IndKabKotaId == pemda.IndKabKotaId).ToList();

            return View();
        }

    }
}