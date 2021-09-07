using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("ML")]
    public partial class Ml
    {
        [Key]
        public int Id { get; set; }
        [Column("CPU")]
        [StringLength(50)]
        public string Cpu { get; set; }
        [StringLength(100)]
        public string License { get; set; }
        public DateTime Expired { get; set; }
        [StringLength(50)]
        public string Chassis { get; set; }
    }
}
