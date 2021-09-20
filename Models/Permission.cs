using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NUNA.Models
{
    public class Permission
    {
        public const string Menu = "Master Menu";
        public const string Menu_View = "Master Menu.View";
        public const string Menu_Create = "Master Menu.Create";
        public const string Menu_Edit = "Master Menu.Edit";

        public const string Personal = "Personal";
        public const string Personal_View = "Personal.View";
        public const string Personal_Crate = "Personal.Create";
        public const string Personal_Edit = "Personal.Edit";
        public const string Personal_Delete = "Personal.Delete";

        public const string RefPosition = "Referensi Position";
        public const string RefPosition_View = "Referensi Position.View";
        public const string RefPosition_Create = "Referensi Position.Create";
        public const string RefPosition_Edit = "Referensi Position.Edit";
        public const string RefPosition_Delete = "Referensi Position.Delete";

        public static readonly string[] AppPermission =
        {
            Menu_View,
            Menu_Create,
            Menu_Edit,

            Personal_View,
            Personal_Crate,
            Personal_Edit,
            Personal_Delete,

            RefPosition_View,
            RefPosition_Create,
            RefPosition_Edit,
            RefPosition_Delete
        };
    }
}
