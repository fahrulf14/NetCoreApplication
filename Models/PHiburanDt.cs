using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("P_Hiburan_Dt")]
    public partial class PHiburanDt
    {
        [Key]
        [Column("IdHiburan_Dt")]
        public Guid IdHiburanDt { get; set; }
        public Guid? IdHiburan { get; set; }
        [StringLength(50)]
        public string Jenis { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Tarif { get; set; }
        public int? Jumlah { get; set; }
        [StringLength(15)]
        public string Satuan { get; set; }

        [ForeignKey(nameof(IdHiburan))]
        [InverseProperty(nameof(PHiburan.PHiburanDt))]
        public virtual PHiburan IdHiburanNavigation { get; set; }
    }
}
