using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("P_BPHTB")]
    public partial class PBphtb
    {
        [Key]
        [Column("IdBPHTB")]
        public Guid IdBphtb { get; set; }
        [StringLength(200)]
        public string Alamat { get; set; }
        [Column("RTRW")]
        [StringLength(10)]
        public string Rtrw { get; set; }
        public Guid? IdTarif { get; set; }
        [Column("IndKabKotaID")]
        public int? IndKabKotaId { get; set; }
        [Column("IndKecamatanID")]
        public int? IndKecamatanId { get; set; }
        [Column("IndKelurahanID")]
        public int? IndKelurahanId { get; set; }
        public int? IdPerolehan { get; set; }
        [Column("NOP")]
        [StringLength(50)]
        public string Nop { get; set; }
        [StringLength(50)]
        public string NoSertifikat { get; set; }
        public double? LuasTanah { get; set; }
        public double? LuasBangunan { get; set; }
        [Column("NJOPTanah", TypeName = "numeric(18,2)")]
        public decimal? Njoptanah { get; set; }
        [Column("NJOPBangunan", TypeName = "numeric(18,2)")]
        public decimal? Njopbangunan { get; set; }
        [Column("NJOPPBB", TypeName = "numeric(18,2)")]
        public decimal? Njoppbb { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? NilaiTransaksi { get; set; }
        [Column("NPOPTKP", TypeName = "numeric(18,2)")]
        public decimal? Npoptkp { get; set; }
        [Column("DPP", TypeName = "numeric(18,2)")]
        public decimal? Dpp { get; set; }
        [Column(TypeName = "numeric(18,2)")]
        public decimal? PajakTerhutang { get; set; }
        [Column("eu")]
        [StringLength(128)]
        public string Eu { get; set; }
        public DateTime? Ed { get; set; }

        [ForeignKey(nameof(IdPerolehan))]
        [InverseProperty(nameof(RefPerolehan.PBphtb))]
        public virtual RefPerolehan IdPerolehanNavigation { get; set; }
        [ForeignKey(nameof(IdTarif))]
        [InverseProperty(nameof(TarifPajak.PBphtb))]
        public virtual TarifPajak IdTarifNavigation { get; set; }
        [ForeignKey(nameof(IndKabKotaId))]
        [InverseProperty("PBphtb")]
        public virtual IndKabKota IndKabKota { get; set; }
        [ForeignKey(nameof(IndKecamatanId))]
        [InverseProperty("PBphtb")]
        public virtual IndKecamatan IndKecamatan { get; set; }
        [ForeignKey(nameof(IndKelurahanId))]
        [InverseProperty("PBphtb")]
        public virtual IndKelurahan IndKelurahan { get; set; }
    }
}
