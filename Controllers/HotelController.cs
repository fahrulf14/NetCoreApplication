using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIP.Models;

namespace SIP.Controllers
{
    public class HotelController : Controller
    {
        private readonly DB_NewContext _context;

        public HotelController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "Parameter" })]
        public async Task<IActionResult> Index()
        {
            //Link
            ViewBag.L = Url.Action("Index");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View(await _context.RefHotel.ToListAsync());
        }

        [Auth(new string[] { "Developers", "Parameter" })]
        [AjaxOnly]
        public IActionResult Create()
        {
            List<SelectListItem> layanan = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Fasilitas Hotel", Value = "Fasilitas Hotel" },
                new SelectListItem() { Text = "Layanan Restoran", Value = "Layanan Restoran" },
                new SelectListItem() { Text = "Layanan Lainnya", Value = "Layanan Lainnya" },
            };
            ViewBag.Layanan = new SelectList(layanan, "Value", "Text");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRef,Uraian,Jenis,FlagAktif")] RefHotel refHotel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(refHotel);
                await _context.SaveChangesAsync();
                TempData["status"] = "create";
                string link = Url.Action("Index");
                return Json(new { success = true, url = link });
            }
            return PartialView(refHotel);
        }

        [Auth(new string[] { "Developers", "Parameter" })]
        [AjaxOnly]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refHotel = await _context.RefHotel.FindAsync(id);
            if (refHotel == null)
            {
                return NotFound();
            }
            List<SelectListItem> layanan = new List<SelectListItem>
            {
                new SelectListItem() { Text = "Fasilitas Hotel", Value = "Fasilitas Hotel" },
                new SelectListItem() { Text = "Layanan Restoran", Value = "Layanan Restoran" },
                new SelectListItem() { Text = "Layanan Lainnya", Value = "Layanan Lainnya" },
            };
            ViewBag.Layanan = new SelectList(layanan, "Value", "Text", refHotel.Jenis);
            return PartialView(refHotel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRef,Uraian,Jenis,FlagAktif")] RefHotel refHotel)
        {
            if (id != refHotel.IdRef)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(refHotel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RefHotelExists(refHotel.IdRef))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "edit";
                string link = Url.Action("Index", "Hotel");
                return Json(new { success = true, url = link });
            }
            return PartialView(refHotel);
        }

        [Auth(new string[] { "Developers", "Parameter" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refHotel = await _context.RefHotel
                .FirstOrDefaultAsync(m => m.IdRef == id);
            if (refHotel == null)
            {
                return NotFound();
            }
            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var refHotel = await _context.RefHotel.FindAsync(id);
            try
            {
                _context.RefHotel.Remove(refHotel);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["status"] = "deletefailed";
                string url = Url.Action("Index");
                return Json(new { success = true, url });
            }

            TempData["status"] = "delete";
            string link = Url.Action("Index");
            return Json(new { success = true, url = link });
        }

        private bool RefHotelExists(int id)
        {
            return _context.RefHotel.Any(e => e.IdRef == id);
        }
    }
}
