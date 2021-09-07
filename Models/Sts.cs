using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("STS")]
    public partial class Sts
    {
        public Sts()
        {
            StsDt = new HashSet<StsDt>();
        }

        [Key]
        [Column("IdSTS")]
        public Guid IdSts { get; set; }
        public Guid? IdBank { get; set; }
        [StringLength(4)]
        public string Tahun { get; set; }
        public int? Nomor { get; set; }
        [Column("NoSTS")]
        [StringLength(50)]
        public string NoSts { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Tanggal { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? JumlahSetoran { get; set; }
        [StringLength(20)]
        public string Keterangan { get; set; }
        public bool FlagKonfirmasi { get; set; }
        public bool FlagValidasi { get; set; }
        [Column(TypeName = "date")]
        public DateTime? TanggalValidasi { get; set; }
        [Column("eu")]
        [StringLength(128)]
        public string Eu { get; set; }
        public DateTime? Ed { get; set; }

        [ForeignKey(nameof(IdBank))]
        [InverseProperty(nameof(Bank.Sts))]
        public virtual Bank IdBankNavigation { get; set; }
        [InverseProperty("IdStsNavigation")]
        public virtual ICollection<StsDt> StsDt { get; set; }
    }
}
