using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SIP.Models;
using SIP.ViewModels;

namespace SIP.Controllers
{
    public class AjaxDataController : Controller
    {
        private readonly DB_NewContext _context;

        public AjaxDataController(DB_NewContext context)
        {
            _context = context;
        }

        public JsonResult Modal()
        {
            TempData["Partial"] = "True";
            return Json(new { success = true });
        }

        public JsonResult Menu(string id)
        {
            HttpContext.Session.SetString("Menu", id);

            if (id != "head")
            {
                HttpContext.Session.SetString("Button", "kt-aside__brand-aside-toggler--active");
            }
            else
            {
                HttpContext.Session.SetString("Button", "");
            }

            return Json(new { success = true });
        }

        // CEK NIK
        public JsonResult CekNik(string id)
        {
            var nik = _context.DataSubjek.Where(d => d.Nik == id);
            if (nik != null)
            {
                var subjek = (from a in nik
                              where a.Nik == id
                              select new
                              {
                                  id = a.IdSubjek,
                                  nama = a.Nama
                              }).ToList();
                return Json(subjek);

            }
            return Json("");
        }

        // CEK NIK
        public JsonResult CekNpwp(string id)
        {
            var npwp = _context.DataSubjek.Where(d => d.Npwp == id);
            if (npwp != null)
            {
                var subjek = (from a in npwp
                              where a.Npwp == id
                              select new
                              {
                                  id = a.IdSubjek,
                                  nama = a.Nama
                              }).ToList();
                return Json(subjek);

            }
            return Json("");
        }

        public JsonResult CekNop(string id)
        {
            var nop = _context.Spop.FirstOrDefault(d => d.Nop == id);
            if (nop != null)
            {
                return Json(id);
            }
            return Json("");
        }

        public JsonResult DropBank(int id)
        {
            var bank = (from b in _context.Bank
                        join r in _context.RefBank on b.IdRef equals r.IdBank
                        join j in _context.RefJenisSetoran on b.IdSetoran equals j.IdSetoran
                        where b.IdSetoran == id
                        select new
                        {
                            value = b.IdBank,
                            text = r.NamaBank + " | " + b.NoRek + " | " + b.NamaRek
                        }).ToList();
            return Json(bank);
        }

        public JsonResult DropSatuanRetribusi(Guid id)
        {
            var satuan = (from tarif in _context.TarifRetribusi
                          where tarif.IdTarif == id
                          select new
                          {
                              satuan = (tarif.Var1 == 1 ? "" + tarif.Satuan1 : tarif.Var1 + "/" + tarif.Satuan1)
                          }).ToList();
            return Json(satuan);
        }

        // LOAD DROPDOWN PROVINSI
        public JsonResult DropProvinsi()
        {
            var pro = _context.IndProvinsi.ToList();
            var prov = (from a in pro
                        select new
                        {
                            id = a.IndProvinsiId,
                            uraian = a.Provinsi
                        }).ToList();
            return Json(prov);
        }

        // LOAD DROPDOWN KOTA
        public JsonResult DropKota(int id)
        {
            var kot = _context.IndKabKota.ToList();
            var kota = (from a in kot
                        where a.IndProvinsiId == id
                        select new
                        {
                            id = a.IndKabKotaId,
                            uraian = a.KabKota
                        }).ToList();
            return Json(kota);
        }

        // LOAD DROPDOWN KECAMATAN
        public JsonResult DropKecamatan(int id)
        {
            var kec = _context.IndKecamatan.ToList();
            var kecamatan = (from a in kec
                             where a.IndKabKotaId == id
                             select new
                             {
                                 id = a.IndKecamatanId,
                                 uraian = a.Kecamatan
                             }).ToList();
            return Json(kecamatan);
        }

        // LOAD DROPDOWN KELURAHAN
        public JsonResult DropKelurahan(int id)
        {
            var kel = _context.IndKelurahan.ToList();
            var kelurahan = (from a in kel
                             where a.IndKecamatanId == id
                             select new
                             {
                                 id = a.IndKelurahanId,
                                 uraian = a.Kelurahan
                             }).ToList();
            return Json(kelurahan);
        }

