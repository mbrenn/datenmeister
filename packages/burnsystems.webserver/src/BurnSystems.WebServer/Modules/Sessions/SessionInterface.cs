using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using BurnSystems.ObjectActivation;
using BurnSystems.Logging;
using BurnSystems.Test;
using BurnSystems.WebServer.Modules.Cookies;

namespace BurnSystems.WebServer.Modules.Sessions
{
    /// <summary>
    /// Implements the session interface
    /// </summary>
    public class SessionInterface : ISessionInterface
    {
        /// <summary>
        /// Defines the logger
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(SessionInterface));

        /// <summary>
        /// Gets or sets the cookiemanagement
        /// </summary>
        [Inject(IsMandatory = true)]
        public ICookieManagement Cookies
        {
            get;
            set;
        }

        /// <summary>
        /// Stores the http Listener Interface
        /// </summary>
        [Inject(IsMandatory=true)]
        public HttpListenerContext Context
        {
            get;
            set;
        }

        /// <summary>
        /// Stores the sessioncontainer
        /// </summary>
        [Inject(IsMandatory=true)]
        public SessionContainer sessionContainer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the configuration
        /// </summary>
        [Inject(IsMandatory = true)]
        public SessionConfiguration Configuration
        {
            get;
            set;
        }

        /// <summary>
        /// Checks, if a session exists with the name in cookie "SessionId". 
        /// If this is not the case, a new session will be created, otherwise
        /// the old session will be restored.
        /// </summary>
        public Session GetSession()
        {
            var cookie = this.Context.Request.Cookies["SessionId"];
            var sessionId = string.Empty;
            var isCookieFresh = false;

            if (cookie == null)
            {
                isCookieFresh = true;
            }
            else
            {
                sessionId = cookie.Value;
            }

            var session = this.sessionContainer[sessionId];

            if (session == null)
            {
                session = this.sessionContainer.CreateNewSession();
                sessionId = session.SessionId;
                this.Cookies.AddCookie(
                    new Cookie("SessionId", sessionId, "/"));
                session.IsSessionFresh = true;
            }
            else
            {
                session.IsSessionFresh = false;
            }

            session.IsCookieFresh = isCookieFresh;

            // Räumt vielleicht die Sessions auf
            this.RemovePerhapsOldSessions();

            return session;
        }

        /// <summary>
        /// Entfernt alle alte Sessions aus dem Speicher.
        /// </summary>
        public void RemoveOldSessions()
        {
            lock (this.sessionContainer.Sessions)
            {
                DateTime now = DateTime.Now;

                this.sessionContainer.Sessions.RemoveAll(
                    x => now - x.LastAccess > this.Configuration.MaximumAge);
            }
        }

        /// <summary>
        /// If this function is called, the garbage collection will be started with
        /// a certain probability. This method should be called after each 
        /// webrequest, so the GC will be executed with the configured probability. 
        /// </summary>
        public void RemovePerhapsOldSessions()
        {
            if (MathHelper.Random.NextDouble() < this.Configuration.CollectorProbability)
            {
                logger.LogEntry(
                       new LogEntry(Localization_WebServer.CollectingSessions, LogLevel.Verbose));
                this.RemoveOldSessions();
            }
        }
    }
}
