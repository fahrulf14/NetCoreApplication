using System.ComponentModel.DataAnnotations;

namespace SIP.Models.BaseApplicationContext
{
    public partial class UserSetting
    {
        [Key]
        public string Id { get; set; }
        [StringLength(75)]
        public string Email { get; set; }
        [StringLength(50)]
        public string Theme { get; set; }
        [StringLength(50)]
        public string Aside { get; set; }
    }
}
