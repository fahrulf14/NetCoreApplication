using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("Ref_Reklame")]
    public partial class RefReklame
    {
        [Key]
        public int IdRef { get; set; }
        [StringLength(50)]
        public string Uraian { get; set; }
        public double? Tambahan { get; set; }
        public bool FlagAktif { get; set; }
    }
}
