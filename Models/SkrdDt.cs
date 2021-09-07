using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("SKRD_Dt")]
    public partial class SkrdDt
    {
        [Key]
        [Column("IdSKRD_Dt")]
        public Guid IdSkrdDt { get; set; }
        [Column("IdSKRD")]
        public Guid? IdSkrd { get; set; }
        public Guid? IdTarif { get; set; }
        public int? Jumlah { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Total { get; set; }

        [ForeignKey(nameof(IdSkrd))]
        [InverseProperty(nameof(Skrd.SkrdDt))]
        public virtual Skrd IdSkrdNavigation { get; set; }
        [ForeignKey(nameof(IdTarif))]
        [InverseProperty(nameof(TarifRetribusi.SkrdDt))]
        public virtual TarifRetribusi IdTarifNavigation { get; set; }
    }
}
