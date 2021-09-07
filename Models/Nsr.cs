using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("NSR")]
    public partial class Nsr
    {
        [Key]
        [Column("IdNSR")]
        public int IdNsr { get; set; }
        [StringLength(30)]
        public string Lokasi { get; set; }
        public int? Ukuran { get; set; }
        [StringLength(10)]
        public string SatuanUkuran { get; set; }
        public int? Waktu { get; set; }
        [StringLength(5)]
        public string SatuanWaktu { get; set; }
        [Column("NSR_Produk", TypeName = "numeric(18,2)")]
        public decimal? NsrProduk { get; set; }
        [Column("NSR_NonProduk", TypeName = "numeric(18,2)")]
        public decimal? NsrNonProduk { get; set; }
        public bool FlagAktif { get; set; }
    }
}
