using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models.BaseApplicationContext
{
    public partial class AspNetUserMenus
    {
        [Key]
        [StringLength(36)]
        public string UserId { get; set; }
        [Key]
        [StringLength(100)]
        public string Menu { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual AspNetUsers User { get; set; }

    }
}
