﻿using Microsoft.AspNetCore.Http;
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

            var Email = _session.Get("Email");
            if (Email != null) Email = Email.Split("@")[0];
            var listPermissionString = _session.Get("Permission");
            var listRolePermissionString = _session.Get("RolePermission");

            if (!string.IsNullOrEmpty(listPermissionString))
            {
                var listPermission = listPermissionString.Split("|").ToList();
                var listRolePermission = listRolePermissionString.Split("|").ToList();

                if (listRolePermission != null)
                {
                    listPermission.AddRange(listRolePermission);
                }

                if (listPermission.Contains(permission))
                {
                    return true;
                }
                else
                {
                    if (Email == "developer")
                    {
                        return true;
                    }

                    return false;
                }
            }
            else if (Email == "developer")
            {
                return true;
            }
            return false;
        }
    }
}
