using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NUNA.Models.BaseApplicationContext
{
    public partial class AspNetRoles
    {
        public AspNetRoles()
        {
            AspNetRoleClaims = new HashSet<AspNetRoleClaims>();
            AspNetUserRoles = new HashSet<AspNetUserRoles>();
            AspNetRolePermissions = new HashSet<AspNetRolePermissions>();
            AspNetRoleMenus = new HashSet<AspNetRoleMenus>();
        }

        [Key]
        public string Id { get; set; }
        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(256)]
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }

        [InverseProperty("Role")]
        public virtual ICollection<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        [InverseProperty("Role")]
        public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }

        public virtual ICollection<AspNetRolePermissions> AspNetRolePermissions { get; set; }
        public virtual ICollection<AspNetRoleMenus> AspNetRoleMenus { get; set; }
    }
}
