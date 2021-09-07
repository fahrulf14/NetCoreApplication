using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    public partial class Bank
    {
        public Bank()
        {
            Sspd = new HashSet<Sspd>();
            Ssrd = new HashSet<Ssrd>();
            Ssrdr = new HashSet<Ssrdr>();
            Sts = new HashSet<Sts>();
            Stts = new HashSet<Stts>();
        }

        [Key]
        public Guid IdBank { get; set; }
        public int? IdRef { get; set; }
        public int? IdSetoran { get; set; }
        [StringLength(50)]
        public string Cabang { get; set; }
        [StringLength(50)]
        public string NoRek { get; set; }
        [StringLength(50)]
        public string NamaRek { get; set; }
        public bool Status { get; set; }

        [ForeignKey(nameof(IdRef))]
        [InverseProperty(nameof(RefBank.Bank))]
        public virtual RefBank IdRefNavigation { get; set; }
        [ForeignKey(nameof(IdSetoran))]
        [InverseProperty(nameof(RefJenisSetoran.Bank))]
        public virtual RefJenisSetoran IdSetoranNavigation { get; set; }
        [InverseProperty("IdBankNavigation")]
        public virtual ICollection<Sspd> Sspd { get; set; }
        [InverseProperty("IdBankNavigation")]
        public virtual ICollection<Ssrd> Ssrd { get; set; }
        [InverseProperty("IdBankNavigation")]
        public virtual ICollection<Ssrdr> Ssrdr { get; set; }
        [InverseProperty("IdBankNavigation")]
        public virtual ICollection<Sts> Sts { get; set; }
        [InverseProperty("IdBankNavigation")]
        public virtual ICollection<Stts> Stts { get; set; }
    }
}
