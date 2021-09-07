using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIP.ViewModels
{
    public class IdentityViewModel
    {

    }

    public partial class UserPegawai
    {
        public Guid IdPegawai { get; set; }
        public string IdUser { get; set; }
        public string Nama { get; set; }
        public string Jabatan { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool LockoutEnabled { get; set; }
    }
}
