using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    public partial class PrintSetting
    {
        [Key]
        public Guid IdSetting { get; set; }
        public int? IdDok { get; set; }
        public int? Jenis { get; set; }
        public bool Layout { get; set; }

        [ForeignKey(nameof(IdDok))]
        [InverseProperty(nameof(RefDokumen.PrintSetting))]
        public virtual RefDokumen IdDokNavigation { get; set; }
    }
}
