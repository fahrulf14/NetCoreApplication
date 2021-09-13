using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NUNA.Models.BaseApplicationContext
{
    [Table("RF_Position")]
    public partial class RF_Position
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string Position { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Personal> Personals { get; set; }
    }
}
