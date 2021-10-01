using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NUNA.Models.BaseApplicationContext;

namespace NUNA.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmailSenderController : Controller
    {
        private readonly BaseApplicationContext _context;

        public EmailSenderController(BaseApplicationContext context)
        {
            _context = context;
        }

        // GET: Admin/EmailSender
        public async Task<IActionResult> Index()
        {
            return View(await _context.MS_EmailSender.ToListAsync());
        }

        // GET: Admin/EmailSender/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mS_EmailSender = await _context.MS_EmailSender
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mS_EmailSender == null)
            {
                return NotFound();
            }

            return View(mS_EmailSender);
        }

        // GET: Admin/EmailSender/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/EmailSender/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Sender,SmtpHost,SmtpPort,SmtpUser,SmtpCred,CreationTime,CreationUser,ModificationTime,ModificationUser")] MS_EmailSender mS_EmailSender)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mS_EmailSender);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mS_EmailSender);
        }

        // GET: Admin/EmailSender/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mS_EmailSender = await _context.MS_EmailSender.FindAsync(id);
            if (mS_EmailSender == null)
            {
                return NotFound();
            }
            return View(mS_EmailSender);
        }

        // POST: Admin/EmailSender/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Sender,SmtpHost,SmtpPort,SmtpUser,SmtpCred,CreationTime,CreationUser,ModificationTime,ModificationUser")] MS_EmailSender mS_EmailSender)
        {
            if (id != mS_EmailSender.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mS_EmailSender);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MS_EmailSenderExists(mS_EmailSender.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(mS_EmailSender);
        }

        // GET: Admin/EmailSender/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mS_EmailSender = await _context.MS_EmailSender
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mS_EmailSender == null)
            {
                return NotFound();
            }

            return View(mS_EmailSender);
        }

        // POST: Admin/EmailSender/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mS_EmailSender = await _context.MS_EmailSender.FindAsync(id);
            _context.MS_EmailSender.Remove(mS_EmailSender);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MS_EmailSenderExists(int id)
        {
            return _context.MS_EmailSender.Any(e => e.Id == id);
        }
    }
}
