using System.Collections.Generic;
using System.Linq;
using NUNA.ViewModels.Menu;

namespace NUNA.Services
{
    public class MenuService
    {
        public MenuAccessDto GetMenuAccessName(string code, List<MenuAccessDto> menu)
        {
            MenuAccessDto result = new();
            if (code.Length == 2)
            {
                result = (from a in menu
                          where a.Code == code
                          select new MenuAccessDto
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
                          select new MenuAccessDto
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
                          select new MenuAccessDto
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
                          select new MenuAccessDto
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
