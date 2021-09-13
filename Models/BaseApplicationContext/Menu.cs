using System.ComponentModel.DataAnnotations;

namespace NUNA.Models.BaseApplicationContext
{
    public partial class Menu
    {
        [Key]
        public int Id { get; set; }
        [StringLength(12)]
        public string Code { get; set; }

        [StringLength(50)]
        public string Nama { get; set; }
        [StringLength(30)]
        public string Controller { get; set; }
        [StringLength(30)]
        public string ActionName { get; set; }
        [StringLength(100)]
        public string IconClass { get; set; }
        public bool IsParent { get; set; }
        [StringLength(10)]
        public string Parent { get; set; }
        public int? NoUrut { get; set; }
        public bool IsActive { get; set; }
    }
}
