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
using SIP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace SIP.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly DB_NewContext _context;
        public LoginModel(SignInManager<IdentityUser> signInManager,
            ILogger<LoginModel> logger, DB_NewContext context)
        {
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
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

        [BindProperty] public Pegawai Pegawai { get; set; }
        [BindProperty] public RefJabatan RefJabatan { get; set; }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/Home");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                var user = _context.AspNetUsers.FirstOrDefault(d => d.Email == Input.Email);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");

                    if (user.LockoutEnabled == false)
                    {
                        await _signInManager.SignOutAsync();

                        HttpContext.Session.Clear();
                        TempData["status"] = "lockout";
                        return Page();
                    }

                    Pegawai = await _context.Pegawai.Include(w => w.IdJabatanNavigation).FirstOrDefaultAsync(w => w.Email == Input.Email);
                    if (Pegawai == null)
                    {
                        await _signInManager.SignOutAsync();

                        HttpContext.Session.Clear();
                        ModelState.AddModelError(string.Empty, "Akun Belum Terdaftar.");
                        TempData["status"] = "akun";
                        return Page();
                    }

                    if (Pegawai.Nama == "Developers")
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

                    HttpContext.Session.SetString("User", user.Id);
                    HttpContext.Session.SetString("Email", Input.Email);
                    HttpContext.Session.SetString("Nama", Pegawai.Nama);
                    HttpContext.Session.SetString("Jabatan", Pegawai.IdJabatanNavigation.Jabatan);

                    TempData["status"] = "login";
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    if (user != null)
                    {
                        ModelState.AddModelError(string.Empty, "Password Salah.");
                        TempData["status"] = "salah";
                        return Page();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Email Tidak Terdaftar!");
                        TempData["status"] = "email";
                        return Page();
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
