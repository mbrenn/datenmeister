//-----------------------------------------------------------------------
// <copyright file="DateTimeHelper.cs" company="Martin Brenn">
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
    /// Hilfsklasse für das Objekt DateTime
    /// </summary>
    public class DateTimeHelper : IParserObject
    {
        /// <summary>
        /// Datetime-Value to be encapsulated
        /// </summary>
        private DateTime dateTime;

        /// <summary>
        /// Initializes a new instance of the DateTimeHelper class.
        /// </summary>
        /// <param name="dateTime">Datetime value to be used</param>
        public DateTimeHelper(DateTime dateTime)
        {
            this.dateTime = dateTime;
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
                case "Day":
                    return this.dateTime.Day;
                case "Date":
                    return this.dateTime.Date;
                case "DayOfWeek":
                    return this.dateTime.DayOfWeek;
                case "DayOfYear":
                    return this.dateTime.DayOfYear;
                case "Hour":
                    return this.dateTime.Hour;
                case "Millisecond":
                    return this.dateTime.Millisecond;
                case "Minute":
                    return this.dateTime.Minute;
                case "Month":
                    return this.dateTime.Month;
                case "Second":
                    return this.dateTime.Second;
                case "Ticks":
                    return this.dateTime.Ticks;
                case "TimeOfDay":
                    return this.dateTime.TimeOfDay;
                case "Year":
                    return this.dateTime.Year;
                case "LongTime":
                    return this.dateTime.ToLongTimeString();
                case "ShortTime":
                    return this.dateTime.ToShortTimeString();
                case "LongDate":
                    return this.dateTime.ToLongDateString();
                case "ShortDate":
                    return this.dateTime.ToShortDateString();
                default:
                    return null;
            }
        }

        /// <summary>
        /// Executes a function
        /// </summary>
        /// <param name="functionName">Name of function</param>
        /// <param name="parameters">Parameters of function</param>
        /// <returns>Value of function</returns>
        public object ExecuteFunction(string functionName, IList<object> parameters)
        {
            return null;
        }
    }
}
