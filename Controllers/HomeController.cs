using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SIP.Models.BaseApplicationContext;

namespace SIP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BaseApplicationContext _appContext;

        public HomeController(ILogger<HomeController> logger, BaseApplicationContext context)
        {
            _logger = logger;
            _appContext = context;
        }

        public IActionResult Index()
        {
            ViewBag.Title = "Home";

            //var tlra = _appContext.TransaksiLra.ToList();
            //_appContext.RemoveRange(tlra);

            //var skpdn = _appContext.Skpdn.ToList();
            //_appContext.RemoveRange(skpdn);

            //var sts = _appContext.Sts.Where(d => d.FlagValidasi).ToList();
            //foreach(var item in sts)
            //{
            //    var data = _appContext.Sts.FirstOrDefault(d => d.IdSts == item.IdSts);
            //    data.FlagValidasi = false;
            //    _appContext.Sts.Update(data);
            //}

            //var sspd = _appContext.Sspd.Where(d => d.FlagBayar).ToList();
            //foreach(var item in sspd)
            //{
            //    var old = _appContext.Sspd.FirstOrDefault(d => d.IdSspd == item.IdSspd);
            //    old.FlagBayar = false;
            //    old.TanggalBayar = null;
            //    old.IdBank = null;
            //    old.StatusSetor = false;
            //    _appContext.Sspd.Update(old);

            //    if (old.IdSptpd != null)
            //    {
            //        var data = _appContext.Sptpd.FirstOrDefault(d => d.IdSptpd == old.IdSptpd);
            //        data.Sk = "SSPD";
            //        data.Keterangan = 0;
            //        data.KreditPajak = 0;

            //        _appContext.Sptpd.Update(data);
            //    }
            //    else if (old.IdSkpd != null)
            //    {
            //        var data = _appContext.Skpd.Include(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdSkpd == old.IdSkpd);
            //        data.Sk = "SSPD";
            //        data.Keterangan = 0;
            //        data.KreditPajak = 0;

            //        _appContext.Skpd.Update(data);
            //    }
            //    else if (old.IdSkpdkb != null)
            //    {
            //        var data = _appContext.Skpdkb.Include(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdSkpdkb == old.IdSkpdkb);
            //        data.Sk = "SSPD";
            //        data.Keterangan = 0;
            //        data.KreditPajak = 0;

            //        _appContext.Skpdkb.Update(data);
            //    }
            //    else if (old.IdSkpdkbt != null)
            //    {
            //        var data = _appContext.Skpdkbt.Include(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdSkpdkbt == old.IdSkpdkbt);
            //        data.Sk = "SSPD";
            //        data.Keterangan = 0;
            //        data.KreditPajak = 0;

            //        _appContext.Skpdkbt.Update(data);
            //    }
            //    else if (old.IdStpd != null)
            //    {
            //        var data = _appContext.Stpd.Include(d => d.IdSkpdNavigation).ThenInclude(d => d.IdSptpdNavigation).FirstOrDefault(d => d.IdStpd == old.IdStpd);
            //        data.Sk = "SSPD";
            //        data.Keterangan = 0;
            //        data.KreditPajak = 0;

            //        _appContext.Stpd.Update(data);
            //    }
            //}

            //var lra = _appContext.Lra.Where(d => d.Tahun == 2020).ToList();
            //foreach (var item in lra)
            //{
            //    var data = _appContext.Lra.FirstOrDefault(d => d.IdLra == item.IdLra);
            //    data.Realisasi = 0;
            //    _appContext.Lra.Update(data);
            //}

            //_appContext.SaveChanges();

            return View();
        }

        public IActionResult DashBoard()
        {
            return View();
        }
    }
}
