using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NUNA.Models.BaseApplicationContext
{
    [Table("TR_EmailNotification")]
    public partial class TR_EmailNotification
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(PersonalId))]
        public int PersonalId { get; set; }

        [ForeignKey(nameof(NotifTypeId))]
        public int NotifTypeId { get; set; }
        public DateTime EmailDate { get; set; }
        public DateTime? SendDate { get; set; }
        public string EmailTo { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public virtual Personals Personals { get; set; }
        public virtual ICollection<TR_EmailAttachment> TR_EmailAttachment { get; set; }
        public virtual ICollection<TR_EmailStatus> TR_EmailStatus { get; set; }

    }
}
