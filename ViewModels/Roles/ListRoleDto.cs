using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NUNA.ViewModels.Roles
{
    public class ListRoleDto
    {
        public string Id { get; set; }
        public string Role { get; set; }
        public string User { get; set; }
    }

    public partial class ListUserDto
    {
        public int Id { get; set; }
        public string Nama { get; set; }
        public string Role { get; set; }
    }

    public partial class UserRoleEditDto
    {
        public string UserId { get; set; }
        public string Nama { get; set; }
    }
}
