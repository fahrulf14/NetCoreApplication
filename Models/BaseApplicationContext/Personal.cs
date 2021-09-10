using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models.BaseApplicationContext
{
    [Table("Personal")]
    public partial class Personal
    {
        [Key]
        public int Id { get; set; }
        //public string PersonalCode { get; set; }
        [StringLength(50)]
        public string Nama { get; set; }
        [StringLength(20)]
        public string UserName { get; set; }
        [Column("NIP")]
        [StringLength(50)]
        public string Nip { get; set; }
        public int? PositionId { get; set; }
        [StringLength(256)]
        public string Email { get; set; }
        public byte[] Signature { get; set; }
        public bool IsActive { get; set; }

        //public virtual RF_Position RF_Position { get; set; }
    }
}
