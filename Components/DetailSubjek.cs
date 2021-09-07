using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIP.Models;

namespace SIP.Components
{
    public class DetailSubjekViewComponent : ViewComponent
    {
        private readonly DB_NewContext _context;

        public DetailSubjekViewComponent(DB_NewContext context)
        {
            _context = context;

        }

        public async Task<IViewComponentResult> InvokeAsync(Guid id, bool edit)
        {
            var model = _context.DataSubjek.Include(d => d.IdBadanHukumNavigation).Include(d => d.IdPekerjaanNavigation).Include(d => d.IndKecamatan).Include(d => d.IndKelurahan).FirstOrDefault(d => d.IdSubjek == id);
            var badan = _context.DataBadan.FirstOrDefault(d => d.IdSubjek == id);
            if (badan != null)
            {
                ViewBag.Jabatan = badan.Jabatan;
                ViewBag.Badan = badan;
            }
            ViewBag.Edit = edit;

            return await Task.FromResult((IViewComponentResult)View("Default", model));
        }
    }
}
