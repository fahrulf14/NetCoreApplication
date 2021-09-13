﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIP.Helpers;
using SIP.Models.BaseApplicationContext;
using SIP.Services;

namespace SIP.Controllers
{
    public class PositionController : Controller
    {
        private readonly BaseApplicationContext _appContext;
        private readonly SessionHandler _session;

        public PositionController(BaseApplicationContext context, SessionHandler session)
        {
            _appContext = context;
            _session = session;
        }

        [Auth(new string[] { "Developers", "Parameter" })]
        public async Task<IActionResult> Index()
        {
            //Link
            ViewBag.L = Url.Action("Index");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            _session.Set("Key", "Testing");
            _session.Get("Key");

            return View(await _appContext.RF_Positions.ToListAsync());
        }

        [Auth(new string[] { "Developers", "Parameter" })]
        [AjaxOnly]
        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Position,IsActive")] RF_Position refPosition)
        {
            if (ModelState.IsValid)
            {
                _appContext.Add(refPosition);
                await _appContext.SaveChangesAsync();
                TempData["status"] = "create";
                string link = Url.Action("Index");
                return Json(new { success = true, url = link });
            }
            return PartialView(refPosition);
        }

        [Auth(new string[] { "Developers", "Parameter" })]
        [AjaxOnly]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refPosition = await _appContext.RF_Positions.FindAsync(id);
            if (refPosition == null)
            {
                return NotFound();
            }
            return PartialView(refPosition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Position,IsActive")] RF_Position refPosition)
        {
            if (id != refPosition.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _appContext.Update(refPosition);
                    await _appContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RefPositionExists(refPosition.Id))
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
            return PartialView(refPosition);
        }

        [Auth(new string[] { "Developers", "Parameter" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refPosition = await _appContext.RF_Positions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (refPosition == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var refPosition = await _appContext.RF_Positions.FindAsync(id);
            try
            {
                _appContext.RF_Positions.Remove(refPosition);
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

        private bool RefPositionExists(int id)
        {
            return _appContext.RF_Positions.Any(e => e.Id == id);
        }
    }
}
