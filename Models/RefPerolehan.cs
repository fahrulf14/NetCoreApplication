using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("Ref_Perolehan")]
    public partial class RefPerolehan
    {
        public RefPerolehan()
        {
            PBphtb = new HashSet<PBphtb>();
        }

        [Key]
        public int IdPerolehan { get; set; }
        [StringLength(100)]
        public string Jenis { get; set; }
        [Column("NPOPTKP", TypeName = "numeric(18,2)")]
        public decimal? Npoptkp { get; set; }
        public bool FlagAktif { get; set; }

        [InverseProperty("IdPerolehanNavigation")]
        public virtual ICollection<PBphtb> PBphtb { get; set; }
    }
}
