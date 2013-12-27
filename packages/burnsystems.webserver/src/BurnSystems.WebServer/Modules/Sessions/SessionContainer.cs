//-----------------------------------------------------------------------
// <copyright file="SessionContainer.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.WebServer.Modules.Sessions
{
    using System;
    using System.Collections.Generic;
    using BurnSystems.Logging;

    /// <summary>
    /// Dieser Sessioncontainer speichert die Sessions im Groüen 
    /// und Ganzen und kümmert sich auch darum, dass veraltete Sessions
    /// nach einer bestimmten Zeitspanne entfernt werden. 
    /// </summary>
    [Serializable()]
    public class SessionContainer
    {
        /// <summary>
        /// Liste von Sessions
        /// </summary>
        private List<Session> sessions = new List<Session>();

        internal List<Session> Sessions
        {
            get { return this.sessions; }
        }

        /// <summary>
        /// Gets a session with the given id. If session does not exist, null will be returned.
        /// </summary>
        /// <param name="sessionId">Id der angeforderten Session</param>
        /// <returns>Gefundene Session oder null, wenn diese nicht gefunden
        /// wird</returns>
        public Session this[string sessionId]
        {
            get
            {
                lock (this.sessions)
                {
                    var session = this.sessions.Find(
                        delegate(Session otherSession)
                        {
                            return otherSession.SessionId == sessionId;
                        });

                    if (session != null)
                    {
                        session.LastAccess = DateTime.Now;
                    }

                    return session;
                }
            }
        }

        /// <summary>
        /// Erstellt eine neue Session
        /// </summary>
        /// <returns>Recently created session with a unique sessionid</returns>
        public Session CreateNewSession()
        {
            lock (this.sessions)
            {
                var session = new Session();
                session.LastAccess = DateTime.Now;
                session.SessionId = StringManipulation.SecureRandomString(32);

                if (this.sessions.Find(x => x.SessionId == session.SessionId) == null)
                {
                    this.sessions.Add(session);
                    return session;
                }
                else
                {
                    // Es wird solang Sessions erzeugt bis eine wirklich
                    // neue SessionId gefunden wurde. 
                    // Die Wahrscheinlichkeit einer endlosen Rekursion
                    // geht auf 0
                    return this.CreateNewSession();
                }
            }
        }
    }
}
