using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("SSRDR")]
    public partial class Ssrdr
    {
        public Ssrdr()
        {
            SsrdrDt = new HashSet<SsrdrDt>();
            StsDt = new HashSet<StsDt>();
        }

        [Key]
        [Column("IdSSRDR")]
        public Guid IdSsrdr { get; set; }
        [StringLength(10)]
        public string IdCoa { get; set; }
        public Guid? IdBank { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Tanggal { get; set; }
        public int? Nomor { get; set; }
        [Column("NoSSRDR")]
        [StringLength(50)]
        public string NoSsrdr { get; set; }
        public int? IdSetoran { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Total { get; set; }
        [Column(TypeName = "date")]
        public DateTime? TanggalValidasi { get; set; }
        public int? Keterangan { get; set; }
        [StringLength(50)]
        public string Nama { get; set; }
        [Column("NIK")]
        [StringLength(50)]
        public string Nik { get; set; }
        public bool StatusSetor { get; set; }
        [Column("eu")]
        [StringLength(128)]
        public string Eu { get; set; }
        public DateTime? Ed { get; set; }

        [ForeignKey(nameof(IdBank))]
        [InverseProperty(nameof(Bank.Ssrdr))]
        public virtual Bank IdBankNavigation { get; set; }
        [ForeignKey(nameof(IdCoa))]
        [InverseProperty(nameof(Coa.Ssrdr))]
        public virtual Coa IdCoaNavigation { get; set; }
        [ForeignKey(nameof(IdSetoran))]
        [InverseProperty(nameof(RefJenisSetoran.Ssrdr))]
        public virtual RefJenisSetoran IdSetoranNavigation { get; set; }
        [InverseProperty("IdSsrdrNavigation")]
        public virtual ICollection<SsrdrDt> SsrdrDt { get; set; }
        [InverseProperty("IdSsrdrNavigation")]
        public virtual ICollection<StsDt> StsDt { get; set; }
    }
}
