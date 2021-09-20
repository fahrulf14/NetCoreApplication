using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NUNA.ViewModels.UserAccount
{
    public class PermissionInputDto
    {
        public string id { get; set; }
        public List<string> menu { get; set; }
        public List<string> permission { get; set; }
        public List<string> role { get; set; }
    }
}
