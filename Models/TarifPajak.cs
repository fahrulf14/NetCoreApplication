using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("Tarif_Pajak")]
    public partial class TarifPajak
    {
        public TarifPajak()
        {
            PBphtb = new HashSet<PBphtb>();
            Sppt = new HashSet<Sppt>();
            Sptpd = new HashSet<Sptpd>();
        }

        [Key]
        public Guid IdTarif { get; set; }
        [StringLength(10)]
        public string IdCoa { get; set; }
        public double? Tarif { get; set; }
        [Column(TypeName = "date")]
        public DateTime? MulaiBerlaku { get; set; }
        public bool FlagAktif { get; set; }

        [ForeignKey(nameof(IdCoa))]
        [InverseProperty(nameof(Coa.TarifPajak))]
        public virtual Coa IdCoaNavigation { get; set; }
        [InverseProperty("IdTarifNavigation")]
        public virtual ICollection<PBphtb> PBphtb { get; set; }
        [InverseProperty("IdTarifNavigation")]
        public virtual ICollection<Sppt> Sppt { get; set; }
        [InverseProperty("IdTarifNavigation")]
        public virtual ICollection<Sptpd> Sptpd { get; set; }
    }
}
