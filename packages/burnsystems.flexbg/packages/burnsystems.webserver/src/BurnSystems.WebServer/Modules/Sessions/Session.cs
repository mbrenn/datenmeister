//-----------------------------------------------------------------------
// <copyright file="Session.cs" company="Martin Brenn">
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
    using BurnSystems.Collections;
    using BurnSystems.Interfaces;

    /// <summary>
    /// Speichert eine Session eines Nutzers. Diese Session ist prinzipiell
    /// serialisierbar
    /// </summary>
    [Serializable()]
    public class Session : IHasWritableIndex<string, object>
    {
        /// <summary>
        /// Stores a value indicating whether the session is fresh
        /// </summary>
        private bool isSessionFresh;

        /// <summary>
        /// Stores a value indicating whether the cookie is fresh
        /// </summary>
        private bool isCookieFresh;

        /// <summary>
        /// Id der Session
        /// </summary>
        private string sessionId;

        /// <summary>
        /// Daten der Session
        /// </summary>
        private NiceDictionary<string, object> data = 
            new NiceDictionary<string, object>();

        /// <summary>
        /// Zeitpunkt an dem der letzte Zugriff auf diese Datenstruktur
        /// erfolgt ist
        /// </summary>
        private DateTime lastAccess;

        /// <summary>
        /// Gets or sets a value indicating whether the session is fresh
        /// </summary>
        public bool IsSessionFresh
        {
            get { return this.isSessionFresh; }
            set { this.isSessionFresh = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the cookie is fresh
        /// </summary>
        public bool IsCookieFresh
        {
            get { return this.isCookieFresh; }
            set { this.isCookieFresh = value; }
        }

        /// <summary>
        /// Gets or sets the id of Session
        /// </summary>
        public string SessionId
        {
            get { return this.sessionId; }
            set { this.sessionId = value; }
        }

        /// <summary>
        /// Gets or sets the time of last access
        /// </summary>
        public DateTime LastAccess
        {
            get { return this.lastAccess; }
            set { this.lastAccess = value; }
        }

        /// <summary>
        /// Gets or sets the data specified by a name
        /// </summary>
        /// <param name="name">Name of request data</param>
        /// <returns>Data behind the name</returns>
        public object this[string name]
        {
            get
            {
                lock (this.data)
                {
                    return this.data[name];
                }
            }

            set
            {
                lock (this.data)
                {
                    this.data[name] = value;
                }
            }
        }

        /// <summary>
        /// Enfernt einen konkreten Eintrag
        /// </summary>
        /// <param name="index">Name of index</param>
        public void Remove(string index)
        {
            lock (this.data)
            {
                this.data.Remove(index);
            }
        }
    }
}
