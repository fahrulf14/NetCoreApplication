using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("Ref_HargaKwh")]
    public partial class RefHargaKwh
    {
        [Key]
        public int IdRef { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Tarif { get; set; }
        [Column(TypeName = "date")]
        public DateTime? TanggalBerlaku { get; set; }
        public bool FlagAktif { get; set; }
    }
}
