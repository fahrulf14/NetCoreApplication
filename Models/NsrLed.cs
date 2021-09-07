using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("NSR_LED")]
    public partial class NsrLed
    {
        [Key]
        [Column("IdNSR")]
        public int IdNsr { get; set; }
        [StringLength(30)]
        public string Lokasi { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? L1 { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? L2 { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? L3 { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? L4 { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? L5 { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? L6 { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? L7 { get; set; }
        public int? Durasi { get; set; }
        public bool FlagAktif { get; set; }
    }
}
