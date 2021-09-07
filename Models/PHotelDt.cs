using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("P_Hotel_Dt")]
    public partial class PHotelDt
    {
        [Key]
        [Column("IdHotel_Dt")]
        public Guid IdHotelDt { get; set; }
        public Guid? IdHotel { get; set; }
        public int? IdRef { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Jumlah { get; set; }

        [ForeignKey(nameof(IdHotel))]
        [InverseProperty(nameof(PHotel.PHotelDt))]
        public virtual PHotel IdHotelNavigation { get; set; }
        [ForeignKey(nameof(IdRef))]
        [InverseProperty(nameof(RefHotel.PHotelDt))]
        public virtual RefHotel IdRefNavigation { get; set; }
    }
}
