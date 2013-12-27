//-----------------------------------------------------------------------
// <copyright file="TimeSpanHelper.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Parser.Helper
{
    using System;
    using System.Collections.Generic;
    using BurnSystems.Interfaces;

    /// <summary>
    /// Diese Hilfsfunktion kapselt den TimeSpan ab und stellt verschiedene Properties
    /// und Funktionen nach außen zur Verfügung
    /// </summary>
    public class TimeSpanHelper : IParserObject
    {
        /// <summary>
        /// Value to be encapsulated
        /// </summary>
        private TimeSpan timeSpan;

        /// <summary>
        /// Initializes a new instance of the TimeSpanHelper class.
        /// </summary>
        /// <param name="timeSpan">Timespan helper to be used</param>
        public TimeSpanHelper(TimeSpan timeSpan)
        {
            this.timeSpan = timeSpan;
        }

        /// <summary>
        /// Gets a property by name
        /// </summary>
        /// <param name="name">Name of property</param>
        /// <returns>Value of property</returns>
        public object GetProperty(string name)
        {
            switch (name)
            {
                case "Seconds":
                    return this.timeSpan.Seconds;
                case "Minutes":
                    return this.timeSpan.Minutes;
                case "Hours":
                    return this.timeSpan.Hours;
                case "Days":
                    return this.timeSpan.Days;
                case "Milliseconds":
                    return this.timeSpan.TotalMilliseconds;
                case "TotalSeconds":
                    return this.timeSpan.TotalSeconds;
                case "TotalMinutes":
                    return this.timeSpan.TotalMinutes;
                case "TotalHours":
                    return this.timeSpan.TotalHours;
                case "TotalMilliseconds":
                    return this.timeSpan.TotalMilliseconds;
                case "RoundForSeconds":
                    return TimeSpan.FromSeconds(Math.Round(this.timeSpan.TotalSeconds));
                case "Format":
                    return MathHelper.FormatTimeSpan(this.timeSpan);
            }

            return null;
        }

        /// <summary>
        /// Executes a function
        /// </summary>
        /// <param name="functionName">Name of function</param>
        /// <param name="parameters">Parameters of function</param>
        /// <returns>Result of function</returns>
        public object ExecuteFunction(string functionName, IList<object> parameters)
        {            
            return null;
        }
    }
}
