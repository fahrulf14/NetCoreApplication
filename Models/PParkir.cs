using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("P_Parkir")]
    public partial class PParkir
    {
        [Key]
        public Guid IdParkir { get; set; }
        [Column("IdSPTPD")]
        public Guid? IdSptpd { get; set; }
        public bool RekapKarcis { get; set; }
        public int? KapasitasMobil { get; set; }
        public int? KapasitasMotor { get; set; }
        public int? JumlahMobil { get; set; }
        public int? JumlahMotor { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? MobilJam { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? MobilNext { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? MobilMax { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? MotorJam { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? MotorNext { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? MotorMax { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? TotalMobil { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? TotalMotor { get; set; }
        [StringLength(50)]
        public string Sistem { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? Pendapatan { get; set; }
        [Column("DPP", TypeName = "numeric(18,2)")]
        public decimal? Dpp { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? PajakTerhutang { get; set; }
        [Column("eu")]
        [StringLength(128)]
        public string Eu { get; set; }
        public DateTime? Ed { get; set; }

        [ForeignKey(nameof(IdSptpd))]
        [InverseProperty(nameof(Sptpd.PParkir))]
        public virtual Sptpd IdSptpdNavigation { get; set; }
    }
}
