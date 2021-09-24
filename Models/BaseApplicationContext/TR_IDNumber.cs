using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NUNA.Models.BaseApplicationContext
{
    [Table("TR_IDNumber")]
    public partial class TR_IDNumber : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [StringLength(30)]
        public string Number { get; set; }
        [ForeignKey(nameof(PersonalId))]
        public int PersonalId { get; set; }
        [ForeignKey(nameof(TypeId))]
        public int TypeId { get; set; }
        public virtual RF_IDType RF_IDType { get; set; }
        public virtual Personals Personals { get; set; }
    }
}
