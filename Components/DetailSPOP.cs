using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIP.Models;

namespace SIP.Components
{
    public class DetailSPOPViewComponent : ViewComponent
    {
        private readonly DB_NewContext _context;

        public DetailSPOPViewComponent(DB_NewContext context)
        {
            _context = context;

        }

        public async Task<IViewComponentResult> InvokeAsync(Guid id)
        {
            var spop = await _context.Spop
               .Include(s => s.IdSubjekNavigation).ThenInclude(s => s.IndKabKota)
               .Include(s => s.IdSubjekNavigation).ThenInclude(s => s.IndKecamatan)
               .Include(s => s.IdSubjekNavigation).ThenInclude(s => s.IndKelurahan)
               .Include(s => s.IndKabKota)
               .Include(s => s.IndKecamatan)
               .Include(s => s.IndKelurahan)
               .FirstOrDefaultAsync(m => m.IdSpop == id);

            return await Task.FromResult((IViewComponentResult)View("Default", spop));
        }
    }
}
