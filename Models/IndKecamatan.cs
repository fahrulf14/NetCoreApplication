using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    public partial class IndKecamatan
    {
        public IndKecamatan()
        {
            DataBadan = new HashSet<DataBadan>();
            DataSubjek = new HashSet<DataSubjek>();
            DataUsaha = new HashSet<DataUsaha>();
            IndKelurahan = new HashSet<IndKelurahan>();
            PBphtb = new HashSet<PBphtb>();
            PReklame = new HashSet<PReklame>();
            Pemda = new HashSet<Pemda>();
            Spop = new HashSet<Spop>();
        }

        [Key]
        [Column("IndKecamatanID")]
        public int IndKecamatanId { get; set; }
        [StringLength(50)]
        public string Kecamatan { get; set; }
        [StringLength(2)]
        public string NoUrutKecamatan { get; set; }
        [StringLength(15)]
        public string KodeKecamatan { get; set; }
        [Column("IndKabKotaID")]
        public int? IndKabKotaId { get; set; }

        [ForeignKey(nameof(IndKabKotaId))]
        [InverseProperty("IndKecamatan")]
        public virtual IndKabKota IndKabKota { get; set; }
        [InverseProperty("IndKecamatan")]
        public virtual ICollection<DataBadan> DataBadan { get; set; }
        [InverseProperty("IndKecamatan")]
        public virtual ICollection<DataSubjek> DataSubjek { get; set; }
        [InverseProperty("IndKecamatan")]
        public virtual ICollection<DataUsaha> DataUsaha { get; set; }
        [InverseProperty("IndKecamatan")]
        public virtual ICollection<IndKelurahan> IndKelurahan { get; set; }
        [InverseProperty("IndKecamatan")]
        public virtual ICollection<PBphtb> PBphtb { get; set; }
        [InverseProperty("IndKecamatan")]
        public virtual ICollection<PReklame> PReklame { get; set; }
        [InverseProperty("IndKecamatan")]
        public virtual ICollection<Pemda> Pemda { get; set; }
        [InverseProperty("IndKecamatan")]
        public virtual ICollection<Spop> Spop { get; set; }
    }
}
