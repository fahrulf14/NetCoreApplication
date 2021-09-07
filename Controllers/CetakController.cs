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
using Newtonsoft.Json;
using SIP.Models;
using SIP.ViewModels;

namespace SIP.Controllers
{
    public class CetakController : Controller
    {
        private readonly DB_NewContext _context;
        private readonly string _pemda = "Dogiyai";

        public CetakController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "Fiskal" })]
        public async Task<IActionResult> Fiskal(Guid id)
        {
            var fiskal = await _context.Fiskal
                .Include(d => d.IdSubjekNavigation).ThenInclude(d => d.IndKecamatan)
                .Include(d => d.IdSubjekNavigation).ThenInclude(d => d.IndKelurahan)
                .FirstOrDefaultAsync(d => d.IdFiskal == id);

            ViewBag.Title = fiskal.NoFiskal;

            var ttd = _context.TandaTangan.Include(d => d.IdPegawaiNavigation).ThenInclude(d => d.IdJabatanNavigation).FirstOrDefault(d => d.IdDokumen == 18);
            ViewBag.TTD = ttd.IdPegawaiNavigation.Nama;
            ViewBag.NIP = ttd.IdPegawaiNavigation.Nip;
            ViewBag.Jabatan = ttd.IdPegawaiNavigation.IdJabatanNavigation.Jabatan;

            var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() +".cshtml";

            return View(cetak, fiskal);
        }

        [Auth(new string[] { "Developers", "Laporan" })]
        public ActionResult LRACetak(string id, DateTime? tanggal1, DateTime? tanggal2)
        {
            if (tanggal1 != null && tanggal2 != null)
            {
                var Lra = _context.Lra.Include(d => d.IdCoaNavigation).Include(d => d.TransaksiLra).Where(d => d.Tahun == tanggal1.Value.Year).OrderBy(d => d.IdCoa).ToList();
                ViewBag.T1 = tanggal1;
                ViewBag.T2 = tanggal2;
                ViewBag.Bulan = string.Format("{0:MMMM}", tanggal1).ToUpper();
                ViewBag.Jenis = id;
                ViewBag.Coa = _context.Coa.ToList();

                ViewBag.TotalAnggaran = Lra.Sum(d => d.Afinal) ?? 0;
                ViewBag.TotalRealisasi = Lra.Sum(d => d.TransaksiLra.Where(e => e.Tanggal >= tanggal1 && e.Tanggal <= tanggal2).Sum(e => e.Jumlah) ?? 0);
                ViewBag.TotalPersen = 0;
                if (ViewBag.TotalRealisasi != 0 && ViewBag.TotalAnggaran != 0)
                {
                    ViewBag.TotalPersen = (double)ViewBag.TotalRealisasi / (double)ViewBag.TotalAnggaran * 100;
                }

                var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

                return View(cetak, Lra);
            }
            else
            {
                return RedirectToAction("Periode", "Lra");
            }
        }

        [Auth(new string[] { "Developers", "Laporan" })]
        public ActionResult LRACetakJenis(string id, DateTime? tanggal1, DateTime? tanggal2)
        {
            if (tanggal1 != null && tanggal2 != null)
            {
                var Lra = _context.Lra.Include(d => d.IdCoaNavigation).Include(d => d.TransaksiLra).Where(d => d.Tahun == tanggal1.Value.Year).OrderBy(d => d.IdCoa).ToList();
                ViewBag.T1 = tanggal1;
                ViewBag.T2 = tanggal2;
                ViewBag.Bulan = string.Format("{0:MMMM}", tanggal1).ToUpper();
                ViewBag.Jenis = id;
                ViewBag.Coa = _context.Coa.ToList();

                ViewBag.TotalAnggaran = Lra.Sum(d => d.Afinal) ?? 0;
                ViewBag.TotalRealisasi = Lra.Sum(d => d.TransaksiLra.Where(e => e.Tanggal >= tanggal1 && e.Tanggal <= tanggal2).Sum(e => e.Jumlah) ?? 0);
                ViewBag.TotalPersen = 0;
                if (ViewBag.TotalRealisasi != 0 && ViewBag.TotalAnggaran != 0)
                {
                    ViewBag.TotalPersen = (double)ViewBag.TotalRealisasi / (double)ViewBag.TotalAnggaran * 100;
                }

                var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

                return View(cetak, Lra);
            }
            else
            {
                return RedirectToAction("Periode", "Lra");
            }
        }

        [Auth(new string[] { "Developers", "Laporan" })]
        public ActionResult LRACetakSubJenis(string id, DateTime? tanggal1, DateTime? tanggal2)
        {
            if (tanggal1 != null && tanggal2 != null)
            {
                var Lra = _context.Lra.Include(d => d.IdCoaNavigation).Include(d => d.TransaksiLra).Where(d => d.Tahun == tanggal1.Value.Year).OrderBy(d => d.IdCoa).ToList();
                ViewBag.T1 = tanggal1;
                ViewBag.T2 = tanggal2;
                ViewBag.Bulan = string.Format("{0:MMMM}", tanggal1).ToUpper();
                ViewBag.Jenis = id;
                ViewBag.Coa = _context.Coa.ToList();

                ViewBag.TotalAnggaran = Lra.Sum(d => d.Afinal) ?? 0;
                ViewBag.TotalRealisasi = Lra.Sum(d => d.TransaksiLra.Where(e => e.Tanggal >= tanggal1 && e.Tanggal <= tanggal2).Sum(e => e.Jumlah) ?? 0);
                ViewBag.TotalPersen = 0;
                if (ViewBag.TotalRealisasi != 0 && ViewBag.TotalAnggaran != 0)
                {
                    ViewBag.TotalPersen = (double)ViewBag.TotalRealisasi / (double)ViewBag.TotalAnggaran * 100;
                }

                var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

                return View(cetak, Lra);
            }
            else
            {
                return RedirectToAction("Periode", "Lra");
            }
        }

        [Auth(new string[] { "Developers", "Pendataan" })]
        public async Task<IActionResult> PajakHiburan(Guid? id)
        {
            var pajak = await _context.PHiburan
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdTarifNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdSubjekNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdUsahaNavigation)
                .FirstOrDefaultAsync(d => d.IdSptpd == id);

            if (pajak.IdSptpdNavigation.Jabatan)
            {
                ViewBag.Jenis = "NOTA JABATAN";
                ViewBag.Dok = "Nota Jabatan";
            }
            else
            {
                ViewBag.Jenis = "SURAT PEMBERITAHUAN PAJAK DAERAH (SPTPD)";
                ViewBag.Dok = "SPTPD";
            }

            ViewBag.Title = pajak.IdSptpdNavigation.NoSptpd;

            var ttd = _context.TandaTangan.Include(d => d.IdPegawaiNavigation).FirstOrDefault(d => d.IdDokumen == 1);
            ViewBag.TTD = ttd.IdPegawaiNavigation.Nama;
            ViewBag.NIP = ttd.IdPegawaiNavigation.Nip;

            var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(cetak, pajak);
        }

        [Auth(new string[] { "Developers", "Pendataan" })]
        public async Task<IActionResult> PajakHotel(Guid id)
        {
            var pajak = await _context.PHotel
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdTarifNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdSubjekNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdUsahaNavigation)
                .FirstOrDefaultAsync(d => d.IdSptpd == id);

            if (pajak.IdSptpdNavigation.Jabatan)
            {
                ViewBag.Jenis = "NOTA JABATAN";
                ViewBag.Dok = "Nota Jabatan";
            }
            else
            {
                ViewBag.Jenis = "SURAT PEMBERITAHUAN PAJAK DAERAH (SPTPD)";
                ViewBag.Dok = "SPTPD";
            }
            var kamar = (from a in _context.PHotelKm
                         where a.IdHotel == pajak.IdHotel
                         select new
                         {
                             sumkamar = a.Jumlah * a.Tarif ?? 0
                         }).ToList();
            ViewBag.KT = ((int)kamar.Sum(d => d.sumkamar));

            var detail = _context.PHotelDt.Include(d => d.IdRefNavigation).Where(d => d.IdHotel == pajak.IdHotel).ToList();
            ViewBag.MT = detail.Where(d => d.IdRefNavigation.Jenis == "Layanan Restoran").Select(d => d.Jumlah).Sum();
            ViewBag.FM = detail.Where(d => d.IdRefNavigation.Jenis == "Layanan Hotel").Select(d => d.Jumlah).Sum();
            ViewBag.F1 = detail.Where(d => d.IdRefNavigation.Uraian.Contains("Fitnes")).Select(d => d.Jumlah).Sum();
            ViewBag.F2 = detail.Where(d => d.IdRefNavigation.Uraian.Contains("Health")).Select(d => d.Jumlah).Sum();
            ViewBag.F3 = detail.Where(d => d.IdRefNavigation.Uraian.Contains("Kolam")).Select(d => d.Jumlah).Sum();
            ViewBag.F4 = detail.Where(d => d.IdRefNavigation.Uraian.Contains("Tenis")).Select(d => d.Jumlah).Sum();
            ViewBag.F5 = detail.Where(d => d.IdRefNavigation.Uraian.Contains("Klub")).Select(d => d.Jumlah).Sum();
            ViewBag.F6 = detail.Where(d => d.IdRefNavigation.Uraian.Contains("Diskotik")).Select(d => d.Jumlah).Sum();
            ViewBag.F7 = detail.Where(d => d.IdRefNavigation.Uraian.Contains("Kafe")).Select(d => d.Jumlah).Sum();
            ViewBag.F8 = detail.Where(d => d.IdRefNavigation.Uraian.Contains("Spa")).Select(d => d.Jumlah).Sum();
            ViewBag.PT = detail.Where(d => d.IdRefNavigation.Jenis == "Layanan Lain").Select(d => d.Jumlah).Sum();
            ViewBag.P1 = detail.Where(d => d.IdRefNavigation.Uraian.Contains("Telepon")).Select(d => d.Jumlah).Sum();
            ViewBag.P2 = detail.Where(d => d.IdRefNavigation.Uraian.Contains("Fax")).Select(d => d.Jumlah).Sum();
            ViewBag.P3 = detail.Where(d => d.IdRefNavigation.Uraian.Contains("Internet")).Select(d => d.Jumlah).Sum();
            ViewBag.P4 = detail.Where(d => d.IdRefNavigation.Uraian.Contains("Photocopy")).Select(d => d.Jumlah).Sum();
            ViewBag.P5 = detail.Where(d => d.IdRefNavigation.Uraian.Contains("Laundry")).Select(d => d.Jumlah).Sum();
            ViewBag.P6 = detail.Where(d => d.IdRefNavigation.Uraian.Contains("Taxi")).Select(d => d.Jumlah).Sum();
            ViewBag.P7 = detail.Where(d => d.IdRefNavigation.Uraian.Contains("Service")).Select(d => d.Jumlah).Sum();

            ViewBag.Title = pajak.IdSptpdNavigation.NoSptpd;

            var ttd = _context.TandaTangan.Include(d => d.IdPegawaiNavigation).FirstOrDefault(d => d.IdDokumen == 1);
            ViewBag.TTD = ttd.IdPegawaiNavigation.Nama;
            ViewBag.NIP = ttd.IdPegawaiNavigation.Nip;

            var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(cetak, pajak);
        }

        [Auth(new string[] { "Developers", "Pendataan" })]
        public async Task<IActionResult> PajakMineral(Guid? id)
        {
            var pajak = await _context.PMineral
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdTarifNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdSubjekNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdUsahaNavigation)
                .FirstOrDefaultAsync(d => d.IdSptpd == id);

            if (pajak.IdSptpdNavigation.Jabatan)
            {
                ViewBag.Jenis = "NOTA JABATAN";
                ViewBag.Dok = "Nota Jabatan";
            }
            else
            {
                ViewBag.Jenis = "SURAT PEMBERITAHUAN PAJAK DAERAH (SPTPD)";
                ViewBag.Dok = "SPTPD";
            }

            ViewBag.Title = pajak.IdSptpdNavigation.NoSptpd;

            var ttd = _context.TandaTangan.Include(d => d.IdPegawaiNavigation).FirstOrDefault(d => d.IdDokumen == 1);
            ViewBag.TTD = ttd.IdPegawaiNavigation.Nama;
            ViewBag.NIP = ttd.IdPegawaiNavigation.Nip;

            var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(cetak, pajak);
        }

        [Auth(new string[] { "Developers", "Pendataan" })]
        public async Task<IActionResult> PajakParkir(Guid? id)
        {
            var pajak = await _context.PParkir
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdTarifNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdSubjekNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdUsahaNavigation)
                .FirstOrDefaultAsync(d => d.IdSptpd == id);

            if (pajak.IdSptpdNavigation.Jabatan)
            {
                ViewBag.Jenis = "NOTA JABATAN";
                ViewBag.Dok = "Nota Jabatan";
            }
            else
            {
                ViewBag.Jenis = "SURAT PEMBERITAHUAN PAJAK DAERAH (SPTPD)";
                ViewBag.Dok = "SPTPD";
            }

            ViewBag.Title = pajak.IdSptpdNavigation.NoSptpd;

            var ttd = _context.TandaTangan.Include(d => d.IdPegawaiNavigation).FirstOrDefault(d => d.IdDokumen == 1);
            ViewBag.TTD = ttd.IdPegawaiNavigation.Nama;
            ViewBag.NIP = ttd.IdPegawaiNavigation.Nip;

            var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(cetak, pajak);
        }

        [Auth(new string[] { "Developers", "Pendataan" })]
        public async Task<IActionResult> PajakPenerangan(Guid? id)
        {
            var pajak = await _context.PPenerangan
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdTarifNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdSubjekNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdUsahaNavigation)
                .FirstOrDefaultAsync(d => d.IdSptpd == id);

            if (pajak.IdSptpdNavigation.Jabatan)
            {
                ViewBag.Jenis = "NOTA JABATAN";
                ViewBag.Dok = "Nota Jabatan";
            }
            else
            {
                ViewBag.Jenis = "SURAT PEMBERITAHUAN PAJAK DAERAH (SPTPD)";
                ViewBag.Dok = "SPTPD";
            }

            ViewBag.Title = pajak.IdSptpdNavigation.NoSptpd;

            var ttd = _context.TandaTangan.Include(d => d.IdPegawaiNavigation).FirstOrDefault(d => d.IdDokumen == 1);
            ViewBag.TTD = ttd.IdPegawaiNavigation.Nama;
            ViewBag.NIP = ttd.IdPegawaiNavigation.Nip;

            var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(cetak, pajak);
        }

        [Auth(new string[] { "Developers", "Pendataan" })]
        public async Task<IActionResult> PajakRestoran(Guid? id)
        {
            var pajak = await _context.PRestoran
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdTarifNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdSubjekNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdUsahaNavigation)
                .FirstOrDefaultAsync(d => d.IdSptpd == id);

            if (pajak.IdSptpdNavigation.Jabatan)
            {
                ViewBag.Jenis = "NOTA JABATAN";
                ViewBag.Dok = "Nota Jabatan";
            }
            else
            {
                ViewBag.Jenis = "SURAT PEMBERITAHUAN PAJAK DAERAH (SPTPD)";
                ViewBag.Dok = "SPTPD";
            }

            var detail = _context.PRestoranDt.Include(d => d.IdRefNavigation).Where(d => d.IdRestoran == pajak.IdRestoran).ToList();
            ViewBag.D1 = detail.Where(d => d.IdRefNavigation.Uraian == "Penjualan Makanan & Minuman").Sum(d => d.Jumlah);
            ViewBag.D2 = detail.Where(d => d.IdRefNavigation.Uraian == "Service Charge").Sum(d => d.Jumlah);
            ViewBag.D3 = detail.Where(d => d.IdRefNavigation.Uraian == "Lainnya").Sum(d => d.Jumlah);

            ViewBag.Title = pajak.IdSptpdNavigation.NoSptpd;

            var ttd = _context.TandaTangan.Include(d => d.IdPegawaiNavigation).FirstOrDefault(d => d.IdDokumen == 1);
            ViewBag.TTD = ttd.IdPegawaiNavigation.Nama;
            ViewBag.NIP = ttd.IdPegawaiNavigation.Nip;

            var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(cetak, pajak);
        }

        [Auth(new string[] { "Developers", "Pendataan" })]
        public async Task<IActionResult> PajakSarangWalet(Guid? id)
        {
            var pajak = await _context.PWalet
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdTarifNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdSubjekNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdUsahaNavigation)
                .FirstOrDefaultAsync(d => d.IdSptpd == id);

            if (pajak.IdSptpdNavigation.Jabatan)
            {
                ViewBag.Jenis = "NOTA JABATAN";
                ViewBag.Dok = "Nota Jabatan";
            }
            else
            {
                ViewBag.Jenis = "SURAT PEMBERITAHUAN PAJAK DAERAH (SPTPD)";
                ViewBag.Dok = "SPTPD";
            }

            ViewBag.Title = pajak.IdSptpdNavigation.NoSptpd;

            var ttd = _context.TandaTangan.Include(d => d.IdPegawaiNavigation).FirstOrDefault(d => d.IdDokumen == 1);
            ViewBag.TTD = ttd.IdPegawaiNavigation.Nama;
            ViewBag.NIP = ttd.IdPegawaiNavigation.Nip;

            var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(cetak, pajak);
        }

        [Auth(new string[] { "Developers", "Pendaftaran" })]
        public async Task<IActionResult> NPWPD(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var dataSubjek = await _context.DataSubjek.Include(d => d.IndKabKota).Include(d => d.IndKecamatan).Include(d => d.IndKelurahan).FirstOrDefaultAsync(d => d.IdSubjek == id);
            if (dataSubjek == null)
            {
                return NotFound();
            }

            ViewBag.Title = dataSubjek.Npwpd ?? dataSubjek.Npwrd;

            var opd = HttpContext.Session.GetString("Opd");
            TextInfo textInfo = new CultureInfo("id-ID", false).TextInfo;
            ViewBag.Opd = textInfo.ToTitleCase(opd.ToLower());

            var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(cetak, dataSubjek);
        }

        [Auth(new string[] { "Developers", "Laporan" })]
        public ActionResult Penerimaan(string id, DateTime? tanggal1, DateTime? tanggal2, bool rincian)
        {
            ViewBag.Saldo = _context.SaldoAnggaran.FirstOrDefault().Saldo;

            var sspd = _context.Sspd
                .Include(s => s.IdSetoranNavigation)
                .Include(s => s.IdCoaNavigation)
                .Include(s => s.StsDt)
                .Where(s => s.FlagBayar && s.TanggalBayar >= tanggal1 && s.TanggalBayar <= tanggal2)
                .Select(x => new BukuPenerimaan
                {
                    Id = x.IdSspd,
                    Tanggal = x.TanggalBayar,
                    Nomor = x.NoSspd,
                    Pembayaran = x.IdSetoranNavigation.Jenis,
                    Kode = x.IdCoa.Substring(0, 1) + "." + x.IdCoa.Substring(1, 1) + "." + x.IdCoa.Substring(2, 1) + "." + x.IdCoa.Substring(3, 2) + "." + x.IdCoa.Substring(5, 2),
                    Uraian = x.IdCoaNavigation.Uraian,
                    Jumlah = x.JumlahSetoran,
                    Jenis = "SSPD",
                    Valid = x.StsDt.FirstOrDefault().IdStsNavigation.TanggalValidasi,
                    Sts = x.StsDt.FirstOrDefault().IdStsNavigation.NoSts,
                    Status = x.StatusSetor
                }).ToList();

            var ssrd = _context.Ssrd
                .Include(s => s.IdCoaNavigation)
                .Include(s => s.IdSetoranNavigation)
                .Include(s => s.StsDt)
                .Where(s => s.FlagBayar && s.TanggalBayar >= tanggal1 && s.TanggalBayar <= tanggal2)
                .Select(x => new BukuPenerimaan
                {
                    Id = x.IdSsrd,
                    Tanggal = x.TanggalBayar,
                    Nomor = x.NoSsrd,
                    Pembayaran = x.IdSetoranNavigation.Jenis,
                    Kode = x.IdCoa.Substring(0, 1) + "." + x.IdCoa.Substring(1, 1) + "." + x.IdCoa.Substring(2, 1) + "." + x.IdCoa.Substring(3, 2) + "." + x.IdCoa.Substring(5, 2),
                    Uraian = x.IdCoaNavigation.Uraian,
                    Jumlah = x.JumlahSetoran,
                    Jenis = "SSRD",
                    Valid = x.StsDt.FirstOrDefault().IdStsNavigation.TanggalValidasi,
                    Sts = x.StsDt.FirstOrDefault().IdStsNavigation.NoSts,
                    Status = x.StatusSetor
                }).ToList();

            var ssrdr = _context.Ssrdr
                .Include(s => s.IdCoaNavigation)
                .Include(s => s.IdSetoranNavigation)
                .Include(s => s.StsDt)
                .Where(s => s.TanggalValidasi >= tanggal1 && s.TanggalValidasi <= tanggal2)
                .Select(x => new BukuPenerimaan
                {
                    Id = x.IdSsrdr,
                    Tanggal = x.TanggalValidasi,
                    Nomor = x.NoSsrdr,
                    Pembayaran = x.IdSetoranNavigation.Jenis,
                    Kode = x.IdCoa.Substring(0, 1) + "." + x.IdCoa.Substring(1, 1) + "." + x.IdCoa.Substring(2, 1) + "." + x.IdCoa.Substring(3, 2) + "." + x.IdCoa.Substring(5, 2),
                    Uraian = x.IdCoaNavigation.Uraian,
                    Jumlah = x.Total,
                    Jenis = "SSRDR",
                    Valid = x.StsDt.FirstOrDefault().IdStsNavigation.TanggalValidasi,
                    Sts = x.StsDt.FirstOrDefault().IdStsNavigation.NoSts,
                    Status = x.StatusSetor
                }).ToList();

            var stts = _context.Stts
                .Include(s => s.IdSpptNavigation).ThenInclude(s => s.IdSpopNavigation)
                .Include(s => s.IdSetoranNavigation)
                .Include(s => s.StsDt)
                .Where(s => s.FlagValidasi && s.TanggalBayar >= tanggal1 && s.TanggalBayar <= tanggal2)
                .Select(x => new BukuPenerimaan
                {
                    Id = x.IdStts,
                    Tanggal = x.TanggalBayar,
                    Nomor = x.IdSpptNavigation.IdSpopNavigation.Nop,
                    Pembayaran = x.IdSetoranNavigation.Jenis,
                    Kode = "4.1.1.12.01",
                    Uraian = "Pajak Bumi dan Bangunan",
                    Jumlah = x.TotalBayar,
                    Jenis = "STTS",
                    Valid = x.StsDt.FirstOrDefault().IdStsNavigation.TanggalValidasi,
                    Sts = x.StsDt.FirstOrDefault().IdStsNavigation.NoSts,
                    Status = x.StatusSetor
                }).ToList();

            var result = sspd.Union(ssrd).Union(ssrdr).Union(stts);

            List<BukuPenerimaan> ListDokumen = new List<BukuPenerimaan>();

            foreach (var item in result)
            {
                ListDokumen.Add(new BukuPenerimaan
                {
                    Id = item.Id,
                    Tanggal = item.Tanggal,
                    Nomor = item.Nomor,
                    Pembayaran = item.Pembayaran,
                    Kode = item.Kode,
                    Uraian = item.Uraian,
                    Jumlah = item.Jumlah ?? 0,
                    Jenis = item.Jenis,
                    Valid = item.Valid,
                    Sts = item.Sts,
                    Status = item.Status
                });
            }

            ViewBag.Title = "Laporan Penerimaan Per Objek";
            ViewBag.T1 = tanggal1;
            ViewBag.T2 = tanggal2;
            ViewBag.Jenis = id;

            string cetak;
            if (rincian)
            {
                var ttd = _context.TandaTangan.Include(d => d.IdPegawaiNavigation).ThenInclude(d => d.IdJabatanNavigation).FirstOrDefault(d => d.IdDokumen == 15);
                ViewBag.TTD = ttd.IdPegawaiNavigation.Nama;
                ViewBag.NIP = ttd.IdPegawaiNavigation.Nip;
                ViewBag.Jabatan = ttd.IdPegawaiNavigation.IdJabatanNavigation.Jabatan;
                ViewBag.LRA = _context.Lra.Where(d => d.Tahun == tanggal1.Value.Year).ToList();

                cetak = "~/Views/Cetak_" + _pemda + "/" + "PenerimaanObjek" + ".cshtml";
            }
            else
            {
                cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";
            }

            return View(cetak, ListDokumen);
        }

        [Auth(new string[] { "Developers", "Laporan" })]
        public ActionResult RekapPenerimaan(DateTime? tanggal1, DateTime? tanggal2)
        {
            var transaksi = _context.TransaksiLra.Include(d => d.IdLraNavigation).ThenInclude(d => d.IdCoaNavigation)
                .Where(d => d.Tanggal >= tanggal1 && d.Tanggal <= tanggal2).OrderBy(d => d.Tanggal).ToList();
            ViewBag.Title = "Rekapitulasi Penerimaan Harian";

            ViewBag.T1 = tanggal1;
            ViewBag.T2 = tanggal2;

            var ttd = _context.TandaTangan.Include(d => d.IdPegawaiNavigation).ThenInclude(d => d.IdJabatanNavigation).FirstOrDefault(d => d.IdDokumen == 15);
            ViewBag.TTD = ttd.IdPegawaiNavigation.Nama;
            ViewBag.NIP = ttd.IdPegawaiNavigation.Nip;
            ViewBag.Jabatan = ttd.IdPegawaiNavigation.IdJabatanNavigation.Jabatan;

            var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(cetak, transaksi);
        }

        [Auth(new string[] { "Developers", "Laporan" })]
        public ActionResult RegisterSTS(DateTime? tanggal1, DateTime? tanggal2)
        {
            var sts = _context.Sts.Include(s => s.StsDt).Where(s => s.StsDt != null && s.Tanggal >= tanggal1 && s.Tanggal <= tanggal2).ToList();

            if (sts.Count() == 0)
            {
                TempData["status"] = "RegStsNull";
                return RedirectToAction("Register", "STS");
            }

            List<RegisterSTS> registerSTS = new List<RegisterSTS>();

            var tahun = tanggal1.Value.Year;

            ViewBag.Title = "Register STS Tahun " + tahun;
            ViewBag.Tahun = tahun;
            ViewBag.Tanggal1 = tanggal1;
            ViewBag.Tanggal2 = tanggal2;

            foreach (var item in sts)
            {
                if (item.Keterangan == "SSPD")
                {
                    //var sspd = _context.StsDt
                    //.Include(s => s.IdSspdNavigation).ThenInclude(s => s.IdCoaNavigation)
                    //.Include(s => s.IdSspdNavigation).ThenInclude(s => s.IdSubjekNavigation)
                    //.Include(s => s.IdSspdNavigation).ThenInclude(s => s.IdUsahaNavigation)
                    //.Include(s => s.IdSspdNavigation).ThenInclude(s => s.IdSptpdNavigation)
                    //.Include(s => s.IdSspdNavigation).ThenInclude(s => s.IdSkpdNavigation).ThenInclude(s => s.IdSptpdNavigation)
                    //.Include(s => s.IdSspdNavigation).ThenInclude(s => s.IdSkpdkbNavigation).ThenInclude(s => s.IdSptpdNavigation)
                    //.Include(s => s.IdSspdNavigation).ThenInclude(s => s.IdSkpdkbtNavigation).ThenInclude(s => s.IdSptpdNavigation)
                    //.Include(s => s.IdSspdNavigation).ThenInclude(s => s.IdStpdNavigation).ThenInclude(s => s.IdSkpdNavigation).ThenInclude(s => s.IdSptpdNavigation)
                    //.Where(s => s.IdSts == item.IdSts).ToList();

                    var sspd = _context.StsDt.Include(s => s.IdSspdNavigation).Where(s => s.IdSts == item.IdSts).ToList();

                    foreach (var dt in sspd)
                    {
                        var data = _context.Sspd
                                .Include(s => s.IdCoaNavigation)
                                .Include(s => s.IdSubjekNavigation)
                                .Include(s => s.IdUsahaNavigation)
                                .FirstOrDefault(s => s.IdSspd == dt.IdSspd);
                        if (dt.IdSspdNavigation.IdSptpd != null)
                        {
                            var surat = _context.Sspd
                                .Include(s => s.IdSptpdNavigation)
                                .FirstOrDefault(s => s.IdSspd == data.IdSspd);
                            registerSTS.Add(new RegisterSTS
                            {
                                Id = item.IdSts,
                                Tanggal = item.Tanggal,
                                NomorSetoran = data.NoSspd,
                                NomorSurat = surat.IdSptpdNavigation.NoSptpd,
                                Uraian = data.IdCoaNavigation.Uraian,
                                NamaWp = data.IdSubjekNavigation.Nama,
                                NamaUsaha = data.IdUsahaNavigation.NamaUsaha,
                                Jumlah = surat.IdSptpdNavigation.Terhutang,
                                Jenis = item.Keterangan,
                                Valid = item.TanggalValidasi,
                                Sts = item.NoSts,
                                Keterangan = "Masa Pajak " + string.Format("{0:dd-MM-yyyy}", surat.IdSptpdNavigation.MasaPajak1),
                            });
                        }
                        else if (dt.IdSspdNavigation.IdSkpd != null)
                        {
                            var surat = _context.Sspd
                                .Include(s => s.IdSkpdNavigation).ThenInclude(s => s.IdSptpdNavigation)
                                .FirstOrDefault(s => s.IdSspd == data.IdSspd);
                            registerSTS.Add(new RegisterSTS
                            {
                                Id = item.IdSts,
                                Tanggal = item.Tanggal,
                                NomorSetoran = data.NoSspd,
                                NomorSurat = surat.IdSkpdNavigation.NoSkpd,
                                Uraian = data.IdCoaNavigation.Uraian,
                                NamaWp = data.IdSubjekNavigation.Nama,
                                NamaUsaha = data.IdUsahaNavigation.NamaUsaha,
                                Jumlah = surat.IdSkpdNavigation.Terhutang,
                                Jenis = item.Keterangan,
                                Valid = item.TanggalValidasi,
                                Sts = item.NoSts,
                                Keterangan = "Masa Pajak " + string.Format("{0:dd-MM-yyyy}", surat.IdSkpdNavigation.IdSptpdNavigation.MasaPajak1),
                            });
                        }
                        else if (dt.IdSspdNavigation.IdSkpdkb != null)
                        {
                            var surat = _context.Sspd
                                .Include(s => s.IdSkpdkbNavigation).ThenInclude(s => s.IdSptpdNavigation)
                                .FirstOrDefault(s => s.IdSspd == data.IdSspd);
                            registerSTS.Add(new RegisterSTS
                            {
                                Id = item.IdSts,
                                Tanggal = item.Tanggal,
                                NomorSetoran = data.NoSspd,
                                NomorSurat = surat.IdSkpdkbNavigation.NoSkpdkb,
                                Uraian = data.IdCoaNavigation.Uraian,
                                NamaWp = data.IdSubjekNavigation.Nama,
                                NamaUsaha = data.IdUsahaNavigation.NamaUsaha,
                                Jumlah = surat.IdSkpdkbNavigation.Terhutang,
                                Jenis = item.Keterangan,
                                Valid = item.TanggalValidasi,
                                Sts = item.NoSts,
                                Keterangan = "Masa Pajak " + string.Format("{0:dd-MM-yyyy}", surat.IdSkpdkbNavigation.IdSptpdNavigation.MasaPajak1),
                            });
                        }
                        else if (dt.IdSspdNavigation.IdSkpdkbt != null)
                        {
                            var surat = _context.Sspd
                                .Include(s => s.IdSkpdkbtNavigation).ThenInclude(s => s.IdSptpdNavigation)
                                .FirstOrDefault(s => s.IdSspd == data.IdSspd);
                            registerSTS.Add(new RegisterSTS
                            {
                                Id = item.IdSts,
                                Tanggal = item.Tanggal,
                                NomorSetoran = data.NoSspd,
                                NomorSurat = surat.IdSkpdkbtNavigation.NoSkpdkbt,
                                Uraian = data.IdCoaNavigation.Uraian,
                                NamaWp = data.IdSubjekNavigation.Nama,
                                NamaUsaha = data.IdUsahaNavigation.NamaUsaha,
                                Jumlah = surat.IdSkpdkbtNavigation.Terhutang,
                                Jenis = item.Keterangan,
                                Valid = item.TanggalValidasi,
                                Sts = item.NoSts,
                                Keterangan = "Masa Pajak " + string.Format("{0:dd-MM-yyyy}", surat.IdSkpdkbtNavigation.IdSptpdNavigation.MasaPajak1),
                            });
                        }
                        else
                        {
                            var surat = _context.Sspd
                                .Include(s => s.IdStpdNavigation).ThenInclude(s => s.IdSkpdNavigation).ThenInclude(s => s.IdSptpdNavigation)
                                .FirstOrDefault(s => s.IdSspd == data.IdSspd);
                            registerSTS.Add(new RegisterSTS
                            {
                                Id = item.IdSts,
                                Tanggal = item.Tanggal,
                                NomorSetoran = data.NoSspd,
                                NomorSurat = surat.IdStpdNavigation.NoStpd,
                                Uraian = data.IdCoaNavigation.Uraian,
                                NamaWp = data.IdSubjekNavigation.Nama,
                                NamaUsaha = data.IdUsahaNavigation.NamaUsaha,
                                Jumlah = surat.IdStpdNavigation.Terhutang,
                                Jenis = item.Keterangan,
                                Valid = item.TanggalValidasi,
                                Sts = item.NoSts,
                                Keterangan = "Masa Pajak " + string.Format("{0:dd-MM-yyyy}", surat.IdStpdNavigation.IdSkpdNavigation.IdSptpdNavigation.MasaPajak1),
                            });
                        }
                    }
                }
                else if (item.Keterangan == "SSRD")
                {
                    var ssrd = _context.StsDt
                        .Include(s => s.IdSsrdNavigation).ThenInclude(s => s.IdCoaNavigation)
                        .Include(s => s.IdSsrdNavigation).ThenInclude(s => s.IdSubjekNavigation)
                        .Include(s => s.IdSsrdNavigation).ThenInclude(s => s.IdSkrdNavigation)
                        .Where(s => s.IdSts == item.IdSts).ToList();

                    foreach (var dt in ssrd)
                    {
                        registerSTS.Add(new RegisterSTS
                        {
                            Id = item.IdSts,
                            Tanggal = item.Tanggal,
                            NomorSetoran = dt.IdSsrdNavigation.NoSsrd,
                            NomorSurat = dt.IdSsrdNavigation.IdSkrdNavigation.NoSkrd,
                            Uraian = dt.IdSsrdNavigation.IdCoaNavigation.Uraian,
                            NamaWp = dt.IdSsrdNavigation.IdSubjekNavigation.Nama,
                            NamaUsaha = "-",
                            Jumlah = dt.IdSsrdNavigation.IdSkrdNavigation.Terhutang,
                            Jenis = item.Keterangan,
                            Valid = item.TanggalValidasi,
                            Sts = item.NoSts,
                            Keterangan = "Masa Retribusi " + string.Format("{0:dd-MM-yyyy}", dt.IdSsrdNavigation.IdSkrdNavigation.MasaRetribusi1) + " s.d " + string.Format("{0:dd-MM-yyyy}", dt.IdSsrdNavigation.IdSkrdNavigation.MasaRetribusi2)
                        });
                    }
                }
                else if (item.Keterangan == "STTS")
                {
                    var stts = _context.StsDt
                        .Include(s => s.IdSttsNavigation).ThenInclude(s => s.IdSpptNavigation).ThenInclude(s => s.IdSpopNavigation).ThenInclude(s => s.IdSubjekNavigation)
                        .Include(s => s.IdSttsNavigation).ThenInclude(s => s.IdSpptNavigation).ThenInclude(s => s.IdSpopNavigation).ThenInclude(s => s.IdCoaNavigation)
                        .Where(s => s.IdSts == item.IdSts).ToList();

                    foreach (var dt in stts)
                    {
                        registerSTS.Add(new RegisterSTS
                        {
                            Id = item.IdSts,
                            Tanggal = item.Tanggal,
                            NomorSetoran = dt.IdSttsNavigation.IdSpptNavigation.IdSpopNavigation.Nop + "/" + dt.IdSttsNavigation.IdSpptNavigation.Tahun,
                            NomorSurat = dt.IdSttsNavigation.IdSpptNavigation.IdSpopNavigation.Nop,
                            Uraian = dt.IdSttsNavigation.IdSpptNavigation.IdSpopNavigation.IdCoaNavigation.Uraian,
                            NamaWp = dt.IdSttsNavigation.IdSpptNavigation.IdSpopNavigation.IdSubjekNavigation.Nama,
                            NamaUsaha = "-",
                            Jumlah = dt.IdSttsNavigation.TotalBayar,
                            Jenis = item.Keterangan,
                            Valid = item.TanggalValidasi,
                            Sts = item.NoSts,
                            Keterangan = "PBB Masa Pajak " + dt.IdSttsNavigation.IdSpptNavigation.Tahun
                        });
                    }
                }
                else if (item.Keterangan == "SSRDR")
                {
                    var ssrdr = _context.StsDt
                        .Include(s => s.IdSsrdrNavigation).ThenInclude(s => s.IdCoaNavigation)
                        .Where(s => s.IdSts == item.IdSts).ToList();

                    foreach (var dt in ssrdr)
                    {
                        registerSTS.Add(new RegisterSTS
                        {
                            Id = item.IdSts,
                            Tanggal = item.Tanggal,
                            NomorSetoran = dt.IdSsrdrNavigation.NoSsrdr,
                            NomorSurat = dt.IdSsrdrNavigation.NoSsrdr,
                            Uraian = dt.IdSsrdrNavigation.IdCoaNavigation.Uraian,
                            NamaWp = "-",
                            NamaUsaha = "-",
                            Jumlah = dt.IdSsrdrNavigation.Total,
                            Jenis = item.Keterangan,
                            Valid = item.TanggalValidasi,
                            Sts = item.NoSts,
                            Keterangan = "Setoran SSRDR Tanggal " + string.Format("{0:dd-MM-yyyy}", dt.IdSsrdrNavigation.Tanggal)
                        });
                    }
                }

                ViewBag.Total = registerSTS.Sum(s => s.Jumlah);

                var pemda = HttpContext.Session.GetString("NamaPemda").Remove(0, 11);
                TextInfo textInfo = new CultureInfo("id-ID", false).TextInfo;
                ViewBag.NamaPemda = textInfo.ToTitleCase(pemda.ToLower());

                var ttd = _context.TandaTangan.Include(d => d.IdPegawaiNavigation).ThenInclude(d => d.IdJabatanNavigation).FirstOrDefault(d => d.IdDokumen == 15);
                ViewBag.TTD = ttd.IdPegawaiNavigation.Nama;
                ViewBag.NIP = ttd.IdPegawaiNavigation.Nip;
                ViewBag.Jabatan = ttd.IdPegawaiNavigation.IdJabatanNavigation.Jabatan;
            }

            var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(cetak, registerSTS);
        }

        [Auth(new string[] { "Developers", "Laporan" })]
        public ActionResult RegisterSPTPD()
        {
            var query = (from c in _context.Coa
                         where (from d in _context.Sptpd where d.FlagKonfirmasi select d.IdCoa).Contains(c.IdCoa)
                         select c).ToList();
            ViewBag.Jenis = query;

            var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(cetak);
        }

        [Auth(new string[] { "Developers", "Laporan" })]
        public ActionResult RegisterSKPD()
        {
            var query = (from c in _context.Coa
                         where (from d in _context.Sptpd where d.FlagKonfirmasi && c.Jenis == "Official Assessment" select d.IdCoa).Contains(c.IdCoa)
                         select c).ToList();
            ViewBag.Jenis = query;

            var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(cetak);
        }

        [Auth(new string[] { "Developers", "Laporan" })]
        public ActionResult RegisterSKPDKB()
        {
            var query = (from c in _context.Coa
                         where (from d in _context.Sptpd where d.FlagKonfirmasi && c.Jenis == "Self Assessment" select d.IdCoa).Contains(c.IdCoa)
                         select c).ToList();
            ViewBag.Jenis = query;

            var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(cetak);
        }

        [Auth(new string[] { "Developers", "Laporan" })]
        public ActionResult RegisterSKPDKBT()
        {
            var query = (from c in _context.Coa
                         where (from d in _context.Sptpd where d.FlagKonfirmasi && c.Jenis == "Self Assessment" select d.IdCoa).Contains(c.IdCoa)
                         select c).ToList();
            ViewBag.Jenis = query;

            var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(cetak);
        }

        [Auth(new string[] { "Developers", "Laporan" })]
        public ActionResult RegisterSTPD()
        {
            var query = (from c in _context.Coa
                         where (from d in _context.Sptpd where d.FlagKonfirmasi && c.Jenis == "Official Assessment" select d.IdCoa).Contains(c.IdCoa)
                         select c).ToList();
            ViewBag.Jenis = query;

            var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(cetak);
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public async Task<IActionResult> SKPD(Guid id)
        {
            var data = await _context.Skpd
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdTarifNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdSubjekNavigation).ThenInclude(d => d.IndKecamatan)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdSubjekNavigation).ThenInclude(d => d.IndKelurahan)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdUsahaNavigation)
                .FirstOrDefaultAsync(m => m.IdSkpd == id);

            if (data.Bunga != 0 || data.Kenaikan != 0)
            {
                ViewBag.Denda = _context.Coa.FirstOrDefault(d => d.IdCoa == data.IdSptpdNavigation.IdCoaNavigation.Denda).Uraian;
            }

            ViewBag.Title = data.NoSkpd;

            ViewBag.Bank = _context.Bank.Include(d => d.IdRefNavigation).Where(d => d.IdSetoran == 1).ToList();

            var opd = HttpContext.Session.GetString("Opd");
            var pemda = HttpContext.Session.GetString("NamaPemda").Remove(0, 11);
            TextInfo textInfo = new CultureInfo("id-ID", false).TextInfo;
            ViewBag.Opd = textInfo.ToTitleCase(opd.ToLower());
            ViewBag.NamaPemda = textInfo.ToTitleCase(pemda.ToLower());

            var title1 = pemda.Replace("KOTA ", "").Replace("KABUPATEN ", "");
            var title2 = HttpContext.Session.GetString("NamaPemda").Replace(title1, "");

            ViewBag.Title1 = title1;
            ViewBag.Title2 = title2;

            var ttd = _context.TandaTangan.Include(d => d.IdPegawaiNavigation).ThenInclude(d => d.IdJabatanNavigation).FirstOrDefault(d => d.IdDokumen == 2);
            ViewBag.TTD = ttd.IdPegawaiNavigation.Nama;
            ViewBag.NIP = ttd.IdPegawaiNavigation.Nip;
            ViewBag.Jabatan = ttd.IdPegawaiNavigation.IdJabatanNavigation.Jabatan;

            var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(cetak, data);
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public async Task<IActionResult> SKPDKB(Guid id)
        {
            var data = await _context.Skpdkb
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdTarifNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdSubjekNavigation).ThenInclude(d => d.IndKecamatan)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdSubjekNavigation).ThenInclude(d => d.IndKelurahan)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdUsahaNavigation)
                .FirstOrDefaultAsync(m => m.IdSkpdkb == id);

            ViewBag.Title = data.NoSkpdkb;

            ViewBag.Bank = _context.Bank.Include(d => d.IdRefNavigation).Where(d => d.IdSetoran == 1).ToList();

            var opd = HttpContext.Session.GetString("Opd");
            var pemda = HttpContext.Session.GetString("NamaPemda").Remove(0, 11);
            TextInfo textInfo = new CultureInfo("id-ID", false).TextInfo;
            ViewBag.Opd = textInfo.ToTitleCase(opd.ToLower());
            ViewBag.NamaPemda = textInfo.ToTitleCase(pemda.ToLower());

            var title1 = pemda.Replace("KOTA ", "").Replace("KABUPATEN ", "");
            var title2 = HttpContext.Session.GetString("NamaPemda").Replace(title1, "");

            ViewBag.Title1 = title1;
            ViewBag.Title2 = title2;

            var ttd = _context.TandaTangan.Include(d => d.IdPegawaiNavigation).ThenInclude(d => d.IdJabatanNavigation).FirstOrDefault(d => d.IdDokumen == 4);
            ViewBag.TTD = ttd.IdPegawaiNavigation.Nama;
            ViewBag.NIP = ttd.IdPegawaiNavigation.Nip;
            ViewBag.Jabatan = ttd.IdPegawaiNavigation.IdJabatanNavigation.Jabatan;

            var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(cetak, data);
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public async Task<IActionResult> SKPDKBT(Guid id)
        {
            var data = await _context.Skpdkbt
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdTarifNavigation)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdSubjekNavigation).ThenInclude(d => d.IndKecamatan)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdSubjekNavigation).ThenInclude(d => d.IndKelurahan)
                .Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdUsahaNavigation)
                .FirstOrDefaultAsync(m => m.IdSkpdkbt == id);

            ViewBag.Title = data.NoSkpdkbt;

            ViewBag.Bank = _context.Bank.Include(d => d.IdRefNavigation).Where(d => d.IdSetoran == 1).ToList();

            var opd = HttpContext.Session.GetString("Opd");
            var pemda = HttpContext.Session.GetString("NamaPemda").Remove(0, 11);
            TextInfo textInfo = new CultureInfo("id-ID", false).TextInfo;
            ViewBag.Opd = textInfo.ToTitleCase(opd.ToLower());
            ViewBag.NamaPemda = textInfo.ToTitleCase(pemda.ToLower());

            var title1 = pemda.Replace("KOTA ", "").Replace("KABUPATEN ", "");
            var title2 = HttpContext.Session.GetString("NamaPemda").Replace(title1, "");

            ViewBag.Title1 = title1;
            ViewBag.Title2 = title2;

            var ttd = _context.TandaTangan.Include(d => d.IdPegawaiNavigation).ThenInclude(d => d.IdJabatanNavigation).FirstOrDefault(d => d.IdDokumen == 5);
            ViewBag.TTD = ttd.IdPegawaiNavigation.Nama;
            ViewBag.NIP = ttd.IdPegawaiNavigation.Nip;
            ViewBag.Jabatan = ttd.IdPegawaiNavigation.IdJabatanNavigation.Jabatan;

            var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(cetak, data);
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public async Task<IActionResult> SKPDN(Guid id)
        {
            var data = await _context.Skpdn
                .Include(s => s.IdSspdNavigation).ThenInclude(s => s.IdSubjekNavigation).ThenInclude(s => s.IndKecamatan)
                .Include(s => s.IdSspdNavigation).ThenInclude(s => s.IdSubjekNavigation).ThenInclude(s => s.IndKelurahan)
                .Include(s => s.IdSspdNavigation).ThenInclude(s => s.IdUsahaNavigation)
                .Include(s => s.IdSspdNavigation).ThenInclude(s => s.IdBankNavigation)
                .Include(s => s.IdCoaNavigation)
                .FirstOrDefaultAsync(m => m.IdSkpdn == id);

            ViewBag.Title = data.NoSkpdn;

            var opd = HttpContext.Session.GetString("Opd");
            var pemda = HttpContext.Session.GetString("NamaPemda").Remove(0, 11);
            TextInfo textInfo = new CultureInfo("id-ID", false).TextInfo;
            ViewBag.Opd = textInfo.ToTitleCase(opd.ToLower());
            ViewBag.NamaPemda = textInfo.ToTitleCase(pemda.ToLower());

            var title1 = pemda.Replace("KOTA ", "").Replace("KABUPATEN ", "");
            var title2 = HttpContext.Session.GetString("NamaPemda").Replace(title1, "");

            ViewBag.Title1 = title1;
            ViewBag.Title2 = title2;

            var ttd = _context.TandaTangan.Include(d => d.IdPegawaiNavigation).ThenInclude(d => d.IdJabatanNavigation).FirstOrDefault(d => d.IdDokumen == 6);
            ViewBag.TTD = ttd.IdPegawaiNavigation.Nama;
            ViewBag.NIP = ttd.IdPegawaiNavigation.Nip;
            ViewBag.Jabatan = ttd.IdPegawaiNavigation.IdJabatanNavigation.Jabatan;

            var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(cetak, data);
        }

        [Auth(new string[] { "Developers", "Retribusi" })]
        public async Task<IActionResult> SKRD(Guid id)
        {
            var data = await _context.Skrd
                .Include(d => d.IdCoaNavigation)
                .Include(d => d.IdSubjekNavigation).ThenInclude(d => d.IndKecamatan)
                .Include(d => d.IdSubjekNavigation).ThenInclude(d => d.IndKelurahan)
                .FirstOrDefaultAsync(m => m.IdSkrd == id);

            ViewBag.Bank = _context.Bank.Include(d => d.IdRefNavigation).Where(d => d.IdSetoran == 1).ToList();

            var opd = HttpContext.Session.GetString("Opd");
            var pemda = HttpContext.Session.GetString("NamaPemda").Remove(0, 11);
            TextInfo textInfo = new CultureInfo("id-ID", false).TextInfo;
            ViewBag.Opd = textInfo.ToTitleCase(opd.ToLower());
            ViewBag.NamaPemda = textInfo.ToTitleCase(pemda.ToLower());

            var title1 = pemda.Replace("KOTA ", "").Replace("KABUPATEN ", "");
            var title2 = HttpContext.Session.GetString("NamaPemda").Replace(title1, "");

            ViewBag.Title1 = title1;
            ViewBag.Title2 = title2;

            ViewBag.Title = data.NoSkrd;
            ViewBag.Detail = _context.SkrdDt.Include(d => d.IdTarifNavigation).Where(d => d.IdSkrd == id).ToList();

            var ttd = _context.TandaTangan.Include(d => d.IdPegawaiNavigation).ThenInclude(d => d.IdJabatanNavigation).FirstOrDefault(d => d.IdDokumen == 8);
            ViewBag.TTD = ttd.IdPegawaiNavigation.Nama;
            ViewBag.NIP = ttd.IdPegawaiNavigation.Nip;
            ViewBag.Jabatan = ttd.IdPegawaiNavigation.IdJabatanNavigation.Jabatan;

            var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(cetak, data);
        }

        [Auth(new string[] { "Developers", "PBB" })]
        public ActionResult SPPT(Guid id)
        {
            var sppt = _context.Sppt.Include(d => d.IdSpopNavigation).ThenInclude(d => d.IdCoaNavigation)
                .Include(d => d.IdTarifNavigation)
                .Include(d => d.IdSpopNavigation).ThenInclude(d => d.IdSubjekNavigation)
                .Include(d => d.IdSpopNavigation).ThenInclude(d => d.IndKabKota)
                .Include(d => d.IdSpopNavigation).ThenInclude(d => d.IndKecamatan)
                .Include(d => d.IdSpopNavigation).ThenInclude(d => d.IdSubjekNavigation).ThenInclude(d => d.IndKecamatan)
                .Include(d => d.IdSpopNavigation).ThenInclude(d => d.IndKelurahan)
                .Include(d => d.IdSpopNavigation).ThenInclude(d => d.IdSubjekNavigation).ThenInclude(d => d.IndKelurahan)
                .Include(d => d.IdSpopNavigation).ThenInclude(d => d.Lspop)
                .FirstOrDefault(d => d.IdSppt == id);

            ViewBag.Bank = _context.Bank.Include(d => d.IdRefNavigation).Where(d => d.Status && d.IdSetoran == 1).Select(d => d.IdRefNavigation.NamaBank).ToList();
            ViewBag.Jumlah = _context.Bank.Include(d => d.IdRefNavigation).Where(d => d.Status && d.IdSetoran == 1).Select(d => d.IdRefNavigation.NamaBank).ToList().Count();
            var ttd = _context.TandaTangan.Include(d => d.IdPegawaiNavigation).ThenInclude(d => d.IdJabatanNavigation).FirstOrDefault(d => d.IdDokumen == 12);
            ViewBag.TTD = ttd.IdPegawaiNavigation.Nama.ToUpper();
            ViewBag.NIP = ttd.IdPegawaiNavigation.Nip;
            ViewBag.Jabatan = ttd.IdPegawaiNavigation.IdJabatanNavigation.Jabatan.ToUpper();

            ViewBag.Title = "SPPT " + sppt.IdSpopNavigation.Nop + " Tahun " + sppt.Tahun;

            var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(cetak, sppt);
        }

        [Auth(new string[] { "Developers", "PBB" })]
        public ActionResult SPPTMasal(IFormCollection formCollection)
        {
            string[] ids = formCollection["ID"];

            if (ids.Count() == 0)
            {
                TempData["status"] = "cetakNull";
                return RedirectToAction("Index");
            }

            List<Sppt> ListSppt = new List<Sppt>();

            foreach (var item in ids)
            {
                ListSppt.Add(new Sppt
                {
                    IdSppt = Guid.Parse(item)
                });
            }
            var IdSppt = ListSppt.Select(d => d.IdSppt).ToList();

            var sppt = _context.Sppt.Include(d => d.IdSpopNavigation).ThenInclude(d => d.IdCoaNavigation)
            .Include(d => d.IdTarifNavigation)
            .Include(d => d.IdSpopNavigation).ThenInclude(d => d.IdCoaNavigation)
            .Include(d => d.IdSpopNavigation).ThenInclude(d => d.IdSubjekNavigation)
            .Include(d => d.IdSpopNavigation).ThenInclude(d => d.IndKabKota)
            .Include(d => d.IdSpopNavigation).ThenInclude(d => d.IndKecamatan)
            .Include(d => d.IdSpopNavigation).ThenInclude(d => d.IndKelurahan)
            .Include(d => d.IdSpopNavigation).ThenInclude(d => d.IdSubjekNavigation).ThenInclude(d => d.IndKecamatan)
            .Include(d => d.IdSpopNavigation).ThenInclude(d => d.IdSubjekNavigation).ThenInclude(d => d.IndKelurahan)
            .Include(d => d.IdSpopNavigation).ThenInclude(d => d.Lspop)
            .Where(d => IdSppt.Contains(d.IdSppt)).ToList();

            ViewBag.Bank = _context.Bank.Include(d => d.IdRefNavigation).Where(d => d.Status && d.IdSetoran == 1).Select(d => d.IdRefNavigation.NamaBank).ToList();
            ViewBag.Jumlah = _context.Bank.Include(d => d.IdRefNavigation).Where(d => d.Status && d.IdSetoran == 1).Select(d => d.IdRefNavigation.NamaBank).ToList().Count();
            var ttd = _context.TandaTangan.Include(d => d.IdPegawaiNavigation).ThenInclude(d => d.IdJabatanNavigation).FirstOrDefault(d => d.IdDokumen == 12);
            ViewBag.TTD = ttd.IdPegawaiNavigation.Nama.ToUpper();
            ViewBag.NIP = ttd.IdPegawaiNavigation.Nip;
            ViewBag.Jabatan = ttd.IdPegawaiNavigation.IdJabatanNavigation.Jabatan.ToUpper();

            var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(cetak, sppt);
        }

        [Auth(new string[] { "Developers", "Pembayaran" })]
        public async Task<IActionResult> SSPD(Guid id)
        {
            var sspd = await _context.Sspd
                .Include(s => s.IdCoaNavigation)
                .Include(s => s.IdSubjekNavigation).ThenInclude(s => s.IndKelurahan)
                .Include(s => s.IdSubjekNavigation).ThenInclude(s => s.IndKecamatan)
                .Include(s => s.IdUsahaNavigation)
                .Include(s => s.IdSptpdNavigation)
                .Include(s => s.IdSkpdNavigation).ThenInclude(s => s.IdSptpdNavigation)
                .Include(s => s.IdSkpdkbNavigation).ThenInclude(s => s.IdSptpdNavigation)
                .Include(s => s.IdSkpdkbtNavigation).ThenInclude(s => s.IdSptpdNavigation)
                .Include(s => s.IdStpdNavigation).ThenInclude(s => s.IdSkpdNavigation).ThenInclude(s => s.IdSptpdNavigation)
                .FirstOrDefaultAsync(m => m.IdSspd == id);

            var denda = _context.Coa.FirstOrDefault(d => d.IdCoa == sspd.IdCoaNavigation.Denda);

            CetakSSPD cetak = new CetakSSPD
            {
                Id = sspd.IdSspd,
                Nama = sspd.IdSubjekNavigation.Nama,
                Usaha = sspd.IdUsahaNavigation.NamaUsaha,
                Alamat = sspd.IdSubjekNavigation.Alamat + "RT. " + sspd.IdSubjekNavigation.Rtrw,
                Kelurahan = "Kel. " + sspd.IdSubjekNavigation.IndKelurahan.Kelurahan,
                Kecamatan = "KEc. " + sspd.IdSubjekNavigation.IndKecamatan.Kecamatan,
                Npwpd = sspd.IdSubjekNavigation.Npwpd,
                Nomor = sspd.NoSspd,
                Tanggal = sspd.Tanggal,
                Coa = sspd.IdCoaNavigation.Uraian,
                KdCoa = sspd.IdCoa,
                CoaDenda = denda.Uraian,
                KdCoaDenda = denda.IdCoa,

            };

            if (sspd.IdSptpd != null)
            {
                cetak.Jenis = "SPTPD";
                cetak.MasaPajak1 = sspd.IdSptpdNavigation.MasaPajak1;
                cetak.MasaPajak2 = sspd.IdSptpdNavigation.MasaPajak2;
                cetak.NomorDok = sspd.IdSptpdNavigation.NoSptpd;
                cetak.Jumlah = sspd.IdSptpdNavigation.Terhutang;
                cetak.JumlahDenda = 0;
                cetak.Terhutang = sspd.IdSptpdNavigation.Terhutang;
            }
            else if (sspd.IdSkpd != null)
            {
                cetak.Jenis = "SKPD";
                cetak.MasaPajak1 = sspd.IdSkpdNavigation.IdSptpdNavigation.MasaPajak1;
                cetak.MasaPajak2 = sspd.IdSkpdNavigation.IdSptpdNavigation.MasaPajak2;
                cetak.NomorDok = sspd.IdSkpdNavigation.NoSkpd;
                cetak.JumlahDenda = (sspd.IdSkpdNavigation.Bunga ?? 0) + (sspd.IdSkpdNavigation.Kenaikan ?? 0);
                cetak.Jumlah = sspd.IdSkpdNavigation.Terhutang - (cetak.JumlahDenda ?? 0);
                cetak.Terhutang = sspd.IdSkpdNavigation.Terhutang;
            }
            else if (sspd.IdSkpdkb != null)
            {
                cetak.Jenis = "SKPDKB";
                cetak.MasaPajak1 = sspd.IdSkpdkbNavigation.IdSptpdNavigation.MasaPajak1;
                cetak.MasaPajak2 = sspd.IdSkpdkbNavigation.IdSptpdNavigation.MasaPajak2;
                cetak.NomorDok = sspd.IdSkpdkbNavigation.NoSkpdkb;
                cetak.JumlahDenda = (sspd.IdSkpdkbNavigation.Bunga ?? 0) + (sspd.IdSkpdkbNavigation.Kenaikan ?? 0);
                cetak.Jumlah = sspd.IdSkpdkbNavigation.KurangBayar;
                cetak.Terhutang = sspd.IdSkpdkbNavigation.Terhutang;
            }
            else if (sspd.IdSkpdkbt != null)
            {
                cetak.Jenis = "SKPDKBT";
                cetak.MasaPajak1 = sspd.IdSkpdkbtNavigation.IdSptpdNavigation.MasaPajak1;
                cetak.MasaPajak2 = sspd.IdSkpdkbtNavigation.IdSptpdNavigation.MasaPajak2;
                cetak.NomorDok = sspd.IdSkpdkbtNavigation.NoSkpdkbt;
                cetak.JumlahDenda = (sspd.IdSkpdkbtNavigation.Bunga ?? 0) + (sspd.IdSkpdkbtNavigation.Kenaikan ?? 0);
                cetak.Jumlah = sspd.IdSkpdkbtNavigation.KurangBayar;
                cetak.Terhutang = sspd.IdSkpdkbtNavigation.Terhutang;
            }
            else if (sspd.IdStpd != null)
            {
                cetak.Jenis = "STPD";
                cetak.MasaPajak1 = sspd.IdStpdNavigation.IdSkpdNavigation.IdSptpdNavigation.MasaPajak1;
                cetak.MasaPajak2 = sspd.IdStpdNavigation.IdSkpdNavigation.IdSptpdNavigation.MasaPajak2;
                cetak.NomorDok = sspd.IdStpdNavigation.NoStpd;
                cetak.JumlahDenda = (sspd.IdStpdNavigation.Bunga ?? 0);
                cetak.Jumlah = sspd.IdStpdNavigation.KurangBayar;
                cetak.Terhutang = sspd.IdStpdNavigation.Terhutang;
            }

            ViewBag.Title = sspd.NoSspd;

            var opd = HttpContext.Session.GetString("Opd");
            var pemda = HttpContext.Session.GetString("NamaPemda").Remove(0, 11);
            TextInfo textInfo = new CultureInfo("id-ID", false).TextInfo;
            ViewBag.Opd = textInfo.ToTitleCase(opd.ToLower());
            ViewBag.NamaPemda = textInfo.ToTitleCase(pemda.ToLower());

            var title1 = pemda.Replace("KOTA ", "").Replace("KABUPATEN ", "");
            var title2 = HttpContext.Session.GetString("NamaPemda").Replace(title1, "");

            ViewBag.Title1 = title1;
            ViewBag.Title2 = title2;

            var ttd = _context.TandaTangan.Include(d => d.IdPegawaiNavigation).ThenInclude(d => d.IdJabatanNavigation).FirstOrDefault(d => d.IdDokumen == 14);
            ViewBag.TTD = ttd.IdPegawaiNavigation.Nama;
            ViewBag.NIP = ttd.IdPegawaiNavigation.Nip;
            ViewBag.Jabatan = ttd.IdPegawaiNavigation.IdJabatanNavigation.Jabatan;

            var ctk = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(ctk, cetak);
        }

        [Auth(new string[] { "Developers", "Pembayaran" })]
        public async Task<IActionResult> KwitansiSSPD(Guid id)
        {
            var sspd = await _context.Sspd
                .Include(s => s.IdCoaNavigation)
                .Include(s => s.IdSubjekNavigation)
                .Include(s => s.IdSubjekNavigation)
                .Include(s => s.IdUsahaNavigation)
                .Include(s => s.IdSptpdNavigation)
                .Include(s => s.IdSkpdNavigation).ThenInclude(s => s.IdSptpdNavigation)
                .Include(s => s.IdSkpdkbNavigation).ThenInclude(s => s.IdSptpdNavigation)
                .Include(s => s.IdSkpdkbtNavigation).ThenInclude(s => s.IdSptpdNavigation)
                .Include(s => s.IdStpdNavigation).ThenInclude(s => s.IdSkpdNavigation).ThenInclude(s => s.IdSptpdNavigation)
                .FirstOrDefaultAsync(m => m.IdSspd == id);

            var denda = _context.Coa.FirstOrDefault(d => d.IdCoa == sspd.IdCoaNavigation.Denda);

            CetakKwitansi cetak = new CetakKwitansi
            {
                Nama = sspd.IdSubjekNavigation.Nama,
                Usaha = sspd.IdUsahaNavigation.NamaUsaha,
                Npwpd = sspd.IdSubjekNavigation.Npwpd ?? sspd.IdSubjekNavigation.Npwrd,
                Nomor = sspd.NoSspd,
                Uraian = (sspd.IdCoaNavigation.Uraian.Contains("Pajak") ? "" : "Pajak ") + sspd.IdCoaNavigation.Uraian,
                KdCoa = sspd.IdCoa,
                Denda = denda.Uraian,
                Total = sspd.JumlahSetoran,
            };

            if (sspd.IdSptpd != null)
            {
                cetak.Masa1 = sspd.IdSptpdNavigation.MasaPajak1;
                cetak.Masa2 = sspd.IdSptpdNavigation.MasaPajak2;
                cetak.JumlahPokok = sspd.IdSptpdNavigation.Terhutang;
                cetak.JumlahDenda = 0;
            }
            else if (sspd.IdSkpd != null)
            {
                cetak.Masa1 = sspd.IdSkpdNavigation.IdSptpdNavigation.MasaPajak1;
                cetak.Masa2 = sspd.IdSkpdNavigation.IdSptpdNavigation.MasaPajak2;
                cetak.JumlahPokok = sspd.IdSkpdNavigation.IdSptpdNavigation.Terhutang;
                cetak.JumlahDenda = sspd.IdSkpdNavigation.Bunga + sspd.IdSkpdNavigation.Kenaikan;
            }
            else if (sspd.IdSkpdkb != null)
            {
                cetak.Masa1 = sspd.IdSkpdkbNavigation.IdSptpdNavigation.MasaPajak1;
                cetak.Masa2 = sspd.IdSkpdkbNavigation.IdSptpdNavigation.MasaPajak2;
                cetak.JumlahPokok = sspd.IdSkpdkbNavigation.IdSptpdNavigation.Terhutang;
                cetak.JumlahDenda = sspd.IdSkpdkbNavigation.Bunga + sspd.IdSkpdkbNavigation.Kenaikan;
            }
            else if (sspd.IdSkpdkbt != null)
            {
                cetak.Masa1 = sspd.IdSkpdkbtNavigation.IdSptpdNavigation.MasaPajak1;
                cetak.Masa2 = sspd.IdSkpdkbtNavigation.IdSptpdNavigation.MasaPajak2;
                cetak.JumlahPokok = sspd.IdSkpdkbtNavigation.IdSptpdNavigation.Terhutang + sspd.IdSkpdkbtNavigation.Tambahan;
                cetak.JumlahDenda = sspd.IdSkpdkbtNavigation.Bunga + sspd.IdSkpdkbtNavigation.Kenaikan;
            }
            else if (sspd.IdStpd != null)
            {
                cetak.Masa1 = sspd.IdStpdNavigation.IdSkpdNavigation.IdSptpdNavigation.MasaPajak1;
                cetak.Masa2 = sspd.IdStpdNavigation.IdSkpdNavigation.IdSptpdNavigation.MasaPajak2;
                cetak.JumlahPokok = sspd.IdStpdNavigation.IdSkpdNavigation.IdSptpdNavigation.Terhutang;
                cetak.JumlahDenda = sspd.IdStpdNavigation.Terhutang + sspd.IdStpdNavigation.IdSkpdNavigation.IdSptpdNavigation.Terhutang;
            }

            ViewBag.Title = sspd.NoSspd;

            var bank = _context.Bank.FirstOrDefault(d => d.IdSetoran == 1);
            ViewBag.Rekening = bank.NoRek;

            var opd = HttpContext.Session.GetString("Opd");
            var pemda = HttpContext.Session.GetString("NamaPemda").Remove(0, 11);
            TextInfo textInfo = new CultureInfo("id-ID", false).TextInfo;
            ViewBag.Opd = textInfo.ToTitleCase(opd.ToLower());
            ViewBag.NamaPemda = textInfo.ToTitleCase(pemda.ToLower());

            var ctk = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(ctk, cetak);
        }

        [Auth(new string[] { "Developers", "Pembayaran" })]
        public async Task<IActionResult> SSRD(Guid id)
        {
            var ssrd = await _context.Ssrd
                .Include(s => s.IdBankNavigation)
                .Include(s => s.IdCoaNavigation)
                .Include(s => s.IdSetoranNavigation)
                .Include(s => s.IdSkrdNavigation)
                .Include(s => s.IdSubjekNavigation).ThenInclude(s => s.IndKelurahan)
                .Include(s => s.IdSubjekNavigation).ThenInclude(s => s.IndKecamatan)
                .FirstOrDefaultAsync(m => m.IdSsrd == id);

            ViewBag.Title = ssrd.NoSsrd;

            var opd = HttpContext.Session.GetString("Opd");
            var pemda = HttpContext.Session.GetString("NamaPemda").Remove(0, 11);
            TextInfo textInfo = new CultureInfo("id-ID", false).TextInfo;
            ViewBag.Opd = textInfo.ToTitleCase(opd.ToLower());
            ViewBag.NamaPemda = textInfo.ToTitleCase(pemda.ToLower());

            var title1 = pemda.Replace("KOTA ", "").Replace("KABUPATEN ", "");
            var title2 = HttpContext.Session.GetString("NamaPemda").Replace(title1, "");

            ViewBag.Title1 = title1;
            ViewBag.Title2 = title2;

            ViewBag.Detail = _context.SkrdDt.Include(d => d.IdTarifNavigation).Where(d => d.IdSkrd == ssrd.IdSkrd).ToList();

            var ttd = _context.TandaTangan.Include(d => d.IdPegawaiNavigation).ThenInclude(d => d.IdJabatanNavigation).FirstOrDefault(d => d.IdDokumen == 17);
            ViewBag.TTD = ttd.IdPegawaiNavigation.Nama;
            ViewBag.NIP = ttd.IdPegawaiNavigation.Nip;
            ViewBag.Jabatan = ttd.IdPegawaiNavigation.IdJabatanNavigation.Jabatan;

            var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(cetak, ssrd);
        }

        [Auth(new string[] { "Developers", "Retribusi" })]
        public async Task<IActionResult> SSRDR(Guid id)
        {
            var ssrdr = await _context.Ssrdr
                .Include(s => s.IdBankNavigation)
                .Include(s => s.IdCoaNavigation)
                .Include(s => s.IdSetoranNavigation)
                .FirstOrDefaultAsync(m => m.IdSsrdr == id);

            ViewBag.Title = ssrdr.NoSsrdr;

            ViewBag.Detail = _context.SsrdrDt.Include(d => d.IdTarifNavigation).Where(d => d.IdSsrdr == ssrdr.IdSsrdr).ToList();

            var ttd = _context.TandaTangan.Include(d => d.IdPegawaiNavigation).ThenInclude(d => d.IdJabatanNavigation).FirstOrDefault(d => d.IdDokumen == 16);
            ViewBag.TTD = ttd.IdPegawaiNavigation.Nama;
            ViewBag.NIP = ttd.IdPegawaiNavigation.Nip;
            ViewBag.Jabatan = ttd.IdPegawaiNavigation.IdJabatanNavigation.Jabatan;

            var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(cetak, ssrdr);
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public async Task<ActionResult> STPD(Guid? id)
        {
            var stpd = await _context.Stpd
                .Include(d => d.IdSkpdNavigation).ThenInclude(d => d.IdSptpdNavigation).ThenInclude(d => d.IdSubjekNavigation).ThenInclude(d => d.IndKecamatan)
                .Include(d => d.IdSkpdNavigation).ThenInclude(d => d.IdSptpdNavigation).ThenInclude(d => d.IdSubjekNavigation).ThenInclude(d => d.IndKelurahan)
                .Include(d => d.IdSkpdNavigation).ThenInclude(d => d.IdSptpdNavigation).ThenInclude(d => d.IdUsahaNavigation)
                .Include(d => d.IdSkpdNavigation).ThenInclude(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation)
                .FirstOrDefaultAsync(d => d.IdStpd == id);

            ViewBag.Title = stpd.NoStpd;

            ViewBag.Bank = _context.Bank.Include(d => d.IdRefNavigation).Where(d => d.IdSetoran == 1).ToList();

            var opd = HttpContext.Session.GetString("Opd");
            var pemda = HttpContext.Session.GetString("NamaPemda").Remove(0, 11);
            TextInfo textInfo = new CultureInfo("id-ID", false).TextInfo;
            ViewBag.Opd = textInfo.ToTitleCase(opd.ToLower());
            ViewBag.NamaPemda = textInfo.ToTitleCase(pemda.ToLower());

            var title1 = pemda.Replace("KOTA ", "").Replace("KABUPATEN ", "");
            var title2 = HttpContext.Session.GetString("NamaPemda").Replace(title1, "");

            ViewBag.Title1 = title1;
            ViewBag.Title2 = title2;

            var ttd = _context.TandaTangan.Include(d => d.IdPegawaiNavigation).ThenInclude(d => d.IdJabatanNavigation).FirstOrDefault(d => d.IdDokumen == 10);
            ViewBag.TTD = ttd.IdPegawaiNavigation.Nama;
            ViewBag.NIP = ttd.IdPegawaiNavigation.Nip;
            ViewBag.Jabatan = ttd.IdPegawaiNavigation.IdJabatanNavigation.Jabatan;

            var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(cetak, stpd);
        }

        [Auth(new string[] { "Developers", "Penyetoran" })]
        public async Task<IActionResult> STS(Guid id)
        {
            var data = await _context.Sts
                .Include(d => d.IdBankNavigation).ThenInclude(d => d.IdRefNavigation)
                .FirstOrDefaultAsync(d => d.IdSts == id);

            List<STSDetail> sTSDetails = new List<STSDetail>();
            List<STSDetail> subDetails = new List<STSDetail>();

            if (data.Keterangan == "SSPD")
            {
                var sspd = _context.StsDt
                .Include(d => d.IdSspdNavigation).ThenInclude(d => d.IdCoaNavigation)
                .Include(d => d.IdSspdNavigation).ThenInclude(d => d.IdSptpdNavigation)
                .Include(d => d.IdSspdNavigation).ThenInclude(d => d.IdSkpdNavigation).ThenInclude(d => d.IdSptpdNavigation)
                .Include(d => d.IdSspdNavigation).ThenInclude(d => d.IdSkpdkbNavigation).ThenInclude(d => d.IdSptpdNavigation)
                .Include(d => d.IdSspdNavigation).ThenInclude(d => d.IdSkpdkbtNavigation).ThenInclude(d => d.IdSptpdNavigation)
                .Include(d => d.IdSspdNavigation).ThenInclude(d => d.IdStpdNavigation).ThenInclude(d => d.IdSkpdNavigation).ThenInclude(d => d.IdSptpdNavigation)
                .Where(d => d.IdSts == id).ToList();

                foreach (var item in sspd)
                {
                    if (item.IdSspdNavigation.IdSptpd != null)
                    {
                        subDetails.Add(new STSDetail
                        {
                            KdCoa = item.IdSspdNavigation.IdCoa,
                            Uraian = item.IdSspdNavigation.IdCoaNavigation.Uraian,
                            Jumlah = item.IdSspdNavigation.JumlahSetoran
                        });
                    }
                    else if (item.IdSspdNavigation.IdSkpd != null)
                    {
                        var skpd = item.IdSspdNavigation.IdSkpdNavigation;
                        if (skpd.Bunga != 0 || skpd.Kenaikan != 0)
                        {
                            var coa = _context.Coa.FirstOrDefault(d => d.IdCoa == item.IdSspdNavigation.IdCoaNavigation.Denda);
                            subDetails.Add(new STSDetail
                            {
                                KdCoa = coa.IdCoa,
                                Uraian = coa.Uraian,
                                Jumlah = skpd.Bunga + skpd.Kenaikan
                            });
                            subDetails.Add(new STSDetail
                            {
                                KdCoa = item.IdSspdNavigation.IdCoa,
                                Uraian = item.IdSspdNavigation.IdCoaNavigation.Uraian,
                                Jumlah = skpd.IdSptpdNavigation.Terhutang
                            });
                        }
                        else
                        {
                            subDetails.Add(new STSDetail
                            {
                                KdCoa = item.IdSspdNavigation.IdCoa,
                                Uraian = item.IdSspdNavigation.IdCoaNavigation.Uraian,
                                Jumlah = item.IdSspdNavigation.JumlahSetoran
                            });
                        }
                    }
                    else if (item.IdSspdNavigation.IdSkpdkb != null)
                    {
                        var skpdkb = item.IdSspdNavigation.IdSkpdkbNavigation;
                        if (skpdkb.Bunga != 0 || skpdkb.Kenaikan != 0)
                        {
                            var coa = _context.Coa.FirstOrDefault(d => d.IdCoa == item.IdSspdNavigation.IdCoaNavigation.Denda);
                            subDetails.Add(new STSDetail
                            {
                                KdCoa = coa.IdCoa,
                                Uraian = coa.Uraian,
                                Jumlah = skpdkb.Bunga + skpdkb.Kenaikan
                            });
                            subDetails.Add(new STSDetail
                            {
                                KdCoa = item.IdSspdNavigation.IdCoa,
                                Uraian = item.IdSspdNavigation.IdCoaNavigation.Uraian,
                                Jumlah = skpdkb.KurangBayar
                            });
                        }
                        else
                        {
                            subDetails.Add(new STSDetail
                            {
                                KdCoa = item.IdSspdNavigation.IdCoa,
                                Uraian = item.IdSspdNavigation.IdCoaNavigation.Uraian,
                                Jumlah = item.IdSspdNavigation.JumlahSetoran
                            });
                        }
                    }
                    else if (item.IdSspdNavigation.IdSkpdkbt != null)
                    {
                        var skpdkbt = item.IdSspdNavigation.IdSkpdkbtNavigation;
                        if (skpdkbt.Bunga != 0 || skpdkbt.Kenaikan != 0)
                        {
                            var coa = _context.Coa.FirstOrDefault(d => d.IdCoa == item.IdSspdNavigation.IdCoaNavigation.Denda);
                            subDetails.Add(new STSDetail
                            {
                                KdCoa = coa.IdCoa,
                                Uraian = coa.Uraian,
                                Jumlah = skpdkbt.Bunga + skpdkbt.Kenaikan
                            });
                            subDetails.Add(new STSDetail
                            {
                                KdCoa = item.IdSspdNavigation.IdCoa,
                                Uraian = item.IdSspdNavigation.IdCoaNavigation.Uraian,
                                Jumlah = skpdkbt.KurangBayar + skpdkbt.Tambahan
                            });
                        }
                        else
                        {
                            subDetails.Add(new STSDetail
                            {
                                KdCoa = item.IdSspdNavigation.IdCoa,
                                Uraian = item.IdSspdNavigation.IdCoaNavigation.Uraian,
                                Jumlah = item.IdSspdNavigation.JumlahSetoran
                            });
                        }
                    }
                    else if (item.IdSspdNavigation.IdStpd != null)
                    {
                        var stpd = item.IdSspdNavigation.IdStpdNavigation;
                        if (stpd.Bunga != 0)
                        {
                            var coa = _context.Coa.FirstOrDefault(d => d.IdCoa == item.IdSspdNavigation.IdCoaNavigation.Denda);
                            subDetails.Add(new STSDetail
                            {
                                KdCoa = coa.IdCoa,
                                Uraian = coa.Uraian,
                                Jumlah = stpd.Terhutang - stpd.IdSkpdNavigation.IdSptpdNavigation.Terhutang
                            });
                            subDetails.Add(new STSDetail
                            {
                                KdCoa = item.IdSspdNavigation.IdCoa,
                                Uraian = item.IdSspdNavigation.IdCoaNavigation.Uraian,
                                Jumlah = stpd.IdSkpdNavigation.IdSptpdNavigation.Terhutang
                            });
                        }
                        else
                        {
                            subDetails.Add(new STSDetail
                            {
                                KdCoa = item.IdSspdNavigation.IdCoa,
                                Uraian = item.IdSspdNavigation.IdCoaNavigation.Uraian,
                                Jumlah = item.IdSspdNavigation.JumlahSetoran
                            });
                        }
                    }
                }

                foreach (var item in subDetails.GroupBy(d => d.KdCoa))
                {
                    sTSDetails.Add(new STSDetail
                    {
                        KdCoa = item.FirstOrDefault().KdCoa,
                        Uraian = item.FirstOrDefault().Uraian,
                        Jumlah = item.Sum(d => d.Jumlah)
                    });
                }
            }

            else if (data.Keterangan == "SSRD")
            {
                var sspd = _context.StsDt
                .Include(d => d.IdSsrdNavigation).ThenInclude(d => d.IdCoaNavigation)
                .Where(d => d.IdSts == id).ToList()
                .GroupBy(d => d.IdSsrdNavigation.IdCoa);

                foreach (var item in sspd)
                {
                    sTSDetails.Add(new STSDetail
                    {
                        KdCoa = item.FirstOrDefault().IdSsrdNavigation.IdCoa,
                        Uraian = item.FirstOrDefault().IdSsrdNavigation.IdCoaNavigation.Uraian,
                        Jumlah = item.Sum(d => d.IdSsrdNavigation.JumlahSetoran)
                    });
                }
            }

            else if (data.Keterangan == "SSRDR")
            {
                var sspd = _context.StsDt
                .Include(d => d.IdSsrdrNavigation).ThenInclude(d => d.IdCoaNavigation)
                .Where(d => d.IdSts == id).ToList()
                .GroupBy(d => d.IdSsrdrNavigation.IdCoa);

                foreach (var item in sspd)
                {
                    sTSDetails.Add(new STSDetail
                    {
                        KdCoa = item.FirstOrDefault().IdSsrdrNavigation.IdCoa,
                        Uraian = item.FirstOrDefault().IdSsrdrNavigation.IdCoaNavigation.Uraian,
                        Jumlah = item.Sum(d => d.IdSsrdrNavigation.Total)
                    });
                }
            }

            else if (data.Keterangan == "STTS")
            {
                var sspd = _context.StsDt
                .Include(d => d.IdSttsNavigation).ThenInclude(d => d.IdSpptNavigation).ThenInclude(d => d.IdSpopNavigation).ThenInclude(d => d.IdCoaNavigation)
                .Where(d => d.IdSts == id).ToList();

                foreach (var item in sspd.GroupBy(d => d.IdSttsNavigation.IdSpptNavigation.IdSpopNavigation.IdCoa))
                {
                    sTSDetails.Add(new STSDetail
                    {
                        KdCoa = item.FirstOrDefault().IdSttsNavigation.IdSpptNavigation.IdSpopNavigation.IdCoa,
                        Uraian = item.FirstOrDefault().IdSttsNavigation.IdSpptNavigation.IdSpopNavigation.IdCoaNavigation.Uraian,
                        Jumlah = item.Sum(d => d.IdSttsNavigation.TotalBayar)
                    });
                }
            }

            ViewBag.Detail = sTSDetails.OrderBy(d => d.KdCoa).ToList();

            ViewBag.Title = data.NoSts;

            var opd = HttpContext.Session.GetString("Opd");
            var pemda = HttpContext.Session.GetString("NamaPemda").Remove(0, 11);
            TextInfo textInfo = new CultureInfo("id-ID", false).TextInfo;
            ViewBag.Opd = textInfo.ToTitleCase(opd.ToLower());
            ViewBag.NamaPemda = textInfo.ToTitleCase(pemda.ToLower());

            var title1 = pemda.Replace("KOTA ", "").Replace("KABUPATEN ", "");
            var title2 = HttpContext.Session.GetString("NamaPemda").Replace(title1, "");

            ViewBag.Title1 = title1;
            ViewBag.Title2 = title2;

            var ttd = _context.TandaTangan.Include(d => d.IdPegawaiNavigation).ThenInclude(d => d.IdJabatanNavigation).FirstOrDefault(d => d.IdDokumen == 15);
            ViewBag.TTD = ttd.IdPegawaiNavigation.Nama;
            ViewBag.NIP = ttd.IdPegawaiNavigation.Nip;
            ViewBag.Jabatan = ttd.IdPegawaiNavigation.IdJabatanNavigation.Jabatan;

            var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(cetak, data);
        }

        [Auth(new string[] { "Developers", "Pembayaran" })]
        public ActionResult STTS(Guid id)
        {
            var stts = _context.Stts.Include(d => d.IdSpptNavigation).ThenInclude(d => d.IdSpopNavigation)
                .Include(d => d.IdBankNavigation)
                .Include(d => d.IdSpptNavigation).ThenInclude(d => d.IdSpopNavigation)
                .Include(d => d.IdSpptNavigation).ThenInclude(d => d.IdTarifNavigation)
                .Include(d => d.IdSpptNavigation).ThenInclude(d => d.IdSpopNavigation).ThenInclude(d => d.IndKelurahan)
                .Include(d => d.IdSpptNavigation).ThenInclude(d => d.IdSpopNavigation).ThenInclude(d => d.IndKecamatan)
                .Include(d => d.IdSpptNavigation).ThenInclude(d => d.IdSpopNavigation).ThenInclude(d => d.IndKabKota)
                .Include(d => d.IdSpptNavigation).ThenInclude(d => d.IdSpopNavigation).ThenInclude(d => d.IdSubjekNavigation).ThenInclude(d => d.IndKecamatan)
                .Include(d => d.IdSpptNavigation).ThenInclude(d => d.IdSpopNavigation).ThenInclude(d => d.IndKelurahan)
                .Include(d => d.IdSpptNavigation).ThenInclude(d => d.IdSpopNavigation).ThenInclude(d => d.IdSubjekNavigation).ThenInclude(d => d.IndKecamatan)
                .FirstOrDefault(d => d.IdStts == id);

            var lspop = _context.Lspop.Where(d => d.IdSpop == stts.IdSpptNavigation.IdSpop).Single();
            ViewBag.Luas = lspop.Luas;

            List<SelectListItem> roman = new List<SelectListItem>
            {
                new SelectListItem() { Text = "I", Value = "XIII" },
                new SelectListItem() { Text = "II", Value = "XIV" },
                new SelectListItem() { Text = "III", Value = "XV" },
                new SelectListItem() { Text = "IV", Value = "XVI" },
                new SelectListItem() { Text = "V", Value = "XVII" },
                new SelectListItem() { Text = "VI", Value = "XVIII" },
                new SelectListItem() { Text = "VII", Value = "XIX" },
                new SelectListItem() { Text = "VIII", Value = "XX" },
                new SelectListItem() { Text = "IX", Value = "XXI" },
                new SelectListItem() { Text = "X", Value = "XXII" },
                new SelectListItem() { Text = "XI", Value = "XXIII" }
            };
            ViewBag.Roman = roman;

            var opd = HttpContext.Session.GetString("Opd");
            var pemda = HttpContext.Session.GetString("NamaPemda").Remove(0, 11);
            TextInfo textInfo = new CultureInfo("id-ID", false).TextInfo;
            ViewBag.Opd = textInfo.ToTitleCase(opd.ToLower());
            ViewBag.NamaPemda = textInfo.ToTitleCase(pemda.ToLower());

            var title1 = pemda.Replace("KOTA ", "").Replace("KABUPATEN ", "");
            var title2 = HttpContext.Session.GetString("NamaPemda").Replace(title1, "");

            ViewBag.Title1 = title1;
            ViewBag.Title2 = title2;

            ViewBag.Title = "STTS " + stts.IdSpptNavigation.IdSpopNavigation.Nop + " Tahun " + stts.IdSpptNavigation.Tahun;

            var ttd = _context.TandaTangan.Include(d => d.IdPegawaiNavigation).ThenInclude(d => d.IdJabatanNavigation).FirstOrDefault(d => d.IdDokumen == 13);
            ViewBag.TTD = ttd.IdPegawaiNavigation.Nama;
            ViewBag.NIP = ttd.IdPegawaiNavigation.Nip;
            ViewBag.Jabatan = ttd.IdPegawaiNavigation.IdJabatanNavigation.Jabatan;

            var cetak = "~/Views/Cetak_" + _pemda + "/" + RouteData.Values["action"].ToString() + ".cshtml";

            return View(cetak, stts);
        }
    }
}
