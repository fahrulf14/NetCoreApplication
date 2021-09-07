using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("Ref_Retribusi")]
    public partial class RefRetribusi
    {
        [Key]
        public int IdRetribusi { get; set; }
        [StringLength(10)]
        public string IdCoa { get; set; }
        public int? Urutan { get; set; }
        [StringLength(20)]
        public string Jenis { get; set; }
        [StringLength(50)]
        public string Uraian { get; set; }
        [StringLength(10)]
        public string Dok { get; set; }
        [StringLength(30)]
        public string Icon { get; set; }
        [StringLength(20)]
        public string Color { get; set; }
        [StringLength(50)]
        public string Link { get; set; }
        public bool FlagAktif { get; set; }

        [ForeignKey(nameof(IdCoa))]
        [InverseProperty(nameof(Coa.RefRetribusi))]
        public virtual Coa IdCoaNavigation { get; set; }
    }
}
