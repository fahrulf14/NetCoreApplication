using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("SSRD")]
    public partial class Ssrd
    {
        public Ssrd()
        {
            StsDt = new HashSet<StsDt>();
        }

        [Key]
        [Column("IdSSRD")]
        public Guid IdSsrd { get; set; }
        public Guid? IdSubjek { get; set; }
        public Guid? IdBank { get; set; }
        [StringLength(10)]
        public string IdCoa { get; set; }
        [Column("IdSKRD")]
        public Guid? IdSkrd { get; set; }
        public int? Nomor { get; set; }
        [Column("NoSSRD")]
        [StringLength(50)]
        public string NoSsrd { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Tanggal { get; set; }
        public int? IdSetoran { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? JumlahSetoran { get; set; }
        public bool FlagBayar { get; set; }
        [Column(TypeName = "date")]
        public DateTime? TanggalBayar { get; set; }
        public bool StatusSetor { get; set; }
        [Column("eu")]
        [StringLength(128)]
        public string Eu { get; set; }
        public DateTime? Ed { get; set; }

        [ForeignKey(nameof(IdBank))]
        [InverseProperty(nameof(Bank.Ssrd))]
        public virtual Bank IdBankNavigation { get; set; }
        [ForeignKey(nameof(IdCoa))]
        [InverseProperty(nameof(Coa.Ssrd))]
        public virtual Coa IdCoaNavigation { get; set; }
        [ForeignKey(nameof(IdSetoran))]
        [InverseProperty(nameof(RefJenisSetoran.Ssrd))]
        public virtual RefJenisSetoran IdSetoranNavigation { get; set; }
        [ForeignKey(nameof(IdSkrd))]
        [InverseProperty(nameof(Skrd.Ssrd))]
        public virtual Skrd IdSkrdNavigation { get; set; }
        [ForeignKey(nameof(IdSubjek))]
        [InverseProperty(nameof(DataSubjek.Ssrd))]
        public virtual DataSubjek IdSubjekNavigation { get; set; }
        [InverseProperty("IdSsrdNavigation")]
        public virtual ICollection<StsDt> StsDt { get; set; }
    }
}
