using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace BurnSystems.WebServer.Modules.MVC
{
    /// <summary>
    /// Defines some basic methods which are used by the specific Action Result instances
    /// </summary>
    public class BaseActionResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the sending already has been finished
        /// </summary>
        public bool HasFinishedSending
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this request may be cached. Per default: No cache
        /// </summary>
        public bool MayBeCached
        {
            get;
            set;
        }

        /// <summary>
        /// Sends the result to webserver
        /// </summary>
        /// <param name="result">Result to be sent</param>
        protected void SendResult(HttpListenerContext listenerContext, string result)
        {
            this.CheckForSending();

            var bytes = Encoding.UTF8.GetBytes(result);
            listenerContext.Response.ContentEncoding = Encoding.UTF8;
            this.SendResult(listenerContext, bytes);

            this.HasFinishedSending = true;
        }

        protected void SendResult(HttpListenerContext listenerContext, byte[] bytes)
        {
            listenerContext.Response.ContentLength64 = bytes.LongLength;

            if (!this.MayBeCached)
            {
                listenerContext.Response.Headers["Last-Modified"] = "no-cache";
                listenerContext.Response.Headers["Expires"] = "Mon, 26 Jul 1997 05:00:00 GMT";
                listenerContext.Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
                listenerContext.Response.Headers["Pragma"] = "no-cache";
                listenerContext.Response.AddHeader("Cache-Control", "post-check=0, pre-check=0");
            }

            using (var stream = listenerContext.Response.OutputStream)
            {
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        /// <summary>
        /// Checks, whether an additional sending is allowed
        /// </summary>
        private void CheckForSending()
        {
            if (this.HasFinishedSending)
            {
                throw new InvalidOperationException(
                    Localization_WebServer.FinishedSending);
            }
        }
    }
}
