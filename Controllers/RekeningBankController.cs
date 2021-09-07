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
    public class RekeningBankController : Controller
    {
        private readonly DB_NewContext _context;

        public RekeningBankController(DB_NewContext context)
        {
            _context = context;
        }

        [Auth(new string[] { "Developers", "Setting" })]
        public async Task<IActionResult> Index()
        {
            //Link
            ViewBag.L = Url.Action("Index");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var dB_NewContext = _context.Bank.Include(b => b.IdSetoranNavigation).Include(b => b.IdRefNavigation);
            return View(await dB_NewContext.ToListAsync());
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public IActionResult Create()
        {
            ViewData["IdSetoran"] = new SelectList(_context.RefJenisSetoran.Where(d => d.IdSetoran != 3), "IdSetoran", "Jenis");
            ViewData["IdRef"] = new SelectList(_context.RefBank.Where(d => d.Status), "IdBank", "NamaBank");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdBank,IdRef,IdSetoran,Cabang,NoRek,NamaRek,Status")] Bank bank)
        {
            if (ModelState.IsValid)
            {
                bank.IdBank = Guid.NewGuid();
                _context.Add(bank);
                await _context.SaveChangesAsync();
                TempData["status"] = "create";
                string link = Url.Action("Index");
                return Json(new { success = true, url = link });
            }
            ViewData["IdSetoran"] = new SelectList(_context.RefJenisSetoran.Where(d => d.IdSetoran != 3), "IdSetoran", "Jenis", bank.IdSetoran);
            ViewData["IdRef"] = new SelectList(_context.RefBank.Where(d => d.Status), "IdBank", "NamaBank", bank.IdRef);
            return PartialView(bank);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bank = await _context.Bank.FindAsync(id);
            if (bank == null)
            {
                return NotFound();
            }
            ViewData["IdSetoran"] = new SelectList(_context.RefJenisSetoran.Where(d => d.IdSetoran != 3), "IdSetoran", "Jenis", bank.IdSetoran);
            ViewData["IdRef"] = new SelectList(_context.RefBank.Where(d => d.Status), "IdBank", "NamaBank", bank.IdRef);
            return PartialView(bank);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdBank,IdRef,IdSetoran,Cabang,NoRek,NamaRek,Status")] Bank bank)
        {
            if (id != bank.IdBank)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bank);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BankExists(bank.IdBank))
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
            ViewData["IdSetoran"] = new SelectList(_context.RefJenisSetoran.Where(d => d.IdSetoran != 3), "IdSetoran", "Jenis", bank.IdSetoran);
            ViewData["IdRef"] = new SelectList(_context.RefBank.Where(d => d.Status), "IdBank", "IdBank", bank.IdRef);
            return PartialView(bank);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bank = await _context.Bank
                .Include(b => b.IdSetoranNavigation)
                .Include(b => b.IdRefNavigation)
                .FirstOrDefaultAsync(m => m.IdBank == id);
            if (bank == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var bank = await _context.Bank.FindAsync(id);
            try
            {
                _context.Bank.Remove(bank);
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

        private bool BankExists(Guid id)
        {
            return _context.Bank.Any(e => e.IdBank == id);
        }
    }
}
