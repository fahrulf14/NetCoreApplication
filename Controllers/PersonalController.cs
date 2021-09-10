using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIP.Models;
using SIP.ViewModels.Personal;

namespace SIP.Controllers
{
    public class PersonalController : Controller
    {
        private readonly BaseApplicationContext _appContext;

        public PersonalController(BaseApplicationContext context)
        {
            _appContext = context;
        }

        [Auth(new string[] { "Developers", "Setting" })]
        public async Task<IActionResult> Index()
        {
            //Link
            ViewBag.L = Url.Action("Index");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var dB_NewContext = (from a in _appContext.Personal
                                 join b in _appContext.RF_Positions on a.PositionId equals b.Id
                                 select new ListAccountDto
                                 {
                                     PersonalId = a.Id,
                                     Nama = a.Nama,
                                     Nip = a.Nip,
                                     Email = a.Email,
                                     Position = b.Position,
                                     IsActive = a.IsActive
                                 });

            ViewBag.Position = _appContext.RF_Positions.ToList();

            return View(await dB_NewContext.ToListAsync());
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public IActionResult Create()
        {
            ViewData["PositionId"] = new SelectList(_appContext.RF_Positions, "Id", "Position");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nama,UserName,Nip,PositionId,IsActive")] Personal Personal)
        {
            if (ModelState.IsValid)
            {
                Personal.IsActive = true;
                _appContext.Add(Personal);
                await _appContext.SaveChangesAsync();
                TempData["status"] = "create";
                string link = Url.Action("Index");
                return Json(new { success = true, url = link });
            }
            ViewData["PositionId"] = new SelectList(_appContext.RF_Positions, "Id", "Position", Personal.PositionId);
            return PartialView(Personal);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Personal = await _appContext.Personal.FindAsync(id);
            if (Personal == null)
            {
                return NotFound();
            }
            ViewData["PositionId"] = new SelectList(_appContext.RF_Positions, "Id", "Position", Personal.PositionId);
            return PartialView(Personal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nama,UserName,Nip,PositionId,IsActive")] Personal Personal)
        {
            if (id != Personal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var old = _appContext.Personal.Find(id);
                try
                {
                    old.IsActive = Personal.IsActive;
                    old.PositionId = Personal.PositionId;
                    old.Nama = Personal.Nama;
                    //old.UserName = Personal.UserName;
                    old.Nip = Personal.Nip;
                    _appContext.Personal.Update(old);
                    await _appContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonalExists(Personal.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["status"] = "edit";
                string link = Url.Action("Index");
                return Json(new { success = true, url = link });
            }
            ViewData["PositionId"] = new SelectList(_appContext.RF_Positions, "Id", "Position", Personal.PositionId);
            return PartialView(Personal);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var Personal = await _appContext.Personal.FirstOrDefaultAsync(m => m.Id == id);
            if (Personal == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var Personal = await _appContext.Personal.FindAsync(id);

            var user = _appContext.AspNetUsers.FirstOrDefault(d => d.Email == Personal.Email);
            if (user != null)
            {
                TempData["status"] = "deletefailed";
                string url = Url.Action("Index");
                return Json(new { success = true, url });
            }

            try
            {
                _appContext.Personal.Remove(Personal);
                await _appContext.SaveChangesAsync();
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

        private bool PersonalExists(int id)
        {
            return _appContext.Personal.Any(e => e.Id == id);
        }
    }
}
