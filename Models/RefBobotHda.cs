using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("Ref_BobotHDA")]
    public partial class RefBobotHda
    {
        [Key]
        public int IdRef { get; set; }
        [StringLength(50)]
        public string Kriteria { get; set; }
        public double? Bobot { get; set; }
    }
}
