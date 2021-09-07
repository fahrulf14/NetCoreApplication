using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    public partial class SaldoAnggaran
    {
        [Key]
        public Guid Id { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal Saldo { get; set; }
    }
}
