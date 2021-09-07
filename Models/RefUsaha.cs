using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("Ref_Usaha")]
    public partial class RefUsaha
    {
        public RefUsaha()
        {
            DataUsaha = new HashSet<DataUsaha>();
        }

        [Key]
        public int IdUsaha { get; set; }
        [StringLength(50)]
        public string Usaha { get; set; }
        public bool FlagAktif { get; set; }

        [InverseProperty("IdJenisNavigation")]
        public virtual ICollection<DataUsaha> DataUsaha { get; set; }
    }
}
