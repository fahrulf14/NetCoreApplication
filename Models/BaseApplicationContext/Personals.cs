using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NUNA.Models.BaseApplicationContext
{
    [Table("Personals")]
    public partial class Personals : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(30)]
        public string UserName { get; set; }
        public DateTime BirthDate { get; set; }
        [StringLength(50)]
        public string BirthPlace { get; set; }
        public bool IsActive { get; set; }
        [StringLength(1)]
        public string Gender { get; set; }
        public bool IsVerified { get; set; }
        public virtual ICollection<TR_Address> TR_Address { get; set; }
        public virtual ICollection<TR_IDNumber> TR_IDNumber { get; set; }
    }
}
