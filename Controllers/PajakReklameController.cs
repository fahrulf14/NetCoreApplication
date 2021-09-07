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
    public class PajakReklameController : Controller
    {
        private readonly DB_NewContext _context;

        public PajakReklameController(DB_NewContext context)
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

            var pReklame = await _context.PReklame
                .Include(p => p.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation)
                .Include(p => p.IdSptpdNavigation).ThenInclude(d => d.IdTarifNavigation)
                .Include(p => p.IndKabKota)
                .Include(p => p.IndKecamatan)
                .Include(p => p.IndKelurahan)
                .FirstOrDefaultAsync(m => m.IdSptpd == id);
            if (pReklame == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.Title = (pReklame.IdSptpdNavigation.Jabatan ? "Detail Nota Jabatan" : "Detail SPTPD");
            ViewBag.SubHeaderTitle = pReklame.IdSptpdNavigation.IdCoaNavigation.Uraian;
            ViewBag.L = Url.Content("/SPTPD/" + id);
            ViewBag.L1 = Url.Action("Details", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            //Status
            if (pReklame.IdSptpdNavigation.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (pReklame.IdSptpdNavigation.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            else if (pReklame.IdSptpdNavigation.Keterangan == 2)
            {
                ViewBag.State = "kt-ribbon--warning";
                ViewBag.Status = "Kurang Bayar";
            }
            else if (pReklame.IdSptpdNavigation.Keterangan == 3)
            {
                ViewBag.State = "kt-ribbon--primary";
                ViewBag.Status = "Lebih Bayar";
            }
            else if (pReklame.IdSptpdNavigation.Keterangan == 4)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPD";
            }
            else if (pReklame.IdSptpdNavigation.Keterangan == 5)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPDKB";
            }
            else if (pReklame.IdSptpdNavigation.Keterangan == 5)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPDKBT";
            }

            if (pReklame.IdSptpdNavigation.IdCoa == "4110403")
            {
                ViewBag.Ukuran = "CM";
                ViewBag.UkuranL = "CM<sup>2</sup>";
            }
            else
            {
                ViewBag.Ukuran = "M";
                ViewBag.UkuranL = "M<sup>2</sup>";
            }

            if (pReklame.IdSptpdNavigation.IdCoa == "4110401" || pReklame.IdSptpdNavigation.IdCoa == "4110402" || pReklame.IdSptpdNavigation.IdCoa == "4110405" || pReklame.IdSptpdNavigation.IdCoa == "4110406" || pReklame.IdSptpdNavigation.IdCoa == "4110407")
            {
                ViewBag.Jumlah = "Buah";
            }
            else if (pReklame.IdSptpdNavigation.IdCoa == "4110408" || pReklame.IdSptpdNavigation.IdCoa == "4110409")
            {
                ViewBag.Jumlah = "Kali Tayang";
            }
            else if (pReklame.IdSptpdNavigation.IdCoa == "4110411")
            {
                ViewBag.Jumlah = "Kali Tayang / Hari";
            }
            else if (pReklame.IdSptpdNavigation.IdCoa == "4110403" || pReklame.IdSptpdNavigation.IdCoa == "4110404")
            {
                ViewBag.Jumlah = "Lembar";
            }
            else
            {
                ViewBag.Jumlah = "Kali Peragaan";
            }

            if (pReklame.IdSptpdNavigation.IdCoa == "4110401" || pReklame.IdSptpdNavigation.IdCoa == "4110402" || pReklame.IdSptpdNavigation.IdCoa == "4110411" || pReklame.IdSptpdNavigation.IdCoa == "4110405")
            {
                ViewBag.Hari = "Hari";
            }
            else if (pReklame.IdSptpdNavigation.IdCoa == "4110406" || pReklame.IdSptpdNavigation.IdCoa == "4110407")
            {
                ViewBag.Hari = "Bulan";
            }

            return View(pReklame);
        }


        [Auth(new string[] { "Developers", "Pendataan" })]
        public async Task<IActionResult> SPTPD(Guid? id, bool jabatan)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pReklame = await _context.PReklame.Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation).FirstOrDefaultAsync(d => d.IdSptpd == id);
            if (pReklame == null)
            {
                return NotFound();
            }

            if (pReklame.IdSptpdNavigation.Jabatan)
            {
                jabatan = pReklame.IdSptpdNavigation.Jabatan;
            }
            TempData["Jabatan"] = jabatan;
            TempData.Keep("Jabatan");

            //Link
            ViewBag.Title = (jabatan ? "Nota Jabatan " : "SPTPD ") + pReklame.IdSptpdNavigation.IdCoaNavigation.Uraian;
            ViewBag.SubHeaderTitle = pReklame.IdSptpdNavigation.IdCoaNavigation.Uraian;
            ViewBag.L = Url.Content("/SPTPD/" + id);
            ViewBag.L1 = Url.Action("SPTPD", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";
            ViewBag.S1 = (jabatan ? "Nota Jabatan " : "SPTPD ");

            ViewData["KelasJalan"] = new SelectList(_context.NsrLed, "Lokasi", "Lokasi", pReklame.KelasJalan);

            List<SelectListItem> letak = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Luar Ruangan", Value = "Luar Ruangan" },
                new SelectListItem() { Text = "Dalam Ruangan", Value = "Dalam Ruangan" }
            };
            ViewData["Letak"] = new SelectList(letak, "Value", "Text", pReklame.Letak);

            List<SelectListItem> status = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Reklame Tetap", Value = "Reklame Tetap" },
                new SelectListItem() { Text = "Reklame Sementara", Value = "Reklame Sementara" }
            };
            ViewData["StatusReklame"] = new SelectList(status, "Value", "Text", pReklame.StatusReklame);

            List<SelectListItem> produk = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Produk", Value = "Produk" },
                new SelectListItem() { Text = "Non Produk", Value = "Non Produk" }
            };
            ViewData["Produk"] = new SelectList(produk, "Value", "Text", pReklame.Produk);

            List<SelectListItem> rokok = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Produk Rokok", Value = "Produk Rokok" },
                new SelectListItem() { Text = "Produk Non Rokok", Value = "Produk Non Rokok" }
            };
            ViewData["Rokok"] = new SelectList(rokok, "Value", "Text", pReklame.Rokok);

            var Pemda = _context.Pemda.FirstOrDefault();
            ViewData["IndKecamatanId"] = new SelectList(_context.IndKecamatan.Where(d => d.IndKabKotaId == Pemda.IndKabKotaId), "IndKecamatanId", "Kecamatan", pReklame.IndKecamatanId);
            ViewData["IndKelurahanId"] = new SelectList(_context.IndKelurahan.Where(d => d.IndKecamatanId == pReklame.IndKecamatanId), "IndKelurahanId", "Kelurahan", pReklame.IndKelurahanId);

            return View("Edit", pReklame);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SPTPD(Guid id, [Bind("IdReklame,IdSptpd,Judul,Teks,AlamatPasang,IndKabKotaId,IndKecamatanId,IndKelurahanId,KelasJalan,StatusReklame,JenisReklame,Bahan,Produk,Rokok,Letak,P1,P2,P3,P4,L1,L2,L3,L4,Luas,Tinggi,JumlahHari,JumlahDetik,Jumlah,TglPasang1,TglPasang2,Nsr,Njop,NilaiKontrak,Dpp,PajakTerhutang,Eu,Ed")] PReklame pReklame, Sptpd sptpd)
        {
            if (id != pReklame.IdSptpd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var pajak = _context.PReklame.FirstOrDefault(d => d.IdSptpd == id);
                    var spt = _context.Sptpd.Find(id);
                    var Pemda = _context.Pemda.FirstOrDefault();

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

                        pajak.NilaiKontrak = pReklame.NilaiKontrak ?? 0;
                        pajak.Judul = pReklame.Judul;
                        pajak.Teks = pReklame.Teks;
                        pajak.AlamatPasang = pReklame.AlamatPasang;
                        pajak.IndKabKotaId = Pemda.IndKabKotaId;
                        pajak.IndKecamatanId = pReklame.IndKecamatanId;
                        pajak.IndKelurahanId = pReklame.IndKelurahanId;
                        pajak.KelasJalan = pReklame.KelasJalan;
                        pajak.StatusReklame = pReklame.StatusReklame;
                        pajak.JenisReklame = pReklame.JenisReklame;
                        pajak.Bahan = pReklame.Bahan;
                        pajak.Produk = pReklame.Produk;
                        pajak.Rokok = pReklame.Rokok;
                        pajak.Letak = pReklame.Letak;
                        pajak.P1 = pReklame.P1 ?? 0;
                        pajak.P2 = pReklame.P2 ?? 0;
                        pajak.P3 = pReklame.P3 ?? 0;
                        pajak.P4 = pReklame.P4 ?? 0;
                        pajak.L1 = pReklame.L1 ?? 0; var L1 = pajak.L1 * pajak.P1;
                        pajak.L2 = pReklame.L2 ?? 0; var L2 = pajak.L2 * pajak.P2;
                        pajak.L3 = pReklame.L3 ?? 0; var L3 = pajak.L3 * pajak.P3;
                        pajak.L4 = pReklame.L4 ?? 0; var L4 = pajak.L4 * pajak.P4;
                        var Luas = L1 + L2 + L3 + L4;
                        pajak.Luas = Luas;
                        pajak.Tinggi = pReklame.Tinggi;
                        pajak.JumlahHari = pReklame.JumlahHari;
                        pajak.JumlahDetik = pReklame.JumlahDetik;
                        pajak.Jumlah = pReklame.Jumlah;
                        pajak.TglPasang1 = sptpd.MasaPajak1;
                        pajak.TglPasang2 = sptpd.MasaPajak2;

                        //PERHITUNGAN

                        decimal NSR;
                        decimal DPP;

                        decimal TLuas = 0;
                        decimal TLokasi = 0;
                        decimal TRokok = 0;
                        decimal TTinggi = 0;

                        if (spt.IdCoa == "4110401" || spt.IdCoa == "4110402")
                        {
                            var nsr = _context.Nsr.FirstOrDefault(d => d.Lokasi == pajak.KelasJalan);
                            if (pajak.Produk == "Produk") { NSR = nsr.NsrProduk ?? 0; }
                            else if (pajak.Produk == "Non Produk") { NSR = nsr.NsrNonProduk ?? 0; }
                            else { NSR = 0; }

                            DPP = NSR * Convert.ToDecimal(Luas) * pajak.JumlahHari * pajak.Jumlah ?? 0;
                        }
                        else if (spt.IdCoa == "4110411")
                        {
                            var nsrl = _context.NsrLed.FirstOrDefault(d => d.Lokasi == pajak.KelasJalan);
                            if (Luas < 8) { NSR = nsrl.L1 ?? 0; }
                            else if (Luas <= 8 && Luas > 16) { NSR = nsrl.L2 ?? 0; }
                            else if (Luas <= 16 && Luas > 24) { NSR = nsrl.L3 ?? 0; }
                            else if (Luas <= 24 && Luas > 32) { NSR = nsrl.L4 ?? 0; }
                            else if (Luas <= 32 && Luas > 50) { NSR = nsrl.L5 ?? 0; }
                            else if (Luas <= 50 && Luas > 100) { NSR = nsrl.L6 ?? 0; }
                            else if (Luas >= 100) { NSR = nsrl.L7 ?? 0; }
                            else { NSR = 0; }

                            var Detik = Math.Ceiling(Convert.ToDecimal(pajak.JumlahDetik) / nsrl.Durasi ?? 0);
                            DPP = NSR * Convert.ToDecimal(Luas) * Detik * pajak.Jumlah ?? 0;

                            // CEK LETAK LUAS LED
                            if (Luas > 200)
                            {
                                //var PLuas = _context.Ket_Reklame.Find(5);
                                TLuas = DPP * Convert.ToDecimal(20) / 100;
                            }
                        }
                        else
                        {
                            var nsrlain = _context.NsrLain.FirstOrDefault(d => d.Jenis == spt.IdCoa);
                            NSR = nsrlain.Nsr ?? 0;

                            decimal Waktu = pajak.JumlahDetik ?? 1;
                            if (nsrlain.SatuanWaktu == "Detik")
                            {
                                Waktu = Math.Ceiling(Convert.ToDecimal(pajak.JumlahDetik) / nsrlain.Waktu ?? 0);
                            }
                            var Hari = pajak.JumlahHari ?? 1;
                            var Jumlah = pajak.Jumlah ?? 1;

                            DPP = NSR * Hari * Jumlah * Waktu;
                        }

                        // CEK LETAK INDOOR / OUTDOOR
                        if (pajak.Letak == "Dalam Ruangan")
                        {
                            //var PLokasi = db.Ket_Reklame.Find(1);
                            TLokasi = DPP * Convert.ToDecimal(50) / 100;
                        }

                        // CEK ROKOK / NON
                        if (pajak.Rokok == "Produk Rokok")
                        {
                            //var PRokok = db.Ket_Reklame.Find(2);
                            TRokok = DPP * Convert.ToDecimal(25) / 100;
                        }

                        // CEK TINGGI
                        if (pajak.Tinggi > 15)
                        {
                            //var PTinggi = db.Ket_Reklame.Find(4);
                            TTinggi = DPP * Convert.ToDecimal(25) / 100;
                        }

                        DPP = DPP + TLuas + TRokok + TTinggi - TLokasi;

                        pajak.Nsr = NSR;

                        if (pajak.NilaiKontrak != null && pajak.NilaiKontrak > DPP)
                        {
                            DPP = Convert.ToDecimal(pReklame.NilaiKontrak);
                        }

                        pajak.Dpp = DPP;

                        if (pajak.Dpp == 0)
                        {
                            TempData["status"] = "DPP0";
                            return RedirectToAction("SPTPD", new { id });
                        }

                        pajak.PajakTerhutang = pajak.Dpp * (decimal)tarif.Tarif / 100;

                        spt.Tanggal = sptpd.Tanggal;
                        spt.MasaPajak1 = sptpd.MasaPajak1;
                        spt.MasaPajak2 = sptpd.MasaPajak2;
                        spt.Tahun = spt.MasaPajak1?.Year.ToString();
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
                                spt.NoSptpd = string.Format("{0:000000}", spt.Nomor) + "/NotaJabatan/Reklame/" + string.Format("{0:MM/yyyy}", spt.Tanggal);
                            }
                            else
                            {
                                var no = _context.Sptpd.Where(e => e.IdCoa.Substring(0, 5) == spt.IdCoa.Substring(0, 5) && e.Jabatan == false && e.Tahun == spt.Tahun).Select(e => e.Nomor).Max() ?? 0;
                                spt.Nomor = no + 1;
                                spt.NoSptpd = string.Format("{0:000000}", spt.Nomor) + "/SPTPD/Reklame/" + string.Format("{0:MM/yyyy}", spt.Tanggal);
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
                            PReklame newPajak = new PReklame
                            {
                                IdReklame = Guid.NewGuid(),
                                IdSptpd = newSpt.IdSptpd
                            };

                            _context.Sptpd.Add(newSpt);
                            _context.PReklame.Add(newPajak);
                        }

                        _context.PReklame.Update(pajak);
                        _context.Sptpd.Update(spt);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PReklameExists(pReklame.IdReklame))
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
            return View(pReklame);
        }

        [Auth(new string[] { "Developers", "Validasi" })]
        public async Task<IActionResult> Validasi(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pReklame = await _context.PReklame
                .Include(p => p.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation)
                .Include(p => p.IdSptpdNavigation).ThenInclude(d => d.IdTarifNavigation)
                .Include(p => p.IndKabKota)
                .Include(p => p.IndKecamatan)
                .Include(p => p.IndKelurahan)
                .FirstOrDefaultAsync(m => m.IdSptpd == id);
            if (pReklame == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.Title = (pReklame.IdSptpdNavigation.Jabatan ? "Validasi Nota Jabatan" : "Validasi SPTPD");
            ViewBag.SubHeaderTitle = pReklame.IdSptpdNavigation.IdCoaNavigation.Uraian;
            ViewBag.L = Url.Content("/SPTPD/" + id);
            ViewBag.L1 = Url.Action("Validasi", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            //Status
            if (pReklame.IdSptpdNavigation.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (pReklame.IdSptpdNavigation.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            else if (pReklame.IdSptpdNavigation.Keterangan == 2)
            {
                ViewBag.State = "kt-ribbon--warning";
                ViewBag.Status = "Kurang Bayar";
            }
            else if (pReklame.IdSptpdNavigation.Keterangan == 3)
            {
                ViewBag.State = "kt-ribbon--primary";
                ViewBag.Status = "Lebih Bayar";
            }
            else if (pReklame.IdSptpdNavigation.Keterangan == 4)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPD";
            }
            else if (pReklame.IdSptpdNavigation.Keterangan == 5)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPDKB";
            }
            else if (pReklame.IdSptpdNavigation.Keterangan == 5)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPDKBT";
            }

            if (pReklame.IdSptpdNavigation.IdCoa == "4110403")
            {
                ViewBag.Ukuran = "CM";
                ViewBag.UkuranL = "CM<sup>2</sup>";
            }
            else
            {
                ViewBag.Ukuran = "M";
                ViewBag.UkuranL = "M<sup>2</sup>";
            }

            if (pReklame.IdSptpdNavigation.IdCoa == "4110401" || pReklame.IdSptpdNavigation.IdCoa == "4110402" || pReklame.IdSptpdNavigation.IdCoa == "4110405" || pReklame.IdSptpdNavigation.IdCoa == "4110406" || pReklame.IdSptpdNavigation.IdCoa == "4110407")
            {
                ViewBag.Jumlah = "Buah";
            }
            else if (pReklame.IdSptpdNavigation.IdCoa == "4110408" || pReklame.IdSptpdNavigation.IdCoa == "4110409")
            {
                ViewBag.Jumlah = "Kali Tayang";
            }
            else if (pReklame.IdSptpdNavigation.IdCoa == "4110411")
            {
                ViewBag.Jumlah = "Kali Tayang / Hari";
            }
            else if (pReklame.IdSptpdNavigation.IdCoa == "4110403" || pReklame.IdSptpdNavigation.IdCoa == "4110404")
            {
                ViewBag.Jumlah = "Lembar";
            }
            else
            {
                ViewBag.Jumlah = "Kali Peragaan";
            }

            if (pReklame.IdSptpdNavigation.IdCoa == "4110401" || pReklame.IdSptpdNavigation.IdCoa == "4110402" || pReklame.IdSptpdNavigation.IdCoa == "4110411" || pReklame.IdSptpdNavigation.IdCoa == "4110405")
            {
                ViewBag.Hari = "Hari";
            }
            else if (pReklame.IdSptpdNavigation.IdCoa == "4110406" || pReklame.IdSptpdNavigation.IdCoa == "4110407")
            {
                ViewBag.Hari = "Bulan";
            }

            return View(pReklame);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Validasi(Guid id, PReklame pReklame)
        {
            if (id != pReklame.IdSptpd)
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
                    if (!PReklameExists(pReklame.IdReklame))
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
            return View(pReklame);
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public async Task<IActionResult> SKPD(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pReklame = await _context.PReklame
                .Include(p => p.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation)
                .Include(p => p.IdSptpdNavigation).ThenInclude(d => d.IdTarifNavigation)
                .Include(p => p.IndKabKota)
                .Include(p => p.IndKecamatan)
                .Include(p => p.IndKelurahan)
                .FirstOrDefaultAsync(m => m.IdSptpd == id);
            if (pReklame == null)
            {
                return NotFound();
            }

            //Link
            if (pReklame.IdSptpdNavigation.Sk != null)
            {
                ViewBag.Title = "Edit SKPD";
                ViewBag.S1 = "Edit SKPD";
            }
            else
            {
                ViewBag.Title = "Buat SKPD";
                ViewBag.S1 = "Buat SKPD";
            }
            ViewBag.SubHeaderTitle = pReklame.IdSptpdNavigation.IdCoaNavigation.Uraian;
            ViewBag.L = Url.Action("SKPD", "Penetapan", new { id });
            ViewBag.L1 = Url.Action("SKPD", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            //Status
            if (pReklame.IdSptpdNavigation.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (pReklame.IdSptpdNavigation.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            else if (pReklame.IdSptpdNavigation.Keterangan == 2)
            {
                ViewBag.State = "kt-ribbon--warning";
                ViewBag.Status = "Kurang Bayar";
            }
            else if (pReklame.IdSptpdNavigation.Keterangan == 3)
            {
                ViewBag.State = "kt-ribbon--primary";
                ViewBag.Status = "Lebih Bayar";
            }
            else if (pReklame.IdSptpdNavigation.Keterangan == 4)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPD";
            }
            else if (pReklame.IdSptpdNavigation.Keterangan == 5)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPDKB";
            }
            else if (pReklame.IdSptpdNavigation.Keterangan == 5)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPDKBT";
            }

            if (pReklame.IdSptpdNavigation.IdCoa == "4110403")
            {
                ViewBag.Ukuran = "CM";
                ViewBag.UkuranL = "CM<sup>2</sup>";
            }
            else
            {
                ViewBag.Ukuran = "M";
                ViewBag.UkuranL = "M<sup>2</sup>";
            }

            if (pReklame.IdSptpdNavigation.IdCoa == "4110401" || pReklame.IdSptpdNavigation.IdCoa == "4110402" || pReklame.IdSptpdNavigation.IdCoa == "4110405" || pReklame.IdSptpdNavigation.IdCoa == "4110406" || pReklame.IdSptpdNavigation.IdCoa == "4110407")
            {
                ViewBag.Jumlah = "Buah";
            }
            else if (pReklame.IdSptpdNavigation.IdCoa == "4110408" || pReklame.IdSptpdNavigation.IdCoa == "4110409")
            {
                ViewBag.Jumlah = "Kali Tayang";
            }
            else if (pReklame.IdSptpdNavigation.IdCoa == "4110411")
            {
                ViewBag.Jumlah = "Kali Tayang / Hari";
            }
            else if (pReklame.IdSptpdNavigation.IdCoa == "4110403" || pReklame.IdSptpdNavigation.IdCoa == "4110404")
            {
                ViewBag.Jumlah = "Lembar";
            }
            else
            {
                ViewBag.Jumlah = "Kali Peragaan";
            }

            if (pReklame.IdSptpdNavigation.IdCoa == "4110401" || pReklame.IdSptpdNavigation.IdCoa == "4110402" || pReklame.IdSptpdNavigation.IdCoa == "4110411" || pReklame.IdSptpdNavigation.IdCoa == "4110405")
            {
                ViewBag.Hari = "Hari";
            }
            else if (pReklame.IdSptpdNavigation.IdCoa == "4110406" || pReklame.IdSptpdNavigation.IdCoa == "4110407")
            {
                ViewBag.Hari = "Bulan";
            }

            return View(pReklame);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SKPD(Guid id, PReklame pReklame)
        {
            if (id != pReklame.IdSptpd)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var sptpd = _context.Sptpd.Include(d => d.IdCoaNavigation).FirstOrDefault(d => d.IdSptpd == id);
                var coa = _context.Coa.AsNoTracking().FirstOrDefault(d => d.IdCoa == sptpd.IdCoaNavigation.Parent);
                var JHari = DateTime.Now.Subtract(sptpd.MasaPajak1 ?? DateTime.Now).Days;
                var MaxBulan = 24;
                var JBulan = Convert.ToDecimal(JHari / 30);
                if (JBulan > MaxBulan)
                {
                    JBulan = MaxBulan;
                }
                var Bunga = sptpd.Terhutang * 2 / 100 * Math.Floor(JBulan);
                decimal Kenaikan;
                if (sptpd.Jabatan)
                {
                    Kenaikan = sptpd.Terhutang ?? 0 * (25 / 100);
                }
                else
                {
                    Kenaikan = 0;
                }

                if (sptpd.Sk != null)
                {
                    var skpd = _context.Skpd.FirstOrDefault(d => d.IdSptpd == id);
                    skpd.Tanggal = DateTime.Now;
                    skpd.JatuhTempo = skpd.Tanggal.Value.AddDays(30);

                    skpd.Bunga = (int)Bunga;
                    skpd.Kenaikan = Kenaikan;
                    skpd.Terhutang = sptpd.Terhutang + skpd.Bunga + skpd.Kenaikan;
                    skpd.KreditPajak = 0;
                    skpd.Keterangan = 0;
                    skpd.Eu = HttpContext.Session.GetString("User");
                    skpd.Ed = DateTime.Now;
                    _context.Skpd.Update(skpd);

                    await _context.SaveChangesAsync();
                    TempData["status"] = "edit";
                    return RedirectToAction("Details", "SKPD", new { id = skpd.IdSkpd });
                }
                else
                {
                    Skpd skpd = new Skpd
                    {
                        IdSkpd = Guid.NewGuid(),
                        IdSptpd = id,
                        Tanggal = DateTime.Now
                    };
                    var noSk = _context.Skpd.Where(d => d.Tanggal.Value.Year == skpd.Tanggal.Value.Year).Select(e => e.Nomor).Max() ?? 0;
                    skpd.Nomor = noSk + 1;
                    skpd.NoSkpd = string.Format("{0:000000}", skpd.Nomor) + "/SKPD/" + string.Format("{0:MM/yyyy}", skpd.Tanggal);
                    skpd.JatuhTempo = skpd.Tanggal.Value.AddDays(30);

                    skpd.Bunga = (int)Bunga;
                    skpd.Kenaikan = Kenaikan;
                    skpd.Terhutang = sptpd.Terhutang + skpd.Bunga + skpd.Kenaikan;
                    skpd.KreditPajak = 0;
                    skpd.Keterangan = 0;
                    skpd.Eu = HttpContext.Session.GetString("User");
                    skpd.Ed = DateTime.Now;
                    _context.Skpd.Add(skpd);

                    sptpd.Sk = "SKPD";
                    sptpd.Eu = HttpContext.Session.GetString("User");
                    sptpd.Ed = DateTime.Now;
                    _context.Sptpd.Update(sptpd);
                    await _context.SaveChangesAsync();
                    TempData["status"] = "create";
                    return RedirectToAction("Details", "SKPD", new { id = skpd.IdSkpd });
                }
            }
            return RedirectToAction("SKPD", new { id });
        }

        private bool PReklameExists(Guid id)
        {
            return _context.PReklame.Any(e => e.IdReklame == id);
        }
    }
}
