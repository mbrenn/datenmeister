//-----------------------------------------------------------------------
// <copyright file="FileProvider.cs" company="Martin Brenn">
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
    using System.IO;
    using System.Threading;

    /// <summary>
    /// This is a file provider, which stores the logs into a file.
    /// The logging file is only opened during the writing of a log entry
    /// One entry gets one line. 
    /// 01.01.2000 15:43;Verbose;Category1,Category2;Message
    /// </summary>
    public class FileProvider : ILogProvider
    {
        /// <summary>
        /// Path to logfile
        /// </summary>
        private readonly string path;

        /// <summary>
        /// Flag, if the Fileprovider is currently in the exception
        /// </summary>
        private static bool currentlyInException;

        /// <summary>
        /// Initializes a new instance of the FileProvider class.
        /// </summary>
        /// <param name="path">Path to file storing the logentries</param>
        public FileProvider(string path)
        {
            this.path = path;
        }

        /// <summary>
        /// Gets the logfile. 
        /// </summary>
        public string Path
        {
            get { return this.path; }
        }

        #region ILogProvider Members

        /// <summary>
        /// Starts this handler, is doing nothing
        /// </summary>
        public void Start()
        {            
        }

        /// <summary>
        /// Stops this handler, is doing nothing
        /// </summary>
        public void Shutdown()
        {            
        }

        /// <summary>
        /// Writes the logentry to the file
        /// </summary>
        /// <param name="entry">Entry to be stored</param>
        public void DoLog(LogEntry entry)
        {
            try
            {
                using (var writer = new StreamWriter(this.path, true))
                {
                    string message = entry.Message.Replace(';', ',')
                        .Replace('\r', ' ').Replace('\n', ' ');
                    writer.WriteLine(
                        "{0};{1};{2};{3}",
                        entry.Created, 
                        entry.LogLevel, 
                        entry.Categories,
                        message);
                }
            }
            catch (IOException)
            {
                // Perhaps a blocked file
                Thread.Sleep(1000);

                if (!currentlyInException)
                {
                    currentlyInException = true;
                    Log.TheLog.LogEntry(
                        new LogEntry(
                            LocalizationBS.FileLogProvider_ExceptionCaught, LogLevel.Fail));
                }

                currentlyInException = false;

                // Retry
                using (var writer = new StreamWriter(this.path, true))
                {
                    string message = entry.Message.Replace(';', ',')
                        .Replace('\r', ' ').Replace('\n', ' ');
                    writer.WriteLine(
                        "{0};{1};{2};{3}",
                        entry.Created, 
                        entry.LogLevel, 
                        entry.Categories, 
                        message);
                }
            }
        }

        #endregion
    }
}
