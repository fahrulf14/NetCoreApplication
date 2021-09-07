using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("Data_Badan")]
    public partial class DataBadan
    {
        [Key]
        public Guid IdBadan { get; set; }
        public Guid? IdSubjek { get; set; }
        [StringLength(50)]
        public string Nama { get; set; }
        [StringLength(200)]
        public string Alamat { get; set; }
        [Column("RTRW")]
        [StringLength(10)]
        public string Rtrw { get; set; }
        [Column("IndProvinsiID")]
        public int? IndProvinsiId { get; set; }
        [Column("IndKabKotaID")]
        public int? IndKabKotaId { get; set; }
        [Column("IndKecamatanID")]
        public int? IndKecamatanId { get; set; }
        [Column("IndKelurahanID")]
        public int? IndKelurahanId { get; set; }
        [StringLength(5)]
        public string KodePos { get; set; }
        [StringLength(13)]
        public string NoTelp { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        [Column("NIK")]
        [StringLength(16)]
        public string Nik { get; set; }
        [Column("NPWP")]
        [StringLength(20)]
        public string Npwp { get; set; }
        [StringLength(1)]
        public string Kelamin { get; set; }
        [Column(TypeName = "date")]
        public DateTime? TglLahir { get; set; }
        [StringLength(30)]
        public string Jabatan { get; set; }
        [StringLength(50)]
        public string PekerjaanLain { get; set; }
        [Column("eu")]
        [StringLength(128)]
        public string Eu { get; set; }
        public DateTime? Ed { get; set; }
        [Column("ed", TypeName = "timestamp(3) without time zone")]
        public DateTime? Ed1 { get; set; }

        [ForeignKey(nameof(IdSubjek))]
        [InverseProperty(nameof(DataSubjek.DataBadan))]
        public virtual DataSubjek IdSubjekNavigation { get; set; }
        [ForeignKey(nameof(IndKabKotaId))]
        [InverseProperty("DataBadan")]
        public virtual IndKabKota IndKabKota { get; set; }
        [ForeignKey(nameof(IndKecamatanId))]
        [InverseProperty("DataBadan")]
        public virtual IndKecamatan IndKecamatan { get; set; }
        [ForeignKey(nameof(IndKelurahanId))]
        [InverseProperty("DataBadan")]
        public virtual IndKelurahan IndKelurahan { get; set; }
        [ForeignKey(nameof(IndProvinsiId))]
        [InverseProperty("DataBadan")]
        public virtual IndProvinsi IndProvinsi { get; set; }
    }
}
