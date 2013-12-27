//-----------------------------------------------------------------------
// <copyright file="JsonList.cs" company="Martin Brenn">
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    
    /// <summary>
    /// Stores a list of json entries
    /// </summary>
    [Obsolete("Use System.Web.Script.Serialization from System.Web.Extensions.dll")]
    public class JsonList : IJsonObject
    {
        /// <summary>
        /// Stores the list of json objects
        /// </summary>
        private List<IJsonObject> list =
            new List<IJsonObject>();

        /// <summary>
        /// Initializes a new instance of the JsonList class.
        /// </summary>
        public JsonList()
        {
        }

        /// <summary>
        /// Initializes a new instance of the JsonList class.
        /// </summary>
        /// <param name="list">List of items to be added</param>
        public JsonList(IEnumerable list)
        {
            foreach (var value in list)
            {
                this.List.Add(
                    JsonObject.ConvertObject(value));
            }
        }

        /// <summary>
        /// Gets a list of json objects
        /// </summary>
        public List<IJsonObject> List
        {
            get { return this.list; }
        }

        /// <summary>
        /// Converts the list to a json string
        /// </summary>
        /// <returns>This object as a json string</returns>
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append('[');

            var komma = string.Empty;

            foreach (var item in this.List)
            {
                stringBuilder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "{0}{1}",
                    komma, 
                    item.ToString());
                komma = ",";
            }

            stringBuilder.Append(']');

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Adds an object to json list
        /// </summary>
        /// <param name="value">Value to be added</param>
        public void Add(object value)
        {
            this.List.Add(
                JsonObject.ConvertObject(value));
        }
    }
}
