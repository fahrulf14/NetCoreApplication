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
        private readonly DB_NewContext _context;

        public UserAccountController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "Setting" })]
        public IActionResult Index()
        {
            var data = (from u in _context.AspNetUsers
                        join p in _context.Pegawai on u.Email equals p.Email
                        join j in _context.RefJabatan on p.IdJabatan equals j.IdJabatan
                        where p.Nama != "Developers"
                        select new
                        {
                            p.IdPegawai,
                            IdUser = u.Id,
                            p.Nama,
                            p.Email,
                            u.PhoneNumber,
                            j.Jabatan,
                            u.LockoutEnabled
                        }).ToList();

            List<UserPegawai> List = new List<UserPegawai>();

            foreach (var item in data)
            {
                List.Add(new UserPegawai
                {
                    IdPegawai = item.IdPegawai,
                    IdUser = item.IdUser,
                    Nama = item.Nama,
                    Jabatan = item.Jabatan,
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

            var aspNetUsers = await _context.AspNetUsers
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
            var aspNetUsers = await _context.AspNetUsers.FindAsync(id);
            try
            {
                _context.AspNetUsers.Remove(aspNetUsers);

                var pegawai = _context.Pegawai.FirstOrDefault(d => d.Email == aspNetUsers.Email);
                pegawai.Email = null;
                _context.Pegawai.Update(pegawai);
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
    }
}
