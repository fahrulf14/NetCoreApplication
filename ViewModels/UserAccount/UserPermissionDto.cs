using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIP.ViewModels.UserAccount
{
    public class UserPermissionDto
    {
        public string UserId { get; set; }
        public string Nama { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public List<string> Role { get; set; }
        public bool IsActive { get; set; }
    }
}
