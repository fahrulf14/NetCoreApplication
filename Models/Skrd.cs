using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("SKRD")]
    public partial class Skrd
    {
        public Skrd()
        {
            SkrdDt = new HashSet<SkrdDt>();
            Ssrd = new HashSet<Ssrd>();
        }

        [Key]
        [Column("IdSKRD")]
        public Guid IdSkrd { get; set; }
        public Guid? IdSubjek { get; set; }
        [StringLength(10)]
        public string IdCoa { get; set; }
        [Column("TanggalSKRD", TypeName = "date")]
        public DateTime? TanggalSkrd { get; set; }
        [StringLength(4)]
        public string Tahun { get; set; }
        public int? Nomor { get; set; }
        [Column("NoSKRD")]
        [StringLength(50)]
        public string NoSkrd { get; set; }
        [Column(TypeName = "date")]
        public DateTime? MasaRetribusi1 { get; set; }
        [Column(TypeName = "date")]
        public DateTime? MasaRetribusi2 { get; set; }
        [Column(TypeName = "date")]
        public DateTime? JatuhTempo { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Bunga { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Kenaikan { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Terhutang { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Kredit { get; set; }
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

        [ForeignKey(nameof(IdCoa))]
        [InverseProperty(nameof(Coa.Skrd))]
        public virtual Coa IdCoaNavigation { get; set; }
        [ForeignKey(nameof(IdSubjek))]
        [InverseProperty(nameof(DataSubjek.Skrd))]
        public virtual DataSubjek IdSubjekNavigation { get; set; }
        [InverseProperty("IdSkrdNavigation")]
        public virtual ICollection<SkrdDt> SkrdDt { get; set; }
        [InverseProperty("IdSkrdNavigation")]
        public virtual ICollection<Ssrd> Ssrd { get; set; }
    }
}
