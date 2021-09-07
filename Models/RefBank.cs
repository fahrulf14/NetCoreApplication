using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("Ref_Bank")]
    public partial class RefBank
    {
        public RefBank()
        {
            Bank = new HashSet<Bank>();
        }

        [Key]
        public int IdBank { get; set; }
        [StringLength(50)]
        public string NamaBank { get; set; }
        [StringLength(30)]
        public string Kode { get; set; }
        public bool Status { get; set; }

        [InverseProperty("IdRefNavigation")]
        public virtual ICollection<Bank> Bank { get; set; }
    }
}
