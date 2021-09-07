using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("Ref_Dokumen")]
    public partial class RefDokumen
    {
        public RefDokumen()
        {
            PrintSetting = new HashSet<PrintSetting>();
            TandaTangan = new HashSet<TandaTangan>();
        }

        [Key]
        public int IdDokumen { get; set; }
        [StringLength(20)]
        public string Nama { get; set; }
        [Required]
        [StringLength(50)]
        public string Dokumen { get; set; }

        [InverseProperty("IdDokNavigation")]
        public virtual ICollection<PrintSetting> PrintSetting { get; set; }
        [InverseProperty("IdDokumenNavigation")]
        public virtual ICollection<TandaTangan> TandaTangan { get; set; }
    }
}
