using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NUNA.Models.BaseApplicationContext
{
    [Table("Officer")]
    public partial class Officer : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(PersonalId))]
        public int PersonalId { get; set; }
        [ForeignKey(nameof(PositionId))]
        public int? PositionId { get; set; }
        [StringLength(75)]
        public byte[] Signature { get; set; }
        public bool IsActive { get; set; }
        public virtual MS_Position MS_Position { get; set; }
    }
}
