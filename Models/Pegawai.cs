using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    public partial class Pegawai
    {
        [Key]
        public Guid IdPegawai { get; set; }
        [StringLength(50)]
        public string Nama { get; set; }
        [StringLength(20)]
        public string Nick { get; set; }
        [Column("NIP")]
        [StringLength(50)]
        public string Nip { get; set; }
        public int? IdJabatan { get; set; }
        [StringLength(256)]
        public string Email { get; set; }
        public byte[] Signature { get; set; }
        public bool Active { get; set; }

        [ForeignKey(nameof(IdJabatan))]
        [InverseProperty(nameof(RefJabatan.Pegawai))]
        public virtual RefJabatan IdJabatanNavigation { get; set; }
    }
}
