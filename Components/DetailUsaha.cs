using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIP.Models;

namespace SIP.Components
{
    public class DetailUsahaViewComponent : ViewComponent
    {
        private readonly DB_NewContext _context;

        public DetailUsahaViewComponent(DB_NewContext context)
        {
            _context = context;

        }

        public async Task<IViewComponentResult> InvokeAsync(Guid id)
        {
            var model = _context.DataUsaha.Include(d => d.IdJenisNavigation).Include(d => d.IdSubjekNavigation).Include(d => d.IndKabKota).Include(d => d.IndKecamatan).Include(d => d.IndKelurahan).Include(d => d.IndProvinsi).FirstOrDefault(d => d.IdUsaha == id);
            
            return await Task.FromResult((IViewComponentResult)View("Default", model));
        }
    }
}
