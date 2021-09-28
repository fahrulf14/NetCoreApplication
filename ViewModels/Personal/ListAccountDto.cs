using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NUNA.ViewModels.Personal
{
    public class ListAccountDto
    {
        public int PersonalId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public bool IsActive { get; set; }
    }
}
