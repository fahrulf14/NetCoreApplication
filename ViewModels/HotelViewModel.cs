using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIP.ViewModels
{
    public class HotelViewModel
    {

    }

    public partial class DetailHotel
    {
        public int IdFasilitas { get; set; }
        public int OmzetFasilitas { get; set; }
        public int IdRestoran { get; set; }
        public int OmzetRestoran { get; set; }
        public int IdLayanan { get; set; }
        public int OmzetLayanan { get; set; }
    }
}
