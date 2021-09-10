using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIP.Models;
using SIP.ViewModels;

namespace SIP.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly BaseApplicationContext _appContext;

        public UserAccountController(BaseApplicationContext context)
        {
            _appContext = context;
        }

        [Auth(new string[] { "Developers", "Setting" })]
        public IActionResult Index()
        {
            var data = (from u in _appContext.AspNetUsers
                        join p in _appContext.Personal on u.Email equals p.Email
                        join j in _appContext.RF_Positions on p.PositionId equals j.Id
                        where p.Nama != "Developers"
                        select new
                        {
                            p.Id,
                            IdUser = u.Id,
                            p.Nama,
                            p.Email,
                            u.PhoneNumber,
                            j.Position,
                            u.LockoutEnabled
                        }).ToList();

            List<UserPersonal> List = new List<UserPersonal>();

            foreach (var item in data)
            {
                List.Add(new UserPersonal
                {
                    PersonalId = item.Id,
                    UserId = item.IdUser,
                    Nama = item.Nama,
                    Position = item.Position,
                    Email = item.Email,
                    PhoneNumber = item.PhoneNumber,
                    LockoutEnabled = item.LockoutEnabled
                });
            }

            return View(List.ToList());
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUsers = await _appContext.AspNetUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspNetUsers == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var aspNetUsers = await _appContext.AspNetUsers.FindAsync(id);
            try
            {
                _appContext.AspNetUsers.Remove(aspNetUsers);

                var Personal = _appContext.Personal.FirstOrDefault(d => d.Email == aspNetUsers.Email);
                Personal.Email = null;
                _appContext.Personal.Update(Personal);
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
    }
}
