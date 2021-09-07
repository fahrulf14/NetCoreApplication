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
    public class LSPOPController : Controller
    {
        private readonly DB_NewContext _context;

        public LSPOPController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "PBB" })]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lspop = await _context.Lspop
                .Include(l => l.IdJenisNavigation)
                .Include(l => l.IdSpopNavigation)
                .FirstOrDefaultAsync(m => m.IdLspop == id);
            if (lspop == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("LSPOP", "PBB", new { id = lspop.IdSpop });
            ViewBag.L1 = Url.Action("Details", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            List<SelectListItem> Konstruksi = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Baja" },
                new SelectListItem() { Text = "Beton" },
                new SelectListItem() { Text = "Batu bata" },
                new SelectListItem() { Text = "Kayu" },
            };
            ViewBag.Konstruksi = new SelectList(Konstruksi, "Text", "Text", lspop.Konstruksi);

            List<SelectListItem> Kondisi = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Sangat baik" },
                new SelectListItem() { Text = "Baik" },
                new SelectListItem() { Text = "Sedang" },
                new SelectListItem() { Text = "Jelek" },
            };
            ViewBag.Kondisi = new SelectList(Kondisi, "Text", "Text", lspop.Kondisi);

            List<SelectListItem> Atap = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Decrabon/Beton/Gtg glazur" },
                new SelectListItem() { Text = "Gtg beton/Alumunium" },
                new SelectListItem() { Text = "Gtg biasa/Sirap" },
                new SelectListItem() { Text = "Asbes" },
                new SelectListItem() { Text = "Seng" },
            };
            ViewBag.Atap = new SelectList(Atap, "Text", "Text", lspop.Atap);

            List<SelectListItem> Dinding = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Kaca alumunium" },
                new SelectListItem() { Text = "Beton" },
                new SelectListItem() { Text = "Batu bata/Conblock" },
                new SelectListItem() { Text = "Kayu" },
                new SelectListItem() { Text = "Seng" },
            };
            ViewBag.Dinding = new SelectList(Dinding, "Text", "Text", lspop.Dinding);

            List<SelectListItem> Lantai = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Marmer" },
                new SelectListItem() { Text = "Keramik" },
                new SelectListItem() { Text = "Teraso" },
                new SelectListItem() { Text = "Ubin Pc/Papan" },
                new SelectListItem() { Text = "Semen" },
            };
            ViewBag.Lantai = new SelectList(Lantai, "Text", "Text", lspop.Lantai);

            List<SelectListItem> Langit2 = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Akustik/Jati" },
                new SelectListItem() { Text = "Triplek/Asbes bambu" },
                new SelectListItem() { Text = "Tidak ada" },
            };
            ViewBag.Langit2 = new SelectList(Langit2, "Text", "Text", lspop.Langit2);

            List<SelectListItem> kolam = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Diplester" },
                new SelectListItem() { Text = "Dengan pelapis" },
            };
            ViewBag.Kolam_Jenis = new SelectList(kolam, "Text", "Text", lspop.KolamJenis);

            List<SelectListItem> Pagar_Jenis = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Baja/Besi" },
                new SelectListItem() { Text = "Bata/Batako" },
                new SelectListItem() { Text = "Kombinasi" },
            };
            ViewBag.Pagar_Jenis = new SelectList(Pagar_Jenis, "Text", "Text", lspop.PagarJenis);

            ViewData["IdJenis"] = new SelectList(_context.RefJenisBangunan, "IdJenis", "Jenis", lspop.IdJenis);

            return View(lspop);
        }

        [Auth(new string[] { "Developers", "PBB" })]
        public IActionResult Create(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("LSPOP", "PBB", new { id });
            ViewBag.L1 = Url.Action("Create", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            ViewBag.IdSpop = id;

            List<SelectListItem> Konstruksi = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Baja" },
                new SelectListItem() { Text = "Beton" },
                new SelectListItem() { Text = "Batu bata" },
                new SelectListItem() { Text = "Kayu" },
            };
            ViewBag.Konstruksi = new SelectList(Konstruksi, "Text", "Text");

            List<SelectListItem> Kondisi = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Sangat baik" },
                new SelectListItem() { Text = "Baik" },
                new SelectListItem() { Text = "Sedang" },
                new SelectListItem() { Text = "Jelek" },
            };
            ViewBag.Kondisi = new SelectList(Kondisi, "Text", "Text");

            List<SelectListItem> Atap = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Decrabon/Beton/Gtg glazur" },
                new SelectListItem() { Text = "Gtg beton/Alumunium" },
                new SelectListItem() { Text = "Gtg biasa/Sirap" },
                new SelectListItem() { Text = "Asbes" },
                new SelectListItem() { Text = "Seng" },
            };
            ViewBag.Atap = new SelectList(Atap, "Text", "Text");

            List<SelectListItem> Dinding = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Kaca alumunium" },
                new SelectListItem() { Text = "Beton" },
                new SelectListItem() { Text = "Batu bata/Conblock" },
                new SelectListItem() { Text = "Kayu" },
                new SelectListItem() { Text = "Seng" },
            };
            ViewBag.Dinding = new SelectList(Dinding, "Text", "Text");

            List<SelectListItem> Lantai = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Marmer" },
                new SelectListItem() { Text = "Keramik" },
                new SelectListItem() { Text = "Teraso" },
                new SelectListItem() { Text = "Ubin Pc/Papan" },
                new SelectListItem() { Text = "Semen" },
            };
            ViewBag.Lantai = new SelectList(Lantai, "Text", "Text");

            List<SelectListItem> Langit2 = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Akustik/Jati" },
                new SelectListItem() { Text = "Triplek/Asbes bambu" },
                new SelectListItem() { Text = "Tidak ada" },
            };
            ViewBag.Langit2 = new SelectList(Langit2, "Text", "Text");

            List<SelectListItem> kolam = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Diplester" },
                new SelectListItem() { Text = "Dengan pelapis" },
            };
            ViewBag.Kolam_Jenis = new SelectList(kolam, "Text", "Text");

            List<SelectListItem> Pagar_Jenis = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Baja/Besi" },
                new SelectListItem() { Text = "Bata/Batako" },
                new SelectListItem() { Text = "Kombinasi" },
            };
            ViewBag.Pagar_Jenis = new SelectList(Pagar_Jenis, "Text", "Text");

            ViewData["IdJenis"] = new SelectList(_context.RefJenisBangunan, "IdJenis", "Jenis");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, [Bind("IdLspop,IdSpop,IdJenis,Luas,JumlahLantai,TahunBangun,TahunRenov,DayaListrik,Kondisi,Konstruksi,Atap,Dinding,Lantai,Langit2,AcSplit,AcWindow,AcSealing,AcSentral,KolamLuas,KolamJenis,HalamanRingan,HalamanSedang,HalamanBerat,HalamanLantai,LapanganBeton1,LapanganBeton2,LapanganAspal1,LapanganAspal2,LapanganTanah1,LapanganTanah2,LiftPenumpang,LiftKapsul,LiftBarang,Eskalator1,Eskalator2,PagarPanjang,PagarJenis,Hydrant,Sprinkle,FireAlarm,PabxJumlah,ArtesisKedalaman,Njop,FlagValidasi,Zona,Eu,Ed")] Lspop lspop)
        {
            if (ModelState.IsValid)
            {
                lspop.IdLspop = Guid.NewGuid();
                lspop.IdSpop = id;
                _context.Add(lspop);
                await _context.SaveChangesAsync();
                return RedirectToAction("LSPOP", "PBB", new { id });
            }
            List<SelectListItem> Konstruksi = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Baja" },
                new SelectListItem() { Text = "Beton" },
                new SelectListItem() { Text = "Batu bata" },
                new SelectListItem() { Text = "Kayu" },
            };
            ViewBag.Konstruksi = new SelectList(Konstruksi, "Text", "Text", lspop.Konstruksi);

            List<SelectListItem> Kondisi = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Sangat baik" },
                new SelectListItem() { Text = "Baik" },
                new SelectListItem() { Text = "Sedang" },
                new SelectListItem() { Text = "Jelek" },
            };
            ViewBag.Kondisi = new SelectList(Kondisi, "Text", "Text", lspop.Kondisi);

            List<SelectListItem> Atap = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Decrabon/Beton/Gtg glazur" },
                new SelectListItem() { Text = "Gtg beton/Alumunium" },
                new SelectListItem() { Text = "Gtg biasa/Sirap" },
                new SelectListItem() { Text = "Asbes" },
                new SelectListItem() { Text = "Seng" },
            };
            ViewBag.Atap = new SelectList(Atap, "Text", "Text", lspop.Atap);

            List<SelectListItem> Dinding = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Kaca alumunium" },
                new SelectListItem() { Text = "Beton" },
                new SelectListItem() { Text = "Batu bata/Conblock" },
                new SelectListItem() { Text = "Kayu" },
                new SelectListItem() { Text = "Seng" },
            };
            ViewBag.Dinding = new SelectList(Dinding, "Text", "Text", lspop.Dinding);

            List<SelectListItem> Lantai = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Marmer" },
                new SelectListItem() { Text = "Keramik" },
                new SelectListItem() { Text = "Teraso" },
                new SelectListItem() { Text = "Ubin Pc/Papan" },
                new SelectListItem() { Text = "Semen" },
            };
            ViewBag.Lantai = new SelectList(Lantai, "Text", "Text", lspop.Lantai);

            List<SelectListItem> Langit2 = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Akustik/Jati" },
                new SelectListItem() { Text = "Triplek/Asbes bambu" },
                new SelectListItem() { Text = "Tidak ada" },
            };
            ViewBag.Langit2 = new SelectList(Langit2, "Text", "Text", lspop.Langit2);

            List<SelectListItem> kolam = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Diplester" },
                new SelectListItem() { Text = "Dengan pelapis" },
            };
            ViewBag.Kolam_Jenis = new SelectList(kolam, "Text", "Text", lspop.KolamJenis);

            List<SelectListItem> Pagar_Jenis = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Baja/Besi" },
                new SelectListItem() { Text = "Bata/Batako" },
                new SelectListItem() { Text = "Kombinasi" },
            };
            ViewBag.Pagar_Jenis = new SelectList(Pagar_Jenis, "Text", "Text", lspop.PagarJenis);

            ViewData["IdJenis"] = new SelectList(_context.RefJenisBangunan, "IdJenis", "Jenis", lspop.IdJenis);
            return View(lspop);
        }

        [Auth(new string[] { "Developers", "PBB" })]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lspop = await _context.Lspop.Include(d => d.IdSpopNavigation).FirstOrDefaultAsync(d => d.IdLspop == id);
            if (lspop == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.L = Url.Action("LSPOP", "PBB", new { id = lspop.IdSpop });
            ViewBag.L1 = Url.Action("Edit", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            List<SelectListItem> Konstruksi = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Baja" },
                new SelectListItem() { Text = "Beton" },
                new SelectListItem() { Text = "Batu bata" },
                new SelectListItem() { Text = "Kayu" },
            };
            ViewBag.Konstruksi = new SelectList(Konstruksi, "Text", "Text", lspop.Konstruksi);

            List<SelectListItem> Kondisi = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Sangat baik" },
                new SelectListItem() { Text = "Baik" },
                new SelectListItem() { Text = "Sedang" },
                new SelectListItem() { Text = "Jelek" },
            };
            ViewBag.Kondisi = new SelectList(Kondisi, "Text", "Text", lspop.Kondisi);

            List<SelectListItem> Atap = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Decrabon/Beton/Gtg glazur" },
                new SelectListItem() { Text = "Gtg beton/Alumunium" },
                new SelectListItem() { Text = "Gtg biasa/Sirap" },
                new SelectListItem() { Text = "Asbes" },
                new SelectListItem() { Text = "Seng" },
            };
            ViewBag.Atap = new SelectList(Atap, "Text", "Text", lspop.Atap);

            List<SelectListItem> Dinding = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Kaca alumunium" },
                new SelectListItem() { Text = "Beton" },
                new SelectListItem() { Text = "Batu bata/Conblock" },
                new SelectListItem() { Text = "Kayu" },
                new SelectListItem() { Text = "Seng" },
            };
            ViewBag.Dinding = new SelectList(Dinding, "Text", "Text", lspop.Dinding);

            List<SelectListItem> Lantai = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Marmer" },
                new SelectListItem() { Text = "Keramik" },
                new SelectListItem() { Text = "Teraso" },
                new SelectListItem() { Text = "Ubin Pc/Papan" },
                new SelectListItem() { Text = "Semen" },
            };
            ViewBag.Lantai = new SelectList(Lantai, "Text", "Text", lspop.Lantai);

            List<SelectListItem> Langit2 = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Akustik/Jati" },
                new SelectListItem() { Text = "Triplek/Asbes bambu" },
                new SelectListItem() { Text = "Tidak ada" },
            };
            ViewBag.Langit2 = new SelectList(Langit2, "Text", "Text", lspop.Langit2);

            List<SelectListItem> kolam = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Diplester" },
                new SelectListItem() { Text = "Dengan pelapis" },
            };
            ViewBag.Kolam_Jenis = new SelectList(kolam, "Text", "Text", lspop.KolamJenis);

            List<SelectListItem> Pagar_Jenis = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Baja/Besi" },
                new SelectListItem() { Text = "Bata/Batako" },
                new SelectListItem() { Text = "Kombinasi" },
            };
            ViewBag.Pagar_Jenis = new SelectList(Pagar_Jenis, "Text", "Text", lspop.PagarJenis);

            ViewData["IdJenis"] = new SelectList(_context.RefJenisBangunan, "IdJenis", "Jenis", lspop.IdJenis);
            return View(lspop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdLspop,IdSpop,IdJenis,Luas,JumlahLantai,TahunBangun,TahunRenov,DayaListrik,Kondisi,Konstruksi,Atap,Dinding,Lantai,Langit2,AcSplit,AcWindow,AcSealing,AcSentral,KolamLuas,KolamJenis,HalamanRingan,HalamanSedang,HalamanBerat,HalamanLantai,LapanganBeton1,LapanganBeton2,LapanganAspal1,LapanganAspal2,LapanganTanah1,LapanganTanah2,LiftPenumpang,LiftKapsul,LiftBarang,Eskalator1,Eskalator2,PagarPanjang,PagarJenis,Hydrant,Sprinkle,FireAlarm,PabxJumlah,ArtesisKedalaman,Njop,FlagValidasi,Zona,Eu,Ed")] Lspop lspop)
        {
            if (id != lspop.IdLspop)
            {
                return NotFound();
            }

            var old = await _context.Lspop.AsNoTracking().FirstOrDefaultAsync(d => d.IdLspop == id);

            if (ModelState.IsValid)
            {
                try
                {
                    lspop.Eu = HttpContext.Session.GetString("User");
                    lspop.Ed = DateTime.Now;
                    _context.Update(lspop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LspopExists(lspop.IdLspop))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("LSPOP", "PBB", new { id = lspop.IdSpop });
            }
            List<SelectListItem> Konstruksi = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Baja" },
                new SelectListItem() { Text = "Beton" },
                new SelectListItem() { Text = "Batu bata" },
                new SelectListItem() { Text = "Kayu" },
            };
            ViewBag.Konstruksi = new SelectList(Konstruksi, "Text", "Text", lspop.Konstruksi);

            List<SelectListItem> Kondisi = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Sangat baik" },
                new SelectListItem() { Text = "Baik" },
                new SelectListItem() { Text = "Sedang" },
                new SelectListItem() { Text = "Jelek" },
            };
            ViewBag.Kondisi = new SelectList(Kondisi, "Text", "Text", lspop.Kondisi);

            List<SelectListItem> Atap = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Decrabon/Beton/Gtg glazur" },
                new SelectListItem() { Text = "Gtg beton/Alumunium" },
                new SelectListItem() { Text = "Gtg biasa/Sirap" },
                new SelectListItem() { Text = "Asbes" },
                new SelectListItem() { Text = "Seng" },
            };
            ViewBag.Atap = new SelectList(Atap, "Text", "Text", lspop.Atap);

            List<SelectListItem> Dinding = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Kaca alumunium" },
                new SelectListItem() { Text = "Beton" },
                new SelectListItem() { Text = "Batu bata/Conblock" },
                new SelectListItem() { Text = "Kayu" },
                new SelectListItem() { Text = "Seng" },
            };
            ViewBag.Dinding = new SelectList(Dinding, "Text", "Text", lspop.Dinding);

            List<SelectListItem> Lantai = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Marmer" },
                new SelectListItem() { Text = "Keramik" },
                new SelectListItem() { Text = "Teraso" },
                new SelectListItem() { Text = "Ubin Pc/Papan" },
                new SelectListItem() { Text = "Semen" },
            };
            ViewBag.Lantai = new SelectList(Lantai, "Text", "Text", lspop.Lantai);

            List<SelectListItem> Langit2 = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Akustik/Jati" },
                new SelectListItem() { Text = "Triplek/Asbes bambu" },
                new SelectListItem() { Text = "Tidak ada" },
            };
            ViewBag.Langit2 = new SelectList(Langit2, "Text", "Text", lspop.Langit2);

            List<SelectListItem> kolam = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Diplester" },
                new SelectListItem() { Text = "Dengan pelapis" },
            };
            ViewBag.Kolam_Jenis = new SelectList(kolam, "Text", "Text", lspop.KolamJenis);

            List<SelectListItem> Pagar_Jenis = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Baja/Besi" },
                new SelectListItem() { Text = "Bata/Batako" },
                new SelectListItem() { Text = "Kombinasi" },
            };
            ViewBag.Pagar_Jenis = new SelectList(Pagar_Jenis, "Text", "Text", lspop.PagarJenis);

            ViewData["IdJenis"] = new SelectList(_context.RefJenisBangunan, "IdJenis", "Jenis", lspop.IdJenis);
            return View(lspop);
        }

        [Auth(new string[] { "Developers", "PBB" })]
        public async Task<IActionResult> NJOP(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lspop = await _context.Lspop.FirstOrDefaultAsync(s => s.IdLspop == id);

            //Link
            ViewBag.L = Url.Action("LSPOP", "PBB", new { id = lspop.IdSpop });
            ViewBag.L1 = Url.Action("NJOP", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            if (lspop == null)
            {
                return NotFound();
            }

            return View(lspop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NJOP(Guid id, [Bind("IdLspop,Njop,Eu,Ed")] Lspop lspop)
        {
            if (id != lspop.IdLspop)
            {
                return NotFound();
            }

            var old = _context.Lspop.FirstOrDefault(d => d.IdLspop == id);

            if (ModelState.IsValid)
            {
                try
                {
                    old.Njop = lspop.Njop;
                    old.Eu = HttpContext.Session.GetString("User");
                    old.Ed = DateTime.Now;
                    _context.Update(old);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LspopExists(lspop.IdLspop))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "edit";
                return RedirectToAction("LSPOP", "PBB", new { id = old.IdSpop });
            }
            return View(lspop);
        }

        [Auth(new string[] { "Developers", "Validasi" })]
        public async Task<IActionResult> Validasi(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lspop = await _context.Lspop
                .Include(l => l.IdJenisNavigation)
                .Include(l => l.IdSpopNavigation)
                .FirstOrDefaultAsync(m => m.IdLspop == id);
            if (lspop == null)
            {
                return NotFound();
            }

            var sppt = _context.Sppt.FirstOrDefault(d => d.IdSpop == lspop.IdSpop);
            if (sppt != null && lspop.FlagValidasi)
            {
                TempData["status"] = "validbatalerror";
                return RedirectToAction("LSPOP", "PBB", new { id = lspop.IdSpop });
            }

            //Link
            ViewBag.L = Url.Action("LSPOP", "PBB", new { id = lspop.IdSpop });
            ViewBag.L1 = Url.Action("Validasi", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View(lspop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Validasi(Guid id, Lspop lspop)
        {
            if (id != lspop.IdLspop)
            {
                return NotFound();
            }

            var old = _context.Lspop.FirstOrDefault(d => d.IdLspop == id);

            old.Eu = HttpContext.Session.GetString("User");
            old.Ed = DateTime.Now;

            if (old.FlagValidasi)
            {
                old.FlagValidasi = false;
                _context.Update(old);
                await _context.SaveChangesAsync();
                TempData["status"] = "validbatal";
            }
            else
            {
                old.FlagValidasi = true;
                _context.Update(old);
                await _context.SaveChangesAsync();
                TempData["status"] = "valid";
            }
            return RedirectToAction("LSPOP", "PBB", new { id = old.IdSpop });
        }

        [Auth(new string[] { "Developers", "PBB" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lspop = await _context.Lspop
                .FirstOrDefaultAsync(m => m.IdLspop == id);
            if (lspop == null)
            {
                return NotFound();
            }

            if (lspop.FlagValidasi)
            {
                return PartialView("_deletevalid");
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var lspop = await _context.Lspop.FindAsync(id);
            try
            {
                _context.Lspop.Remove(lspop);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["status"] = "deletefailed";
                string url = Url.Action("LSPOP", "PBB", new { id = lspop.IdSpop });
                return Json(new { success = true, url });
            }
            
            TempData["status"] = "delete";
            string link = Url.Action("LSPOP", "PBB", new { id = lspop.IdSpop });
            return Json(new { success = true, url = link });
        }

        private bool LspopExists(Guid id)
        {
            return _context.Lspop.Any(e => e.IdLspop == id);
        }
    }
}
