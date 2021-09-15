using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUNA.Helpers;
using NUNA.Models.BaseApplicationContext;
using NUNA.Services;
using NUNA.ViewModels.Toastr;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NUNA.Controllers
{
    public class MenuController : Controller
    {
        private readonly BaseApplicationContext _appContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JsonResultService _result = new JsonResultService();
        public MenuController(BaseApplicationContext context, UserManager<IdentityUser> userManager)
        {
            _appContext = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            //Link
            ViewBag.L = Url.Action("Index");
            ViewBag.L1 = "";
            ViewBag.L2 = "";
            ViewBag.L3 = "";

            return View();
        }

        public IActionResult Details(int id)
        {
            var result = (from a in _appContext.Menu
                          where a.Id == id
                          select a).FirstOrDefault();

            return PartialView(result);
        }

        public JsonResult GetMenuAccess()
        {
            List<string> root = new List<string> { "Menu" };

            var menu = (from a in _appContext.Menu
                        select a).ToList();

            var listPermission = (from x in root
                                  select new
                                  {
                                      id = "0",
                                      text = x,
                                      children = (from a in menu
                                                  where a.Parent == "0"
                                                  orderby a.NoUrut
                                                  select new
                                                  {
                                                      id = a.Id.ToString(),
                                                      text = a.Nama,
                                                      icon = a.IsActive ? "" : "fa fa-folder kt-font-default",
                                                      children = (from b in menu
                                                                   where b.Parent == a.Code
                                                                   orderby b.NoUrut
                                                                   select new
                                                                   {
                                                                       id = b.Id.ToString(),
                                                                       text = b.Nama,
                                                                       icon = b.IsActive ? "" : "fa fa-folder kt-font-default",
                                                                       children = (from c in menu
                                                                                    where c.Parent == b.Code
                                                                                    orderby c.NoUrut
                                                                                    select new
                                                                                    {
                                                                                        id = c.Id.ToString(),
                                                                                        text = c.Nama,
                                                                                        icon = c.IsActive ? "" : "fa fa-folder kt-font-default",
                                                                                        children = (from d in menu
                                                                                                    where d.Parent == c.Code
                                                                                                    orderby d.NoUrut
                                                                                                    select new
                                                                                                    {
                                                                                                        id = d.Id.ToString(),
                                                                                                        text = d.Nama,
                                                                                                        icon = d.IsActive ? "" : "fa fa-folder kt-font-default"
                                                                                                    }).ToList()
                                                                                    }).ToList()
                                                                   }).ToList()
                                                  }).ToList()
                                  }).ToList();

            return Json(listPermission);
        }

        public IActionResult Create(int id)
        {
            if (id == 0)
            {
                ViewBag.Parent = "0";

                return PartialView();
            }
            var result = (from a in _appContext.Menu
                          where a.Id == id
                          select a).FirstOrDefault();

            ViewBag.Parent = result.Code;

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Menu input)
        {
            if (ModelState.IsValid)
            {
                var code = "";
                var menu = (from a in _appContext.Menu
                            where a.Parent == input.Parent
                            select a).ToList();

                var lastNumber = menu.Select(d => d.Code).OrderByDescending(d => d).FirstOrDefault();

                if (lastNumber == null)
                {
                    code = input.Parent + "01";
                }
                else
                {
                    if (lastNumber.Length == 2)
                    {
                        var number = lastNumber.Substring(0, 1) == "0" ? lastNumber[1..] : lastNumber;
                        code = (int.Parse(number) + 1).ToString().PadLeft(2, '0');
                    }
                    else if (lastNumber.Length == 4)
                    {
                        var number = lastNumber.Substring(2, 1) == "0" ? lastNumber[3..] : lastNumber;
                        code = lastNumber.Substring(0, 2) + (int.Parse(number) + 1).ToString().PadLeft(2, '0');
                    }
                    else if (lastNumber.Length == 6)
                    {
                        var number = lastNumber.Substring(4, 1) == "0" ? lastNumber[5..] : lastNumber;
                        code = lastNumber.Substring(0, 4) + (int.Parse(number) + 1).ToString().PadLeft(2, '0');
                    }
                    else if (lastNumber.Length == 8)
                    {
                        var number = lastNumber.Substring(6, 1) == "0" ? lastNumber[7..] : lastNumber;
                        code = lastNumber.Substring(0, 6) + (int.Parse(number) + 1).ToString().PadLeft(2, '0');
                    }
                }

                try
                {
                    Menu insert = new Menu
                    {
                        ActionName = input.ActionName,
                        Controller = input.ActionName != null ? input.Controller : null,
                        Code = code,
                        IconClass = input.IconClass,
                        IsActive = true,
                        IsParent = input.IsParent,
                        Nama = input.Nama,
                        Parent = input.Parent,
                        NoUrut = menu.Max(d => d.NoUrut) + 1
                    };

                    _appContext.Add(insert);
                    _appContext.SaveChanges();
                }
                catch
                {
                    return Json(_result.Error(Message.ErrorSave));
                }

                return Json(_result.Success(Url.Action("Index"))).WithSuccess(Message.Save);
            }

            return Json(_result.Error(Message.InvalidForm));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = (from a in _appContext.Menu
                          where a.Id == id
                          select a).FirstOrDefaultAsync();

            return PartialView(await result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Menu input)
        {
            if (id != input.Id)
            {
                return Json(_result.Error(Message.NotFound));
            }

            if (ModelState.IsValid)
            {
                var update = _appContext.Menu.Find(id);
                update.Controller = input.ActionName != null ? input.Controller : null;
                update.ActionName = input.ActionName;
                update.IconClass = input.IconClass;
                update.IsActive = input.IsActive;
                update.IsParent = input.IsParent;
                update.Nama = input.Nama;
                try 
                {
                    _appContext.Update(update);
                    await _appContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuExists(input.Id))
                    {
                        return Json(_result.Error(Message.NotExist));
                    }
                    else
                    {
                        return Json(_result.Error(Message.ErrorUpdate));
                    }
                }

                return Json(_result.Success(Url.Action("Index"))).WithSuccess(Message.Update);
            }

            return Json(_result.Error(Message.InvalidForm));
        }

        public IActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private bool MenuExists(int id)
        {
            return _appContext.Menu.Any(e => e.Id == id);
        }
    }
}
