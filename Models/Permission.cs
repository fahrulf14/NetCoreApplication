using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NUNA.Models
{
    public class Permission
    {
        public const string Personal = "Personal";
        public const string Personal_Crate = "Personal.Create";
        public const string Personal_Edit = "Personal.Edit";
        public const string Personal_Delete = "Personal.Delete";

        public const string RefPosition = "Referensi Position";
        public const string RefPosition_Create = "Referensi Position.Create";
        public const string RefPosition_Edit = "Referensi Position.Edit";
        public const string RefPosition_Delete = "Referensi Position.Delete";

        public static readonly string[] AppPermission =
        {
            Personal_Crate,
            Personal_Edit,
            Personal_Delete,
            RefPosition_Create,
            RefPosition_Edit,
            RefPosition_Delete
        };
    }
}
