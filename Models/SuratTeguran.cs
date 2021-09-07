using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    public partial class SuratTeguran
    {
        [Key]
        public Guid IdTeguran { get; set; }
        [Column("IdSPTPD")]
        public Guid? IdSptpd { get; set; }
        public int? Nomor { get; set; }
        [StringLength(30)]
        public string NoTeguran { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Tanggal { get; set; }
        public bool FlagKonfirmasi { get; set; }

        [ForeignKey(nameof(IdSptpd))]
        [InverseProperty(nameof(Sptpd.SuratTeguran))]
        public virtual Sptpd IdSptpdNavigation { get; set; }
    }
}
