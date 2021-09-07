using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("Tarif_Retribusi")]
    public partial class TarifRetribusi
    {
        public TarifRetribusi()
        {
            SkrdDt = new HashSet<SkrdDt>();
            SsrdrDt = new HashSet<SsrdrDt>();
        }

        [Key]
        public Guid IdTarif { get; set; }
        [StringLength(10)]
        public string IdCoa { get; set; }
        [StringLength(100)]
        public string Objek { get; set; }
        [StringLength(20)]
        public string Jenis { get; set; }
        public int? Var1 { get; set; }
        [StringLength(20)]
        public string Satuan1 { get; set; }
        public int? Var2 { get; set; }
        [StringLength(20)]
        public string Satuan2 { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Tarif { get; set; }
        public bool FlagAktif { get; set; }

        [ForeignKey(nameof(IdCoa))]
        [InverseProperty(nameof(Coa.TarifRetribusi))]
        public virtual Coa IdCoaNavigation { get; set; }
        [InverseProperty("IdTarifNavigation")]
        public virtual ICollection<SkrdDt> SkrdDt { get; set; }
        [InverseProperty("IdTarifNavigation")]
        public virtual ICollection<SsrdrDt> SsrdrDt { get; set; }
    }
}
