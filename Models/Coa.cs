using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    public partial class Coa
    {
        public Coa()
        {
            Lra = new HashSet<Lra>();
            NsrNjop = new HashSet<NsrNjop>();
            RefHargaPasar = new HashSet<RefHargaPasar>();
            RefRetribusi = new HashSet<RefRetribusi>();
            Skpdn = new HashSet<Skpdn>();
            Skrd = new HashSet<Skrd>();
            Spop = new HashSet<Spop>();
            Sptpd = new HashSet<Sptpd>();
            Sspd = new HashSet<Sspd>();
            Ssrd = new HashSet<Ssrd>();
            Ssrdr = new HashSet<Ssrdr>();
            TarifPajak = new HashSet<TarifPajak>();
            TarifRetribusi = new HashSet<TarifRetribusi>();
        }

        [Key]
        [StringLength(10)]
        public string IdCoa { get; set; }
        [Column("KDCoa")]
        [StringLength(10)]
        public string Kdcoa { get; set; }
        [StringLength(200)]
        public string Uraian { get; set; }
        [StringLength(20)]
        public string Jenis { get; set; }
        [StringLength(20)]
        public string Parent { get; set; }
        public int? Tingkat { get; set; }
        [StringLength(10)]
        public string Denda { get; set; }
        public bool Status { get; set; }

        [InverseProperty("IdCoaNavigation")]
        public virtual ICollection<Lra> Lra { get; set; }
        [InverseProperty("IdCoaNavigation")]
        public virtual ICollection<NsrNjop> NsrNjop { get; set; }
        [InverseProperty("IdCoaNavigation")]
        public virtual ICollection<RefHargaPasar> RefHargaPasar { get; set; }
        [InverseProperty("IdCoaNavigation")]
        public virtual ICollection<RefRetribusi> RefRetribusi { get; set; }
        [InverseProperty("IdCoaNavigation")]
        public virtual ICollection<Skpdn> Skpdn { get; set; }
        [InverseProperty("IdCoaNavigation")]
        public virtual ICollection<Skrd> Skrd { get; set; }
        [InverseProperty("IdCoaNavigation")]
        public virtual ICollection<Spop> Spop { get; set; }
        [InverseProperty("IdCoaNavigation")]
        public virtual ICollection<Sptpd> Sptpd { get; set; }
        [InverseProperty("IdCoaNavigation")]
        public virtual ICollection<Sspd> Sspd { get; set; }
        [InverseProperty("IdCoaNavigation")]
        public virtual ICollection<Ssrd> Ssrd { get; set; }
        [InverseProperty("IdCoaNavigation")]
        public virtual ICollection<Ssrdr> Ssrdr { get; set; }
        [InverseProperty("IdCoaNavigation")]
        public virtual ICollection<TarifPajak> TarifPajak { get; set; }
        [InverseProperty("IdCoaNavigation")]
        public virtual ICollection<TarifRetribusi> TarifRetribusi { get; set; }
    }
}
