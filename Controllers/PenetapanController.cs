using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIP.Models;
using SIP.ViewModels;
using Attributes;

namespace SIP.Controllers
{
    public class PenetapanController : Controller
    {
        private readonly DB_NewContext _context;

        public PenetapanController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public IActionResult Index()
        {
            //Link
            ViewBag.L = Url.Action("Index", null);
            ViewBag.L1 = Url.Action("Index", null);
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View();
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
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

        [Auth(new string[] { "Developers", "Penetapan" })]
        public async Task<IActionResult> Kewajiban(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sptpd = await _context.Sptpd
                .Include(d => d.IdCoaNavigation)
                .Include(d => d.IdSubjekNavigation)
                .Include(d => d.IdUsahaNavigation)
                .Where(d => d.IdUsaha == id).ToListAsync();
            ViewBag.IdUsaha = id;
            ViewBag.IdSubjek = _context.DataUsaha.FirstOrDefault(d => d.IdUsaha == id).IdSubjek;

            //Link
            ViewBag.L = Url.Action("Kewajiban", new { id });
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View(sptpd);

        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public async Task<IActionResult> SKPD(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = await _context.Sptpd.Include(s => s.IdCoaNavigation).FirstOrDefaultAsync(s => s.IdSptpd == id);
            if (data == null)
            {
                return NotFound();
            }

            var SkList = _context.Skpd.Include(d => d.IdSptpdNavigation).Include(d => d.Sspd).Where(d => d.IdSptpdNavigation.IdUsaha == data.IdUsaha && d.IdSptpdNavigation.IdCoa == data.IdCoa);

            var Sptpd = await _context.Sptpd
                .Include(s => s.IdCoaNavigation)
                .Where(s => s.IdUsaha == data.IdUsaha && s.IdCoa == data.IdCoa && s.FlagValidasi && s.Sk == null).ToListAsync();

            var Skpd = Sptpd.Where(d => d.Keterangan == 0);

            ViewBag.IdSptpd = id;
            ViewBag.IdUsaha = data.IdUsaha;

            ViewBag.ListData = Skpd.ToList();
            ViewBag.List = Skpd.Count();

            //Link
            ViewBag.Title = "SKPD Pajak " + data.IdCoaNavigation.Uraian;
            ViewBag.Portlet = data.IdCoaNavigation.Uraian;
            ViewBag.L = Url.Action("Kewajiban", new { id = data.IdUsaha });
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View(await SkList.ToListAsync());
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public async Task<IActionResult> SKPDKB(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = await _context.Sptpd.Include(s => s.IdCoaNavigation).FirstOrDefaultAsync(s => s.IdSptpd == id);
            if (data == null)
            {
                return NotFound();
            }

            var SkList = _context.Skpdkb.Include(d => d.IdSptpdNavigation).Include(d => d.Sspd).Where(d => d.IdSptpdNavigation.IdUsaha == data.IdUsaha && d.IdSptpdNavigation.IdCoa == data.IdCoa);

            var Sptpd = await _context.Sptpd
                .Include(s => s.IdCoaNavigation)
                .Where(s => s.IdUsaha == data.IdUsaha && s.IdCoa == data.IdCoa && s.FlagValidasi && s.Sk == null).ToListAsync();

            var Skpdkb = Sptpd.Where(d => d.Keterangan == 0 || d.Keterangan == 2);

            ViewBag.IdSptpd = id;
            ViewBag.IdUsaha = data.IdUsaha;

            ViewBag.ListData = Skpdkb.ToList();
            ViewBag.List = Skpdkb.Count();

            //Link
            ViewBag.Title = "SKPDKB Pajak " + data.IdCoaNavigation.Uraian;
            ViewBag.Portlet = data.IdCoaNavigation.Uraian;
            ViewBag.L = Url.Action("Kewajiban", new { id = data.IdUsaha });
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View(await SkList.ToListAsync());
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public async Task<IActionResult> SKPDKBT(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = await _context.Sptpd.Include(s => s.IdCoaNavigation).FirstOrDefaultAsync(s => s.IdSptpd == id);
            if (data == null)
            {
                return NotFound();
            }

            var SkList = _context.Skpdkbt.Include(d => d.IdSptpdNavigation).Include(d => d.Sspd).Where(d => d.IdSptpdNavigation.IdUsaha == data.IdUsaha && d.IdSptpdNavigation.IdCoa == data.IdCoa);

            var Sptpd = await _context.Sptpd
                .Include(s => s.IdCoaNavigation)
                .Where(s => s.IdUsaha == data.IdUsaha && s.IdCoa == data.IdCoa && s.FlagValidasi && s.Sk == null).ToListAsync();

            var Skpdkbt = Sptpd.Where(d => d.Keterangan == 0 || d.Keterangan == 1 || d.Keterangan == 2);

            ViewBag.IdSptpd = id;
            ViewBag.IdUsaha = data.IdUsaha;

            ViewBag.ListData = Skpdkbt.ToList();
            ViewBag.List = Skpdkbt.Count();

            //Link
            ViewBag.Title = "SKPDKBT Pajak " + data.IdCoaNavigation.Uraian;
            ViewBag.Portlet = data.IdCoaNavigation.Uraian;
            ViewBag.L = Url.Action("Kewajiban", new { id = data.IdUsaha });
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View(await SkList.ToListAsync());
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public async Task<IActionResult> SKPDN(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = await _context.Sptpd.Include(s => s.IdCoaNavigation).FirstOrDefaultAsync(s => s.IdSptpd == id);
            if (data == null)
            {
                return NotFound();
            }

            var SkList = _context.Skpdn
                .Include(d => d.IdSspdNavigation).ThenInclude(d => d.IdSptpdNavigation)
                .Where(d => d.IdSspdNavigation.IdUsaha == data.IdUsaha && d.IdCoa == data.IdCoa && d.NoSkpdn != null);

            var Skpdn = await _context.Skpdn.Include(d => d.IdSspdNavigation).Where(d => d.IdSspdNavigation.IdUsaha == data.IdUsaha && d.IdCoa == data.IdCoa && d.NoSkpdn == null).ToListAsync();

            ViewBag.IdSptpd = id;
            ViewBag.IdUsaha = data.IdUsaha;

            ViewBag.ListData = Skpdn;
            ViewBag.List = Skpdn.Count();
            ViewBag.Jenis = data.IdCoaNavigation.Jenis;

            //Link
            ViewBag.Title = "SKPDN Pajak " + data.IdCoaNavigation.Uraian;
            ViewBag.Portlet = data.IdCoaNavigation.Uraian;
            ViewBag.L = Url.Action("Kewajiban", new { id = data.IdUsaha });
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View(await SkList.ToListAsync());
        }

        [Auth(new string[] { "Developers", "Penetapan" })]
        public async Task<IActionResult> STPD(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var data = await _context.Sptpd.Include(s => s.IdCoaNavigation).FirstOrDefaultAsync(s => s.IdSptpd == id);
            if (data == null)
            {
                return NotFound();
            }

            var SkList = _context.Stpd.Include(d => d.IdSkpdNavigation).ThenInclude(d => d.IdSptpdNavigation).Include(d => d.Sspd).Where(d => d.IdSkpdNavigation.IdSptpdNavigation.IdUsaha == data.IdUsaha && d.IdSkpdNavigation.IdSptpdNavigation.IdCoa == data.IdCoa);

            var Skpd = _context.Skpd.Include(d => d.IdSptpdNavigation).Where(d => d.IdSptpdNavigation.IdCoa == data.IdCoa && d.IdSptpdNavigation.IdUsaha == data.IdUsaha && d.FlagValidasi && d.Sk == null);

            var Stpd = Skpd.Where(d=> d.Keterangan == 0);

            ViewBag.IdSptpd = id;
            ViewBag.IdUsaha = data.IdUsaha;

            ViewBag.ListData = Stpd.ToList();
            ViewBag.List = Stpd.Count();

            //Link
            ViewBag.Title = "STPD Pajak " + data.IdCoaNavigation.Uraian;
            ViewBag.Portlet = data.IdCoaNavigation.Uraian;
            ViewBag.L = Url.Action("Kewajiban", new { id });
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View(await SkList.ToListAsync());
        }
    }
}
