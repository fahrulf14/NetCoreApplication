using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("Ref_BobotSDA")]
    public partial class RefBobotSda
    {
        public RefBobotSda()
        {
            PAirTanahDt = new HashSet<PAirTanahDt>();
        }

        [Key]
        public int IdRef { get; set; }
        [StringLength(50)]
        public string Kriteria { get; set; }
        public double? Bobot { get; set; }

        [InverseProperty("IdSumberNavigation")]
        public virtual ICollection<PAirTanahDt> PAirTanahDt { get; set; }
    }
}
