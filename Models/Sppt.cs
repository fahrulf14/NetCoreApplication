using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("SPPT")]
    public partial class Sppt
    {
        public Sppt()
        {
            Stts = new HashSet<Stts>();
        }

        [Key]
        [Column("IdSPPT")]
        public Guid IdSppt { get; set; }
        [Column("IdSPOP")]
        public Guid? IdSpop { get; set; }
        public Guid? IdTarif { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Tanggal { get; set; }
        [Column(TypeName = "date")]
        public DateTime? JatuhTempo { get; set; }
        public int? Tahun { get; set; }
        [Column("NJOPBumi", TypeName = "numeric(18,2)")]
        public decimal? Njopbumi { get; set; }
        [Column("NJOPBangunan", TypeName = "numeric(18,2)")]
        public decimal? Njopbangunan { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? TotalBumi { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? TotalBangunan { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? DasarPengenaan { get; set; }
        [Column("NJOPTKP", TypeName = "numeric(18,2)")]
        public decimal? Njoptkp { get; set; }
        [Column("NJOPHitung", TypeName = "numeric(18,2)")]
        public decimal? Njophitung { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Terhutang { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Kredit { get; set; }
        public bool FlagValidasi { get; set; }
        public int? Keterangan { get; set; }
        public bool Status { get; set; }
        [Column("eu")]
        [StringLength(128)]
        public string Eu { get; set; }
        public DateTime? Ed { get; set; }
        [Column("ed")]
        public DateTime? Ed1 { get; set; }

        [ForeignKey(nameof(IdSpop))]
        [InverseProperty(nameof(Spop.Sppt))]
        public virtual Spop IdSpopNavigation { get; set; }
        [ForeignKey(nameof(IdTarif))]
        [InverseProperty(nameof(TarifPajak.Sppt))]
        public virtual TarifPajak IdTarifNavigation { get; set; }
        [InverseProperty("IdSpptNavigation")]
        public virtual ICollection<Stts> Stts { get; set; }
    }
}
