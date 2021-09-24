using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NUNA.Models.BaseApplicationContext
{
    [Table("MS_Position")]
    public partial class MS_Position
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string Position { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Officer> Officer { get; set; }
    }
}
