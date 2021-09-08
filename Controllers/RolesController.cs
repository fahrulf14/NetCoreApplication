﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attributes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIP.Models;
using SIP.ViewModels;

namespace SIP.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DB_NewContext _context;
        public RolesController(DB_NewContext context, RoleManager<IdentityRole> roleMgr)
        {
            _roleManager = roleMgr;
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

            var data = (from role in _context.AspNetRoles
                        join userrole in _context.AspNetUserRoles on role.Id equals userrole.RoleId
                        join user in _context.AspNetUsers on userrole.UserId equals user.Id
                        join Personal in _context.Personal on user.Email equals Personal.Email
                        where role.Name != "Developers"
                        select new 
                        {
                            role.Id,
                            Role = role.Name,
                            User = Personal.Nama
                        });

            List<ListRole> listRole = new List<ListRole>();

            foreach (var item in data)
            {
                listRole.Add(new ListRole
                {
                    Id = item.Id,
                    Role = item.Role,
                    User = item.User
                });
            }

            var subselect = (from d in listRole select d.Id).ToList();
            var sisa = (from role in _context.AspNetRoles
                       where !subselect.Contains(role.Id) && role.Name != "Developers"
                       select role).ToList();
            ViewBag.Sisa = sisa;

            return View(listRole);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        public IActionResult Modul()
        {
            //Link
            ViewBag.L = Url.Action("Modul");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var data = (from user in _context.AspNetUsers
                        join userrole in _context.AspNetUserRoles on user.Id equals userrole.UserId
                        join role in _context.AspNetRoles on userrole.RoleId equals role.Id
                        join Personal in _context.Personal on user.Email equals Personal.Email
                        where Personal.Nama != "Developers"
                        select new
                        {
                            Personal.Id,
                            Role = role.Name,
                            User = Personal.Nama
                        });

            List<ListUser> listUser = new List<ListUser>();

            foreach (var item in data)
            {
                listUser.Add(new ListUser
                {
                    Id = item.Id,
                    Role = item.Role,
                    Nama = item.User
                });
            }

            var subselect = (from d in listUser select d.Id).ToList();
            var sisa = (from Personal in _context.Personal
                        where Personal.Email != null && !subselect.Contains(Personal.Id) && Personal.Nama != "Developers"
                        select Personal).ToList();
            ViewBag.Sisa = sisa;

            return View(listUser);
        }

        [HttpPost]
        public JsonResult ChangeRoles(string user, string role, bool check)
        {
            if (check)
            {
                AspNetUserRoles aspNetUserRoles = new AspNetUserRoles
                {
                    RoleId = role,
                    UserId = user
                };
                _context.AspNetUserRoles.Add(aspNetUserRoles);
            }
            else
            {
                var aspNetUserRoles = _context.AspNetUserRoles.FirstOrDefault(d => d.RoleId == role && d.UserId == user);
                _context.Remove(aspNetUserRoles);
            }
            _context.SaveChanges();
            return Json(new { success = true });
        }

        public JsonResult LoadUsers(string id)
        {
            var data = (from role in _context.AspNetUserRoles
                        where role.RoleId == id && role.RoleId != "4ac0100f-3192-493b-92b5-3b3336f215ed"
                        select new
                        {
                            iduser = role.UserId
                        }).ToList();
            return Json(data);
        }

        public JsonResult LoadRoles(string id)
        {
            var data = (from role in _context.AspNetUserRoles
                        where role.UserId == id where role.RoleId != "4ac0100f-3192-493b-92b5-3b3336f215ed"
                        select new
                        {
                            idrole = role.RoleId
                        }).ToList();
            return Json(data);
        }

        [Auth(new string[] { "Developers" })]
        [AjaxOnly]
        public IActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,NormalizedName,ConcurrencyStamp")] AspNetRoles aspNetRoles)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(aspNetRoles.Name));
                if (result.Succeeded)
                {
                    TempData["status"] = "create";
                    var url = Url.Action("Index");
                    return Json(new { success = true, url });
                }
            }
            return PartialView(aspNetRoles);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetRoles = await _context.AspNetRoles.FindAsync(id);
            if (aspNetRoles == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.Title = "Ubah Akses " + aspNetRoles.Name;
            ViewBag.L = Url.Action("Index");
            ViewBag.L1 = Url.Action("Edit", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var data = (from user in _context.AspNetUsers
                        join Personal in _context.Personal on user.Email equals Personal.Email
                        where Personal.Nama != "Developers"
                        select new
                        {
                            UserId = user.Id,
                            Personal.Nama
                        });

            List<UserRoleEdit> ListUser = new List<UserRoleEdit>();

            foreach (var item in data)
            {
                ListUser.Add(new UserRoleEdit
                {
                    UserId = item.UserId,
                    Nama = item.Nama
                });

            }

            ViewBag.Personal = ListUser;

            return View(aspNetRoles);
        }

        [Auth(new string[] { "Developers", "Setting" })]
        public async Task<IActionResult> Update(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var Personal = await _context.Personal.FirstOrDefaultAsync(p => p.Id == id);
            if (Personal == null)
            {
                return NotFound();
            }

            //Link
            ViewBag.Title = "Ubah Akses " + Personal.Nama;
            ViewBag.L = Url.Action("Modul");
            ViewBag.L1 = Url.Action("Update", new { id });
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var user = _context.AspNetUsers.FirstOrDefault(d => d.Email == Personal.Email);

            var roles = _context.AspNetRoles.Where(d => d.Name != "Developers").ToList();

            ViewBag.Position = _context.RF_Positions.Where(d => d.Id == Personal.PositionId).Select(d => d.Position).FirstOrDefault();

            ViewBag.IdUser = user.Id;
            ViewBag.Roles = roles;

            return View(Personal);
        }

        [Auth(new string[] { "Developers" })]
        [AjaxOnly]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetRoles = await _context.AspNetRoles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspNetRoles == null)
            {
                return NotFound();
            }

            return PartialView("_delete");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var aspNetRoles = await _context.AspNetRoles.FindAsync(id);
            try
            {
                _context.AspNetRoles.Remove(aspNetRoles);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["status"] = "deletefailed";
                string url = Url.Action("Index");
                return Json(new { success = true, url });
            }
            
            TempData["status"] = "delete";
            string link = Url.Action("Index", "Roles");
            return Json(new { success = true, url = link });
        }

        private bool RolesExists(string id)
        {
            return _context.AspNetRoles.Any(e => e.Id == id);
        }
    }
}
