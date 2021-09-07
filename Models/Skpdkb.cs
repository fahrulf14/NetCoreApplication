using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("SKPDKB")]
    public partial class Skpdkb
    {
        public Skpdkb()
        {
            Sspd = new HashSet<Sspd>();
        }

        [Key]
        [Column("IdSKPDKB")]
        public Guid IdSkpdkb { get; set; }
        [Column("IdSPTPD")]
        public Guid? IdSptpd { get; set; }
        public int? Nomor { get; set; }
        [Column("NoSKPDKB")]
        [StringLength(30)]
        public string NoSkpdkb { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Tanggal { get; set; }
        [Column(TypeName = "date")]
        public DateTime? JatuhTempo { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? PokokPajak { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? KompKelebihan { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Lainnya { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? KurangBayar { get; set; }
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

        [ForeignKey(nameof(IdSptpd))]
        [InverseProperty(nameof(Sptpd.Skpdkb))]
        public virtual Sptpd IdSptpdNavigation { get; set; }
        [InverseProperty("IdSkpdkbNavigation")]
        public virtual ICollection<Sspd> Sspd { get; set; }
    }
}
