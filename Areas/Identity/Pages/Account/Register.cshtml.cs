using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Attributes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SIP.Models;

namespace SIP.Areas.Identity.Pages.Account
{
    [Auth(new string[] { "Developers", "Setting" })]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly DB_NewContext _context;
        //private readonly DB_NewContext db = new DB_NewContext();

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            DB_NewContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public PegawaiModel Pegawais { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Pegawai")]
            public Guid IdPegawai { get; set; }
           
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public class PegawaiModel
        {
            public string Nama { get; set; }
            [StringLength(20)]
            public string Nick { get; set; }
            [Column("NIP")]
            [StringLength(50)]
            public string Nip { get; set; }
            public int? IdJabatan { get; set; }
            public bool Active { get; set; }
            public string Email { get; set; }
            [StringLength(256)]

            [ForeignKey(nameof(IdJabatan))]
            [InverseProperty(nameof(RefJabatan.Pegawai))]
            public virtual RefJabatan IdJabatanNavigation { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            ViewData["IdPegawai"] = new SelectList(_context.Pegawai.Where(d => d.Email == null), "IdPegawai", "Nama");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var pegawai = _context.Pegawai.FirstOrDefault(p => p.IdPegawai == Input.IdPegawai);
                    pegawai.Email = Input.Email;
                    _context.Pegawai.Update(pegawai);

                    var dataUser = _context.AspNetUsers.FirstOrDefault(d => d.Email == Input.Email);
                    dataUser.EmailConfirmed = true;
                    _context.AspNetUsers.Update(dataUser);
                    _context.SaveChanges();

                    TempData["status"] = "create";
                    string link = Url.Action("Index", "UserAccount");
                    return new JsonResult(new { success = true, url = link });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
