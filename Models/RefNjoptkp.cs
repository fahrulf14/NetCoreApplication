using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("Ref_NJOPTKP")]
    public partial class RefNjoptkp
    {
        [Key]
        public int IdRef { get; set; }
        [Column("NJOPTKP", TypeName = "numeric(18,2)")]
        public decimal? Njoptkp { get; set; }
        [StringLength(4)]
        public string Tahun { get; set; }
    }
}
