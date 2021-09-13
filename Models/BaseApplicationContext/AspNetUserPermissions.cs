using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NUNA.Models.BaseApplicationContext
{
    public partial class AspNetUserPermissions
    {
        [Key]
        [StringLength(36)]
        public string UserId { get; set; }
        [Key]
        [StringLength(100)]
        public string Permission { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual AspNetUsers User { get; set; }

    }
}
