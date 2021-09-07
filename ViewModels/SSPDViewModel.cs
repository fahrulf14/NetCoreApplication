using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIP.ViewModels
{
    public class SSPDViewModel
    {

    }

    public partial class ListData
    {
        public Guid Id { get; set; }
        public string Jenis { get; set; }
        public string Nomor { get; set; }
        public string Masa { get; set; }
        public string Hutang { get; set; }
        public string Coa { get; set; }
    }

    public partial class CetakSSPD
    {
        public Guid Id { get; set; }
        public string Nama { get; set; }
        public string Usaha { get; set; }
        public string Alamat { get; set; }
        public string Kelurahan { get; set; }
        public string Kecamatan { get; set; }
        public string Npwpd { get; set; }
        public string Nomor { get; set; }
        public DateTime? Tanggal { get; set; }
        public DateTime? MasaPajak1 { get; set; }
        public DateTime? MasaPajak2 { get; set; }
        public string Jenis { get; set; }
        public string NomorDok { get; set; }
        public string Coa { get; set; }
        public string KdCoa { get; set; }
        public decimal? Jumlah { get; set; }
        public string CoaDenda { get; set; }
        public string KdCoaDenda { get; set; }
        public decimal? JumlahDenda { get; set; }
        public decimal? Terhutang { get; set; }
    }

    public partial class CetakKwitansi
    {
        public string Nama { get; set; }
        public string Usaha { get; set; }
        public string Npwpd { get; set; }
        public string Nomor { get; set; }
        public DateTime? Masa1 { get; set; }
        public DateTime? Masa2 { get; set; }
        public string KdCoa { get; set; }
        public string Uraian { get; set; }
        public string Denda { get; set; }
        public decimal? JumlahPokok { get; set; }
        public decimal? JumlahDenda { get; set; }
        public decimal? Total { get; set; }
    }
}
