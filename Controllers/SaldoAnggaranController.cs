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
    public class SaldoAnggaranController : Controller
    {
        private readonly DB_NewContext _context;

        public SaldoAnggaranController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "Setting" })]
        public IActionResult Index()
        {
            //Link
            ViewBag.L = Url.Action("Index");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var saldo = _context.SaldoAnggaran.FirstOrDefault();

            return View(saldo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Id,Saldo")] SaldoAnggaran saldoAnggaran)
        {
            if (ModelState.IsValid)
            {
                _context.Update(saldoAnggaran);
                await _context.SaveChangesAsync();
                TempData["status"] = "edit";
                return RedirectToAction(nameof(Index));
            }
            return View(saldoAnggaran);
        }
    }
}
