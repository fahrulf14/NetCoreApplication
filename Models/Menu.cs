using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    public partial class Menu
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string Nama { get; set; }
        [StringLength(50)]
        public string Controller { get; set; }
        [StringLength(50)]
        public string ActionName { get; set; }
        [StringLength(75)]
        public string IconClass { get; set; }
        public bool IsParent { get; set; }
        public int? ParentId { get; set; }
        public int? NoUrut { get; set; }
        public bool FlagAktif { get; set; }
    }
}
