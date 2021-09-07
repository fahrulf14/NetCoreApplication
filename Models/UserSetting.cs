using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    public partial class UserSetting
    {
        [Key]
        public string Id { get; set; }
        [StringLength(256)]
        public string Email { get; set; }
        [StringLength(50)]
        public string Theme { get; set; }
        [StringLength(50)]
        public string Aside { get; set; }
    }
}
