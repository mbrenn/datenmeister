using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Modules.Cookies
{
    /// <summary>
    /// A cookie management, that does not store or return cookie, it really does nothing
    /// </summary>
    public class DummyCookieManagement : ICookieManagement
    {
        public void AddCookie(System.Net.Cookie cookie)
        {
        }

        public void DeleteCookie(string name)
        {
        }

        public string GetCookie(string name)
        {
            return null;
        }
    }
}
