using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NUNA.Models.BaseApplicationContext
{
    [Table("ID_KabupatenKota")]
    public partial class ID_KabupatenKota
    {
        [Key]
        public int Id { get; set; }
        public string KabupatenKota { get; set; }
        [ForeignKey(nameof(ProvinsiId))]
        public int ProvinsiId { get; set; }

        public virtual ID_Provinsi ID_Provinsi { get; set; }
        public virtual ICollection<ID_Kecamatan> ID_Kecamatan { get; set; }
        public virtual ICollection<TR_Address> TR_Address { get; set; }
    }
}
