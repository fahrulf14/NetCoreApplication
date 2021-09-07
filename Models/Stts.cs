using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("STTS")]
    public partial class Stts
    {
        public Stts()
        {
            StsDt = new HashSet<StsDt>();
        }

        [Key]
        [Column("IdSTTS")]
        public Guid IdStts { get; set; }
        [Column("IdSPPT")]
        public Guid? IdSppt { get; set; }
        [Column(TypeName = "date")]
        public DateTime? TanggalBayar { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Denda { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? TotalBayar { get; set; }
        public int? IdSetoran { get; set; }
        public Guid? IdBank { get; set; }
        public bool StatusSetor { get; set; }
        public bool FlagValidasi { get; set; }
        [Column(TypeName = "date")]
        public DateTime? TanggalValidasi { get; set; }
        [Column("SN")]
        [StringLength(50)]
        public string Sn { get; set; }
        [Column("eu")]
        [StringLength(128)]
        public string Eu { get; set; }
        public DateTime? Ed { get; set; }

        [ForeignKey(nameof(IdBank))]
        [InverseProperty(nameof(Bank.Stts))]
        public virtual Bank IdBankNavigation { get; set; }
        [ForeignKey(nameof(IdSetoran))]
        [InverseProperty(nameof(RefJenisSetoran.Stts))]
        public virtual RefJenisSetoran IdSetoranNavigation { get; set; }
        [ForeignKey(nameof(IdSppt))]
        [InverseProperty(nameof(Sppt.Stts))]
        public virtual Sppt IdSpptNavigation { get; set; }
        [InverseProperty("IdSttsNavigation")]
        public virtual ICollection<StsDt> StsDt { get; set; }
    }
}
