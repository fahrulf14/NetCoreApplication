﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NUNA.Services
{
    public class UserService
    {
        private readonly IHttpContextAccessor _context;
        public UserService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public string GetUser()
        {
            return _context.HttpContext.User?.Identity?.Name;
        }
    }
}
