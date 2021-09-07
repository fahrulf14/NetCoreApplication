using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SIP.Models;

namespace SIP.Areas.Identity.Pages.Account
{
    public class AccountController : Controller
    {
        private readonly DB_NewContext _context;

        public AccountController(DB_NewContext context)
        {
            _context = context;
        }

        //GET: LOGIN
        public IActionResult Login_Partial()
        {
            return PartialView();
        }

        ////POST: LOGIN
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login_Partia()
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(refBadanHukum);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!RefBadanHukumExists(refBadanHukum.IdBadanHukum))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        TempData["status"] = "edit";
        //        string link = Url.Action("Index", "BadanHukum");
        //        return Json(new { success = true, url = link });
        //    }
        //    return PartialView(refBadanHukum);
        //}
    }
}