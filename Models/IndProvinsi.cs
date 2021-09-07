using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    public partial class IndProvinsi
    {
        public IndProvinsi()
        {
            DataBadan = new HashSet<DataBadan>();
            DataSubjek = new HashSet<DataSubjek>();
            DataUsaha = new HashSet<DataUsaha>();
            IndKabKota = new HashSet<IndKabKota>();
            Pemda = new HashSet<Pemda>();
        }

        [Key]
        [Column("IndProvinsiID")]
        public int IndProvinsiId { get; set; }
        [StringLength(50)]
        public string Provinsi { get; set; }
        [StringLength(15)]
        public string NomorProvinsi { get; set; }
        [StringLength(2)]
        public string KodeProvinsi { get; set; }

        [InverseProperty("IndProvinsi")]
        public virtual ICollection<DataBadan> DataBadan { get; set; }
        [InverseProperty("IndProvinsi")]
        public virtual ICollection<DataSubjek> DataSubjek { get; set; }
        [InverseProperty("IndProvinsi")]
        public virtual ICollection<DataUsaha> DataUsaha { get; set; }
        [InverseProperty("IndProvinsi")]
        public virtual ICollection<IndKabKota> IndKabKota { get; set; }
        [InverseProperty("IndProvinsi")]
        public virtual ICollection<Pemda> Pemda { get; set; }
    }
}
