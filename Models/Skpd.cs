using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("SKPD")]
    public partial class Skpd
    {
        public Skpd()
        {
            Sspd = new HashSet<Sspd>();
            Stpd = new HashSet<Stpd>();
        }

        [Key]
        [Column("IdSKPD")]
        public Guid IdSkpd { get; set; }
        [Column("IdSPTPD")]
        public Guid? IdSptpd { get; set; }
        public int? Nomor { get; set; }
        [Column("NoSKPD")]
        [StringLength(50)]
        public string NoSkpd { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Tanggal { get; set; }
        [Column(TypeName = "date")]
        public DateTime? JatuhTempo { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Bunga { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Kenaikan { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Terhutang { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? KreditPajak { get; set; }
        [Column(TypeName = "date")]
        public DateTime? TanggalDiserahkan { get; set; }
        public bool FlagValidasi { get; set; }
        [Column(TypeName = "date")]
        public DateTime? TanggalValidasi { get; set; }
        public int? Keterangan { get; set; }
        [StringLength(50)]
        public string Sk { get; set; }
        [Column("eu")]
        [StringLength(128)]
        public string Eu { get; set; }
        public DateTime? Ed { get; set; }
        [Column("ed", TypeName = "timestamp(3) without time zone")]
        public DateTime? Ed1 { get; set; }

        [ForeignKey(nameof(IdSptpd))]
        [InverseProperty(nameof(Sptpd.Skpd))]
        public virtual Sptpd IdSptpdNavigation { get; set; }
        [InverseProperty("IdSkpdNavigation")]
        public virtual ICollection<Sspd> Sspd { get; set; }
        [InverseProperty("IdSkpdNavigation")]
        public virtual ICollection<Stpd> Stpd { get; set; }
    }
}
