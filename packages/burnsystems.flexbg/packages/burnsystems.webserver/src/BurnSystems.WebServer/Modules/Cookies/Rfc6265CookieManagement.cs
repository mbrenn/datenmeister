using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace BurnSystems.WebServer.Modules.Cookies
{
    /// <summary>
    /// Implements a cookie management according to RFC 6265.
    /// </summary>
    /// <remarks>
    /// Unfortunetaly, Internet Explorer does not store cookies over a browser session, 
    /// if Expires header has not been set</remarks>
    public class Rfc6265CookieManagement : ICookieManagement
    {
        /// <summary>
        /// Gets or sets a value indicating whether we have full debug messages
        /// </summary>
        private bool haveFullDebug = true;

        private static ILog logger = new ClassLogger(typeof(Rfc6265CookieManagement));

        /// <summary>
        /// Gets or sets the listener context
        /// </summary>
        [Inject(IsMandatory=true)]
        public HttpListenerContext ListenerContext
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a cookie value by name
        /// </summary>
        /// <param name="name">Name  of the cookie</param>
        /// <returns>Value of the found cookie or null</returns>
        public string GetCookie(string name)
        {
            var cookie = this.ListenerContext.Request.Cookies[name];
            if (cookie == null)
            {
                return null;
            }

            return cookie.Value;
        }

        /// <summary>
        /// Adds a cookie
        /// </summary>
        /// <param name="cookie">Cookie to be added</param>
        public void AddCookie(Cookie cookie)
        {
            // Creates the set-header
            var cookieText = new StringBuilder();

            if (cookie.Name.Contains('='))
            {
                throw new InvalidOperationException("Cookie.Name may not contain '='");
            }

            if (!cookie.Value.All(
                x => x == 0x21 || (x >= 0x23 && x <= 0x2B) || (x >= 0x2d && x <= 0x3a) || (x >= 0x3c && x <= 0x5b) || (x >= 0x5d && x <= 0x7e)))
            {
                throw new InvalidOperationException("Cookie.Value may only contain '%x21 / %x23-2B / %x2D-3A / %x3C-5B / %x5D-7E'");
            }

            cookieText.AppendFormat("{0}={1}", cookie.Name, cookie.Value);

            // Expires and Max-Age
            if (cookie.Expires != DateTime.MinValue)
            {
                if (cookie.Expires > DateTime.Now)
                {
                    cookieText.AppendFormat(
                        ";Expires={0};Max-Age={1}",
                        cookie.Expires.ToUniversalTime().ToString("R"),
                        Math.Round((cookie.Expires - DateTime.Now).TotalSeconds));
                }
                else
                {
                    cookieText.AppendFormat(
                        ";Expires={0}",
                        cookie.Expires.ToUniversalTime().ToString("R"));
                }
            }

            // Domain
            if (!string.IsNullOrEmpty(cookie.Domain))
            {
                cookieText.AppendFormat(";Domain={0}", cookie.Domain);
            }

            // Path
            if (!string.IsNullOrEmpty(cookie.Path))
            {
                cookieText.AppendFormat(";Path={0}", cookie.Path);
            }

            // Secure
            if (cookie.Secure)
            {
                cookieText.Append(";Secure");
            }

            // HttpOnly
            if (cookie.HttpOnly)
            {
                cookieText.Append(";HttpOnly");
            }

            // Comment, not in RFC 6252, but will be parsed into extension-av, so we are compliant
            if (!string.IsNullOrEmpty(cookie.Comment))
            {
                cookieText.AppendFormat(";Comment{0}", cookie.Domain);
            }

            this.ListenerContext.Response.AppendHeader(
                "Set-Cookie",
                cookieText.ToString());

            if (haveFullDebug)
            {
                logger.Verbose(
                    string.Format(
                        "Cookie '{0}' for '{1}'",
                        cookie.Name,
                        this.ListenerContext.Request.Url.ToString()));
            }
        }

        /// <summary>
        /// Deletes a cookie
        /// </summary>
        /// <param name="name"></param>
        public void DeleteCookie(string name)
        {
            var cookie = new Cookie(name, string.Empty);
            cookie.Expires = DateTime.Now.AddYears(-1);

            this.AddCookie(cookie);
        }
    }
}
