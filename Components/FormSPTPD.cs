using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIP.Models;

namespace SIP.Components
{
    public class FormSPTPDViewComponent : ViewComponent
    {
        private readonly DB_NewContext _context;

        public FormSPTPDViewComponent(DB_NewContext context)
        {
            _context = context;

        }

        public IViewComponentResult Invoke(Guid id)

        {
            var model = _context.Sptpd.Find(id);

            List<SelectListItem> bulan = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Januari", Value = "1" },
                new SelectListItem() { Text = "Februari", Value = "2" },
                new SelectListItem() { Text = "Maret", Value = "3" },
                new SelectListItem() { Text = "April", Value = "4" },
                new SelectListItem() { Text = "Mei", Value = "5" },
                new SelectListItem() { Text = "Juni", Value = "6" },
                new SelectListItem() { Text = "Juli", Value = "7" },
                new SelectListItem() { Text = "Agustus", Value = "8" },
                new SelectListItem() { Text = "September", Value = "9" },
                new SelectListItem() { Text = "Oktober", Value = "10" },
                new SelectListItem() { Text = "November", Value = "11" },
                new SelectListItem() { Text = "Desember", Value = "12" }

            };
            ViewData["MasaPajak"] = new SelectList(bulan, "Value", "Text", model.MasaPajak1?.Month);

            return View("Default", model);
        }
    }
}
