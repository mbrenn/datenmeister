//-----------------------------------------------------------------------
// <copyright file="ThreadWatcherItem.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Synchronisation
{
    using System;
    using System.Threading;

    /// <summary>
    /// Diese Delegatstruktur wird für die Benachrichtigung 
    /// von Threadabbrüchen genutzt. 
    /// </summary>
    public delegate void ThreadAbortAction();

    /// <summary>
    /// Diese Hilfsklasse dient als Speicherobjekt für die zu 
    /// überwachenden Threads. 
    /// </summary>
    internal class ThreadWatcherItem
    {
        /// <summary>
        /// Initializes a new instance of the ThreadWatcherItem class.
        /// </summary>
        /// <param name="thread">Thread to be watched.</param>
        /// <param name="timeOut">Time, when thread should be aborted</param>
        /// <param name="threadAbortDelegate">Delegate called after abortion.</param>
        public ThreadWatcherItem(
            Thread thread, 
            DateTime timeOut,
            ThreadAbortAction threadAbortDelegate)
        {
            this.Thread = thread;
            this.TimeOut = timeOut;
            this.OnThreadAbort = threadAbortDelegate;
        }

        /// <summary>
        /// Gets or sets the watched thread
        /// </summary>
        public Thread Thread
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the datetime, when thread has to be aborted
        /// </summary>
        public DateTime TimeOut
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the delegate to be called, if thread is aborted by threadwatcher
        /// </summary>
        public ThreadAbortAction OnThreadAbort
        {
            get;
            set;
        }
    }
}
