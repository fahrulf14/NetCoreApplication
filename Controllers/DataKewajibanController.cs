using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIP.Models;

namespace SIP.Controllers
{
    public class DataKewajibanController : Controller
    {
        private readonly DB_NewContext _context;

        public DataKewajibanController(DB_NewContext context)
        {
            _context = context;
        }

        [Route("DataKewajiban/{id}")]
        [Auth(new string[] { "Developers", "Pendataan" })]
        public async Task<IActionResult> Index(Guid? id) 
        {
            var dB_NewContext = await _context.Sptpd
                .Include(d => d.IdCoaNavigation)
                .Include(d => d.IdSubjekNavigation)
                .Include(d => d.IdUsahaNavigation)
                .Where(d => d.IdUsaha == id).ToListAsync();
            ViewBag.IdUsaha = id;
            ViewBag.IdSubjek = _context.DataUsaha.FirstOrDefault(d => d.IdUsaha == id).IdSubjek;

            //Link
            ViewBag.L = Url.Content("/DataKewajiban/" + id);
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View(dB_NewContext);
            
        }

        [Auth(new string[] { "Developers", "Pendataan" })]
        [AjaxOnly]
        public IActionResult Create(Guid? id, string jenis)
        {
            if (id == null || jenis == null)
            {
                return NotFound();
            }

            ViewBag.Jenis = jenis;

            var subselect = _context.Sptpd.Where(d => d.IdUsaha == id).Select(d => d.IdCoa).ToList();
            var coa = (from c in _context.Coa
                      where c.Tingkat == 5 && c.Parent.Substring(0,3) == "411" && c.Status && c.Jenis == jenis && c.Parent != "41112" && !subselect.Contains(c.IdCoa)
                      select new
                      {
                          c.IdCoa,
                          c.Uraian
                      }).ToList();
            ViewData["IdCoa"] = new SelectList(coa.Where(d => d.IdCoa != "4111301"), "IdCoa", "Uraian");

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid id, Sptpd sptpd)
        {
            if (ModelState.IsValid)
            {
                var data = await _context.DataUsaha.AsNoTracking().FirstOrDefaultAsync(d => d.IdUsaha == id);
                var pemda = await _context.Pemda.AsNoTracking().FirstOrDefaultAsync();
                var coa = sptpd.IdCoa.Substring(0, 5);

                sptpd.IdSptpd = Guid.NewGuid();
                sptpd.IdUsaha = id;
                sptpd.IdSubjek = data.IdSubjek;
                sptpd.KreditPajak = 0;
                sptpd.Keterangan = 0;
                _context.Add(sptpd);

                if (coa == "41101")
                {
                    PHotel pHotel = new PHotel
                    {
                        IdHotel = Guid.NewGuid(),
                        IdSptpd = sptpd.IdSptpd
                    };
                    _context.Add(pHotel);
                }
                else if (coa == "41102")
                {
                    PRestoran pRestoran = new PRestoran
                    {
                        IdRestoran = Guid.NewGuid(),
                        IdSptpd = sptpd.IdSptpd
                    };
                    _context.Add(pRestoran);
                }
                else if (coa == "41103")
                {
                    PHiburan pHiburan = new PHiburan
                    {
                        IdHiburan = Guid.NewGuid(),
                        IdSptpd = sptpd.IdSptpd
                    };
                    _context.Add(pHiburan);
                }
                else if (coa == "41104")
                {
                    PReklame pReklame = new PReklame
                    {
                        IdReklame = Guid.NewGuid(),
                        IdSptpd = sptpd.IdSptpd
                    };
                    _context.Add(pReklame);
                }
                else if (coa == "41105")
                {
                    PPenerangan pPenerangan = new PPenerangan
                    {
                        IdPenerangan = Guid.NewGuid(),
                        IdSptpd = sptpd.IdSptpd
                    };
                    _context.Add(pPenerangan);
                }
                else if (coa == "41107")
                {
                    PParkir pParkir = new PParkir
                    {
                        IdParkir = Guid.NewGuid(),
                        IdSptpd = sptpd.IdSptpd
                    };
                    _context.Add(pParkir);
                }
                else if (coa == "41108")
                {
                    PAirTanah pAirTanah = new PAirTanah
                    {
                        IdAirTanah = Guid.NewGuid(),
                        IdSptpd = sptpd.IdSptpd
                    };
                    _context.Add(pAirTanah);
                }
                else if (coa == "41109")
                {
                    PWalet pWalet = new PWalet
                    {
                        IdWalet = Guid.NewGuid(),
                        IdSptpd = sptpd.IdSptpd
                    };
                    _context.Add(pWalet);
                }
                else if (coa == "41111")
                {
                    PMineral pMineral = new PMineral
                    {
                        IdMineral = Guid.NewGuid(),
                        IdSptpd = sptpd.IdSptpd
                    };
                    _context.Add(pMineral);
                }
                await _context.SaveChangesAsync();
                TempData["status"] = "create";
                string link = Url.Content("/DataKewajiban/" + id);
                return Json(new { success = true, url = link });
            }
            return PartialView(sptpd);
        }

        [Auth(new string[] { "Developers", "Pendataan" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sptpd = await _context.Sptpd
                .FirstOrDefaultAsync(m => m.IdSptpd == id);
            if (sptpd == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var sptpd = await _context.Sptpd.FindAsync(id);
            var coa = sptpd.IdCoa.Substring(0, 5);

            try
            {
                if (coa == "41101")
                {
                    var sub = await _context.PHotel.FirstOrDefaultAsync(d => d.IdSptpd == id);
                    _context.PHotel.Remove(sub);
                }
                else if (coa == "41102")
                {
                    var sub = await _context.PRestoran.FirstOrDefaultAsync(d => d.IdSptpd == id);
                    _context.PRestoran.Remove(sub);
                }
                else if (coa == "41103")
                {
                    var sub = await _context.PHiburan.FirstOrDefaultAsync(d => d.IdSptpd == id);
                    _context.PHiburan.Remove(sub);
                }
                else if (coa == "41104")
                {
                    var sub = await _context.PReklame.FirstOrDefaultAsync(d => d.IdSptpd == id);
                    _context.PReklame.Remove(sub);
                }
                else if (coa == "41105")
                {
                    var sub = await _context.PPenerangan.FirstOrDefaultAsync(d => d.IdSptpd == id);
                    _context.PPenerangan.Remove(sub);
                }
                else if (coa == "41107")
                {
                    var sub = await _context.PParkir.FirstOrDefaultAsync(d => d.IdSptpd == id);
                    _context.PParkir.Remove(sub);
                }
                else if (coa == "41108")
                {
                    var sub = await _context.PAirTanah.FirstOrDefaultAsync(d => d.IdSptpd == id);
                    _context.PAirTanah.Remove(sub);
                }
                else if (coa == "41109")
                {
                    var sub = await _context.PWalet.FirstOrDefaultAsync(d => d.IdSptpd == id);
                    _context.PWalet.Remove(sub);
                }
                else if (coa == "41111")
                {
                    var sub = await _context.PMineral.FirstOrDefaultAsync(d => d.IdSptpd == id);
                    _context.PMineral.Remove(sub);
                }

                _context.Sptpd.Remove(sptpd);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["status"] = "deletefailed";
                string url = Url.Content("/DataKewajiban/" + sptpd.IdUsaha);
                return Json(new { success = true, url });
            }
            TempData["status"] = "delete";
            string link = Url.Content("/DataKewajiban/" + sptpd.IdUsaha);
            return Json(new { success = true, url = link });
        }
    }
}
