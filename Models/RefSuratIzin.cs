using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("Ref_SuratIzin")]
    public partial class RefSuratIzin
    {
        public RefSuratIzin()
        {
            DataIzin = new HashSet<DataIzin>();
        }

        [Key]
        public int IdSuratIzin { get; set; }
        [StringLength(50)]
        public string NamaSuratIzin { get; set; }
        public bool FlagAktif { get; set; }

        [InverseProperty("IdSuratIzinNavigation")]
        public virtual ICollection<DataIzin> DataIzin { get; set; }
    }
}
