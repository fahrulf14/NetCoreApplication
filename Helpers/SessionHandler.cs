using NUNA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NUNA.Helpers
{
    public class SessionHandler
    {
        public string Get(string key)
        {
            try
            {
                return SessionService.Current.Session.Get<string>(key);
            }
            catch(Exception ex)
            {
                var a = ex.Message;
            }
            return "";
        }

        public void Set(string key, string value)
        {
            SessionService.Current.Session.Set<string>(key, value);
        }
    }
}
