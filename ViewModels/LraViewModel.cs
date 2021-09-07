using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SIP.ViewModels
{
    public class LraViewModel
    {

    }

    public partial class PeriodeLra
    {
        [Required]
        public DateTime Tanggal1 { get; set; }

        [Required]
        public DateTime Tanggal2 { get; set; }

        [Required]
        public int Bulan { get; set; }

        [Required]
        public int TahunBulan { get; set; }

        [Required]
        public int Tahun { get; set; }

        public bool Rincian { get; set; }
    }
}
