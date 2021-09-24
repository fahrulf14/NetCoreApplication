using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NUNA.Models.BaseApplicationContext
{
    [Table("ID_Kecamatan")]
    public partial class ID_Kecamatan
    {
        [Key]
        public int Id { get; set; }
        public string Kecamatan { get; set; }
        [ForeignKey(nameof(KabupatenKotaId))]
        public int KabupatenKotaId { get; set; }

        public virtual ID_KabupatenKota ID_KabupatenKota { get; set; }
        public virtual ICollection<ID_Kelurahan> ID_Kelurahan { get; set; }
        public virtual ICollection<TR_Address> TR_Address { get; set; }
    }
}
