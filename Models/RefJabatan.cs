using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("Ref_Jabatan")]
    public partial class RefJabatan
    {
        public RefJabatan()
        {
            Pegawai = new HashSet<Pegawai>();
        }

        [Key]
        public int IdJabatan { get; set; }
        [StringLength(50)]
        public string Jabatan { get; set; }
        public bool FlagAktif { get; set; }

        [InverseProperty("IdJabatanNavigation")]
        public virtual ICollection<Pegawai> Pegawai { get; set; }
    }
}
