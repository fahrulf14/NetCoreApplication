using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("P_Hotel")]
    public partial class PHotel
    {
        public PHotel()
        {
            PHotelDt = new HashSet<PHotelDt>();
            PHotelKm = new HashSet<PHotelKm>();
        }

        [Key]
        public Guid IdHotel { get; set; }
        [Column("IdSPTPD")]
        public Guid? IdSptpd { get; set; }
        public bool KasRegister { get; set; }
        public bool Pembukuan { get; set; }
        [Column("DPP", TypeName = "numeric(18,2)")]
        public decimal? Dpp { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? PajakTerhutang { get; set; }
        [Column("eu")]
        [StringLength(128)]
        public string Eu { get; set; }
        public DateTime? Ed { get; set; }

        [ForeignKey(nameof(IdSptpd))]
        [InverseProperty(nameof(Sptpd.PHotel))]
        public virtual Sptpd IdSptpdNavigation { get; set; }
        [InverseProperty("IdHotelNavigation")]
        public virtual ICollection<PHotelDt> PHotelDt { get; set; }
        [InverseProperty("IdHotelNavigation")]
        public virtual ICollection<PHotelKm> PHotelKm { get; set; }
    }
}
