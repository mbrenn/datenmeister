using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.Logging
{
    /// <summary>
    /// Extension methods for logging interface
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Logs an entry
        /// </summary>
        /// <param name="log">Log where entry shall be added</param>
        /// <param name="level">Level to be logged</param>
        /// <param name="entry">Text to be logged</param>
        public static void LogEntry(this ILog log, LogLevel level, string entry)
        {
            log.LogEntry(new LogEntry(entry, level));
        }

        /// <summary>
        /// Sends out a message with LogLevel Critical
        /// </summary>
        /// <param name="log">Log to be used</param>
        /// <param name="entry">Text to be send out</param>
        public static void Critical(this ILog log, string entry)
        {
            LogEntry(log, LogLevel.Critical, entry);
        }
        
        /// <summary>
        /// Sends out a message with LogLevel Fail
        /// </summary>
        /// <param name="log">Log to be used</param>
        /// <param name="entry">Text to be send out</param>
        public static void Fail(this ILog log, string entry)
        {
            LogEntry(log, LogLevel.Fail, entry);
        }

        /// <summary>
        /// Sends out a message with LogLevel Fatal
        /// </summary>
        /// <param name="log">Log to be used</param>
        /// <param name="entry">Text to be send out</param>
        public static void Fatal(this ILog log, string entry)
        {
            LogEntry(log, LogLevel.Fatal, entry);
        }

        /// <summary>
        /// Sends out a message with LogLevel Message
        /// </summary>
        /// <param name="log">Log to be used</param>
        /// <param name="entry">Text to be send out</param>
        public static void Message(this ILog log, string entry)
        {
            LogEntry(log, LogLevel.Message, entry);
        }

        /// <summary>
        /// Sends out a message with LogLevel Notify
        /// </summary>
        /// <param name="log">Log to be used</param>
        /// <param name="entry">Text to be send out</param>
        public static void Notify(this ILog log, string entry)
        {
            LogEntry(log, LogLevel.Notify, entry);
        }

        /// <summary>
        /// Sends out a message with LogLevel Verbose
        /// </summary>
        /// <param name="log">Log to be used</param>
        /// <param name="entry">Text to be send out</param>
        public static void Verbose(this ILog log, string entry)
        {
            LogEntry(log, LogLevel.Verbose, entry);
        }
    }
}
