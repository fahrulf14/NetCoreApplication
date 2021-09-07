using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    public partial class IndKelurahan
    {
        public IndKelurahan()
        {
            DataBadan = new HashSet<DataBadan>();
            DataSubjek = new HashSet<DataSubjek>();
            DataUsaha = new HashSet<DataUsaha>();
            PBphtb = new HashSet<PBphtb>();
            PReklame = new HashSet<PReklame>();
            Pemda = new HashSet<Pemda>();
            Spop = new HashSet<Spop>();
        }

        [Key]
        [Column("IndKelurahanID")]
        public int IndKelurahanId { get; set; }
        [StringLength(50)]
        public string Kelurahan { get; set; }
        [StringLength(15)]
        public string KodeKelurahan { get; set; }
        [StringLength(2)]
        public string NoUrutKelurahan { get; set; }
        [Column("IndKecamatanID")]
        public int IndKecamatanId { get; set; }

        [ForeignKey(nameof(IndKecamatanId))]
        [InverseProperty("IndKelurahan")]
        public virtual IndKecamatan IndKecamatan { get; set; }
        [InverseProperty("IndKelurahan")]
        public virtual ICollection<DataBadan> DataBadan { get; set; }
        [InverseProperty("IndKelurahan")]
        public virtual ICollection<DataSubjek> DataSubjek { get; set; }
        [InverseProperty("IndKelurahan")]
        public virtual ICollection<DataUsaha> DataUsaha { get; set; }
        [InverseProperty("IndKelurahan")]
        public virtual ICollection<PBphtb> PBphtb { get; set; }
        [InverseProperty("IndKelurahan")]
        public virtual ICollection<PReklame> PReklame { get; set; }
        [InverseProperty("IndKelurahan")]
        public virtual ICollection<Pemda> Pemda { get; set; }
        [InverseProperty("IndKelurahan")]
        public virtual ICollection<Spop> Spop { get; set; }
    }
}
