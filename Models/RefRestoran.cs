using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("Ref_Restoran")]
    public partial class RefRestoran
    {
        public RefRestoran()
        {
            PRestoranDt = new HashSet<PRestoranDt>();
        }

        [Key]
        public int IdRef { get; set; }
        [StringLength(50)]
        public string Uraian { get; set; }
        public bool FlagAktif { get; set; }

        [InverseProperty("IdRefNavigation")]
        public virtual ICollection<PRestoranDt> PRestoranDt { get; set; }
    }
}
