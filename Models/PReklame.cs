using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("P_Reklame")]
    public partial class PReklame
    {
        [Key]
        public Guid IdReklame { get; set; }
        [Column("IdSPTPD")]
        public Guid? IdSptpd { get; set; }
        [StringLength(50)]
        public string Judul { get; set; }
        [StringLength(100)]
        public string Teks { get; set; }
        [StringLength(200)]
        public string AlamatPasang { get; set; }
        [Column("IndKabKotaID")]
        public int? IndKabKotaId { get; set; }
        [Column("IndKecamatanID")]
        public int? IndKecamatanId { get; set; }
        [Column("IndKelurahanID")]
        public int? IndKelurahanId { get; set; }
        [StringLength(30)]
        public string KelasJalan { get; set; }
        [StringLength(50)]
        public string StatusReklame { get; set; }
        [StringLength(30)]
        public string JenisReklame { get; set; }
        [StringLength(50)]
        public string Bahan { get; set; }
        [StringLength(50)]
        public string Produk { get; set; }
        [StringLength(20)]
        public string Rokok { get; set; }
        [StringLength(20)]
        public string Letak { get; set; }
        public double? P1 { get; set; }
        public double? P2 { get; set; }
        public double? P3 { get; set; }
        public double? P4 { get; set; }
        public double? L1 { get; set; }
        public double? L2 { get; set; }
        public double? L3 { get; set; }
        public double? L4 { get; set; }
        public double? Luas { get; set; }
        public double? Tinggi { get; set; }
        public int? JumlahHari { get; set; }
        public int? JumlahDetik { get; set; }
        public int? Jumlah { get; set; }
        [Column(TypeName = "date")]
        public DateTime? TglPasang1 { get; set; }
        [Column(TypeName = "date")]
        public DateTime? TglPasang2 { get; set; }
        [Column("NSR", TypeName = "numeric(18,2)")]
        public decimal? Nsr { get; set; }
        [Column("NJOP", TypeName = "numeric(18,2)")]
        public decimal? Njop { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? NilaiKontrak { get; set; }
        [Column("DPP", TypeName = "numeric(18,2)")]
        public decimal? Dpp { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? PajakTerhutang { get; set; }
        [Column("eu")]
        [StringLength(128)]
        public string Eu { get; set; }
        public DateTime? Ed { get; set; }
        [Column("ed", TypeName = "timestamp(3) without time zone")]
        public DateTime? Ed1 { get; set; }

        [ForeignKey(nameof(IdSptpd))]
        [InverseProperty(nameof(Sptpd.PReklame))]
        public virtual Sptpd IdSptpdNavigation { get; set; }
        [ForeignKey(nameof(IndKabKotaId))]
        [InverseProperty("PReklame")]
        public virtual IndKabKota IndKabKota { get; set; }
        [ForeignKey(nameof(IndKecamatanId))]
        [InverseProperty("PReklame")]
        public virtual IndKecamatan IndKecamatan { get; set; }
        [ForeignKey(nameof(IndKelurahanId))]
        [InverseProperty("PReklame")]
        public virtual IndKelurahan IndKelurahan { get; set; }
    }
}
