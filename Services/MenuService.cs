using System.Collections.Generic;
using System.Linq;
using SIP.Models;

namespace SIP.Services
{
    public class MenuService
    {
        public MenuAccess GetMenuAccessName(string code, List<MenuAccess> menu)
        {
            MenuAccess result = new MenuAccess();
            if (code.Length == 2)
            {
                result = (from a in menu
                          where a.Code == code
                          select new MenuAccess
                          {
                              Code = a.Code,
                              Nama = a.Nama,
                              Parent = a.Parent
                          }).FirstOrDefault();
            }
            else if (code.Length == 4)
            {
                result = (from a in menu
                          join b in menu on a.Parent equals b.Code
                          where a.Code == code
                          select new MenuAccess
                          {
                              Code = a.Code,
                              Nama = b.Nama + "." + a.Nama,
                              Parent = a.Parent
                          }).FirstOrDefault();
            }
            else if (code.Length == 6)
            {
                result = (from a in menu
                          join b in menu on a.Parent equals b.Code
                          join c in menu on b.Parent equals c.Code
                          where a.Code == code
                          select new MenuAccess
                          {
                              Code = a.Code,
                              Nama = c.Nama + "." + b.Nama + "." + a.Nama,
                              Parent = a.Parent
                          }).FirstOrDefault();
            }
            else if (code.Length == 8)
            {
                result = (from a in menu
                          join b in menu on a.Parent equals b.Code
                          join c in menu on b.Parent equals c.Code
                          join d in menu on c.Parent equals d.Code
                          where a.Code == code
                          select new MenuAccess
                          {
                              Code = a.Code,
                              Nama = b.Nama + "." + a.Nama,
                              Parent = a.Parent
                          }).FirstOrDefault();
            }

            return result;
        }
    }
}
