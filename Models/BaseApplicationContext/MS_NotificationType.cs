using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NUNA.Models.BaseApplicationContext
{
    [Table("MS_NotificationType")]
    public partial class MS_NotificationType : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(SenderId))]
        public int SenderId { get; set; }
        public string NotifTypeCode { get; set; }
        public string NotifTypeName { get; set; }
        public bool IsActive { get; set; }
        public virtual MS_EmailSender MS_EmailSender { get; set; }
    }
}
