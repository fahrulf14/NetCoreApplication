using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("Ref_JenisSetoran")]
    public partial class RefJenisSetoran
    {
        public RefJenisSetoran()
        {
            Bank = new HashSet<Bank>();
            Sspd = new HashSet<Sspd>();
            Ssrd = new HashSet<Ssrd>();
            Ssrdr = new HashSet<Ssrdr>();
            Stts = new HashSet<Stts>();
        }

        [Key]
        public int IdSetoran { get; set; }
        [StringLength(20)]
        public string Kode { get; set; }
        [StringLength(50)]
        public string Jenis { get; set; }
        public bool FlagAktif { get; set; }

        [InverseProperty("IdSetoranNavigation")]
        public virtual ICollection<Bank> Bank { get; set; }
        [InverseProperty("IdSetoranNavigation")]
        public virtual ICollection<Sspd> Sspd { get; set; }
        [InverseProperty("IdSetoranNavigation")]
        public virtual ICollection<Ssrd> Ssrd { get; set; }
        [InverseProperty("IdSetoranNavigation")]
        public virtual ICollection<Ssrdr> Ssrdr { get; set; }
        [InverseProperty("IdSetoranNavigation")]
        public virtual ICollection<Stts> Stts { get; set; }
    }
}
