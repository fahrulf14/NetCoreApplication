using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NUNA.Models.BaseApplicationContext
{
    [Table("TR_Address")]
    public partial class TR_Address
    {
        [Key]
        public int Id { get; set; }
        [StringLength(150)]
        public string Address { get; set; }
        [StringLength(30)]
        public string ContactInfo { get; set; }
        [StringLength(15)]
        public string PhoneNumber { get; set; }
        [StringLength(8)]
        public string PostCode { get; set; }
        [StringLength(10)]
        public string Tag { get; set; }
        public bool IsDefault { get; set; }
        [ForeignKey(nameof(PersonalId))]
        public int PersonalId { get; set; }
        [ForeignKey(nameof(ProvinsiId))]
        public int ProvinsiId { get; set; }
        [ForeignKey(nameof(KabupatenKotaId))]
        public int KabupatenKotaId { get; set; }
        [ForeignKey(nameof(KecamatanId))]
        public int KecamatanId { get; set; }
        [ForeignKey(nameof(KelurahanId))]
        public int KelurahanId { get; set; }
        public virtual Personals Personals { get; set; }
        public virtual ID_Provinsi ID_Provinsi { get; set; }
        public virtual ID_KabupatenKota ID_KabupatenKota { get; set; }
        public virtual ID_Kecamatan ID_Kecamatan { get; set; }
        public virtual ID_Kelurahan ID_Kelurahan { get; set; }
    }
}
