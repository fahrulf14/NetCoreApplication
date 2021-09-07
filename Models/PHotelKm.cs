using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("P_Hotel_Km")]
    public partial class PHotelKm
    {
        [Key]
        [Column("IdHotel_Km")]
        public Guid IdHotelKm { get; set; }
        public Guid? IdHotel { get; set; }
        [StringLength(50)]
        public string NamaKamar { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Tarif { get; set; }
        public int? Jumlah { get; set; }

        [ForeignKey(nameof(IdHotel))]
        [InverseProperty(nameof(PHotel.PHotelKm))]
        public virtual PHotel IdHotelNavigation { get; set; }
    }
}
