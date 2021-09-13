using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NUNA.ViewModels.Personal
{
    public class ListAccountDto
    {
        public int PersonalId { get; set; }
        public string Nama { get; set; }
        public string Nip { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
