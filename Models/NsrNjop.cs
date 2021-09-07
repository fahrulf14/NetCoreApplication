using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("NSR_NJOP")]
    public partial class NsrNjop
    {
        [Key]
        public Guid IdNsrNjop { get; set; }
        [StringLength(10)]
        public string IdCoa { get; set; }
        [StringLength(50)]
        public string Jenis { get; set; }
        [StringLength(50)]
        public string SubJenis { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Njop { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Strategis1 { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? NonStrategis1 { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Strategis2 { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? NonStrategis2 { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Strategis3 { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? NonStrategis3 { get; set; }
        [StringLength(50)]
        public string Satuan { get; set; }

        [ForeignKey(nameof(IdCoa))]
        [InverseProperty(nameof(Coa.NsrNjop))]
        public virtual Coa IdCoaNavigation { get; set; }
    }
}
