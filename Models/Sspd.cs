using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("SSPD")]
    public partial class Sspd
    {
        public Sspd()
        {
            Skpdn = new HashSet<Skpdn>();
            StsDt = new HashSet<StsDt>();
        }

        [Key]
        [Column("IdSSPD")]
        public Guid IdSspd { get; set; }
        public Guid? IdSubjek { get; set; }
        public Guid? IdUsaha { get; set; }
        public Guid? IdBank { get; set; }
        [StringLength(10)]
        public string IdCoa { get; set; }
        [Column("IdSPTPD")]
        public Guid? IdSptpd { get; set; }
        [Column("IdSKPD")]
        public Guid? IdSkpd { get; set; }
        [Column("IdSKPDKB")]
        public Guid? IdSkpdkb { get; set; }
        [Column("IdSKPDKBT")]
        public Guid? IdSkpdkbt { get; set; }
        [Column("IdSTPD")]
        public Guid? IdStpd { get; set; }
        [StringLength(4)]
        public string Tahun { get; set; }
        public int? Nomor { get; set; }
        [Column("NoSSPD")]
        [StringLength(50)]
        public string NoSspd { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Tanggal { get; set; }
        public int? IdSetoran { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? JumlahSetoran { get; set; }
        public bool FlagBayar { get; set; }
        [Column(TypeName = "date")]
        public DateTime? TanggalBayar { get; set; }
        public bool StatusSetor { get; set; }
        [StringLength(50)]
        public string NoValidasi { get; set; }
        [Column("eu")]
        [StringLength(128)]
        public string Eu { get; set; }
        public DateTime? Ed { get; set; }
        [Column("ed", TypeName = "timestamp(3) without time zone")]
        public DateTime? Ed1 { get; set; }

        [ForeignKey(nameof(IdBank))]
        [InverseProperty(nameof(Bank.Sspd))]
        public virtual Bank IdBankNavigation { get; set; }
        [ForeignKey(nameof(IdCoa))]
        [InverseProperty(nameof(Coa.Sspd))]
        public virtual Coa IdCoaNavigation { get; set; }
        [ForeignKey(nameof(IdSetoran))]
        [InverseProperty(nameof(RefJenisSetoran.Sspd))]
        public virtual RefJenisSetoran IdSetoranNavigation { get; set; }
        [ForeignKey(nameof(IdSkpd))]
        [InverseProperty(nameof(Skpd.Sspd))]
        public virtual Skpd IdSkpdNavigation { get; set; }
        [ForeignKey(nameof(IdSkpdkb))]
        [InverseProperty(nameof(Skpdkb.Sspd))]
        public virtual Skpdkb IdSkpdkbNavigation { get; set; }
        [ForeignKey(nameof(IdSkpdkbt))]
        [InverseProperty(nameof(Skpdkbt.Sspd))]
        public virtual Skpdkbt IdSkpdkbtNavigation { get; set; }
        [ForeignKey(nameof(IdSptpd))]
        [InverseProperty(nameof(Sptpd.Sspd))]
        public virtual Sptpd IdSptpdNavigation { get; set; }
        [ForeignKey(nameof(IdStpd))]
        [InverseProperty(nameof(Stpd.Sspd))]
        public virtual Stpd IdStpdNavigation { get; set; }
        [ForeignKey(nameof(IdSubjek))]
        [InverseProperty(nameof(DataSubjek.Sspd))]
        public virtual DataSubjek IdSubjekNavigation { get; set; }
        [ForeignKey(nameof(IdUsaha))]
        [InverseProperty(nameof(DataUsaha.Sspd))]
        public virtual DataUsaha IdUsahaNavigation { get; set; }
        [InverseProperty("IdSspdNavigation")]
        public virtual ICollection<Skpdn> Skpdn { get; set; }
        [InverseProperty("IdSspdNavigation")]
        public virtual ICollection<StsDt> StsDt { get; set; }
    }
}
