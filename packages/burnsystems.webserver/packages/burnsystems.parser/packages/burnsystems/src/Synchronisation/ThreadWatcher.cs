//-----------------------------------------------------------------------
// <copyright file="ThreadWatcher.cs" company="Martin Brenn">
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
    using BurnSystems.Logging;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// Diese Klasse verarbeitet Threads und überprüft, ob 
    /// die Threads nach einer gewissen Zeit schon beendet sind. 
    /// Ist dies nicht der Fall, so werden sie hart abgebrochen. 
    /// </summary>
    public static class ThreadWatcher
    {
        /// <summary>
        /// Logger being used 
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(ThreadWatcher));

        /// <summary>
        /// Liste der zu überwachenden Threads. Dieses Objekt
        /// ist auch für die 
        /// </summary>
        private static List<ThreadWatcherItem> watchedThreads
            = new List<ThreadWatcherItem>();

        /// <summary>
        /// Flag, ob die Threads geprüft werden sollen.
        /// </summary>
        private static volatile bool checkingThreads = false;

        /// <summary>
        /// Diese Loop überwacht die Threads auf Abbruch
        /// </summary>
        private static Thread watchLoop;

        /// <summary>
        /// Dieses Ereignis wird genutzt, um bei Bedarf 
        /// das Warten auf den Event zu unterbrechen.
        /// </summary>
        private static AutoResetEvent resetEvent =
            new AutoResetEvent(false);

        /// <summary>
        /// Die Zeit, die zwischen zwei Pollingvorgängen 
        /// maximal vergehen kann
        /// </summary>
        private static TimeSpan pollingTime =
            TimeSpan.FromMilliseconds(1000);

        /// <summary>
        /// Fügt einen neuen Thread hinzu
        /// </summary>
        /// <param name="thread">Thread to be watched</param>
        /// <param name="timeOut">Timeout, ab dem der Thread abgebrochen
        /// werden soll.</param>
        /// <returns>Disposable interface, which stops the watch
        /// during disposal</returns>
        public static IDisposable WatchThread(Thread thread, TimeSpan timeOut)
        {
            return
                WatchThread(thread, timeOut, null);
        }

        /// <summary>
        /// Fügt einen neuen Thread hinzu
        /// </summary>
        /// <param name="thread">Thread to be watched</param>
        /// <param name="timeOut">Timeout for watching threads
        /// werden soll.</param>
        /// <param name="actionDelegate">Delegate, which is
        /// called if thread is aborted</param>
        /// <returns>Disposable interface, which stops the watch
        /// during disposal</returns>
        public static IDisposable WatchThread(
            Thread thread,
            TimeSpan timeOut,
            ThreadAbortAction actionDelegate)
        {
            lock (watchedThreads)
            {
                watchedThreads.Add(
                    new ThreadWatcherItem(
                        thread,
                        DateTime.Now + timeOut,
                        actionDelegate));

                if (watchedThreads.Count == 1)
                {
                    // Startet den Thread
                    checkingThreads = true;
                    watchLoop = new Thread(WatchLoop);
                    watchLoop.IsBackground = true;
                    watchLoop.Priority = ThreadPriority.AboveNormal;
                    watchLoop.Name = "BurnSystems.ThreadWatcher";
                    watchLoop.Start();
                }
            }

            return new WatchHelper(thread);
        }

        /// <summary>
        /// Nimmt einen Thread von der Liste herunter.
        /// </summary>
        /// <param name="thread">Thread, der von der Beobachtungsliste 
        /// heruntergenommen werden soll.</param>
        private static void UnwatchThread(Thread thread)
        {
            lock (watchedThreads)
            {
                var item = watchedThreads.Find(
                    x => x.Thread.ManagedThreadId == thread.ManagedThreadId);
                if (item != null)
                {
                    watchedThreads.Remove(item);
                }

                if (watchedThreads.Count == 0)
                {
                    // Kein Thread mehr zu beobachten, stoppe Loop
                    checkingThreads = false;
                }
            }

            resetEvent.Set();
        }

        /// <summary>
        /// Diese Threadschleife wird genutzt um die einzelnen Threads 
        /// zu überwachen. 
        /// </summary>
        private static void WatchLoop()
        {
            while (checkingThreads)
            {
                resetEvent.WaitOne(pollingTime, false);

                lock (watchedThreads)
                {
                    // Überprüft, ob eine der Threads getötet werden soll
                    var now = DateTime.Now;
                    var threadsToBeRemoved = new List<ThreadWatcherItem>();

                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        // Bei einem aktiven Debugger werden keine Threads getötet
                        continue;
                    }

                    for (var n = 0; n < watchedThreads.Count; n++)
                    {
                        var item = watchedThreads[n];
                        if (item == null)
                        {
                            logger.Fail("item == null, not expected, but mitigated");
                            continue;
                        }

                        if (item.TimeOut < now)
                        {
                            // Thread muss getötet werden
                            logger.Fail("Thread has been killed: " + item.Thread.Name);

                            item.Thread.Abort();
                            if (item.OnThreadAbort != null)
                            {
                                item.OnThreadAbort();
                            }
                        }

                        if (!item.Thread.IsAlive)
                        {
                            threadsToBeRemoved.Add(item);
                        }
                    }

                    // Entfernt nun die Threads aus der internen Liste
                    foreach (var item in threadsToBeRemoved)
                    {
                        watchedThreads.Remove(item);
                    }

                    // Wenn kein Thread mehr zu beobachten ist, so 
                    // wird dann die Überwachung eingestellt. 
                    checkingThreads =
                        watchedThreads.Count != 0;
                    if (!checkingThreads)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Diese Hilfsklasse ermöglicht das Nutzen der einfachen
        /// using-Syntax zur Überwachung von Threads
        /// </summary>
        private class WatchHelper : IDisposable
        {
            /// <summary>
            /// Der Thread, der überwacht wird. 
            /// </summary>
            private Thread thread;

            /// <summary>
            /// Initializes a new instance of the WatchHelper class.
            /// </summary>
            /// <param name="thread">Thread to be watched</param>
            public WatchHelper(Thread thread)
            {
                this.thread = thread;
            }

            /// <summary>
            /// Finalizes an instance of the WatchHelper class.
            /// </summary>
            ~WatchHelper()
            {
                this.Dispose(false);
            }

            #region IDisposable Member

            /// <summary>
            /// Diese Methode wird aufgerufen, wenn das 
            /// Objekt weggeworfen wird
            /// </summary>
            public void Dispose()
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }

            /// <summary>
            /// Removes the thread from threadwatcher
            /// </summary>
            /// <param name="disposing">Value, indicating, if
            /// called by Dispose()</param>
            public void Dispose(bool disposing)
            {
                ThreadWatcher.UnwatchThread(this.thread);
            }

            #endregion
        }
    }
}
