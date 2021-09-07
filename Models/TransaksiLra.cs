using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("TransaksiLRA")]
    public partial class TransaksiLra
    {
        [Key]
        public Guid IdTransaksi { get; set; }
        [Column("IdLRA")]
        public Guid? IdLra { get; set; }
        [StringLength(50)]
        public string Sumber { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Tanggal { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Jumlah { get; set; }

        [ForeignKey(nameof(IdLra))]
        [InverseProperty(nameof(Lra.TransaksiLra))]
        public virtual Lra IdLraNavigation { get; set; }
    }
}
