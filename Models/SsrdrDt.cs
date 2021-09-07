using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("SSRDR_Dt")]
    public partial class SsrdrDt
    {
        [Key]
        [Column("IdSSRDR_Dt")]
        public Guid IdSsrdrDt { get; set; }
        [Column("IdSSRDR")]
        public Guid? IdSsrdr { get; set; }
        public Guid? IdTarif { get; set; }
        [StringLength(10)]
        public string Nomor1 { get; set; }
        [StringLength(10)]
        public string Nomor2 { get; set; }
        public int? Jumlah { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Total { get; set; }

        [ForeignKey(nameof(IdSsrdr))]
        [InverseProperty(nameof(Ssrdr.SsrdrDt))]
        public virtual Ssrdr IdSsrdrNavigation { get; set; }
        [ForeignKey(nameof(IdTarif))]
        [InverseProperty(nameof(TarifRetribusi.SsrdrDt))]
        public virtual TarifRetribusi IdTarifNavigation { get; set; }
    }
}
