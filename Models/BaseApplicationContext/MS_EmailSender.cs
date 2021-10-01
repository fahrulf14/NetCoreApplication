using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NUNA.Models.BaseApplicationContext
{
    [Table("MS_EmailSender")]
    public partial class MS_EmailSender : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Sender { get; set; }
        public string SmtpHost { get; set; }
        public string SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpCred { get; set; }
        public virtual ICollection<MS_NotificationType> MS_NotificationType { get; set; }
    }
}
