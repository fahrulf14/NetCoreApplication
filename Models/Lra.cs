using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("LRA")]
    public partial class Lra
    {
        public Lra()
        {
            TransaksiLra = new HashSet<TransaksiLra>();
        }

        [Key]
        [Column("IdLRA")]
        public Guid IdLra { get; set; }
        [StringLength(10)]
        public string IdCoa { get; set; }
        public int? Tahun { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Anggaran { get; set; }
        [Column("APerubahan", TypeName = "numeric(18,2)")]
        public decimal? Aperubahan { get; set; }
        [Column("AFinal", TypeName = "numeric(18,2)")]
        public decimal? Afinal { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal Realisasi { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Januari { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Februari { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Maret { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? April { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Mei { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Juni { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Juli { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Agustus { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? September { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Oktober { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? November { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Desember { get; set; }

        [ForeignKey(nameof(IdCoa))]
        [InverseProperty(nameof(Coa.Lra))]
        public virtual Coa IdCoaNavigation { get; set; }
        [InverseProperty("IdLraNavigation")]
        public virtual ICollection<TransaksiLra> TransaksiLra { get; set; }
    }
}
