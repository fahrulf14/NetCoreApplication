using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("Ref_Hotel")]
    public partial class RefHotel
    {
        public RefHotel()
        {
            PHotelDt = new HashSet<PHotelDt>();
        }

        [Key]
        public int IdRef { get; set; }
        [StringLength(50)]
        public string Uraian { get; set; }
        [StringLength(30)]
        public string Jenis { get; set; }
        public bool FlagAktif { get; set; }

        [InverseProperty("IdRefNavigation")]
        public virtual ICollection<PHotelDt> PHotelDt { get; set; }
    }
}
