//-----------------------------------------------------------------------
// <copyright file="JsonString.cs" company="Martin Brenn">
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
    /// Stores a string
    /// </summary>
    [Obsolete("Use System.Web.Script.Serialization from System.Web.Extensions.dll")]
    public class JsonString : IJsonObject
    {
        /// <summary>
        /// Value of the instance
        /// </summary>
        private string value;

        /// <summary>
        /// Initializes a new instance of the JsonString class.
        /// </summary>
        /// <param name="value">Value to be set</param>
        public JsonString(string value)
        {
            this.value = value;
        }

        /// <summary>
        /// Converts the string to a json string
        /// </summary>
        /// <returns>This object as a json string</returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "\"{0}\"",
                this.value
                    .Replace("\\", "\\\\")
                    .Replace("\"", "\\\"")
                    .Replace("\t", "\\t")
                    .Replace("\r", "\\r")
                    .Replace("\n", "\\n"));
        }
    }
}
