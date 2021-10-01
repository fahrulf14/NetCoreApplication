using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NUNA.ViewModels.Account
{
    public class SendEmailVerificationDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
    }
}
