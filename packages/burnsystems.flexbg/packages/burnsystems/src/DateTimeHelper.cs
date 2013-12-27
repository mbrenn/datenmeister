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

namespace BurnSystems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// This static helper class is used to manipulate DateTime-Structures
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// Gets the first occurance of the hour in the past. 
        /// If current Date is 02.01. 12:00 and <c>hour</c> is 13, 
        /// the date 01.01. 13:00 will be returned
        /// </summary>
        /// <param name="time">Time to be resumed</param>
        /// <param name="hour">Requested hour</param>
        /// <returns>Time in the past with matching hour</returns>
        public static DateTime GoBackToHour(this DateTime time, int hour)
        {
            if (time.Hour >= hour)
            {
                return new DateTime(
                    time.Year,
                    time.Month,
                    time.Day,
                    hour,
                    0,
                    0);
            }
            else
            {
                var yesterday = time.Subtract(
                    TimeSpan.FromDays(1));

                return new DateTime(
                    yesterday.Year,
                    yesterday.Month,
                    yesterday.Day,
                    hour,
                    0,
                    0);
            }
        }
    }
}
