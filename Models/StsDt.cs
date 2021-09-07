using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("STS_Dt")]
    public partial class StsDt
    {
        [Key]
        [Column("IdDetailSTS")]
        public Guid IdDetailSts { get; set; }
        [Column("IdSTS")]
        public Guid? IdSts { get; set; }
        [Column("IdSTTS")]
        public Guid? IdStts { get; set; }
        [Column("IdSSPD")]
        public Guid? IdSspd { get; set; }
        [Column("IdSSRD")]
        public Guid? IdSsrd { get; set; }
        [Column("IdSSRDR")]
        public Guid? IdSsrdr { get; set; }

        [ForeignKey(nameof(IdSspd))]
        [InverseProperty(nameof(Sspd.StsDt))]
        public virtual Sspd IdSspdNavigation { get; set; }
        [ForeignKey(nameof(IdSsrd))]
        [InverseProperty(nameof(Ssrd.StsDt))]
        public virtual Ssrd IdSsrdNavigation { get; set; }
        [ForeignKey(nameof(IdSsrdr))]
        [InverseProperty(nameof(Ssrdr.StsDt))]
        public virtual Ssrdr IdSsrdrNavigation { get; set; }
        [ForeignKey(nameof(IdSts))]
        [InverseProperty(nameof(Sts.StsDt))]
        public virtual Sts IdStsNavigation { get; set; }
        [ForeignKey(nameof(IdStts))]
        [InverseProperty(nameof(Stts.StsDt))]
        public virtual Stts IdSttsNavigation { get; set; }
    }
}
