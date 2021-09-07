using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("P_Restoran")]
    public partial class PRestoran
    {
        public PRestoran()
        {
            PRestoranDt = new HashSet<PRestoranDt>();
        }

        [Key]
        public Guid IdRestoran { get; set; }
        [Column("IdSPTPD")]
        public Guid? IdSptpd { get; set; }
        public bool KasRegister { get; set; }
        public bool Pembukuan { get; set; }
        [Column("DPP", TypeName = "numeric(18,2)")]
        public decimal? Dpp { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? PajakTerhutang { get; set; }
        [Column("eu")]
        [StringLength(128)]
        public string Eu { get; set; }
        public DateTime? Ed { get; set; }
        [Column("ed", TypeName = "timestamp(3) without time zone")]
        public DateTime? Ed1 { get; set; }

        [ForeignKey(nameof(IdSptpd))]
        [InverseProperty(nameof(Sptpd.PRestoran))]
        public virtual Sptpd IdSptpdNavigation { get; set; }
        [InverseProperty("IdRestoranNavigation")]
        public virtual ICollection<PRestoranDt> PRestoranDt { get; set; }
    }
}
