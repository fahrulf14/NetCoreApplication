using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("Ref_JenisBank")]
    public partial class RefJenisBank
    {
        [Key]
        public int IdJenis { get; set; }
        [StringLength(50)]
        public string Jenis { get; set; }
    }
}
