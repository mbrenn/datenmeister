//-----------------------------------------------------------------------
// <copyright file="JsonNumber.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Net.Json
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Stores a number
    /// </summary>
    [Obsolete("Use System.Web.Script.Serialization from System.Web.Extensions.dll")]
    public class JsonNumber : IJsonObject
    {
        /// <summary>
        /// Value of the instance
        /// </summary>
        private double value;

        /// <summary>
        /// Initializes a new instance of the JsonNumber class.
        /// </summary>
        /// <param name="value">Value to be set</param>
        public JsonNumber(double value)
        {
            this.value = value;
        }

        /// <summary>
        /// Converts the number to a json string
        /// </summary>
        /// <returns>This object as a json string</returns>
        public override string ToString()
        {
            return this.value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
