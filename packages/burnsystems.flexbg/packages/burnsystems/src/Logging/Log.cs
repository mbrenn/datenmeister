//-----------------------------------------------------------------------
// <copyright file="Log.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using BurnSystems.Collections;

    /// <summary>
    /// The different loglevels from less important to very importang
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Everything will be logged
        /// </summary>
        Everything = 0,

        /// <summary>
        /// Some information, that is only interesting at verbose level
        /// </summary>
        Verbose = 1,

        /// <summary>
        /// Notifications, which are quite interesting
        /// </summary>
        Notify = 2,

        /// <summary>
        /// Messages, which should be evaluated
        /// </summary>
        Message = 3,

        /// <summary>
        /// An action failed and the application can continue normally
        /// </summary>
        Fail = 4,

        /// <summary>
        /// An action failed and the application can continue with some minor
        /// problems or dataloss
        /// </summary>
        Critical = 5,

        /// <summary>
        /// An action failed and the execution of the application was stopped
        /// </summary>
        Fatal = 6
    }

    /// <summary>
    /// The log class can be used to create logs for specific providers.
    /// </summary>
    public class Log : BurnSystems.Logging.ILog, IDisposable
    {
        /// <summary>
        /// Singleton storing the only log
        /// </summary>
        private static Log singleton;

        /// <summary>
        /// Liste von Logprovider
        /// </summary>
        private List<ILogProvider> logProviders;

        /// <summary>
        /// Loglevel ab dem der Log überhaupt aktiv wird
        /// </summary>
        private LogLevel filterLevel = LogLevel.Message;

        /// <summary>
        /// Used synchronisationobject for logging
        /// </summary>
        private object syncObject = new object();

        /// <summary>
        /// Datetime, when log had been created
        /// </summary>
        private DateTime logCreationDate = DateTime.Now;

        /// <summary>
        /// Initializes a new instance of the Log class.
        /// </summary>
        public Log()
        {
            this.logProviders = new List<ILogProvider>();
        }

        /// <summary>
        /// Finalizes an instance of the Log class.
        /// </summary>
        ~Log()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Gets a singleton for logging
        /// </summary>
        public static Log TheLog
        {
            get
            {
                if (singleton == null)
                {
                    lock (typeof(Log))
                    {
                        if (singleton == null)
                        {
                            singleton = new Log();
                        }
                    }
                }

                return singleton;
            }
        }

        /// <summary>
        /// Gets or sets the active filterlevel
        /// </summary>
        public LogLevel FilterLevel
        {
            get { return this.filterLevel; }
            set { this.filterLevel = value; }
        }

        /// <summary>
        /// Gibt eine Liste von LogProvider zurück
        /// </summary>
        /// <returns>An array of logproviders</returns>
        public ILogProvider[] GetLogProviders()
        {
            return this.logProviders.ToArray();
        }

        /// <summary>
        /// Fügt einen neuen LogProvider hinzu
        /// </summary>
        /// <param name="logProvider">Logprovider to be added</param>
        public void AddLogProvider(ILogProvider logProvider)
        {
            logProvider.Start();
            this.logProviders.Add(logProvider);
        }

        /// <summary>
        /// Removes log provider
        /// </summary>
        /// <param name="logProvider">Logprovider to be removed</param>
        public void RemoveLogProvider(ILogProvider logProvider)
        {
            int position = this.logProviders.IndexOf(logProvider);

            if (position == -1)
            {
                throw new ArgumentException("logprovider is not in internal list", "logProvider");
            }

            logProvider.Shutdown();
            this.logProviders.RemoveAt(position);
        }

        /// <summary>
        /// Removes all log providers and resets all internal variables
        /// </summary>
        public void Reset()
        {
            this.filterLevel = LogLevel.Message;
            ListHelper.ForEach(
                this.logProviders,
                x => x.Shutdown());

            this.logProviders.Clear();
        }

        /// <summary>
        /// Logs entry
        /// </summary>
        /// <param name="entry">Entry to be logged</param>
        public void LogEntry(LogEntry entry)
        {
            entry.RelativeTime = DateTime.Now - this.logCreationDate;
            lock (this.syncObject)
            {
                if ((int)entry.LogLevel >= (int)this.filterLevel)
                {
                    this.logProviders.ForEach(
                        x => x.DoLog(entry)); 
                }
            }
        }

        #region IDisposable Member

        /// <summary>
        /// Disposes this object
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes this object
        /// </summary>
        /// <param name="disposing">Flag, if disposed by <c>Dispose()</c></param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Reset();
            }
        }

        #endregion
    }
}
