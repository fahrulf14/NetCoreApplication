using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("Ref_HargaPasar")]
    public partial class RefHargaPasar
    {
        [Key]
        public int IdHarga { get; set; }
        [StringLength(10)]
        public string IdCoa { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Harga { get; set; }
        public bool FlagAktif { get; set; }

        [ForeignKey(nameof(IdCoa))]
        [InverseProperty(nameof(Coa.RefHargaPasar))]
        public virtual Coa IdCoaNavigation { get; set; }
    }
}
