using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("Ref_BobotKP")]
    public partial class RefBobotKp
    {
        public RefBobotKp()
        {
            PAirTanahDt = new HashSet<PAirTanahDt>();
        }

        [Key]
        public int IdRef { get; set; }
        [StringLength(50)]
        public string Kelompok { get; set; }
        public double? Fna1 { get; set; }
        public double? Fna2 { get; set; }
        public double? Fna3 { get; set; }
        public double? Fna4 { get; set; }
        public double? Fna5 { get; set; }
        public double? Fna6 { get; set; }

        [InverseProperty("IdKelNavigation")]
        public virtual ICollection<PAirTanahDt> PAirTanahDt { get; set; }
    }
}
