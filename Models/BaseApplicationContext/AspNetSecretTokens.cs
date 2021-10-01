using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NUNA.Models.BaseApplicationContext
{
    public partial class AspNetSecretTokens
    {
        [Key]
        [StringLength(36)]
        public string UserId { get; set; }
        [Key]
        public string Token { get; set; }
        public string Type { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Status { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual AspNetUsers User { get; set; }

    }
}
