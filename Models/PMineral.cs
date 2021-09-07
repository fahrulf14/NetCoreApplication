using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("P_Mineral")]
    public partial class PMineral
    {
        [Key]
        public Guid IdMineral { get; set; }
        [Column("IdSPTPD")]
        public Guid? IdSptpd { get; set; }
        public double? Volume { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? HargaJual { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? HargaDasar { get; set; }
        [Column("DPP", TypeName = "numeric(18,2)")]
        public decimal? Dpp { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? PajakTerhutang { get; set; }
        [StringLength(50)]
        public string NamaProyek { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Anggaran { get; set; }
        [Column("eu")]
        [StringLength(128)]
        public string Eu { get; set; }
        public DateTime? Ed { get; set; }

        [ForeignKey(nameof(IdSptpd))]
        [InverseProperty(nameof(Sptpd.PMineral))]
        public virtual Sptpd IdSptpdNavigation { get; set; }
    }
}
