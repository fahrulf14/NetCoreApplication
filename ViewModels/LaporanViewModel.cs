using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIP.ViewModels
{
    public class LaporanViewModel
    {

    }

    public partial class ListSurat
    {
        public Guid Id { get; set; }
        public string Link { get; set; }
        public string Nomor { get; set; }
        public string Masa { get; set; }
        public DateTime? Tanggal { get; set; }
        public decimal? Hutang { get; set; }
        public string Coa { get; set; }
        public string Status { get; set; }
    }

    public class BukuPenerimaan
    {
        public Guid Id { get; set; }
        public DateTime? Tanggal { get; set; }
        public string Nomor { get; set; }
        public string Pembayaran { get; set; }
        public string Kode { get; set; }
        public string Uraian { get; set; }
        public decimal? Jumlah { get; set; }
        public string Jenis { get; set; }
        public DateTime? Valid { get; set; }
        public string Sts { get; set; }
        public bool Status { get; set; }
    }

    public class RegisterSTS
    {
        public Guid Id { get; set; }
        public DateTime? Tanggal { get; set; }
        public string NomorSurat { get; set; }
        public string NomorSetoran { get; set; }
        public string Uraian { get; set; }
        public string NamaWp { get; set; }
        public string NamaUsaha { get; set; }
        public decimal? Pokok { get; set; }
        public decimal? Jumlah { get; set; }
        public string Jenis { get; set; }
        public DateTime? Valid { get; set; }
        public string Sts { get; set; }
        public string Keterangan { get; set; }
    }

    public class STSDetail
    {
        public string KdCoa { get; set; }
        public string Uraian { get; set; }
        public decimal? Jumlah { get; set; }
    }

    public class CetakParamModel
    {
        public int SortIndex { get; set; }
        public bool? Status { get; set; }

        public string Tipe1 { get; set; }

        public string Tipe2 { get; set; }

        public string Tipe3 { get; set; }

        public Guid DataId { get; set; }

        public DateTime? DateMin { get; set; }

        public DateTime? DateMax { get; set; }
    }
}
