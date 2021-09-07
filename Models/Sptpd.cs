using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("SPTPD")]
    public partial class Sptpd
    {
        public Sptpd()
        {
            PAirTanah = new HashSet<PAirTanah>();
            PHiburan = new HashSet<PHiburan>();
            PHotel = new HashSet<PHotel>();
            PMineral = new HashSet<PMineral>();
            PParkir = new HashSet<PParkir>();
            PPenerangan = new HashSet<PPenerangan>();
            PReklame = new HashSet<PReklame>();
            PRestoran = new HashSet<PRestoran>();
            PWalet = new HashSet<PWalet>();
            Skpd = new HashSet<Skpd>();
            Skpdkb = new HashSet<Skpdkb>();
            Skpdkbt = new HashSet<Skpdkbt>();
            Sspd = new HashSet<Sspd>();
            SuratTeguran = new HashSet<SuratTeguran>();
        }

        [Key]
        [Column("IdSPTPD")]
        public Guid IdSptpd { get; set; }
        public Guid? IdSubjek { get; set; }
        public Guid? IdUsaha { get; set; }
        [StringLength(10)]
        public string IdCoa { get; set; }
        public Guid? IdTarif { get; set; }
        [StringLength(4)]
        public string Tahun { get; set; }
        public int? Nomor { get; set; }
        [Column("NoSPTPD")]
        [StringLength(50)]
        public string NoSptpd { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Tanggal { get; set; }
        [Column(TypeName = "date")]
        public DateTime? MasaPajak1 { get; set; }
        [Column(TypeName = "date")]
        public DateTime? MasaPajak2 { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Terhutang { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? KreditPajak { get; set; }
        [Column(TypeName = "date")]
        public DateTime? TanggalTerima { get; set; }
        [Column(TypeName = "date")]
        public DateTime? TanggalDiserahkan { get; set; }
        public bool FlagValidasi { get; set; }
        public bool FlagKonfirmasi { get; set; }
        [Column(TypeName = "date")]
        public DateTime? TanggalKonfirmasi { get; set; }
        [StringLength(20)]
        public string Jenis { get; set; }
        public bool Jabatan { get; set; }
        public int? Keterangan { get; set; }
        [StringLength(10)]
        public string Sk { get; set; }
        public bool FlagBatal { get; set; }
        [Column("eu")]
        [StringLength(128)]
        public string Eu { get; set; }
        public DateTime? Ed { get; set; }
        [Column("ed", TypeName = "timestamp(3) without time zone")]
        public DateTime? Ed1 { get; set; }

        [ForeignKey(nameof(IdCoa))]
        [InverseProperty(nameof(Coa.Sptpd))]
        public virtual Coa IdCoaNavigation { get; set; }
        [ForeignKey(nameof(IdSubjek))]
        [InverseProperty(nameof(DataSubjek.Sptpd))]
        public virtual DataSubjek IdSubjekNavigation { get; set; }
        [ForeignKey(nameof(IdTarif))]
        [InverseProperty(nameof(TarifPajak.Sptpd))]
        public virtual TarifPajak IdTarifNavigation { get; set; }
        [ForeignKey(nameof(IdUsaha))]
        [InverseProperty(nameof(DataUsaha.Sptpd))]
        public virtual DataUsaha IdUsahaNavigation { get; set; }
        [InverseProperty("IdSptpdNavigation")]
        public virtual ICollection<PAirTanah> PAirTanah { get; set; }
        [InverseProperty("IdSptpdNavigation")]
        public virtual ICollection<PHiburan> PHiburan { get; set; }
        [InverseProperty("IdSptpdNavigation")]
        public virtual ICollection<PHotel> PHotel { get; set; }
        [InverseProperty("IdSptpdNavigation")]
        public virtual ICollection<PMineral> PMineral { get; set; }
        [InverseProperty("IdSptpdNavigation")]
        public virtual ICollection<PParkir> PParkir { get; set; }
        [InverseProperty("IdSptpdNavigation")]
        public virtual ICollection<PPenerangan> PPenerangan { get; set; }
        [InverseProperty("IdSptpdNavigation")]
        public virtual ICollection<PReklame> PReklame { get; set; }
        [InverseProperty("IdSptpdNavigation")]
        public virtual ICollection<PRestoran> PRestoran { get; set; }
        [InverseProperty("IdSptpdNavigation")]
        public virtual ICollection<PWalet> PWalet { get; set; }
        [InverseProperty("IdSptpdNavigation")]
        public virtual ICollection<Skpd> Skpd { get; set; }
        [InverseProperty("IdSptpdNavigation")]
        public virtual ICollection<Skpdkb> Skpdkb { get; set; }
        [InverseProperty("IdSptpdNavigation")]
        public virtual ICollection<Skpdkbt> Skpdkbt { get; set; }
        [InverseProperty("IdSptpdNavigation")]
        public virtual ICollection<Sspd> Sspd { get; set; }
        [InverseProperty("IdSptpdNavigation")]
        public virtual ICollection<SuratTeguran> SuratTeguran { get; set; }
    }
}
