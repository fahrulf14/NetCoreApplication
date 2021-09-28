using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NUNA.Models;
using NUNA.Models.BaseApplicationContext;
using NUNA.Services;
using NUNA.ViewModels;
using NUNA.ViewModels.Personal;

namespace NUNA.Controllers
{
    public class PersonalController : Controller
    {
        private readonly BaseApplicationContext _appContext;
        private readonly MenuService _menuService = new MenuService();

        public PersonalController(BaseApplicationContext context)
        {
            _appContext = context;
        }

        [Authorization(Permission.Personal)]
        public async Task<IActionResult> Index()
        {
            //Link
            ViewBag.L = Url.Action("Index");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var dB_NewContext = (from a in _appContext.Personals
                                 join b in _appContext.AspNetUsers on a.UserName equals b.UserName into user
                                 from b in user.DefaultIfEmpty()
                                 select new ListAccountDto
                                 {
                                     PersonalId = a.Id,
                                     Name = a.Name,
                                     Username = a.UserName,
                                     Email = b.Email != null ? b.Email : "-",
                                     Gender = a.Gender == "L" ? "Male" : "Female",
                                     IsActive = a.IsActive
                                 });

            return View(await dB_NewContext.ToListAsync());
        }

        public async Task<IActionResult> Views()
        {
            //Link
            ViewBag.L = Url.Action("Views");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var dB_NewContext = (from a in _appContext.Personals
                                 join b in _appContext.AspNetUsers on a.UserName equals b.UserName into user
                                 from b in user.DefaultIfEmpty()
                                 select new ListAccountDto
                                 {
                                     PersonalId = a.Id,
                                     Name = a.Name,
                                     Email = b.Email != null ? b.Email : "-",
                                     Gender = a.Gender == "L" ? "Male" : "Female",
                                     IsActive = a.IsActive
                                 });

            return View(await dB_NewContext.ToListAsync());
        }

        [Authorization(Permission.Personal_Crate)]
        [AjaxOnly]
        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Personals Personal)
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
            return PartialView(Personal);
        }

        [Authorization(Permission.Personal_Edit)]
        [AjaxOnly]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Personal = await _appContext.Personals.FindAsync(id);
            if (Personal == null)
            {
                return NotFound();
            }
            return PartialView(Personal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Personals Personal)
        {
            if (id != Personal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var old = _appContext.Personals.Find(id);
                try
                {
                    old.IsActive = Personal.IsActive;
                    old.Name = Personal.Name;
                    //old.UserName = Personal.UserName;
                    _appContext.Update(old);
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
            return PartialView(Personal);
        }

        [Authorization(Permission.Personal_Delete)]
        [AjaxOnly]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var Personal = await _appContext.Personals.FirstOrDefaultAsync(m => m.Id == id);
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
            var Personal = await _appContext.Personals.FindAsync(id);

            var user = _appContext.AspNetUsers.FirstOrDefault(d => d.UserName == Personal.UserName);
            if (user != null)
            {
                TempData["status"] = "deletefailed";
                string url = Url.Action("Index");
                return Json(new { success = true, url });
            }

            try
            {
                _appContext.Personals.Remove(Personal);
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
            return _appContext.Personals.Any(e => e.Id == id);
        }
    }
}
