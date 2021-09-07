using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    public partial class Pemda
    {
        [Key]
        public Guid IdPemda { get; set; }
        [StringLength(100)]
        public string NamaPemda { get; set; }
        [Column("NamaOPD")]
        [StringLength(100)]
        public string NamaOpd { get; set; }
        [StringLength(200)]
        public string Alamat { get; set; }
        [StringLength(20)]
        public string Telp { get; set; }
        [StringLength(20)]
        public string Telp2 { get; set; }
        [StringLength(50)]
        public string Website { get; set; }
        [StringLength(50)]
        public string AplikasiLink { get; set; }
        public string Logo { get; set; }
        public string LogoCetak { get; set; }
        [StringLength(5)]
        public string KodePos { get; set; }
        [Column("IndKelurahanID")]
        public int? IndKelurahanId { get; set; }
        [Column("IndKecamatanID")]
        public int? IndKecamatanId { get; set; }
        [Column("IndKabKotaID")]
        public int? IndKabKotaId { get; set; }
        [Column("IndProvinsiID")]
        public int? IndProvinsiId { get; set; }
        [StringLength(2)]
        public string NoUrutKecamatan { get; set; }
        [StringLength(2)]
        public string NoUrutKelurahan { get; set; }
        public bool FlagAktif { get; set; }
        [Column("eu")]
        [StringLength(128)]
        public string Eu { get; set; }
        public DateTime? Ed { get; set; }
        [Column("ed")]
        public DateTime? Ed1 { get; set; }

        [ForeignKey(nameof(IndKabKotaId))]
        [InverseProperty("Pemda")]
        public virtual IndKabKota IndKabKota { get; set; }
        [ForeignKey(nameof(IndKecamatanId))]
        [InverseProperty("Pemda")]
        public virtual IndKecamatan IndKecamatan { get; set; }
        [ForeignKey(nameof(IndKelurahanId))]
        [InverseProperty("Pemda")]
        public virtual IndKelurahan IndKelurahan { get; set; }
        [ForeignKey(nameof(IndProvinsiId))]
        [InverseProperty("Pemda")]
        public virtual IndProvinsi IndProvinsi { get; set; }
    }
}
