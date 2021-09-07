using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("Ref_Pekerjaan")]
    public partial class RefPekerjaan
    {
        public RefPekerjaan()
        {
            DataSubjek = new HashSet<DataSubjek>();
        }

        [Key]
        public int IdPekerjaan { get; set; }
        [Required]
        [StringLength(50)]
        public string Pekerjaan { get; set; }
        public bool FlagAktif { get; set; }

        [InverseProperty("IdPekerjaanNavigation")]
        public virtual ICollection<DataSubjek> DataSubjek { get; set; }
    }
}
