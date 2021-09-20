using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attributes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NUNA.Helpers;
using NUNA.Models.BaseApplicationContext;
using NUNA.Services;
using NUNA.ViewModels;
using NUNA.ViewModels.Menu;
using NUNA.ViewModels.Roles;
using NUNA.ViewModels.Toastr;
using NUNA.ViewModels.UserAccount;

namespace NUNA.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly BaseApplicationContext _appContext;
        private readonly MenuService _menuService = new MenuService();
        private readonly JsonResultService _result = new JsonResultService();
        public RolesController(BaseApplicationContext context, RoleManager<IdentityRole> roleMgr)
        {
            _roleManager = roleMgr;
            _appContext = context;
        }

        public IActionResult Index()
        {
            //Link
            ViewBag.L = Url.Action("Index");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var data = (from role in _appContext.AspNetRoles
                        where role.Name != "Developers"
                        select new ListRoleDto
                        {
                            Id = role.Id,
                            Role = role.Name,
                            UserCount = _appContext.AspNetUserRoles.Where(d => d.RoleId == role.Id).Count(),
                            AccessCount = _appContext.AspNetRolePermissions.Where(d => d.RoleId == role.Id).Count(),
                        }).ToList();

            return View(data);
        }

        public IActionResult Modul()
        {
            //Link
            ViewBag.L = Url.Action("Modul");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            var data = (from user in _appContext.AspNetUsers
                        join userrole in _appContext.AspNetUserRoles on user.Id equals userrole.UserId
                        join role in _appContext.AspNetRoles on userrole.RoleId equals role.Id
                        join Personal in _appContext.Personal on user.Email equals Personal.Email
                        where Personal.Nama != "Developers"
                        select new
                        {
                            Personal.Id,
                            Role = role.Name,
                            User = Personal.Nama
                        });

            List<ListUserDto> listUser = new List<ListUserDto>();

            foreach (var item in data)
            {
                listUser.Add(new ListUserDto
                {
                    Id = item.Id,
                    Role = item.Role,
                    Nama = item.User
                });
            }

            var subselect = (from d in listUser select d.Id).ToList();
            var sisa = (from Personal in _appContext.Personal
                        where Personal.Email != null && !subselect.Contains(Personal.Id) && Personal.Nama != "Developers"
                        select Personal).ToList();
            ViewBag.Sisa = sisa;

            return View(listUser);
        }

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

        public IActionResult Permission(string id)
        {
            var data = (from r in _appContext.AspNetRoles
                        where r.Id == id
                        select new RolePermissionDto
                        {
                            RoleId = r.Id,
                            Role = r.Name,
                            UserCount = _appContext.AspNetUserRoles.Where(d => d.RoleId == id).Count(),
                            AccessCount = _appContext.AspNetRolePermissions.Where(d => d.RoleId == id).Count()
                        }).FirstOrDefault();


            TempData["RoleId"] = data.RoleId;

            return View(data);
        }

        public JsonResult GetPermission()
        {
            List<string> permission = Models.Permission.AppPermission.ToList();

            var RoleId = TempData["RoleId"].ToString();

            var access = (from a in _appContext.AspNetRolePermissions
                          where a.RoleId == RoleId
                          select a).ToList();

            var listPermission = (from a in permission
                                  group a by a.Split(".")[0]
                                  into G
                                  select new
                                  {
                                      id = G.Key,
                                      text = G.Key,
                                      children = (from x in permission
                                                  join y in access on x equals y.Permission into roles
                                                  from y in roles.DefaultIfEmpty()
                                                  where x.Split(".")[0] == G.Key
                                                  select new
                                                  {
                                                      id = x,
                                                      text = x.Split(".")[1],
                                                      icon = x.Split(".")[1] == "Create" ? "fa fa-plus-square kt-font-default" :
                                                                x.Split(".")[1] == "Edit" ? "fa fa-pen-square kt-font-default" :
                                                                x.Split(".")[1] == "Delete" ? "fa fa-minus-square kt-font-default" : "fa fa-check-square kt-font-default",
                                                      state = new
                                                      {
                                                          selected = y != null ? true : false
                                                      }
                                                  })
                                  }).ToList();

            return Json(listPermission);
        }

        [HttpPost]
        public IActionResult UpdatePermission([FromBody] PermissionInputDto input)
        {

            #region Permission
            var permission = (from a in _appContext.AspNetRolePermissions
                              where a.RoleId == input.id
                              select a.Permission).ToList();

            var deletePermission = (from a in permission
                                    where !input.permission.Contains(a)
                                    select new AspNetRolePermissions
                                    {
                                        RoleId = input.id,
                                        Permission = a
                                    }).ToList();

            var insertPermission = (from a in input.permission
                                    where !permission.Contains(a)
                                    select new AspNetRolePermissions
                                    {
                                        RoleId = input.id,
                                        Permission = a
                                    }).ToList();

            if (deletePermission.Count != 0)
            {
                _appContext.RemoveRange(deletePermission);
            }
            if (insertPermission.Count != 0)
            {
                _appContext.AddRange(insertPermission);
            }

            #endregion

            #region Menu
            var menu = (from a in _appContext.AspNetRoleMenus
                        where a.RoleId == input.id
                        select a.Menu).ToList();

            var deleteMenu = (from a in menu
                              where !input.menu.Contains(a)
                              select new AspNetRoleMenus
                              {
                                  RoleId = input.id,
                                  Menu = a
                              }).ToList();

            var insertMenu = (from a in input.menu
                              where !menu.Contains(a)
                              select new AspNetRoleMenus
                              {
                                  RoleId = input.id,
                                  Menu = a
                              }).ToList();

            if (deleteMenu.Count != 0)
            {
                _appContext.RemoveRange(deleteMenu);
            }
            if (insertMenu.Count != 0)
            {
                _appContext.AddRange(insertMenu);
            }

            #endregion

            _appContext.SaveChanges();
            return Json(_result.Success(Url.Action("Index"))).WithSuccess(Message.Save);
        }

        public JsonResult GetMenuAccess()
        {
            List<object> result = new List<object>();
            List<MenuAccessDto> menuAccess = GetMenuList();

            var RoleId = TempData["RoleId"].ToString();

            var access = (from a in _appContext.AspNetRoleMenus
                          where a.RoleId == RoleId
                          select a).ToList();

            var listPermission = (from a in menuAccess.Where(d => d.Code.Length == 2)
                                  join x in access on a.Nama equals x.Menu into xa
                                  from x in xa.DefaultIfEmpty()
                                  where a.Parent == "0"
                                  select new
                                  {
                                      id = a.Nama,
                                      text = a.Nama,
                                      state = new
                                      {
                                          selected = x != null ? true : false
                                      },
                                      children = (from b in menuAccess.Where(d => d.Code.Length == 4)
                                                  join y in access on b.Nama equals y.Menu into yb
                                                  from y in yb.DefaultIfEmpty()
                                                  where b.Nama.Split(".")[0] == a.Nama
                                                  select new
                                                  {
                                                      id = b.Nama,
                                                      text = b.Nama.Split(".")[1],
                                                      state = new
                                                      {
                                                          selected = y != null ? true : false
                                                      },
                                                      children = (from c in menuAccess.Where(d => d.Code.Length == 6)
                                                                  join z in access on c.Nama equals z.Menu into zc
                                                                  from z in zc.DefaultIfEmpty()
                                                                  where c.Nama.Split(".")[1] == b.Nama.Split(".")[1]
                                                                  select new
                                                                  {
                                                                      id = c.Nama,
                                                                      text = c.Nama.Split(".")[2],
                                                                      state = new
                                                                      {
                                                                          selected = z != null ? true : false
                                                                      },
                                                                      children = (from d in menuAccess.Where(d => d.Code.Length == 8)
                                                                                  join v in access on d.Nama equals v.Menu into vd
                                                                                  from v in vd.DefaultIfEmpty()
                                                                                  where d.Nama.Split(".")[2] == c.Nama.Split(".")[2]
                                                                                  select new
                                                                                  {
                                                                                      id = d.Nama,
                                                                                      text = d.Nama.Split(".")[2],
                                                                                      state = new
                                                                                      {
                                                                                          selected = v != null ? true : false
                                                                                      }
                                                                                  }).ToList()
                                                                  }).ToList()
                                                  }).ToList()
                                  }).ToList();

            return Json(listPermission);
        }

        public List<MenuAccessDto> GetMenuList()
        {
            var menu = (from a in _appContext.Menu
                        select new MenuAccessDto
                        {
                            Nama = a.Nama,
                            Code = a.Code,
                            Parent = a.Parent
                        }).ToList();

            var result = (from a in _appContext.Menu
                          where a.Parent == "0" && a.IsActive
                          select new MenuAccessDto
                          {
                              Nama = a.Nama,
                              Code = a.Code,
                              Parent = a.Parent
                          }).ToList();

            var data = (from a in _appContext.Menu
                        where a.Parent != "0" && a.IsActive
                        select new MenuAccessDto
                        {
                            Nama = _menuService.GetMenuAccessName(a.Code, menu).Nama,
                            Code = a.Code,
                            Parent = a.Parent
                        }).ToList();

            result.AddRange(data);

            return result;
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetRoles = await _appContext.AspNetRoles.FindAsync(id);
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

            var data = (from user in _appContext.AspNetUsers
                        join Personal in _appContext.Personal on user.Email equals Personal.Email
                        where Personal.Nama != "Developers"
                        select new
                        {
                            UserId = user.Id,
                            Personal.Nama
                        });

            List<UserRoleEditDto> ListUser = new List<UserRoleEditDto>();

            foreach (var item in data)
            {
                ListUser.Add(new UserRoleEditDto
                {
                    UserId = item.UserId,
                    Nama = item.Nama
                });

            }

            ViewBag.Personal = ListUser;

            return View(aspNetRoles);
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var Personal = await _appContext.Personal.FirstOrDefaultAsync(p => p.Id == id);
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

            var user = _appContext.AspNetUsers.FirstOrDefault(d => d.Email == Personal.Email);

            var roles = _appContext.AspNetRoles.Where(d => d.Name != "Developers").ToList();

            ViewBag.Position = _appContext.RF_Positions.Where(d => d.Id == Personal.PositionId).Select(d => d.Position).FirstOrDefault();

            ViewBag.IdUser = user.Id;
            ViewBag.Roles = roles;

            return View(Personal);
        }

        [AjaxOnly]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetRoles = await _appContext.AspNetRoles
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
            var aspNetRoles = await _appContext.AspNetRoles.FindAsync(id);
            try
            {
                _appContext.AspNetRoles.Remove(aspNetRoles);
                await _appContext.SaveChangesAsync();
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
            return _appContext.AspNetRoles.Any(e => e.Id == id);
        }
    }
}
