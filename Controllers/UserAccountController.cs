using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attributes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using NUNA.Helpers;
using NUNA.Models.BaseApplicationContext;
using NUNA.Services;
using NUNA.ViewModels.Menu;
using NUNA.ViewModels.Toastr;
using NUNA.ViewModels.UserAccount;

namespace NUNA.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly BaseApplicationContext _appContext;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly MenuService _menuService = new MenuService();
        private readonly JsonResultService _result = new JsonResultService();

        public UserAccountController(
            BaseApplicationContext context,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager
            )
        {
            _appContext = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
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
                        }).ToListAsync();

            List<PersonalAccountDto> List = new List<PersonalAccountDto>();

            foreach (var item in  await data)
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

        public async Task<IActionResult> Create()
        {
            ViewBag.ListPersonal = await (from a in _appContext.Personal
                                          where a.Email == null
                                          select new SelectListItem
                                          {
                                              Value = a.Id.ToString(),
                                              Text = a.Nama
                                          }).ToListAsync();

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserAccountInputDto input)
        {
            if (ModelState.IsValid)
            {
                var UserName = input.Email.Split("@")[0];

                var checkUsername = (from a in _appContext.AspNetUsers
                                     where a.UserName.ToLower() == UserName.ToLower()
                                     select a.UserName).Any();
                if (checkUsername)
                {
                    return Json(_result.Error(Message.UserExist));
                }

                var user = new IdentityUser { UserName = UserName, Email = input.Email };
                var result = await _userManager.CreateAsync(user, input.Password);
                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var Personal = _appContext.Personal.FirstOrDefault(p => p.Id == input.PersonalId);
                    Personal.Email = input.Email;
                    _appContext.Personal.Update(Personal);

                    var dataUser = _appContext.AspNetUsers.FirstOrDefault(d => d.Email == input.Email);
                    dataUser.EmailConfirmed = true;
                    _appContext.AspNetUsers.Update(dataUser);
                    _appContext.SaveChanges();

                    return Json(_result.Success(Url.Action("Index"))).WithSuccess(Message.Save);
                }
                else
                {
                    return Json(_result.Error(Message.ErrorSave));
                }
            }
            return Json(_result.Error(Message.InvalidForm));
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
            var RoleId = (from a in _appContext.AspNetUserRoles
                          where a.UserId == UserId
                          select a.RoleId).ToList();

            var access = (from a in _appContext.AspNetUserPermissions
                          where a.UserId == UserId
                          select a).ToList();

            var roleAccess = (from a in _appContext.AspNetRolePermissions
                              where RoleId.Contains(a.RoleId)
                              select a.Permission).Distinct().ToList();

            var listPermission = (from a in permission
                                  group a by a.Split(".")[0]
                                  into G
                                  select new
                                  {
                                      id = G.Key,
                                      text = G.Key,
                                      state = new
                                      {
                                          disabled = (from b in roleAccess
                                                      where b == G.Key
                                                      select b).Any(),
                                          selected = (from b in roleAccess
                                                      where b == G.Key
                                                      select b).Any()
                                      },
                                      children = (from x in permission
                                                  join y in access on x equals y.Permission into users
                                                  from y in users.DefaultIfEmpty()
                                                  join z in roleAccess on x equals z into roles
                                                  from z in roles.DefaultIfEmpty()
                                                  where x.Split(".")[0] == G.Key
                                                  select new
                                                  {
                                                      id = x,
                                                      text = x.Split(".")[1],
                                                      icon = x.Split(".")[1] == "View" ? "fa fa-th-list kt-font-default" :
                                                                x.Split(".")[1] == "Create" ? "fa fa-plus-square kt-font-default" :
                                                                x.Split(".")[1] == "Edit" ? "fa fa-pen-square kt-font-default" :
                                                                x.Split(".")[1] == "Delete" ? "fa fa-minus-square kt-font-default" : "fa fa-check-square kt-font-default",
                                                      state = new
                                                      {
                                                          disabled = z != null,
                                                          selected = z != null || (y != null)
                                                      }
                                                  })
                                  }).ToList();

            return Json(listPermission);
        }

        [HttpPost]
        public IActionResult UpdatePermission([FromBody] PermissionInputDto input)
        {
            var RoleId = (from a in _appContext.AspNetUserRoles
                          where a.UserId == input.id
                          select a.RoleId).ToList();
            #region Permission
            var rolePermission = (from a in _appContext.AspNetRolePermissions
                                  where RoleId.Contains(a.RoleId)
                                  select a.Permission).Distinct().ToList();

            var permission = (from a in _appContext.AspNetUserPermissions
                              where a.UserId == input.id
                              select a.Permission).ToList();

            var deletePermission = (from a in permission
                                    where !input.permission.Contains(a) && !rolePermission.Contains(a)
                                    select new AspNetUserPermissions 
                                    {
                                        UserId = input.id,
                                        Permission = a
                                    }).ToList();

            var insertPermission = (from a in input.permission
                                    where !permission.Contains(a) && !rolePermission.Contains(a)
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
            var roleMemu = (from a in _appContext.AspNetRoleMenus
                            where RoleId.Contains(a.RoleId)
                            select a.Menu).Distinct().ToList();

            var menu = (from a in _appContext.AspNetUserMenus
                        where a.UserId == input.id
                        select a.Menu).ToList();

            var deleteMenu = (from a in menu
                              where !input.menu.Contains(a) && !roleMemu.Contains(a)
                              select new AspNetUserMenus
                              {
                                  UserId = input.id,
                                  Menu = a
                              }).ToList();

            var insertMenu = (from a in input.menu
                              where !menu.Contains(a) && !roleMemu.Contains(a)
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
            return Json(_result.Success(Url.Action("Index"))).WithSuccess(Message.Save);
        }

        public JsonResult GetMenuAccess()
        {
            List<object> result = new List<object>();
            List<MenuAccessDto> menuAccess = GetMenuList();

            var UserId = TempData["UserId"].ToString();
            var RoleId = (from a in _appContext.AspNetUserRoles
                          where a.UserId == UserId
                          select a.RoleId).ToList();

            var access = (from a in _appContext.AspNetUserMenus
                          where a.UserId == UserId
                          select a).ToList();

            var roleAccess = (from a in _appContext.AspNetRoleMenus
                              where RoleId.Contains(a.RoleId)
                              select a.Menu).Distinct().ToList();

            var listPermission = (from a in menuAccess.Where(d => d.Code.Length == 2)
                                  join x in access on a.Nama equals x.Menu into xa
                                  from x in xa.DefaultIfEmpty()
                                  join o in roleAccess on a.Nama equals o into ao
                                  from o in ao.DefaultIfEmpty()
                                  where a.Parent == "0"
                                  select new
                                  {
                                      id = a.Nama,
                                      text = a.Nama,
                                      state = new
                                      {
                                          disabled = o != null,
                                          selected = o != null || x != null
                                      },
                                      children = (from b in menuAccess.Where(d => d.Code.Length == 4)
                                                  join y in access on b.Nama equals y.Menu into yb
                                                  from y in yb.DefaultIfEmpty()
                                                  join p in roleAccess on b.Nama equals p into bp
                                                  from p in bp.DefaultIfEmpty()
                                                  where b.Nama.Split(".")[0] == a.Nama
                                                  select new
                                                  {
                                                      id = b.Nama,
                                                      text = b.Nama.Split(".")[1],
                                                      state = new
                                                      {
                                                          disabled = p != null,
                                                          selected = p != null || y != null
                                                      },
                                                      children = (from c in menuAccess.Where(d => d.Code.Length == 6)
                                                                  join z in access on c.Nama equals z.Menu into zc
                                                                  from z in zc.DefaultIfEmpty()
                                                                  join q in roleAccess on c.Nama equals q into cq
                                                                  from q in cq.DefaultIfEmpty()
                                                                  where c.Nama.Split(".")[1] == b.Nama.Split(".")[1]
                                                                  select new
                                                                  {
                                                                      id = c.Nama,
                                                                      text = c.Nama.Split(".")[2],
                                                                      state = new
                                                                      {
                                                                          disabled = q != null,
                                                                          selected = q != null || z != null
                                                                      },
                                                                      children = (from d in menuAccess.Where(d => d.Code.Length == 8)
                                                                                  join v in access on d.Nama equals v.Menu into vd
                                                                                  from v in vd.DefaultIfEmpty()
                                                                                  join r in roleAccess on d.Nama equals r into dr
                                                                                  from r in dr.DefaultIfEmpty()
                                                                                  where d.Nama.Split(".")[2] == c.Nama.Split(".")[2]
                                                                                  select new
                                                                                  {
                                                                                      id = d.Nama,
                                                                                      text = d.Nama.Split(".")[2],
                                                                                      state = new
                                                                                      {
                                                                                          disabled = r != null,
                                                                                          selected = r != null || v != null
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

        public IActionResult Roles(string id)
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

            TempData["UserId"] = data.UserId;

            return View(data);
        }

        public JsonResult GetRoleAccess()
        {
            List<string> root = new List<string> { "Roles" };
            var roles = _appContext.AspNetRoles.Where(d => d.Name != "Developers").ToList();

            var UserId = TempData["UserId"].ToString();

            var access = (from a in _appContext.AspNetUserRoles
                          where a.UserId == UserId
                          select a).ToList();

            var listPermission = (from r in root
                                  select new
                                  {
                                      id = "0",
                                      text = r,
                                      children = (from a in roles
                                                  join x in access on a.Id equals x.RoleId into xa
                                                  from x in xa.DefaultIfEmpty()
                                                  select new
                                                  {
                                                      id = a.Id,
                                                      text = a.Name,
                                                      state = new
                                                      {
                                                          selected = x != null ? true : false
                                                      }
                                                  }).ToList()
                                  }).ToList();

            return Json(listPermission);
        }

        [HttpPost]
        public IActionResult UpdateRoles([FromBody] PermissionInputDto input)
        {
            #region UpdateRole
            var role = (from a in _appContext.AspNetUserRoles
                        where a.UserId == input.id
                        select a.RoleId).ToList();

            var deleteRole = (from a in role
                              where !input.role.Contains(a)
                              select new AspNetUserRoles
                              {
                                  UserId = input.id,
                                  RoleId = a
                              }).ToList();

            var insertRole = (from a in input.role.Where(d => d != "0")
                              where !role.Contains(a)
                              select new AspNetUserRoles
                              {
                                  UserId = input.id,
                                  RoleId = a
                              }).ToList();

            if (deleteRole.Count != 0)
            {
                _appContext.RemoveRange(deleteRole);
            }
            if (insertRole.Count != 0)
            {
                _appContext.AddRange(insertRole);
            }
            #endregion UpdateRole

            _appContext.SaveChanges();
            return Json(_result.Success(Url.Action("Index"))).WithSuccess(Message.Save);
        }

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
