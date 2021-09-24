using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUNA.Models.BaseApplicationContext;

namespace NUNA.Components
{
    public class AsideIdentityViewComponent : ViewComponent
    {
        private readonly BaseApplicationContext _appContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;

        public AsideIdentityViewComponent(BaseApplicationContext context, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _appContext = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext.Session;
        }

        public async Task<IViewComponentResult> InvokeAsync()
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

            var model = _appContext.Menu.Where(d => d.IsActive).OrderBy(d => d.NoUrut).ToList();

            return await Task.FromResult((IViewComponentResult)View("Default", model));
        }
    }
}
