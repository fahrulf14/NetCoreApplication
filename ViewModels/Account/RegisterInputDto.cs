using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NUNA.ViewModels.Account
{
    public class RegisterInputDto
    {
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Input valid email address.")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name ="Nama")]
        public string Name { get; set; }
        public string ReturnUrl { get; set; }
    }
}
