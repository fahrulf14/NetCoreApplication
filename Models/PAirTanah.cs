using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("P_AirTanah")]
    public partial class PAirTanah
    {
        [Key]
        public Guid IdAirTanah { get; set; }
        [Column("IdSPTPD")]
        public Guid? IdSptpd { get; set; }
        public Guid? IdDetail { get; set; }
        public double? Volume { get; set; }
        [Column("DPP", TypeName = "numeric(18,2)")]
        public decimal? Dpp { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? PajakTerhutang { get; set; }
        [Column("eu")]
        [StringLength(128)]
        public string Eu { get; set; }
        public DateTime? Ed { get; set; }

        [ForeignKey(nameof(IdDetail))]
        [InverseProperty(nameof(PAirTanahDt.PAirTanah))]
        public virtual PAirTanahDt IdDetailNavigation { get; set; }
        [ForeignKey(nameof(IdSptpd))]
        [InverseProperty(nameof(Sptpd.PAirTanah))]
        public virtual Sptpd IdSptpdNavigation { get; set; }
    }
}
