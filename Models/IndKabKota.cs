using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    public partial class IndKabKota
    {
        public IndKabKota()
        {
            DataBadan = new HashSet<DataBadan>();
            DataSubjek = new HashSet<DataSubjek>();
            DataUsaha = new HashSet<DataUsaha>();
            IndKecamatan = new HashSet<IndKecamatan>();
            PBphtb = new HashSet<PBphtb>();
            PReklame = new HashSet<PReklame>();
            Pemda = new HashSet<Pemda>();
            Spop = new HashSet<Spop>();
        }

        [Key]
        [Column("IndKabKotaID")]
        public int IndKabKotaId { get; set; }
        [StringLength(50)]
        public string KabKota { get; set; }
        [StringLength(15)]
        public string KodeKabKota { get; set; }
        public int? NoUrutKabKota { get; set; }
        [Column("IndProvinsiID")]
        public int IndProvinsiId { get; set; }

        [ForeignKey(nameof(IndProvinsiId))]
        [InverseProperty("IndKabKota")]
        public virtual IndProvinsi IndProvinsi { get; set; }
        [InverseProperty("IndKabKota")]
        public virtual ICollection<DataBadan> DataBadan { get; set; }
        [InverseProperty("IndKabKota")]
        public virtual ICollection<DataSubjek> DataSubjek { get; set; }
        [InverseProperty("IndKabKota")]
        public virtual ICollection<DataUsaha> DataUsaha { get; set; }
        [InverseProperty("IndKabKota")]
        public virtual ICollection<IndKecamatan> IndKecamatan { get; set; }
        [InverseProperty("IndKabKota")]
        public virtual ICollection<PBphtb> PBphtb { get; set; }
        [InverseProperty("IndKabKota")]
        public virtual ICollection<PReklame> PReklame { get; set; }
        [InverseProperty("IndKabKota")]
        public virtual ICollection<Pemda> Pemda { get; set; }
        [InverseProperty("IndKabKota")]
        public virtual ICollection<Spop> Spop { get; set; }
    }
}
