using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("NSR_Lain")]
    public partial class NsrLain
    {
        [Key]
        [Column("IdNSR")]
        public int IdNsr { get; set; }
        [StringLength(30)]
        public string Jenis { get; set; }
        public int? Ukuran { get; set; }
        [StringLength(10)]
        public string SatuanUkuran { get; set; }
        public int? Waktu { get; set; }
        [StringLength(5)]
        public string SatuanWaktu { get; set; }
        [Column("NSR", TypeName = "numeric(18,2)")]
        public decimal? Nsr { get; set; }
        public bool FlagAktif { get; set; }
    }
}
