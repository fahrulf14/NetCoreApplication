using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIP.Models;

namespace SIP.Components
{
    public class DetailSkpdkbViewComponent : ViewComponent
    {
        private readonly DB_NewContext _context;

        public DetailSkpdkbViewComponent(DB_NewContext context)
        {
            _context = context;

        }

        public async Task<IViewComponentResult> InvokeAsync(Guid id)
        {
            var model = _context.Skpdkb.Include(d => d.IdSptpdNavigation).ThenInclude(d => d.IdCoaNavigation).FirstOrDefault(d => d.IdSkpdkb == id);

            //Status
            if (model.Keterangan == 0)
            {
                ViewBag.State = "kt-ribbon--danger";
                ViewBag.Status = "Belum Bayar";
            }
            else if (model.Keterangan == 1)
            {
                ViewBag.State = "kt-ribbon--success";
                ViewBag.Status = "Lunas";
            }
            else if (model.Keterangan == 2)
            {
                ViewBag.State = "kt-ribbon--warning";
                ViewBag.Status = "Kurang Bayar";
            }
            else if (model.Keterangan == 3)
            {
                ViewBag.State = "kt-ribbon--primary";
                ViewBag.Status = "Lebih Bayar";
            }
            else if (model.Keterangan == 4)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPD";
            }
            else if (model.Keterangan == 5)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPDKB";
            }
            else if (model.Keterangan == 5)
            {
                ViewBag.State = "kt-ribbon--info";
                ViewBag.Status = "SKPDKBT";
            }

            return await Task.FromResult((IViewComponentResult)View("Default", model));
        }
    }
}
