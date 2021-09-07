using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("Ref_BobotKP_Batas")]
    public partial class RefBobotKpBatas
    {
        [Key]
        public int IdRef { get; set; }
        public int? Batas1 { get; set; }
        public int? Batas2 { get; set; }
        public int? Batas3 { get; set; }
        public int? Batas4 { get; set; }
        public int? Batas5 { get; set; }
    }
}
