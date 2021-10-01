using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using NUNA.Helpers;
using NUNA.Models.BaseApplicationContext;
using NUNA.Services;
using NUNA.ViewModels.Account;
using NUNA.ViewModels.Toastr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace NUNA.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly BaseApplicationContext _appContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;
        private readonly JsonResultService _result = new();

        public AccountController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            BaseApplicationContext appContext,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appContext = appContext;
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext.Session;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("Login")]
        public IActionResult Login(string returnUrl)
        {
            LoginInputDto dataViews = new()
            {
                ReturnUrl = Request.GetTypedHeaders().Referer.AbsoluteUri
            };

            return PartialView(dataViews);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputDto input)
        {
            if (ModelState.IsValid)
            {
                var userName = input.Email.Split("@")[0];
                var result = await _signInManager.PasswordSignInAsync(userName, input.Password, input.RememberMe, lockoutOnFailure: true);

                var user = await _userManager.FindByEmailAsync(input.Email);

                if (result.Succeeded)
                {
                    var dataPersonals = await _appContext.Personals.FirstOrDefaultAsync(w => w.UserName.ToLower() == userName.ToLower());
                    if (dataPersonals == null)
                    {
                        await _signInManager.SignOutAsync();

                        HttpContext.Session.Clear();
                        return Json(_result.Error(Message.UserNotExist));
                    }

                    var permission = (from a in _appContext.AspNetUserPermissions
                                      where a.UserId == user.Id
                                      select a.Permission).ToList();

                    var roles = await _userManager.GetRolesAsync(user);

                    var rolePermission = (from a in _appContext.AspNetRolePermissions
                                          join b in _appContext.AspNetRoles on a.RoleId equals b.Id
                                          where roles.Contains(b.Name)
                                          select a.Permission).ToList();

                    _session.SetString("User", user.Id);
                    _session.SetString("Email", input.Email);
                    _session.SetString("Username", userName);
                    _session.SetString("Nama", dataPersonals.Name);
                    _session.SetString("Permission", string.Join("|", permission));
                    _session.SetString("RolePermission", string.Join("|", rolePermission));

                    return Json(_result.Success(input.ReturnUrl)).WithSuccess(Message.LoginSuccess);
                }
                //if (result.RequiresTwoFactor)
                //{
                //    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, Input.RememberMe });
                //}
                if (result.IsLockedOut)
                {
                    return Json(_result.Error(Message.Lockout));
                }
                else
                {
                    if (user != null)
                    {
                        return Json(_result.Error(Message.WrongPassword));
                    }
                    else
                    {
                        return Json(_result.Error(Message.WrongEmail));
                    }
                }
            }
            return Json(_result.Error(Message.InvalidForm));
        }

        [Route("Register")]
        [Route("Account/Register")]
        public IActionResult Register()
        {
            var returnUrl = Request.GetTypedHeaders().Referer?.AbsoluteUri ?? "/Home";
            RegisterInputDto dataViews = new()
            {
                ReturnUrl = returnUrl.Contains("Register") ? "/Home" : returnUrl
            };

            return View(dataViews);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Register")]
        [Route("Account/Register")]
        public async Task<IActionResult> Register(RegisterInputDto input)
        {
            if (ModelState.IsValid)
            {
                var UserName = input.Email.Split("@")[0];

                var checkUsername = (from a in _appContext.AspNetUsers
                                     where a.UserName.ToLower() == UserName.ToLower()
                                     select a.UserName).Any();
                if (checkUsername)
                {
                    UserName += DateTime.Now.Millisecond.ToString().Substring(0, 2);
                }

                var user = new IdentityUser { UserName = UserName, Email = input.Email };
                var result = await _userManager.CreateAsync(user, input.Password);
                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                    SendEmailVerificationDto dataEmail = new()
                    {
                        Email = input.Email,
                        Name = input.Name,
                        Url = ""
                    };

                    SendVerificationEmail(dataEmail);

                    AspNetSecretTokens verificaTiontoken = new()
                    {
                        UserId = user.Id,
                        Token = token,
                        ExpirationDate = DateTime.Now.AddDays(2),
                        Type = "Registration",
                        Status = "New"
                    };
                    _appContext.Add(verificaTiontoken);

                    Personals personals = new()
                    {
                        IsActive = false,
                        Name = input.Name,
                        UserName = UserName
                    };
                    _appContext.Add(personals);
                    await _appContext.SaveChangesAsync();

                    return RedirectToAction(input.ReturnUrl).WithSuccess(Message.Save);
                }
                else
                {
                    return View(input).WithError(Message.ErrorSave);
                }
            }
            return View(input).WithError(Message.InvalidForm);
        }

        [HttpPost]
        public IActionResult RedirectUrl()
        {
            return Json(_result.Success(Url.Action("Register")));
        }

        private static void SendVerificationEmail(SendEmailVerificationDto input)
        {
            var templatePath = "wwwroot/template/email/Verify Account Email Template.html";
            var template = StringHelper.ReadFile(templatePath);

            var html = StringHelper.MappedValueToHTML(template, input);

            var smtpHost = AppSettingHelper.GetValue.EmailSetting.SmtpHost;
            var username = AppSettingHelper.GetValue.EmailSetting.SmtpUser;
            var password = AppSettingHelper.GetValue.EmailSetting.SmtpCred;
            var smtpPort = AppSettingHelper.GetValue.EmailSetting.SmtpPort;
            var emailFrom = AppSettingHelper.GetValue.EmailSetting.From;

            MailMessage mail = new()
            {
                From = new MailAddress(emailFrom),
                Subject = "Konfirmasi Akun",
                Body = html,
                IsBodyHtml = true
            };
            mail.To.Add(new MailAddress(input.Email));

            using SmtpClient smtp = new(smtpHost, int.Parse(smtpPort));
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = new NetworkCredential(username, password);
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(mail);
        }

        public IActionResult Registration()
        {
            //bool isValid = (from a in _appContext.AspNetSecretTokens
            //                where a.UserId == token && a.Type == "Registration" && a.ExpirationDate.AddMinutes(10) >= DateTime.UtcNow
            //                select a).Any();

            //if (isValid)
            //{
            //    return View();
            //}

            //return RedirectToAction("Index", "Error");
            return View();
        }
    }
}
