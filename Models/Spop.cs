using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("SPOP")]
    public partial class Spop
    {
        public Spop()
        {
            Lspop = new HashSet<Lspop>();
            Sppt = new HashSet<Sppt>();
        }

        [Key]
        [Column("IdSPOP")]
        public Guid IdSpop { get; set; }
        public Guid? IdSubjek { get; set; }
        [StringLength(10)]
        public string IdCoa { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Tanggal { get; set; }
        [Column(TypeName = "date")]
        public DateTime? JatuhTempo { get; set; }
        public int? Tahun { get; set; }
        public int? Nomor { get; set; }
        [StringLength(50)]
        public string NoFormulir { get; set; }
        [Column("NOP")]
        [StringLength(50)]
        public string Nop { get; set; }
        [Column("IndKabKotaID")]
        public int? IndKabKotaId { get; set; }
        [Column("IndKecamatanID")]
        public int? IndKecamatanId { get; set; }
        [Column("IndKelurahanID")]
        public int? IndKelurahanId { get; set; }
        [StringLength(50)]
        public string NamaJalan { get; set; }
        [Column("RT")]
        [StringLength(3)]
        public string Rt { get; set; }
        [Column("RW")]
        [StringLength(3)]
        public string Rw { get; set; }
        [StringLength(20)]
        public string BlokKavNo { get; set; }
        [StringLength(5)]
        public string KodePos { get; set; }
        public double? LuasTanah { get; set; }
        [StringLength(50)]
        public string JenisTanah { get; set; }
        [StringLength(3)]
        public string Zona { get; set; }
        public int? JumlahBangunan { get; set; }
        [Column("NJOP", TypeName = "numeric(18,2)")]
        public decimal? Njop { get; set; }
        [Column(TypeName = "numeric(18,0)")]
        public decimal? Xcordinate { get; set; }
        [Column(TypeName = "numeric(18,0)")]
        public decimal? Ycordinate { get; set; }
        public bool FlagValidasi { get; set; }
        [Column("eu")]
        [StringLength(128)]
        public string Eu { get; set; }
        public DateTime? Ed { get; set; }
        [Column("ed")]
        public DateTime? Ed1 { get; set; }

        [ForeignKey(nameof(IdCoa))]
        [InverseProperty(nameof(Coa.Spop))]
        public virtual Coa IdCoaNavigation { get; set; }
        [ForeignKey(nameof(IdSubjek))]
        [InverseProperty(nameof(DataSubjek.Spop))]
        public virtual DataSubjek IdSubjekNavigation { get; set; }
        [ForeignKey(nameof(IndKabKotaId))]
        [InverseProperty("Spop")]
        public virtual IndKabKota IndKabKota { get; set; }
        [ForeignKey(nameof(IndKecamatanId))]
        [InverseProperty("Spop")]
        public virtual IndKecamatan IndKecamatan { get; set; }
        [ForeignKey(nameof(IndKelurahanId))]
        [InverseProperty("Spop")]
        public virtual IndKelurahan IndKelurahan { get; set; }
        [InverseProperty("IdSpopNavigation")]
        public virtual ICollection<Lspop> Lspop { get; set; }
        [InverseProperty("IdSpopNavigation")]
        public virtual ICollection<Sppt> Sppt { get; set; }
    }
}
