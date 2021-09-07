using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("STPD")]
    public partial class Stpd
    {
        public Stpd()
        {
            Sspd = new HashSet<Sspd>();
        }

        [Key]
        [Column("IdSTPD")]
        public Guid IdStpd { get; set; }
        [Column("IdSKPD")]
        public Guid? IdSkpd { get; set; }
        public int? Nomor { get; set; }
        [Column("NoSTPD")]
        [StringLength(30)]
        public string NoStpd { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Tanggal { get; set; }
        [Column(TypeName = "date")]
        public DateTime? JatuhTempo { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? KurangBayar { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Bunga { get; set; }
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

        [ForeignKey(nameof(IdSkpd))]
        [InverseProperty(nameof(Skpd.Stpd))]
        public virtual Skpd IdSkpdNavigation { get; set; }
        [InverseProperty("IdStpdNavigation")]
        public virtual ICollection<Sspd> Sspd { get; set; }
    }
}
