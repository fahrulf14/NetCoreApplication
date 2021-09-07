using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("P_Restoran_Dt")]
    public partial class PRestoranDt
    {
        [Key]
        [Column("IdRestoran_Dt")]
        public Guid IdRestoranDt { get; set; }
        public Guid? IdRestoran { get; set; }
        public int? IdRef { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Jumlah { get; set; }

        [ForeignKey(nameof(IdRef))]
        [InverseProperty(nameof(RefRestoran.PRestoranDt))]
        public virtual RefRestoran IdRefNavigation { get; set; }
        [ForeignKey(nameof(IdRestoran))]
        [InverseProperty(nameof(PRestoran.PRestoranDt))]
        public virtual PRestoran IdRestoranNavigation { get; set; }
    }
}
