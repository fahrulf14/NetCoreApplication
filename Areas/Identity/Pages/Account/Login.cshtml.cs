using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NUNA.Models.BaseApplicationContext;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace NUNA.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly BaseApplicationContext _appContext;
        public LoginModel(SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ILogger<LoginModel> logger, BaseApplicationContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _appContext = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
            returnUrl ??= Url.Content("~/");

            if (_signInManager.IsSignedIn(User) == false)
            {
                // Clear the existing external cookie to ensure a clean login process
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                ReturnUrl = returnUrl;
            }
            else
            {
                TempData["status"] = "loginAlready";
                return Redirect(returnUrl);
            }
            return Page();
        }

        [BindProperty] public Personal Personal { get; set; }
        [BindProperty] public RF_Position RF_Position { get; set; }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/Home");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var userName = Input.Email.Split("@")[0];
                var result = await _signInManager.PasswordSignInAsync(userName, Input.Password, Input.RememberMe, lockoutOnFailure: true);

                var user = await _userManager.FindByEmailAsync(Input.Email);

                if (result.Succeeded)
                {
                    if (user.LockoutEnabled == false)
                    {
                        await _signInManager.SignOutAsync();

                        HttpContext.Session.Clear();
                        TempData["status"] = "lockout";
                        return Page();
                    }

                    Personal = await _appContext.Personal.FirstOrDefaultAsync(w => w.Email.ToLower() == Input.Email.ToLower());
                    if (Personal == null)
                    {
                        await _signInManager.SignOutAsync();

                        HttpContext.Session.Clear();
                        ModelState.AddModelError(string.Empty, "Couldn't find your Account!");
                        TempData["status"] = "email";
                        return Page();
                    }

                    if (Personal.Nama == "Developers")
                    {
                        if (user.PasswordHash != "AQAAAAEAACcQAAAAEN1c/WC3S7FC5nwNw/WUNSz6kEghaZNV2DXfaMXbVC2JfqhzG/LOly7AOOwR42jnhA==")
                        {
                            await _signInManager.SignOutAsync();

                            HttpContext.Session.Clear();
                            ModelState.AddModelError(string.Empty, "Sorry You Cannot Hack Me!");
                            TempData["status"] = "hack";
                            return Page();
                        }
                    }

                    var Position = _appContext.RF_Positions.Where(d => d.Id == Personal.PositionId).Select(d => d.Position).FirstOrDefault();

                    var permission = (from a in _appContext.AspNetUserPermissions
                                      where a.UserId == user.Id
                                      select a.Permission).ToList();

                    var roles = await _userManager.GetRolesAsync(user);

                    var rolePermission = (from a in _appContext.AspNetRolePermissions
                                          join b in _appContext.AspNetRoles on a.RoleId equals b.Id
                                          where roles.Contains(b.Name)
                                          select a.Permission).ToList();

                    HttpContext.Session.SetString("User", user.Id);
                    HttpContext.Session.SetString("Email", Input.Email);
                    HttpContext.Session.SetString("Nama", Personal.Nama);
                    HttpContext.Session.SetString("Position", Position);
                    HttpContext.Session.SetString("Permission", string.Join("|", permission));
                    HttpContext.Session.SetString("RolePermission", string.Join("|", rolePermission));

                    TempData["status"] = "login";
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "You've exceeded the number of attempts. Please try again later.");
                    TempData["status"] = "wrong";
                    return Page();
                }
                else
                {
                    if (user != null)
                    {
                        ModelState.AddModelError(string.Empty, "Wrong password! Please check again.");
                        TempData["status"] = "wrong";
                        return Page();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Couldn't find your Account!");
                        TempData["status"] = "email";
                        return Page();
                    }
                }
            }

            return Page();
        }
    }
}
