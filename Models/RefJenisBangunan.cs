using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("Ref_JenisBangunan")]
    public partial class RefJenisBangunan
    {
        public RefJenisBangunan()
        {
            Lspop = new HashSet<Lspop>();
        }

        [Key]
        public int IdJenis { get; set; }
        [StringLength(50)]
        public string Jenis { get; set; }
        public bool FlagAktif { get; set; }

        [InverseProperty("IdJenisNavigation")]
        public virtual ICollection<Lspop> Lspop { get; set; }
    }
}