        // LOAD DROPDOWN KECAMATAN
        public JsonResult GetKecamatan()
        {
            var Pemda = _context.Pemda.First();
            var kec = _context.IndKecamatan.ToList();
            var kecamatan = (from a in kec
                             where a.IndKabKotaId == Pemda.IndKabKotaId
                             select new
                             {
                                 id = a.IndKecamatanId,
                                 uraian = a.Kecamatan
                             }).ToList();
            return Json(kecamatan);
        }

        public JsonResult GetSubjekDetail(JQueryDataTableParamModel param)
        {

            var allRecords = _context.DataSubjek; //All records
            IEnumerable<DataSubjek> filteredRecords;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredRecords = allRecords.Where(d => d.Nama.ToLower().Contains(param.sSearch.ToLower())
                || d.Nik.Contains(param.sSearch)
                || d.Npwpd.Replace(".", "").Trim().Contains(param.sSearch.Trim())).Take(10);
            }
            else
            {
                filteredRecords = allRecords.OrderBy(d => d.NoPokok).Take(5);
            }

            var data = (from s in filteredRecords
                        select new
                        {
                            id = s.IdSubjek,
                            nama = s.Nama,
                            nik = s.Nik,
                            npwp = s.Npwp,
                            npwpd = s.Npwpd,
                            status = (s.Status ? "1" : "0")
                        }).ToArray();
            return Json(new
            {
                param.sEcho,
                iTotalRecords = allRecords.Count(),
                iTotalDisplayRecords = filteredRecords.Count(),
                aaData = data
            });
        }

        public JsonResult GetSubjekRetribusi(JQueryDataTableParamModel param)
        {

            var allRecords = _context.DataSubjek.Where(d => d.Npwrd != null); //All records
            IEnumerable<DataSubjek> filteredRecords;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredRecords = allRecords.Where(d => d.Nama.ToLower().Contains(param.sSearch.ToLower())
                || d.Nik.Contains(param.sSearch)
                || d.Npwpd.Replace(".", "").Trim().Contains(param.sSearch.Trim())).Take(10);
            }
            else
            {
                filteredRecords = allRecords.OrderBy(d => d.NoPokok).Take(5);
            }

            var data = (from s in filteredRecords
                        select new
                        {
                            id = s.IdSubjek,
                            nama = s.Nama,
                            nik = s.Nik,
                            npwp = s.Npwp,
                            npwpd = s.Npwpd,
                            status = (s.Status ? "1" : "0")
                        }).ToArray();
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

