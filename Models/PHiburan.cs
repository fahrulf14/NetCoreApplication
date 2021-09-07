using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("P_Hiburan")]
    public partial class PHiburan
    {
        public PHiburan()
        {
            PHiburanDt = new HashSet<PHiburanDt>();
        }

        [Key]
        public Guid IdHiburan { get; set; }
        [Column("IdSPTPD")]
        public Guid? IdSptpd { get; set; }
        [Column("JPertunjukanHb")]
        public int? JpertunjukanHb { get; set; }
        [Column("JPertunjukanHl")]
        public int? JpertunjukanHl { get; set; }
        [Column("JPengunjungHb")]
        public int? JpengunjungHb { get; set; }
        [Column("JPengunjungHl")]
        public int? JpengunjungHl { get; set; }
        [Column("JMesin")]
        public int? Jmesin { get; set; }
        [Column("JRuangan")]
        public int? Jruangan { get; set; }
        [Column("JKartuBebas")]
        public int? JkartuBebas { get; set; }
        public bool PenjualanMesinTiket { get; set; }
        public bool Pembukuan { get; set; }
        public bool KasRegister { get; set; }
        [Column("DPP", TypeName = "numeric(18,2)")]
        public decimal? Dpp { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? PajakTerhutang { get; set; }
        [Column("eu")]
        [StringLength(128)]
        public string Eu { get; set; }
        public DateTime? Ed { get; set; }

        [ForeignKey(nameof(IdSptpd))]
        [InverseProperty(nameof(Sptpd.PHiburan))]
        public virtual Sptpd IdSptpdNavigation { get; set; }
        [InverseProperty("IdHiburanNavigation")]
        public virtual ICollection<PHiburanDt> PHiburanDt { get; set; }
    }
}
