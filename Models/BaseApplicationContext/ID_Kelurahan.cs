using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NUNA.Models.BaseApplicationContext
{
    [Table("ID_Kelurahan")]
    public partial class ID_Kelurahan
    {
        [Key]
        public int Id { get; set; }
        public string Kelurahan { get; set; }
        [ForeignKey(nameof(KecamatanId))]
        public int KecamatanId { get; set; }

        public virtual ID_Kecamatan ID_Kecamatan { get; set; }
        public virtual ICollection<TR_Address> TR_Address { get; set; }
    }
}
