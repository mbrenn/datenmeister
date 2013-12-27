using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.Logging
{
    /// <summary>
    /// Sublogger, which always add the Name of the logger to each logmessage. 
    /// Called class logger, because it is usually used per class
    /// </summary>
    public class ClassLogger : ILog
    {
        /// <summary>
        /// Gets the log instance to be used
        /// </summary>
        public ILog Log
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the name of the instance to be used
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the ClassLogger class.
        /// </summary>
        /// <param name="name">Name of the class logger</param>
        public ClassLogger(string name)
        {
            this.Log = Logging.Log.TheLog;
            this.Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the ClassLogger class.
        /// </summary>
        /// <param name="type">Type, where logger is hosted</param>
        public ClassLogger(Type type)
        {
            this.Log = Logging.Log.TheLog;
            this.Name = type.FullName;
        }

        /// <summary>
        /// Initializes a new instance of the ClassLogger class.
        /// </summary>
        /// <param name="name">Name of the class logger</param>
        /// <param name="log">Logger to be used for logging</param>
        public ClassLogger(string name, ILog log)
        {
            this.Log = log;
            this.Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the ClassLogger class.
        /// </summary>
        /// <param name="type">Type, where logger is hosted</param>
        /// <param name="log">Logger to be used for logging</param>
        public ClassLogger(Type type, ILog log)
        {
            this.Log = log;
            this.Name = type.FullName;
        }

        /// <summary>
        /// Adds a log entry
        /// </summary>
        /// <param name="entry">Entry to be logged</param>
        public void LogEntry(LogEntry entry)
        {
            if (string.IsNullOrEmpty(entry.Categories))
            {
                entry.Categories = this.Name;
            }
            else
            {
                entry.Categories = string.Format(
                    "{0}, {1}",
                    entry.Categories,
                    this.Name);
            }

            this.Log.LogEntry(entry);
        }
    }
}
