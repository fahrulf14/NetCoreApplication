using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NUNA.Models.BaseApplicationContext
{
    [Table("TR_EmailStatus")]
    public partial class TR_EmailStatus
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(EmailId))]
        public int EmailId { get; set; }
        public DateTime ExecutionTime { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public virtual TR_EmailNotification TR_EmailNotification { get; set; }
    }
}
