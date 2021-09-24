using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NUNA.Models.BaseApplicationContext
{
    [Table("RF_IDType")]
    public partial class RF_IDType
    {
        [Key]
        public int Id { get; set; }
        [StringLength(30)]
        public string Type { get; set; }
        public virtual ICollection<TR_IDNumber> TR_IDNumber { get; set; }
    }
}
