using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace BurnSystems.WebServer.Modules.Cookies
{
    /// <summary>
    /// Defines the interface for cookie management
    /// </summary>
    public interface ICookieManagement
    {
        /// <summary>
        /// Adds a cookie to HTTP Request. 
        /// </summary>
        /// <param name="cookie">Cookie to be added</param>
        void AddCookie(Cookie cookie);

        /// <summary>
        /// Deletes a cookie
        /// </summary>
        /// <param name="name">Name of the cookie</param>
        void DeleteCookie(string name);

        string GetCookie(string name);
    }
}
