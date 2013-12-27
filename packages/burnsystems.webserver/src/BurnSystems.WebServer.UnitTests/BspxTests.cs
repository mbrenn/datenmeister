using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Net;
using System.Web.Script.Serialization;
using BurnSystems.WebServer.Modules.UserManagement;

namespace BurnSystems.WebServer.UnitTests
{
    [TestFixture]
    public class BspxTests
    {
        [Test]
        public void TestPostControllerForGet()
        {
            using (var server = ServerTests.CreateServer())
            {
                var webClient = new WebClient();
                webClient.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);

                var data = webClient.DownloadString("http://localhost:8081/file/post.bspx");
                Assert.That(data.Contains("<input type=\"submit\" value=\"Abschicken\" />"));
            }
        }

        [Test]
        public void TestPostControllerForPost()
        {
            using (var server = ServerTests.CreateServer())
            {
                var webClient = new WebClient();
                webClient.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);

                var request= WebRequest.Create("http://localhost:8081/file/post.bspx");
                request.Method = "POST";
                
                var requestBytes = Encoding.UTF8.GetBytes("Prename=Hans&Name=Wurst");
                using (var inputStream = request.GetRequestStream())
                {
                    inputStream.Write(requestBytes, 0, requestBytes.Length);
                }

                var response = request.GetResponse();

                using (var outputStream = response.GetResponseStream())
                {
                    var totalLength = Convert.ToInt32(response.ContentLength);
                    var responseBytes = new byte[totalLength];
                    outputStream.Read(responseBytes, 0, totalLength);

                    var data = Encoding.UTF8.GetString(responseBytes);
                    Assert.That(data.Contains("Hans"));
                }
            }
        }

        [Test]
        public void TestSessions()
        {
            using (var server = ServerTests.CreateServer())
            {
                var cookie = GetCookie();

                // Check, if we got our session
                var url = "http://localhost:8081/file/session.bspx";
                
                var request = GetSessionRequest(cookie, url);
                var response = request.GetResponse();

                using (var outputStream = response.GetResponseStream())
                {
                    var totalLength = Convert.ToInt32(response.ContentLength);
                    var responseBytes = new byte[totalLength];
                    outputStream.Read(responseBytes, 0, totalLength);

                    var data = Encoding.UTF8.GetString(responseBytes);
                    Assert.That(data.Contains("Wurst"));
                }
            }
        }

        /// <summary>
        /// Gets the response for a web request, including a cookie
        /// </summary>
        /// <param name="cookie">Cookie of the session</param>
        /// <param name="url">Url to be called</param>
        /// <returns>Webresponse</returns>
        public static WebRequest GetSessionRequest(string cookie, string url)
        {
            var request = WebRequest.Create(url);
            request.Headers["Cookie"] = "SessionId=" + cookie;
            return request;
        }

        /// <summary>
        /// Gets the response for a web request, including a cookie
        /// </summary>
        /// <param name="cookie">Cookie of the session</param>
        /// <param name="url">Url to be called</param>
        /// <returns>Webresponse</returns>
        public static WebRequest GetSessionRequest(string cookie, string url, string post)
        {
            var request = WebRequest.Create(url);
            request.Headers["Cookie"] = "SessionId=" + cookie;

            if (post != null)
            {
                request.Method = "POST";
                using (var inputStream = request.GetRequestStream())
                {
                    var bytes = Encoding.UTF8.GetBytes(post);
                    inputStream.Write(bytes, 0, bytes.Length);
                }
            }

            return request;
        }

        public static string GetResponseText(WebRequest request)
        {
            var response = request.GetResponse();

            using (var outputStream = response.GetResponseStream())
            {
                var totalLength = Convert.ToInt32(response.ContentLength);
                var responseBytes = new byte[totalLength];
                outputStream.Read(responseBytes, 0, totalLength);

                return Encoding.UTF8.GetString(responseBytes);
            }
        }

        public static T GetResponseObject<T>(WebRequest request)
        {
            var value = GetResponseText(request);
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<T>(value);
        }

        public static T GetResponseObject<T>(string cookie, string url, string post = null)
        {
            var request = GetSessionRequest(cookie, url, post);
            var value = GetResponseText(request);
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<T>(value);
        }

        public static string GetCookie()
        {
            string cookie;

            var webClient = new WebClient();
            webClient.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);

            {
                var request = WebRequest.Create("http://localhost:8081/file/session.bspx");
                request.Method = "POST";

                var requestBytes = Encoding.UTF8.GetBytes("Value=Wurst");
                using (var inputStream = request.GetRequestStream())
                {
                    inputStream.Write(requestBytes, 0, requestBytes.Length);
                }

                var response = request.GetResponse();
                var cookieText = response.Headers["Set-Cookie"];
                Assert.That(string.IsNullOrEmpty(cookieText), Is.False);
                var posEqual = cookieText.IndexOf("=");
                var posSemicolon = cookieText.IndexOf(";");
                cookie = cookieText.Substring(posEqual + 1, posSemicolon - posEqual - 1).Trim();
                Assert.That(cookie != null);

                using (var outputStream = response.GetResponseStream())
                {
                    var totalLength = Convert.ToInt32(response.ContentLength);
                    var responseBytes = new byte[totalLength];
                    outputStream.Read(responseBytes, 0, totalLength);
                }
            }
            return cookie;
        }
    }
}