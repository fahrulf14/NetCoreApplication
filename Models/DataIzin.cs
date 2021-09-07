using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("Data_Izin")]
    public partial class DataIzin
    {
        [Key]
        public Guid IdIzin { get; set; }
        public Guid? IdUsaha { get; set; }
        public int? IdSuratIzin { get; set; }
        [StringLength(50)]
        public string Nomor { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Tanggal { get; set; }
        [Column(TypeName = "date")]
        public DateTime? MasaBerlaku { get; set; }
        [StringLength(100)]
        public string PemberiIzin { get; set; }
        [Column("eu")]
        [StringLength(128)]
        public string Eu { get; set; }
        public DateTime? Ed { get; set; }
        [Column("ed", TypeName = "timestamp(3) without time zone")]
        public DateTime? Ed1 { get; set; }

        [ForeignKey(nameof(IdSuratIzin))]
        [InverseProperty(nameof(RefSuratIzin.DataIzin))]
        public virtual RefSuratIzin IdSuratIzinNavigation { get; set; }
        [ForeignKey(nameof(IdUsaha))]
        [InverseProperty(nameof(DataUsaha.DataIzin))]
        public virtual DataUsaha IdUsahaNavigation { get; set; }
    }
}
