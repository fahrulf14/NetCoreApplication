using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIP.Models;

namespace SIP.Components
{
    public class DetailSkrdViewComponent : ViewComponent
    {
        private readonly DB_NewContext _context;

        public DetailSkrdViewComponent(DB_NewContext context)
        {
            _context = context;

        }

        public async Task<IViewComponentResult> InvokeAsync(Guid id)
        {
            var model = _context.Skrd.Include(d => d.IdSubjekNavigation).Include(d => d.IdCoaNavigation).FirstOrDefault(d => d.IdSkrd == id);

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
            

            return await Task.FromResult((IViewComponentResult)View("Default", model));
        }
    }
}
