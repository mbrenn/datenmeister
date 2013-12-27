//-----------------------------------------------------------------------
// <copyright file="HttpPostRequest.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Net
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using System.Web;
    using BurnSystems.Test;

    /// <summary>
    /// über diese Klasse nutzt die internen .Net-Routinen und stellt
    /// ein einfaches Interface zur Erzeugung eines Post-HTTP-Requests
    /// zur Verfügung. 
    /// </summary>
    public class HttpPostRequest
    {
        /// <summary>
        /// Used WebRequest
        /// </summary>
        private HttpWebRequest request;

        /// <summary>
        /// Used WebResponse
        /// </summary>
        private HttpWebResponse response;

        /// <summary>
        /// Variables, which should be send as POST variables
        /// </summary>
        private Dictionary<string, string> postVariables =
            new Dictionary<string, string>();

        /// <summary>
        /// Gets the dictionary with variables, which should be sent
        /// as POST-Request
        /// </summary>
        public Dictionary<string, string> PostVariables
        {
            get { return this.postVariables; }
        }

        /// <summary>
        /// Gibt die Webresponse für diesen Request zurück
        /// </summary>
        /// <param name="url">Url of request</param>
        /// <returns>Webresponse of request</returns>
        public HttpWebResponse GetResponse(string url)
        {
            return this.GetResponse(new Uri(url));
        }

        /// <summary>
        /// Gibt die Webresponse für diesen Request zurück
        /// </summary>
        /// <param name="url">Url of webrequest</param>
        /// <returns>Webresponse of request</returns>
        public HttpWebResponse GetResponse(Uri url)
        {
            if (this.request == null)
            {
                this.request = WebRequest.Create(url) as HttpWebRequest;
                Ensure.IsNotNull(this.request);

                var builder = new StringBuilder();
                var first = true;
                foreach (KeyValuePair<string, string> pair in this.postVariables)
                {
                    if (!first)
                    {
                        builder.Append('&');
                    }

                    builder.AppendFormat(
                        "{0}={1}",
                        HttpUtility.UrlEncode(pair.Key),
                        HttpUtility.UrlEncode(pair.Value));
                    first = false;
                }

                byte[] postData = Encoding.ASCII.GetBytes(builder.ToString());
                this.request.Method = "POST";
                this.request.ContentLength = postData.Length;
                this.request.ContentType = 
                    "application/x-www-form-urlencoded; encoding='utf-8'";

                using (var stream = this.request.GetRequestStream())
                {
                    stream.Write(postData, 0, postData.Length);
                }
            }

            if (this.response == null)
            {
                this.response = this.request.GetResponse() as HttpWebResponse;
            }

            return this.response;
        }
    }
}
