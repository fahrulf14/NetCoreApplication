using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIP.ViewModels
{
    public class PendaftaranViewModel
    {

    }

    public partial class EditSubjek
    {
        public Guid IdSubjek { get; set; }
        public string Nama { get; set; }
        public string Alamat { get; set; }
        public string Rtrw { get; set; }
        public int? IndProvinsiId { get; set; }
        public int? IndKabKotaId { get; set; }
        public int? IndKecamatanId { get; set; }
        public int? IndKelurahanId { get; set; }
        public string NoTelp { get; set; }
        public string KodePos { get; set; }
        public string Email { get; set; }
        public string Nik { get; set; }
        public string Npwp { get; set; }
        public string Kelamin { get; set; }
        public DateTime? TglLahir { get; set; }
        public int? IdPekerjaan { get; set; }
        public string PekerjaanLain { get; set; }
        public string NamaInstansi { get; set; }
        public int? IdBadanHukum { get; set; }
        public DateTime? TglDaftar { get; set; }
        public int? NoPokok { get; }
        public string Npwpd { get; }
        public string Npwrd { get; }
        public bool Status { get; set; }
        public string Eu { get; set; }
        public DateTime? Ed { get; set; }
    }

    public partial class DataPribadi
    {
        public Guid IdSubjek { get; set; }
        public string Nama { get; set; }
        public string Npwpd { get; set; }
        public string Alamat { get; set; }
        public string Kecamatan { get; set; }
        public string Pekerjaan { get; set; }
        public string Status { get; set; }

    }
}
