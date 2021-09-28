using System.ComponentModel.DataAnnotations;

namespace NUNA.Models.BaseApplicationContext
{
    public partial class MenuTopbar
    {
        [Key]
        public int Id { get; set; }

        [StringLength(12)]
        public string Code { get; set; }


        [StringLength(50)]
        [Display(Name = "Menu Name")]
        public string Nama { get; set; }

        [StringLength(30)]
        [Display(Name = "Controller")]
        public string ControllerName { get; set; }

        [StringLength(30)]
        [Display(Name = "Action")]
        public string ActionName { get; set; }

        [Display(Name = "Icon Class", Prompt = "Icon Class")]
        public string IconClass { get; set; }

        [Display(Name = "Is Parent")]
        public bool IsParent { get; set; }
        [StringLength(10)]
        public string Parent { get; set; }

        [Display(Name = "Sqeuence Number")]
        public int? NoUrut { get; set; }

        [StringLength(10)]
        public string Badge { get; set; }

        [StringLength(20)]
        public string BadgeStates { get; set; }

        [Display(Name = "Status")]
        public bool IsActive { get; set; }
    }
}
