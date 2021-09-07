using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SIP.Models;

namespace SIP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DB_NewContext _context;

        public HomeController(ILogger<HomeController> logger, DB_NewContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Title = "Home";

            //var tlra = _context.TransaksiLra.ToList();
            //_context.RemoveRange(tlra);

            //var skpdn = _context.Skpdn.ToList();
            //_context.RemoveRange(skpdn);

            //var sts = _context.Sts.Where(d => d.FlagValidasi).ToList();
            //foreach(var item in sts)
            //{
            //    var data = _context.Sts.FirstOrDefault(d => d.IdSts == item.IdSts);
            //    data.FlagValidasi = false;
            //    _context.Sts.Update(data);
            //}

            //var sspd = _context.Sspd.Where(d => d.FlagBayar).ToList();
            //foreach(var item in sspd)
            //{
            //    var old = _context.Sspd.FirstOrDefault(d => d.IdSspd == item.IdSspd);
            //    old.FlagBayar = false;
            //    old.TanggalBayar = null;
            //    old.IdBank = null;
            //    old.StatusSetor = false;
            //    _context.Sspd.Update(old);

            //    if (old.IdSptpd != null)
            //    {
            //        var data = _context.Sptpd.FirstOrDefault(d => d.IdSptpd == old.IdSptpd);
            //        data.Sk = "SSPD";
            //        data.Keterangan = 0;
            //        data.KreditPajak = 0;

            //        _context.Sptpd.Update(data);
            //    }
            //    else if (old.IdSkpd != null)
            //    {
            //        var data = _context.Skpd.Include(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdSkpd == old.IdSkpd);
            //        data.Sk = "SSPD";
            //        data.Keterangan = 0;
            //        data.KreditPajak = 0;

            //        _context.Skpd.Update(data);
            //    }
            //    else if (old.IdSkpdkb != null)
            //    {
            //        var data = _context.Skpdkb.Include(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdSkpdkb == old.IdSkpdkb);
            //        data.Sk = "SSPD";
            //        data.Keterangan = 0;
            //        data.KreditPajak = 0;

            //        _context.Skpdkb.Update(data);
            //    }
            //    else if (old.IdSkpdkbt != null)
            //    {
            //        var data = _context.Skpdkbt.Include(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdSkpdkbt == old.IdSkpdkbt);
            //        data.Sk = "SSPD";
            //        data.Keterangan = 0;
            //        data.KreditPajak = 0;

            //        _context.Skpdkbt.Update(data);
            //    }
            //    else if (old.IdStpd != null)
            //    {
            //        var data = _context.Stpd.Include(d => d.IdSkpdNavigation).ThenInclude(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdStpd == old.IdStpd);
            //        data.Sk = "SSPD";
            //        data.Keterangan = 0;
            //        data.KreditPajak = 0;

            //        _context.Stpd.Update(data);
            //    }
            //}

            //var lra = _context.Lra.Where(d => d.Tahun == 2020).ToList();
            //foreach (var item in lra)
            //{
            //    var data = _context.Lra.FirstOrDefault(d => d.IdLra == item.IdLra);
            //    data.Realisasi = 0;
            //    _context.Lra.Update(data);
            //}

            //_context.SaveChanges();

            return View();
        }

        public IActionResult DashBoard()
        {
            return View();
        }
    }
}
