using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUNA.Models;
using NUNA.Models.BaseApplicationContext;
using NUNA.Services;
using NUNA.ViewModels.Menu;

namespace NUNA.Components
{
    public class AsideViewComponent : ViewComponent
    {
        private readonly BaseApplicationContext _appContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly MenuService _menuService = new MenuService();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;

        public AsideViewComponent(BaseApplicationContext context, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _appContext = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext.Session;
        }

        public async Task<IViewComponentResult> InvokeAsync(string controller, string action)
        {
            if (_session.GetString("Nama") == null || _session.GetString("Email") == null || _session.GetString("Permission") == null)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var role = await _appContext.AspNetUserRoles.Where(d => d.UserId == user.Id).Select(d => d.RoleId).ToListAsync();
                var Personal = await _appContext.Personals.FirstOrDefaultAsync(d => d.UserName.ToLower() == user.UserName.ToLower());

                var permission = (from a in _appContext.AspNetUserPermissions
                                  where a.UserId == user.Id
                                  select a.Permission).ToList();

                var rolePermission = (from a in _appContext.AspNetRolePermissions
                                      where role.Contains(a.RoleId)
                                      select a.Permission).ToList();

                _session.SetString("Email", user.Email);
                _session.SetString("Username", user.UserName);
                _session.SetString("Nama", Personal.Name);
                _session.SetString("Permission", string.Join("|", permission));
                _session.SetString("RolePermission", string.Join("|", rolePermission));
            }

            var userId = await _userManager.GetUserAsync(HttpContext.User);

            var access = (from a in _appContext.AspNetUserMenus
                          where a.UserId == userId.Id
                          select a).ToList();

            var model = _appContext.Menu.Where(d => d.IsActive).OrderBy(d => d.NoUrut).ToList();

            var menuList = (from a in model
                            select new MenuAccessDto
                            {
                                Code = a.Code,
                                Parent = a.Parent,
                                Nama = a.Nama
                            }).ToList();

            var menuAccess = (from a in model
                              join b in access on _menuService.GetMenuAccessName(a.Code, menuList).Nama equals b.Menu
                              select new MenuAccessDto
                              {
                                  Code = a.Code,
                                  Parent = a.Parent,
                                  Nama = b.Menu
                              }).ToList();

            List<string> parent = new List<string>(); 
            foreach(var item in menuAccess)
            {
                var data = item.Nama.Split(".");
                if(data != null) { parent.AddRange(data); }
            }

            var parentMenu = (from a in model
                              where parent.Contains(a.Nama)
                              select a).Distinct().ToList();

            var cekPermission = (from a in model
                                 join b in menuAccess on a.Code equals b.Code
                                 select a).ToList();

            cekPermission.AddRange(parentMenu);

            var menu = _appContext.Menu.Where(d => d.Controller == controller && d.ActionName == action).FirstOrDefault();
            if (menu != null)
            {
                if (menu.Parent != "0")
                {
                    var child = _appContext.Menu.Where(d => d.Code == menu.Parent).FirstOrDefault();
                    if (child.Parent != "0")
                    {
                        var sub = _appContext.Menu.Where(d => d.Code == child.Parent).FirstOrDefault();
                        if (sub.Parent != "0")
                        {
                            ViewBag.Parent = sub.Parent;
                            ViewBag.SubParent = child.Parent;
                            ViewBag.Sub = menu.Parent;
                        }
                        else
                        {
                            ViewBag.Parent = child.Parent;
                            ViewBag.SubParent = menu.Parent;
                        }
                    }
                    else
                    {
                        ViewBag.Parent = menu.Parent;
                    }
                }
                else
                {
                    ViewBag.Parent = "0";
                }
            }

            return await Task.FromResult((IViewComponentResult)View("Default", cekPermission.Distinct().ToList()));
        }
    }
}
