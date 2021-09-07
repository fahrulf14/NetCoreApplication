using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    public partial class TandaTangan
    {
        [Key]
        public Guid IdTtd { get; set; }
        public int? IdDokumen { get; set; }
        public Guid? IdPegawai { get; set; }

        [ForeignKey(nameof(IdDokumen))]
        [InverseProperty(nameof(RefDokumen.TandaTangan))]
        public virtual RefDokumen IdDokumenNavigation { get; set; }
        [ForeignKey(nameof(IdPegawai))]
        [InverseProperty(nameof(Pegawai.TandaTangan))]
        public virtual Pegawai IdPegawaiNavigation { get; set; }
    }
}
