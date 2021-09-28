using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NUNA.Models.BaseApplicationContext
{
    public partial class AspNetRolePermissions
    {
        [Key]
        [StringLength(36)]
        public string RoleId { get; set; }
        [Key]
        [StringLength(100)]
        public string Permission { get; set; }
        [ForeignKey(nameof(RoleId))]
        public virtual AspNetRoles Role { get; set; }

    }
}
