using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NUNA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NUNA.Helpers
{
    public static class PermissionHelper
    {
        public static bool IsAuthorized(string permission)
        {
            SessionHandler _session = new SessionHandler();

            var listPermissionString = _session.Get("Permission");

            if (!string.IsNullOrEmpty(listPermissionString)){
                var listPermission = listPermissionString.Split("|").ToList();

                if (listPermission.Contains(permission))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}
