//-----------------------------------------------------------------------
// <copyright file="JsonBoolean.cs" company="Martin Brenn">
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
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Stores a boolean value
    /// </summary>
    [Obsolete("Use System.Web.Script.Serialization from System.Web.Extensions.dll")]
    public class JsonBoolean : IJsonObject
    {
        /// <summary>
        /// Stores the value
        /// </summary>
        private bool value;

        /// <summary>
        /// Initializes a new instance of the JsonBoolean class.
        /// </summary>
        /// <param name="value">Value to be set</param>
        public JsonBoolean(bool value)
        {
            this.value = value;
        }

        /// <summary>
        /// Converts the boolean to a json string
        /// </summary>
        /// <returns>This object as a json string</returns>
        public override string ToString()
        {
            return this.value ? "true" : "false";
        }
    }
}
