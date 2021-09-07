using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("Data_Usaha")]
    public partial class DataUsaha
    {
        public DataUsaha()
        {
            DataIzin = new HashSet<DataIzin>();
            PAirTanahDt = new HashSet<PAirTanahDt>();
            Sptpd = new HashSet<Sptpd>();
            Sspd = new HashSet<Sspd>();
        }

        [Key]
        public Guid IdUsaha { get; set; }
        public Guid? IdSubjek { get; set; }
        public int? IdJenis { get; set; }
        [StringLength(50)]
        public string NamaUsaha { get; set; }
        [StringLength(200)]
        public string AlamatUsaha { get; set; }
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
        [StringLength(13)]
        public string NoTelp { get; set; }
        [StringLength(6)]
        public string KodePos { get; set; }
        public bool Status { get; set; }
        [Column("eu")]
        [StringLength(128)]
        public string Eu { get; set; }
        public DateTime? Ed { get; set; }
        [Column("ed", TypeName = "timestamp(3) without time zone")]
        public DateTime? Ed1 { get; set; }

        [ForeignKey(nameof(IdJenis))]
        [InverseProperty(nameof(RefUsaha.DataUsaha))]
        public virtual RefUsaha IdJenisNavigation { get; set; }
        [ForeignKey(nameof(IdSubjek))]
        [InverseProperty(nameof(DataSubjek.DataUsaha))]
        public virtual DataSubjek IdSubjekNavigation { get; set; }
        [ForeignKey(nameof(IndKabKotaId))]
        [InverseProperty("DataUsaha")]
        public virtual IndKabKota IndKabKota { get; set; }
        [ForeignKey(nameof(IndKecamatanId))]
        [InverseProperty("DataUsaha")]
        public virtual IndKecamatan IndKecamatan { get; set; }
        [ForeignKey(nameof(IndKelurahanId))]
        [InverseProperty("DataUsaha")]
        public virtual IndKelurahan IndKelurahan { get; set; }
        [ForeignKey(nameof(IndProvinsiId))]
        [InverseProperty("DataUsaha")]
        public virtual IndProvinsi IndProvinsi { get; set; }
        [InverseProperty("IdUsahaNavigation")]
        public virtual ICollection<DataIzin> DataIzin { get; set; }
        [InverseProperty("IdUsahaNavigation")]
        public virtual ICollection<PAirTanahDt> PAirTanahDt { get; set; }
        [InverseProperty("IdUsahaNavigation")]
        public virtual ICollection<Sptpd> Sptpd { get; set; }
        [InverseProperty("IdUsahaNavigation")]
        public virtual ICollection<Sspd> Sspd { get; set; }
    }
}
