using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("P_Penerangan")]
    public partial class PPenerangan
    {
        [Key]
        public Guid IdPenerangan { get; set; }
        [Column("IdSPTPD")]
        public Guid? IdSptpd { get; set; }
        [StringLength(50)]
        public string Sumber { get; set; }
        [StringLength(100)]
        public string Golongan { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Bulan { get; set; }
        public double? JumlahKwh { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? TarifListrik { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Tagihan { get; set; }
        public bool Rekap { get; set; }
        [Column("DPP", TypeName = "numeric(18,2)")]
        public decimal? Dpp { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? PajakTerhutang { get; set; }
        [Column("eu")]
        [StringLength(128)]
        public string Eu { get; set; }
        public DateTime? Ed { get; set; }

        [ForeignKey(nameof(IdSptpd))]
        [InverseProperty(nameof(Sptpd.PPenerangan))]
        public virtual Sptpd IdSptpdNavigation { get; set; }
    }
}
