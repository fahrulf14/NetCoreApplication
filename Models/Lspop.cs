using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIP.Models
{
    [Table("LSPOP")]
    public partial class Lspop
    {
        [Key]
        [Column("IdLSPOP")]
        public Guid IdLspop { get; set; }
        [Column("IdSPOP")]
        public Guid? IdSpop { get; set; }
        public int? IdJenis { get; set; }
        public double? Luas { get; set; }
        public int? JumlahLantai { get; set; }
        [StringLength(4)]
        public string TahunBangun { get; set; }
        [StringLength(4)]
        public string TahunRenov { get; set; }
        public int? DayaListrik { get; set; }
        [StringLength(15)]
        public string Kondisi { get; set; }
        [StringLength(15)]
        public string Konstruksi { get; set; }
        [StringLength(50)]
        public string Atap { get; set; }
        [StringLength(25)]
        public string Dinding { get; set; }
        [StringLength(25)]
        public string Lantai { get; set; }
        [StringLength(25)]
        public string Langit2 { get; set; }
        [Column("AC_Split")]
        public int? AcSplit { get; set; }
        [Column("AC_Window")]
        public int? AcWindow { get; set; }
        [Column("AC_Sealing")]
        public int? AcSealing { get; set; }
        [Column("AC_Sentral")]
        public bool AcSentral { get; set; }
        [Column("Kolam_Luas")]
        public int? KolamLuas { get; set; }
        [Column("Kolam_Jenis")]
        [StringLength(20)]
        public string KolamJenis { get; set; }
        [Column("Halaman_Ringan")]
        public int? HalamanRingan { get; set; }
        [Column("Halaman_Sedang")]
        public int? HalamanSedang { get; set; }
        [Column("Halaman_Berat")]
        public int? HalamanBerat { get; set; }
        [Column("Halaman_Lantai")]
        public int? HalamanLantai { get; set; }
        [Column("Lapangan_Beton1")]
        public int? LapanganBeton1 { get; set; }
        [Column("Lapangan_Beton2")]
        public int? LapanganBeton2 { get; set; }
        [Column("Lapangan_Aspal1")]
        public int? LapanganAspal1 { get; set; }
        [Column("Lapangan_Aspal2")]
        public int? LapanganAspal2 { get; set; }
        [Column("Lapangan_Tanah1")]
        public int? LapanganTanah1 { get; set; }
        [Column("Lapangan_Tanah2")]
        public int? LapanganTanah2 { get; set; }
        [Column("Lift_Penumpang")]
        public int? LiftPenumpang { get; set; }
        [Column("Lift_Kapsul")]
        public int? LiftKapsul { get; set; }
        [Column("Lift_Barang")]
        public int? LiftBarang { get; set; }
        public int? Eskalator1 { get; set; }
        public int? Eskalator2 { get; set; }
        [Column("Pagar_Panjang")]
        public int? PagarPanjang { get; set; }
        [Column("Pagar_Jenis")]
        [StringLength(20)]
        public string PagarJenis { get; set; }
        public bool Hydrant { get; set; }
        public bool Sprinkle { get; set; }
        public bool FireAlarm { get; set; }
        [Column("PABX_Jumlah")]
        public int? PabxJumlah { get; set; }
        [Column("Artesis_Kedalaman")]
        public int? ArtesisKedalaman { get; set; }
        [Column("NJOP", TypeName = "numeric(18,2)")]
        public decimal? Njop { get; set; }
        public bool FlagValidasi { get; set; }
        [StringLength(3)]
        public string Zona { get; set; }
        [Column("eu")]
        [StringLength(128)]
        public string Eu { get; set; }
        public DateTime? Ed { get; set; }
        [Column("ed")]
        public DateTime? Ed1 { get; set; }

        [ForeignKey(nameof(IdJenis))]
        [InverseProperty(nameof(RefJenisBangunan.Lspop))]
        public virtual RefJenisBangunan IdJenisNavigation { get; set; }
        [ForeignKey(nameof(IdSpop))]
        [InverseProperty(nameof(Spop.Lspop))]
        public virtual Spop IdSpopNavigation { get; set; }
    }
}
