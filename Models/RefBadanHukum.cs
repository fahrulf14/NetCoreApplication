using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("Ref_BadanHukum")]
    public partial class RefBadanHukum
    {
        public RefBadanHukum()
        {
            DataSubjek = new HashSet<DataSubjek>();
        }

        [Key]
        public int IdBadanHukum { get; set; }
        [StringLength(50)]
        public string BadanHukum { get; set; }
        public bool FlagAktif { get; set; }

        [InverseProperty("IdBadanHukumNavigation")]
        public virtual ICollection<DataSubjek> DataSubjek { get; set; }
    }
}
