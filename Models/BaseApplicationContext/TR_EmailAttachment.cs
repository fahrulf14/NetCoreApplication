using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NUNA.Models.BaseApplicationContext
{
    [Table("TR_EmailAttachment")]
    public partial class TR_EmailAttachment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(EmailId))]
        public int EmailId { get; set; }
        public string FileName { get; set; }
        public string Attachment { get; set; }
        public virtual TR_EmailNotification TR_EmailNotification { get; set; }
    }
}
