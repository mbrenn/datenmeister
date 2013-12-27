//-----------------------------------------------------------------------
// <copyright file="ConsoleProvider.cs" company="Martin Brenn">
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
    using System.Diagnostics;

    /// <summary>
    /// This provider writes all data on the console
    /// </summary>
    public class DebugProvider : ILogProvider
    {
        /// <summary>
        /// Initializes a new instance of the DebugProvider class.
        /// </summary>
        public DebugProvider()
        {
        }

        /// <summary>
        /// Nothing is done
        /// </summary>
        public void Start()
        {
        }

        /// <summary>
        /// Nothing is done
        /// </summary>
        public void Shutdown()
        {
        }

        /// <summary>
        /// Writes the logentry to console
        /// </summary>
        /// <param name="entry">Entry to be logged</param>
        public void DoLog(LogEntry entry)
        {
            Console.WriteLine(
                "[{1}: {0:f4}s] {2}",
                entry.RelativeTime.TotalSeconds,
                entry.LogLevel.ToString(),
                entry.Message);
        }
    }
}
