using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("SKPDN")]
    public partial class Skpdn
    {
        [Key]
        [Column("IdSKPDN")]
        public Guid IdSkpdn { get; set; }
        public int? Nomor { get; set; }
        [Column("NoSKPDN")]
        [StringLength(30)]
        public string NoSkpdn { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Tanggal { get; set; }
        [Column(TypeName = "date")]
        public DateTime? MasaPajak1 { get; set; }
        [Column(TypeName = "date")]
        public DateTime? MasaPajak2 { get; set; }
        public Guid? IdSptpd { get; set; }
        [Column("IdSSPD")]
        public Guid? IdSspd { get; set; }
        [StringLength(50)]
        public string Jenis { get; set; }
        [StringLength(30)]
        public string NoSurat { get; set; }
        [Column(TypeName = "date")]
        public DateTime? TanggalSurat { get; set; }
        [StringLength(10)]
        public string IdCoa { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? PokokPajak { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? KompKelebihan { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Lainnya { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Tambahan { get; set; }
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
        [Column("eu")]
        [StringLength(128)]
        public string Eu { get; set; }
        public DateTime? Ed { get; set; }
        [Column("ed", TypeName = "timestamp(3) without time zone")]
        public DateTime? Ed1 { get; set; }

        [ForeignKey(nameof(IdCoa))]
        [InverseProperty(nameof(Coa.Skpdn))]
        public virtual Coa IdCoaNavigation { get; set; }
        [ForeignKey(nameof(IdSspd))]
        [InverseProperty(nameof(Sspd.Skpdn))]
        public virtual Sspd IdSspdNavigation { get; set; }
    }
}
