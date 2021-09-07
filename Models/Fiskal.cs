using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    public partial class Fiskal
    {
        [Key]
        public Guid IdFiskal { get; set; }
        public Guid? IdSubjek { get; set; }
        public int? Nomor { get; set; }
        [StringLength(50)]
        public string NoFiskal { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Tanggal { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Berakhir { get; set; }
        [StringLength(50)]
        public string Tujuan { get; set; }

        [ForeignKey(nameof(IdSubjek))]
        [InverseProperty(nameof(DataSubjek.Fiskal))]
        public virtual DataSubjek IdSubjekNavigation { get; set; }
    }
}
