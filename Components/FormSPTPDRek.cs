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
    public class FormSPTPDRekViewComponent : ViewComponent
    {
        private readonly DB_NewContext _context;

        public FormSPTPDRekViewComponent(DB_NewContext context)
        {
            _context = context;

        }

        public IViewComponentResult Invoke(Guid id, string? jenis)

        {
            var model = _context.Sptpd.Find(id);

            if (jenis == "Bulanan")
            {
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
            }
            else
            {
                var year = DateTime.Now.Year;
                List<SelectListItem> tahunList = new List<SelectListItem>();
                int tahun = year - 2;
                for (var i = 0; i < 5; i++)
                {
                    var th = tahun + i;
                    tahunList.Add(new SelectListItem() { Text = th.ToString(), Value = th.ToString() });
                }
                ViewData["Tahun"] = new SelectList(tahunList, "Value", "Text", model.MasaPajak1.HasValue ? model.MasaPajak1.Value.Year : year - 2);
            }

            return View("Default", model);
        }
    }
}