            //Check whether the users should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //filteredRecords = repository.Labs
                filteredRecords = allRecords.Where(d => d.Nama.ToLower().Contains(param.sSearch.ToLower())
                || d.Nik.Contains(param.sSearch)
                || d.Npwpd.Replace(".", "").Contains(param.sSearch)).Take(100);
            }
            else
            {
                filteredRecords = allRecords.Take(100);
            }

            if (param.status.HasValue)
            {
                filteredRecords = filteredRecords.Where(d => d.Status == param.status);
            }

            if (!string.IsNullOrEmpty(param.tipe))
            {
                filteredRecords = filteredRecords.Where(d => d.IdPekerjaan.ToString() == param.tipe);
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
                        join k in _context.IndKecamatan on s.IndKecamatanId equals k.IndKecamatanId
                        join p in _context.RefPekerjaan on s.IdPekerjaan equals p.IdPekerjaan
                        join u in _context.DataUsaha on s.IdSubjek equals u.IdSubjek into d
                        from u in d.DefaultIfEmpty()
                        select new
                        {
                            id = s.IdSubjek,
                            nama = s.Nama,
                            nik = s.Nik,
                            npwpd = s.Npwpd,
                            alamat = s.Alamat,
                            kecamatan = k.Kecamatan,
                            pekerjaan = p.Pekerjaan,
                            status = (s.Status ? "1" : "0"),
                            usaha = d.FirstOrDefault()?.NamaUsaha ?? string.Empty
                        }).Distinct().ToArray().Take(100);
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

            //Check whether the users should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //filteredRecords = repository.Labs
                filteredRecords = allRecords.Where(d => d.Nama.ToLower().Contains(param.sSearch.ToLower())
                || d.Npwp.Replace(".", "").Contains(param.sSearch)
                || d.Npwpd.Replace(".", "").Contains(param.sSearch)).Take(100);
            }
            else
            {
                filteredRecords = allRecords.Take(100);
            }

            if (param.status.HasValue)
            {
                filteredRecords = filteredRecords.Where(d => d.Status == param.status);
            }

            if (!string.IsNullOrEmpty(param.tipe))
            {
                filteredRecords = filteredRecords.Where(d => d.IdBadanHukum.ToString() == param.tipe);
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
                        join k in _context.IndKecamatan on s.IndKecamatanId equals k.IndKecamatanId
                        join p in _context.RefBadanHukum on s.IdBadanHukum equals p.IdBadanHukum
                        join u in _context.DataUsaha on s.IdSubjek equals u.IdSubjek into d
                        from u in d.DefaultIfEmpty()
                        select new
                        {
                            id = s.IdSubjek,
                            nama = s.Nama,
                            npwp = s.Npwp,
                            npwpd = s.Npwpd,
                            alamat = s.Alamat,
                            kecamatan = k.Kecamatan,
                            badan = p.BadanHukum,
                            status = (s.Status ? "1" : "0"),
                            usaha = d.FirstOrDefault()?.NamaUsaha ?? string.Empty
                        }).Distinct().ToArray().Take(100);
            return Json(new
            {
                param.sEcho,
                iTotalRecords = allRecords.Count(),
                iTotalDisplayRecords = filteredRecords.Count(),
                aaData = data
            });
        }

        public JsonResult GetSubjekPenetapan(JQueryDataTableParamModel param)
        {
            var allRecords = _context.DataSubjek.Where(d => d.Npwpd != null); //All records
            IEnumerable<DataSubjek> filteredRecords;

            //Check whether the users should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //filteredRecords = repository.Labs
                filteredRecords = allRecords.Where(d => d.Nama.ToLower().Contains(param.sSearch.ToLower())
                || d.Nik.Contains(param.sSearch)
                || d.Npwp.Replace(".", "").Contains(param.sSearch)
                || d.Npwpd.Replace(".", "").Contains(param.sSearch)).Take(100);
            }
            else
            {
                filteredRecords = allRecords.Take(100);
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
                        join u in _context.DataUsaha on s.IdSubjek equals u.IdSubjek into d
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
                            usaha = d.FirstOrDefault()?.NamaUsaha ?? string.Empty
                        }).Distinct().ToArray().Take(100);
            return Json(new
            {
                param.sEcho,
                iTotalRecords = allRecords.Count(),
                iTotalDisplayRecords = filteredRecords.Count(),
                aaData = data
            });
        }

        public JsonResult GetSubjekPBB(JQueryDataTableParamModel param)
        {
            var allRecords = _context.DataSubjek.Where(d => d.Npwpd != null); //All records
            IEnumerable<DataSubjek> filteredRecords;

            //Check whether the users should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //filteredRecords = repository.Labs
                filteredRecords = allRecords.Where(d => d.Nama.ToLower().Contains(param.sSearch.ToLower())
                || d.Nik.Contains(param.sSearch)
                || d.Npwp.Replace(".", "").Contains(param.sSearch)
                || d.Npwpd.Replace(".", "").Contains(param.sSearch)).Take(100);
            }
            else
            {
                filteredRecords = allRecords.Take(100);
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
                        join spop in _context.Spop on s.IdSubjek equals spop.IdSubjek into spo
                        from spop in spo.DefaultIfEmpty()
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
                            spop = spo.FirstOrDefault()?.Nop ?? string.Empty
                        }).Distinct().ToArray().Take(100);
            return Json(new
            {
                param.sEcho,
                iTotalRecords = allRecords.Count(),
                iTotalDisplayRecords = filteredRecords.Count(),
                aaData = data
            });
        }

        public JsonResult GetSubjekPBB_Kec(JQueryDataTableParamModel param)
        {
            var allRecords = _context.DataSubjek.Where(d => d.Npwpd != null); //All records
            IEnumerable<DataSubjek> filteredRecords;

            //Check whether the users should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //filteredRecords = repository.Labs
                filteredRecords = allRecords.Where(d => d.Nama.ToLower().Contains(param.sSearch.ToLower())
                || d.Nik.Contains(param.sSearch)
                || d.Npwp.Replace(".", "").Contains(param.sSearch)
                || d.Npwpd.Replace(".", "").Contains(param.sSearch)).Take(100);
            }
            else
            {
                filteredRecords = allRecords.Take(100);
            }

            if (param.status.HasValue)
            {
                filteredRecords = filteredRecords.Where(d => d.Status == param.status);
            }

            if (!string.IsNullOrEmpty(param.tipe))
            {
                filteredRecords = filteredRecords.Where(d => d.IndKecamatanId.ToString() == param.tipe);
            }

            if (!string.IsNullOrEmpty(param.tipe2))
            {
                filteredRecords = filteredRecords.Where(d => d.IndKelurahanId.ToString() == param.tipe2);
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
                        join l in _context.IndKelurahan on s.IndKelurahanId equals l.IndKelurahanId
                        join spop in _context.Spop on s.IdSubjek equals spop.IdSubjek into spo
                        from spop in spo.DefaultIfEmpty()
                        select new
                        {
                            id = s.IdSubjek,
                            nama = s.Nama,
                            nik = s.Nik ?? "-",
                            npwp = s.Npwp ?? "-",
                            npwpd = s.Npwpd,
                            alamat = s.Alamat,
                            kecamatan = k.Kecamatan,
                            kelurahan = l.Kelurahan,
                            status = (s.Status ? "1" : "0"),
                            spop = spo.FirstOrDefault()?.Nop ?? string.Empty
                        }).Distinct().ToArray().Take(100);
            return Json(new
            {
                param.sEcho,
                iTotalRecords = allRecords.Count(),
                iTotalDisplayRecords = filteredRecords.Count(),
                aaData = data
            });
        }

        [AcceptVerbs("Post")]
        public JsonResult GetSPTPD(string postData)
        {
            var param = JsonConvert.DeserializeObject<CetakParamModel>(postData);

            var allRecords = _context.Sptpd.Include(d => d.IdSubjekNavigation).Include(d => d.IdCoaNavigation).Where(d => d.FlagKonfirmasi); //All records
            IEnumerable<Sptpd> filteredRecords;

            filteredRecords = allRecords;

            if (!string.IsNullOrEmpty(param.Tipe1.Trim()))
            {
                if (int.Parse(param.Tipe1) >= 3)
                {
                    var Sk = "SKPD";
                    if (param.Tipe1 == "5") { Sk = "SKPDKB"; }
                    else if (param.Tipe1 == "6") { Sk = "SKPDKBT"; }
                    filteredRecords = filteredRecords.Where(d => d.Sk == Sk);
                }
                else
                {
                    filteredRecords = filteredRecords.Where(d => d.Keterangan.ToString() == param.Tipe1 && d.Sk == null);
                }
            }

            if (!string.IsNullOrEmpty(param.Tipe2))
            {
                filteredRecords = filteredRecords.Where(d => d.IdCoaNavigation.Jenis == param.Tipe2);
            }

            if (!string.IsNullOrEmpty(param.Tipe3))
            {
                filteredRecords = filteredRecords.Where(d => d.IdCoa == param.Tipe3);
            }

            if (param.DateMin != null && param.DateMax != null)
            {
                filteredRecords = filteredRecords.Where(d => d.Tanggal >= param.DateMin && d.Tanggal <= param.DateMax);
            }

            Func<Sptpd, string> orderingFunction = (c => param.SortIndex == 0 ? c.Tanggal.ToString() :
                                                       param.SortIndex == 1 ? c.NoSptpd :
                                                       param.SortIndex == 2 ? c.IdSubjekNavigation.Nama :
                                                   "");

            filteredRecords = filteredRecords.OrderBy(orderingFunction);

            var data = (from s in filteredRecords
                        join w in _context.DataSubjek on s.IdSubjek equals w.IdSubjek
                        join u in _context.DataUsaha on s.IdUsaha equals u.IdUsaha
                        join c in _context.Coa on s.IdCoa equals c.IdCoa
                        select new
                        {
                            nama = w.Nama,
                            usaha = u.NamaUsaha,
                            nomor = s.NoSptpd,
                            masa = string.Format("{0:MMM yyyy}", s.MasaPajak1),
                            tanggal = string.Format("{0:dd MMMM yyyy}", s.Tanggal),
                            terhutang = s.Terhutang,
                            setor = s.KreditPajak,
                            uraian = c.Uraian,
                            status = s.Sk ?? (s.Keterangan == 0 ? "Belum Bayar" : s.Keterangan == 1 ? "Lunas" : "-")
                        }).ToList();
            return Json(data);
        }

        [AcceptVerbs("Post")]
        public JsonResult GetSKPD(string postData)
        {
            var param = JsonConvert.DeserializeObject<CetakParamModel>(postData);

            var allRecords = _context.Skpd
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdSubjekNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation).Where(d => d.Nomor != null); //All records
            IEnumerable<Skpd> filteredRecords;

            filteredRecords = allRecords;

            if (!string.IsNullOrEmpty(param.Tipe1.Trim()))
            {
                if (int.Parse(param.Tipe1) >= 3)
                {
                    var Sk = "STPD";
                    filteredRecords = filteredRecords.Where(d => d.Sk == Sk);
                }
                else
                {
                    filteredRecords = filteredRecords.Where(d => d.Keterangan.ToString() == param.Tipe1 && d.Sk == null);
                }
            }

            if (!string.IsNullOrEmpty(param.Tipe2))
            {
                filteredRecords = filteredRecords.Where(d => d.IdSptpdNavigation.IdCoa == param.Tipe2);
            }

            if (param.DateMin != null && param.DateMax != null)
            {
                filteredRecords = filteredRecords.Where(d => d.Tanggal >= param.DateMin && d.Tanggal <= param.DateMax);
            }

            Func<Skpd, string> orderingFunction = (c => param.SortIndex == 0 ? c.Tanggal.ToString() :
                                                       param.SortIndex == 1 ? c.NoSkpd :
                                                       param.SortIndex == 2 ? c.IdSptpdNavigation.IdSubjekNavigation.Nama :
                                                   "");

            filteredRecords = filteredRecords.OrderBy(orderingFunction);

            var data = (from s in filteredRecords
                        join p in _context.Sptpd on s.IdSptpd equals p.IdSptpd
                        join w in _context.DataSubjek on p.IdSubjek equals w.IdSubjek
                        join u in _context.DataUsaha on p.IdUsaha equals u.IdUsaha
                        join c in _context.Coa on p.IdCoa equals c.IdCoa
                        select new
                        {
                            nama = w.Nama,
                            usaha = u.NamaUsaha,
                            nomor = s.NoSkpd,
                            masa = string.Format("{0:MMM yyyy}", p.MasaPajak1),
                            tanggal = string.Format("{0:dd MMMM yyyy}", s.Tanggal),
                            terhutang = s.Terhutang,
                            setor = s.KreditPajak,
                            uraian = c.Uraian,
                            status = s.Sk ?? (s.Keterangan == 0 ? "Belum Bayar" : s.Keterangan == 1 ? "Lunas" : "-")
                        }).ToList();
            return Json(data);
        }

        [AcceptVerbs("Post")]
        public JsonResult GetSKPDKB(string postData)
        {
            var param = JsonConvert.DeserializeObject<CetakParamModel>(postData);

            var allRecords = _context.Skpdkb
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdSubjekNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation).Where(d => d.Nomor != null); //All records
            IEnumerable<Skpdkb> filteredRecords;

            filteredRecords = allRecords;

            if (!string.IsNullOrEmpty(param.Tipe1.Trim()))
            {
                if (int.Parse(param.Tipe1) >= 3)
                {
                    var Sk = "STPD";
                    filteredRecords = filteredRecords.Where(d => d.Sk == Sk);
                }
                else
                {
                    filteredRecords = filteredRecords.Where(d => d.Keterangan.ToString() == param.Tipe1 && d.Sk == null);
                }
            }

            if (!string.IsNullOrEmpty(param.Tipe2))
            {
                filteredRecords = filteredRecords.Where(d => d.IdSptpdNavigation.IdCoa == param.Tipe2);
            }

            if (param.DateMin != null && param.DateMax != null)
            {
                filteredRecords = filteredRecords.Where(d => d.Tanggal >= param.DateMin && d.Tanggal <= param.DateMax);
            }

            Func<Skpdkb, string> orderingFunction = (c => param.SortIndex == 0 ? c.Tanggal.ToString() :
                                                       param.SortIndex == 1 ? c.NoSkpdkb :
                                                       param.SortIndex == 2 ? c.IdSptpdNavigation.IdSubjekNavigation.Nama :
                                                   "");

            filteredRecords = filteredRecords.OrderBy(orderingFunction);

            var data = (from s in filteredRecords
                        join p in _context.Sptpd on s.IdSptpd equals p.IdSptpd
                        join w in _context.DataSubjek on p.IdSubjek equals w.IdSubjek
                        join u in _context.DataUsaha on p.IdUsaha equals u.IdUsaha
                        join c in _context.Coa on p.IdCoa equals c.IdCoa
                        select new
                        {
                            nama = w.Nama,
                            usaha = u.NamaUsaha,
                            nomor = s.NoSkpdkb,
                            masa = string.Format("{0:MMM yyyy}", p.MasaPajak1),
                            tanggal = string.Format("{0:dd MMMM yyyy}", s.Tanggal),
                            terhutang = s.Terhutang,
                            setor = s.KreditPajak,
                            uraian = c.Uraian,
                            status = s.Keterangan == 0 ? "Belum Bayar" : s.Keterangan == 1 ? "Lunas" : "-"
                        }).ToList();
            return Json(data);
        }

        [AcceptVerbs("Post")]
        public JsonResult GetSKPDKBT(string postData)
        {
            var param = JsonConvert.DeserializeObject<CetakParamModel>(postData);

            var allRecords = _context.Skpdkbt
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdSubjekNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation).Where(d => d.Nomor != null); //All records
            IEnumerable<Skpdkbt> filteredRecords;

            filteredRecords = allRecords;

            if (!string.IsNullOrEmpty(param.Tipe1.Trim()))
            {
                if (int.Parse(param.Tipe1) >= 3)
                {
                    var Sk = "STPD";
                    filteredRecords = filteredRecords.Where(d => d.Sk == Sk);
                }
                else
                {
                    filteredRecords = filteredRecords.Where(d => d.Keterangan.ToString() == param.Tipe1 && d.Sk == null);
                }
            }

            if (!string.IsNullOrEmpty(param.Tipe2))
            {
                filteredRecords = filteredRecords.Where(d => d.IdSptpdNavigation.IdCoa == param.Tipe2);
            }

            if (param.DateMin != null && param.DateMax != null)
            {
                filteredRecords = filteredRecords.Where(d => d.Tanggal >= param.DateMin && d.Tanggal <= param.DateMax);
            }

            Func<Skpdkbt, string> orderingFunction = (c => param.SortIndex == 0 ? c.Tanggal.ToString() :
                                                       param.SortIndex == 1 ? c.NoSkpdkbt :
                                                       param.SortIndex == 2 ? c.IdSptpdNavigation.IdSubjekNavigation.Nama :
                                                   "");

            filteredRecords = filteredRecords.OrderBy(orderingFunction);

            var data = (from s in filteredRecords
                        join p in _context.Sptpd on s.IdSptpd equals p.IdSptpd
                        join w in _context.DataSubjek on p.IdSubjek equals w.IdSubjek
                        join u in _context.DataUsaha on p.IdUsaha equals u.IdUsaha
                        join c in _context.Coa on p.IdCoa equals c.IdCoa
                        select new
                        {
                            nama = w.Nama,
                            usaha = u.NamaUsaha,
                            nomor = s.NoSkpdkbt,
                            masa = string.Format("{0:MMM yyyy}", p.MasaPajak1),
                            tanggal = string.Format("{0:dd MMMM yyyy}", s.Tanggal),
                            terhutang = s.Terhutang,
                            setor = s.KreditPajak,
                            uraian = c.Uraian,
                            status = s.Keterangan == 0 ? "Belum Bayar" : s.Keterangan == 1 ? "Lunas" : "-"
                        }).ToList();
            return Json(data);
        }

        [AcceptVerbs("Post")]
        public JsonResult GetSTPD(string postData)
        {
            var param = JsonConvert.DeserializeObject<CetakParamModel>(postData);

            var allRecords = _context.Stpd
                .Include(d => d.IdSkpdNavigation).ThenInclude(d => d.IdSptpdNavigation).ThenInclude(d => d.IdSubjekNavigation)
                .Include(d => d.IdSkpdNavigation).ThenInclude(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation).Where(d => d.Nomor != null); //All records
            IEnumerable<Stpd> filteredRecords;

            filteredRecords = allRecords;

            if (!string.IsNullOrEmpty(param.Tipe1.Trim()))
            {
                if (int.Parse(param.Tipe1) >= 3)
                {
                    var Sk = "STPD";
                    filteredRecords = filteredRecords.Where(d => d.Sk == Sk);
                }
                else
                {
                    filteredRecords = filteredRecords.Where(d => d.Keterangan.ToString() == param.Tipe1 && d.Sk == null);
                }
            }

            if (!string.IsNullOrEmpty(param.Tipe2))
            {
                filteredRecords = filteredRecords.Where(d => d.IdSkpdNavigation.IdSptpdNavigation.IdCoa == param.Tipe2);
            }

            if (param.DateMin != null && param.DateMax != null)
            {
                filteredRecords = filteredRecords.Where(d => d.Tanggal >= param.DateMin && d.Tanggal <= param.DateMax);
            }

            Func<Stpd, string> orderingFunction = (c => param.SortIndex == 0 ? c.Tanggal.ToString() :
                                                       param.SortIndex == 1 ? c.NoStpd :
                                                       param.SortIndex == 2 ? c.IdSkpdNavigation.IdSptpdNavigation.IdSubjekNavigation.Nama :
                                                   "");

            filteredRecords = filteredRecords.OrderBy(orderingFunction);

            var data = (from s in filteredRecords
                        join k in _context.Skpd on s.IdSkpd equals k.IdSkpd
                        join p in _context.Sptpd on k.IdSptpd equals p.IdSptpd
                        join w in _context.DataSubjek on p.IdSubjek equals w.IdSubjek
                        join u in _context.DataUsaha on p.IdUsaha equals u.IdUsaha
                        join c in _context.Coa on p.IdCoa equals c.IdCoa
                        select new
                        {
                            nama = w.Nama,
                            usaha = u.NamaUsaha,
                            nomor = s.NoStpd,
                            masa = string.Format("{0:MMM yyyy}", p.MasaPajak1),
                            tanggal = string.Format("{0:dd MMMM yyyy}", s.Tanggal),
                            terhutang = s.Terhutang,
                            setor = s.KreditPajak,
                            uraian = c.Uraian,
                            status = s.Keterangan == 0 ? "Belum Bayar" : s.Keterangan == 1 ? "Lunas" : "-"
                        }).ToList();
            return Json(data);
        }
    }
}