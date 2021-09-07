using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIP.ViewModels
{
    public class RolesViewModel
    {

    }

    public partial class ListRole
    {
        public string Id { get; set; }
        public string Role { get; set; }
        public string User { get; set; }
    }

    public partial class ListUser
    {
        public string Id { get; set; }
        public string Nama { get; set; }
        public string Role { get; set; }
    }

    public partial class UserRoleEdit
    {
        public string IdUser { get; set; }
        public string Nama { get; set; }
    }
}
