using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NUNA.Models.BaseApplicationContext
{
    [Table("ID_Provinsi")]
    public partial class ID_Provinsi
    {
        [Key]
        public int Id { get; set; }
        public string Provinsi { get; set; }
        public virtual ICollection<ID_KabupatenKota> ID_KabupatenKota { get; set; }
        public virtual ICollection<TR_Address> TR_Address { get; set; }
    }
}
