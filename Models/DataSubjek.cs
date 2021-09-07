using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("Data_Subjek")]
    public partial class DataSubjek
    {
        public DataSubjek()
        {
            DataBadan = new HashSet<DataBadan>();
            DataUsaha = new HashSet<DataUsaha>();
            Fiskal = new HashSet<Fiskal>();
            Skrd = new HashSet<Skrd>();
            Spop = new HashSet<Spop>();
            Sptpd = new HashSet<Sptpd>();
            Sspd = new HashSet<Sspd>();
            Ssrd = new HashSet<Ssrd>();
        }

        [Key]
        public Guid IdSubjek { get; set; }
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
        [StringLength(20)]
        public string NoHp { get; set; }
        [StringLength(13)]
        public string NoTelp { get; set; }
        [StringLength(5)]
        public string KodePos { get; set; }
        [StringLength(30)]
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
        public int? IdPekerjaan { get; set; }
        [StringLength(50)]
        public string PekerjaanLain { get; set; }
        [StringLength(50)]
        public string NamaInstansi { get; set; }
        public int? IdBadanHukum { get; set; }
        [Column(TypeName = "date")]
        public DateTime? TglDaftar { get; set; }
        public int? NoPokok { get; set; }
        [Column("NPWPD")]
        [StringLength(20)]
        public string Npwpd { get; set; }
        [Column("NPWRD")]
        [StringLength(20)]
        public string Npwrd { get; set; }
        public bool Status { get; set; }
        public bool? Registered { get; set; }
        [Column("eu")]
        [StringLength(128)]
        public string Eu { get; set; }
        public DateTime? Ed { get; set; }
        [Column("ed")]
        public DateTime? Ed1 { get; set; }

        [ForeignKey(nameof(IdBadanHukum))]
        [InverseProperty(nameof(RefBadanHukum.DataSubjek))]
        public virtual RefBadanHukum IdBadanHukumNavigation { get; set; }
        [ForeignKey(nameof(IdPekerjaan))]
        [InverseProperty(nameof(RefPekerjaan.DataSubjek))]
        public virtual RefPekerjaan IdPekerjaanNavigation { get; set; }
        [ForeignKey(nameof(IndKabKotaId))]
        [InverseProperty("DataSubjek")]
        public virtual IndKabKota IndKabKota { get; set; }
        [ForeignKey(nameof(IndKecamatanId))]
        [InverseProperty("DataSubjek")]
        public virtual IndKecamatan IndKecamatan { get; set; }
        [ForeignKey(nameof(IndKelurahanId))]
        [InverseProperty("DataSubjek")]
        public virtual IndKelurahan IndKelurahan { get; set; }
        [ForeignKey(nameof(IndProvinsiId))]
        [InverseProperty("DataSubjek")]
        public virtual IndProvinsi IndProvinsi { get; set; }
        [InverseProperty("IdSubjekNavigation")]
        public virtual ICollection<DataBadan> DataBadan { get; set; }
        [InverseProperty("IdSubjekNavigation")]
        public virtual ICollection<DataUsaha> DataUsaha { get; set; }
        [InverseProperty("IdSubjekNavigation")]
        public virtual ICollection<Fiskal> Fiskal { get; set; }
        [InverseProperty("IdSubjekNavigation")]
        public virtual ICollection<Skrd> Skrd { get; set; }
        [InverseProperty("IdSubjekNavigation")]
        public virtual ICollection<Spop> Spop { get; set; }
        [InverseProperty("IdSubjekNavigation")]
        public virtual ICollection<Sptpd> Sptpd { get; set; }
        [InverseProperty("IdSubjekNavigation")]
        public virtual ICollection<Sspd> Sspd { get; set; }
        [InverseProperty("IdSubjekNavigation")]
        public virtual ICollection<Ssrd> Ssrd { get; set; }
    }
}
