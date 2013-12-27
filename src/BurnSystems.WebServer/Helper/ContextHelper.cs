using System;
using System.Globalization;
using System.Net;
using BurnSystems.WebServer.Dispatcher;
using BurnSystems.Logging;

namespace BurnSystems.WebServer.Helper
{
    /// <summary>
    /// Some methods for context
    /// </summary>
    public static class ContextHelper
    {
        /// <summary>
        /// Logging for this class
        /// </summary>
        private static ClassLogger logger = new ClassLogger(typeof(ContextHelper));

        /// <summary>
        /// Disables the browser cache, so page will be 
        /// always refreshed
        /// </summary>
        public static void DisableBrowserCache(this HttpListenerContext context)
        {
            context.Response.Headers["Last-Modified"] = "no-cache";
            context.Response.Headers["Expires"] = "Mon, 26 Jul 1997 05:00:00 GMT";
            context.Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            context.Response.Headers["Pragma"] = "no-cache";
            // context.Response.AddHeader("Cache-Control", "post-check=0, pre-check=0");
        }

        /// <summary>
        /// Sets that the cache gets expired by time and won't be requested from browser for the given time
        /// </summary>
        /// <param name="info">Information containing the context dispatch information</param>
        /// <param name="expirationTime">Expiration date</param>
        public static bool SetCacheExpiration(this ContextDispatchInformation info, DateTime localModificationDate, TimeSpan expirationTime, byte[] content)
        {
            var done = false;

            localModificationDate = localModificationDate.ToUniversalTime();

            var isModifiedSince = info.Context.Request.Headers["If-Modified-Since"];
            if (isModifiedSince != null)
            {
                var positionSemicolon = isModifiedSince.IndexOf(";");

                if (positionSemicolon != -1)
                {
                    isModifiedSince = isModifiedSince.Substring(0, positionSemicolon);
                }

                try
                {
                    var cacheDate = DateTime.Parse(isModifiedSince).ToUniversalTime();

                    if ((localModificationDate - TimeSpan.FromSeconds(2)) < cacheDate)
                    {
                        info.Context.Response.StatusCode = 304;
                        done = true;
                    }
                }
                catch (FormatException)
                {
                    // Do not handle, 
                    // Header 'If-Modified-Since' might contain values like 'no-cache' or other
                    // This might prevent the correct handling
                }
            }


            info.Context.Response.AddHeader(
                "Last-Modified",
                localModificationDate.ToUniversalTime().ToString("r"));
            info.Context.Response.AddHeader(
                "Date",
                DateTime.Now.ToUniversalTime().ToString("r"));
            info.Context.Response.AddHeader(
                "Expires",
                DateTime.Now.Add(expirationTime).ToString("r"));
            info.Context.Response.AddHeader(
                "Cache-Control",
                "max-age=" + Math.Round(expirationTime.TotalSeconds).ToString());
            info.Context.Response.AddHeader(
                "ETag",
                string.Format(
                    "\"{0}\"",
                    StringManipulation.Sha1(content)));

            return done;
        }

        /// <summary>
        /// Checks whether we browser still has a valid copy of the file within the cache, if true
        /// 304 status response will be returned, otherwise the caller is responsible to send the payload
        /// to the browser. 
        /// The HTTP Headers are created, so browser will store payload in cache and rerequest if necessary. 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="localModificationDate"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool CheckForCache(this ContextDispatchInformation info, DateTime localModificationDate, byte[] content)
        {
            try
            {
                var done = false;
                localModificationDate = localModificationDate.ToUniversalTime();

                var isModifiedSince = info.Context.Request.Headers["If-Modified-Since"];
                if (isModifiedSince != null)
                {
                    var positionSemicolon = isModifiedSince.IndexOf(";");

                    if (positionSemicolon != -1)
                    {
                        isModifiedSince = isModifiedSince.Substring(0, positionSemicolon);
                    }

                    var cacheDate = DateTime.Parse(isModifiedSince).ToUniversalTime();

                    if ((localModificationDate - TimeSpan.FromSeconds(2)) < cacheDate)
                    {
                        info.Context.Response.StatusCode = 304;
                        done = true;
                    }
                }

                // Fügt den Header hinzu
                info.Context.Response.AddHeader(
                    "Last-Modified",
                    localModificationDate.ToString("r"));
                info.Context.Response.AddHeader(
                    "ETag",
                    string.Format(
                        "\"{0}\"",
                        StringManipulation.Sha1(content)));
                info.Context.Response.AddHeader(
                    "Date",
                    DateTime.Now.ToUniversalTime().ToString("r"));
                info.Context.Response.AddHeader(
                    "Cache-Control",
                    "max-age=0");
                return done;
            }
            catch (Exception)
            {
                logger.LogEntry(LogLevel.Message, "Error during test of cache: If-Modified-Since: " + info.Context.Request.Headers["If-Modified-Since"]);
                return false;
            }
        }

        public static bool IsDeflatingAccepted(this ContextDispatchInformation info)
        {
            var header = info.Context.Request.Headers["Accept-Encoding"];
            if (header == null)
            {
                return false;
            }

            if (header.Contains("deflate"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }  
    }
}
