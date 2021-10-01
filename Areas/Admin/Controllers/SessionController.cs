using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NUNA.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SessionController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISession _session;

        public SessionController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _session = _httpContextAccessor.HttpContext.Session;
        }

        public JsonResult Modal()
        {
            TempData["Partial"] = "True";
            return Json(new { success = true });
        }

        public JsonResult Menu(string id)
        {
            _session.SetString("Menu", id);

            if (id != "head")
            {
                _session.SetString("Button", "kt-aside__brand-aside-toggler--active");
            }
            else
            {
                _session.SetString("Button", "");
            }

            return Json(new { success = true });
        }

        public string GetSession(string input)
        {
            return _session.GetString(input);
        }
    }
}
