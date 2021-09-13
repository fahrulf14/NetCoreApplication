using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NUNA.Models.BaseApplicationContext
{
    public partial class AspNetRoleMenus
    {
        [Key]
        [StringLength(36)]
        public string RoleId { get; set; }
        [Key]
        [StringLength(100)]
        public string Menu { get; set; }
        [ForeignKey(nameof(RoleId))]
        public virtual AspNetRoles Role { get; set; }

    }
}
