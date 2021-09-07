using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("P_Walet")]
    public partial class PWalet
    {
        [Key]
        public Guid IdWalet { get; set; }
        [Column("IdSPTPD")]
        public Guid? IdSptpd { get; set; }
        [Column(TypeName = "numeric(18,0)")]
        public decimal? Volume { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? HargaPasar { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? HargaJual { get; set; }
        [Column("DPP", TypeName = "numeric(18,2)")]
        public decimal? Dpp { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? PajakTerhutang { get; set; }
        [Column("eu")]
        [StringLength(128)]
        public string Eu { get; set; }
        public DateTime? Ed { get; set; }

        [ForeignKey(nameof(IdSptpd))]
        [InverseProperty(nameof(Sptpd.PWalet))]
        public virtual Sptpd IdSptpdNavigation { get; set; }
    }
}
