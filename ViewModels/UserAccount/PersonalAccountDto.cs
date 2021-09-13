using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIP.ViewModels.UserAccount
{
    public class PersonalAccountDto
    {
        public int PersonalId { get; set; }
        public string UserId { get; set; }
        public string Nama { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool LockoutEnabled { get; set; }
    }
}
