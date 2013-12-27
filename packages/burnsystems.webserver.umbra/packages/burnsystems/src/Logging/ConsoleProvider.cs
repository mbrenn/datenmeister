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
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// This provider writes all data on the console
    /// </summary>
    public class ConsoleProvider : ILogProvider
    {
        /// <summary>
        /// Stores the mapping of console colors
        /// </summary>
        private Dictionary<LogLevel, ConsoleColor> consoleColors =
            new Dictionary<LogLevel, ConsoleColor>();

        /// <summary>
        /// Initializes a new instance of the ConsoleProvider class.
        /// </summary>
        public ConsoleProvider()
            : this(false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ConsoleProvider class.
        /// <c>simpleOutput</c> defines if a colorful output should be used.
        /// </summary>
        /// <param name="simpleOutput">true, wenn nur eine einfache
        /// Ausgabe erzeugt werden soll. </param>
        public ConsoleProvider(bool simpleOutput)
        {
            this.SimpleOutput = simpleOutput;

            consoleColors[LogLevel.Verbose] = ConsoleColor.DarkGray;
            consoleColors[LogLevel.Notify] = ConsoleColor.DarkGreen;
            consoleColors[LogLevel.Message] = ConsoleColor.Green;
            consoleColors[LogLevel.Fail] = ConsoleColor.Yellow;
            consoleColors[LogLevel.Critical] = ConsoleColor.Red;
            consoleColors[LogLevel.Fatal] = ConsoleColor.Magenta;
        }

        /// <summary>
        /// Gets or sets a value indicating whether a simple output without
        /// date and loglevel should be used.
        /// </summary>
        public bool SimpleOutput
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the categories shall be shown 
        /// on console
        /// </summary>
        public bool ShowCategories
        {
            get;
            set;
        }
        
        #region ILogProvider Members

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
            var color = Console.ForegroundColor;
            ConsoleColor newColor;

            if (!this.consoleColors.TryGetValue(entry.LogLevel, out newColor))
            {
                newColor = ConsoleColor.White;
            }

            Console.ForegroundColor = newColor;

            if (this.SimpleOutput)
            {
                if (!string.IsNullOrEmpty(entry.Categories))
                {
                    Console.Write(entry.Categories + ": ");
                }

                Console.WriteLine(entry.Message);
            }
            else
            {
                if (this.ShowCategories)
                {
                    if (!string.IsNullOrEmpty(entry.Categories))
                    {
                        Console.Write(entry.Categories + ": ");
                    }
                }

                Console.WriteLine(
                    "[{1}: {0:f4}s] {2}",
                    entry.RelativeTime.TotalSeconds,
                    entry.LogLevel.ToString(),
                    entry.Message);
            }

            Console.ForegroundColor = color;
        }

        #endregion
    }
}
