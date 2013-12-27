using BurnSystems.Logging;
using System;

namespace BurnSystems.WebServer.Dispatcher
{
    /// <summary>
    /// Offers some default filters
    /// </summary>
    public static class DispatchFilter
    {
        private static ILog logger = new ClassLogger(typeof(DispatchFilter));

        public static Func<ContextDispatchInformation, bool> All
        {
            get { return (x) => true; }
        }

        public static Func<ContextDispatchInformation, bool> None
        {
            get { return (x) => false; }
        }

        /// <summary>
        /// Checks, if the host matches (case-insensitive)
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static Func<ContextDispatchInformation, bool> ByHost(string host)
        {
            return (x) =>
               x.RequestUrl.Host.ToLower() == host.ToLower();
        }

        /// <summary>
        /// Checks, if the host matches (case-insensitive)
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static Func<ContextDispatchInformation, bool> ByUrl(string url)
        {
            if (!url.StartsWith("/"))
            {
                throw new ArgumentException("url does not start with '/'.");
            }

            if (!url.EndsWith("/"))
            {
                logger.LogEntry(LogLevel.Verbose, "Url does not end with '/': " + url);
            }

            url = url.ToLower();

            return (x) =>
               x.RequestUrl.AbsolutePath.ToLower().StartsWith(url);
        }

        /// <summary>
        /// Checks, if the host matches (case-insensitive)
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static Func<ContextDispatchInformation, bool> ByExactUrl(string url)
        {
            if (!url.StartsWith("/"))
            {
                throw new ArgumentException("url does not start with '/'.");
            }

            return (x) =>
               x.RequestUrl.AbsolutePath == url;
        }

        public static Func<ContextDispatchInformation, bool> And(Func<ContextDispatchInformation, bool> f1, Func<ContextDispatchInformation, bool> f2)
        {
            return (x) => f1(x) && f2(x);
        }

        public static Func<ContextDispatchInformation, bool> Or(Func<ContextDispatchInformation, bool> f1, Func<ContextDispatchInformation, bool> f2)
        {
            return (x) => f1(x) && f2(x);
        }
    }
}
