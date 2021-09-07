using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("P_AirTanah_Dt")]
    public partial class PAirTanahDt
    {
        public PAirTanahDt()
        {
            PAirTanah = new HashSet<PAirTanah>();
        }

        [Key]
        public Guid IdDetail { get; set; }
        public Guid? IdUsaha { get; set; }
        [StringLength(20)]
        public string JenisAir { get; set; }
        [StringLength(20)]
        public string KualitasAir { get; set; }
        public int? IdSumber { get; set; }
        public int? IdKel { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Modal { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Operasional { get; set; }
        public int? UsiaProduksi { get; set; }
        public int? DebitSumur { get; set; }
        [Column("HAB", TypeName = "numeric(18,2)")]
        public decimal? Hab { get; set; }
        public double? Fna1 { get; set; }
        public double? Fna2 { get; set; }
        public double? Fna3 { get; set; }
        public double? Fna4 { get; set; }
        public double? Fna5 { get; set; }
        public double? Fna6 { get; set; }
        [Column("HDA1", TypeName = "numeric(18,2)")]
        public decimal? Hda1 { get; set; }
        [Column("HDA2", TypeName = "numeric(18,2)")]
        public decimal? Hda2 { get; set; }
        [Column("HDA3", TypeName = "numeric(18,2)")]
        public decimal? Hda3 { get; set; }
        [Column("HDA4", TypeName = "numeric(18,2)")]
        public decimal? Hda4 { get; set; }
        [Column("HDA5", TypeName = "numeric(18,2)")]
        public decimal? Hda5 { get; set; }
        [Column("HDA6", TypeName = "numeric(18,2)")]
        public decimal? Hda6 { get; set; }
        [Column("eu")]
        [StringLength(128)]
        public string Eu { get; set; }
        public DateTime? Ed { get; set; }

        [ForeignKey(nameof(IdKel))]
        [InverseProperty(nameof(RefBobotKp.PAirTanahDt))]
        public virtual RefBobotKp IdKelNavigation { get; set; }
        [ForeignKey(nameof(IdSumber))]
        [InverseProperty(nameof(RefBobotSda.PAirTanahDt))]
        public virtual RefBobotSda IdSumberNavigation { get; set; }
        [ForeignKey(nameof(IdUsaha))]
        [InverseProperty(nameof(DataUsaha.PAirTanahDt))]
        public virtual DataUsaha IdUsahaNavigation { get; set; }
        [InverseProperty("IdDetailNavigation")]
        public virtual ICollection<PAirTanah> PAirTanah { get; set; }
    }
}
