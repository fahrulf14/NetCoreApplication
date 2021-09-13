using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIP.Models;
using SIP.Models.BaseApplicationContext;
using SIP.Services;
using SIP.ViewModels;
using SIP.ViewModels.UserAccount;

namespace SIP.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly BaseApplicationContext _appContext;
        private readonly MenuService _menuService = new MenuService();

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

            List<PersonalAccountDto> List = new List<PersonalAccountDto>();

            foreach (var item in data)
            {
                List.Add(new PersonalAccountDto
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

        public IActionResult Permission(string id)
        {
            var data = (from u in _appContext.AspNetUsers
                        join p in _appContext.Personal on u.Email equals p.Email
                        join j in _appContext.RF_Positions on p.PositionId equals j.Id
                        where u.Id == id
                        select new UserPermissionDto
                        {
                            UserId = u.Id,
                            Nama = p.Nama,
                            Email = p.Email,
                            Position = j.Position,
                            IsActive = p.IsActive
                        }).FirstOrDefault();

            var role = (from a in _appContext.AspNetRoles
                        join b in _appContext.AspNetUserRoles on a.Id equals b.RoleId
                        where b.UserId == id
                        select a.Name).ToList();

            if (role.Count != 0)
            {
                data.Role = role;
            }

            TempData["UserId"] = data.UserId;

            return View(data);
        }

        public JsonResult GetPermission()
        {
            List<string> permission = Models.Permission.AppPermission.ToList();

            var UserId = TempData["UserId"].ToString();

            var access = (from a in _appContext.AspNetUserPermissions
                          where a.UserId == UserId
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
                                                      icon = x.Split(".")[1] == "Create" ? "fa fa-plus-square kt - font-default" :
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
        public ActionResult UpdatePermission([FromBody] PermissionInputDto input)
        {

            #region Permission
            var permission = (from a in _appContext.AspNetUserPermissions
                              where a.UserId == input.id
                              select a.Permission).ToList();

            var deletePermission = (from a in permission
                                    where !input.permission.Contains(a)
                                    select new AspNetUserPermissions 
                                    {
                                        UserId = input.id,
                                        Permission = a
                                    }).ToList();

            var insertPermission = (from a in input.permission
                                    where !permission.Contains(a)
                                    select new AspNetUserPermissions
                                    {
                                        UserId = input.id,
                                        Permission = a
                                    }).ToList();

            if(deletePermission.Count != 0)
            {
                _appContext.RemoveRange(deletePermission);
            }
            if(insertPermission.Count != 0)
            {
                _appContext.AddRange(insertPermission);
            }

            #endregion

            #region Menu
            var menu = (from a in _appContext.AspNetUserMenus
                              where a.UserId == input.id
                              select a.Menu).ToList();

            var deleteMenu = (from a in menu
                                    where !input.menu.Contains(a)
                                    select new AspNetUserMenus
                                    {
                                        UserId = input.id,
                                        Menu = a
                                    }).ToList();

            var insertMenu = (from a in input.menu
                                    where !menu.Contains(a)
                                    select new AspNetUserMenus
                                    {
                                        UserId = input.id,
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
            string message = "SUCCESS";
            return Json(new { Message = message });
        }

        public JsonResult GetMenuAccess()
        {
            List<object> result = new List<object>();
            List<string> menuAccess = GetMenuList();

            var UserId = TempData["UserId"].ToString();

            var access = (from a in _appContext.AspNetUserMenus
                          where a.UserId == UserId
                          select a).ToList();

            List<string> level1 = new List<string>();
            List<string> level2 = new List<string>();
            List<string> level3 = new List<string>();

            foreach (var item in menuAccess)
            {
                if (item.Split(".").Count() == 1)
                {
                    level1.Add(item);
                }
                else if (item.Split(".").Count() == 2)
                {
                    level2.Add(item);
                }
                else if (item.Split(".").Count() == 3)
                {
                    level3.Add(item);
                }
            }

            var listPermission = (from a in level1
                                  join x in access on a equals x.Menu into xa
                                  from x in xa.DefaultIfEmpty()
                                  select new
                                  {
                                      id = a,
                                      text = a,
                                      state = new
                                      {
                                          selected = x != null ? true : false
                                      },
                                      children = (from bc in level2
                                                  where bc.Split(".")[0] == a
                                                  select bc).FirstOrDefault() == null ? null :
                                                  (from b in level2
                                                   join y in access on b equals y.Menu into yb
                                                   from y in yb.DefaultIfEmpty()
                                                   where b.Split(".")[0] == a
                                                   select new
                                                   {
                                                       id = b,
                                                       text = b.Split(".")[1],
                                                       state = new
                                                       {
                                                           selected = y != null ? true : false
                                                       },
                                                       children = (from cc in level3
                                                                   where cc.Split(".")[1] == b.Split(".")[1]
                                                                   select cc).FirstOrDefault() == null ? null :
                                                                   (from c in level3
                                                                    join z in access on c equals z.Menu into zc
                                                                    from z in zc.DefaultIfEmpty()
                                                                    where c.Split(".")[1] == b.Split(".")[1]
                                                                    select new
                                                                    {
                                                                        id = c,
                                                                        text = c.Split(".")[2],
                                                                        state = new
                                                                        {
                                                                            selected = z != null ? true : false
                                                                        }
                                                                    }).ToList()
                                                   }).ToList()
                                  }).ToList();

            return Json(listPermission);
        }

        public List<string> GetMenuList()
        {
            var menu = (from a in _appContext.Menu
                        select new MenuAccess
                        {
                            Nama = a.Nama,
                            Code = a.Code,
                            Parent = a.Parent
                        }).ToList();

            var result = (from a in _appContext.Menu
                          where a.Parent == "0" && a.IsActive
                          select a.Nama).ToList();

            var data = (from a in _appContext.Menu
                        where a.Parent != "0" && a.IsActive
                        select _menuService.GetMenuAccessName(a.Code, menu).Nama).ToList();

            result.AddRange(data);

            return result;
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
