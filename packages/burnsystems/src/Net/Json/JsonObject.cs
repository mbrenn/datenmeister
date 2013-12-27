//-----------------------------------------------------------------------
// <copyright file="JsonObject.cs" company="Martin Brenn">
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
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// Stores the information about a json object
    /// </summary>
    [Obsolete("Use System.Web.Script.Serialization from System.Web.Extensions.dll")]
    public class JsonObject : IJsonObject
    {
        /// <summary>
        /// Stores the properties 
        /// </summary>
        private Dictionary<string, IJsonObject> properties =
            new Dictionary<string, IJsonObject>();

        /// <summary>
        /// Gets or sets the property of a certain key
        /// </summary>
        /// <param name="key">Key to be set</param>
        /// <returns>Value behind the key</returns>
        public object this[string key]
        {
            get
            {
                return this.properties[key];
            }

            set
            {
                this.properties[key] = ConvertObject(value);
            }
        }

        /// <summary>
        /// Creates a json object by providing an instance of 
        /// supported object
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <returns>Converted json object</returns>
        public static IJsonObject ConvertObject(object value)
        {
            if (value == null)
            {
                // Null
                return null;
            }

            var jsonValue = value as IJsonObject;
            if (jsonValue != null)
            {
                return jsonValue;
            }

            var stringValue = value as string;
            if (stringValue != null)
            {
                return new JsonString(stringValue);
            }

            if (value is bool)
            {
                return new JsonBoolean((bool)value);
            }

            if (value is int || value is long || value is double)
            {
                return new JsonNumber(Convert.ToDouble(value, CultureInfo.InvariantCulture));
            }

            if (value is DateTime)
            {
                // TODO: Include JSON-DateTime value here
                return new JsonString(value.ToString());
            }

            throw new NotSupportedException(value.GetType().FullName);
        }

        /// <summary>
        /// Converts a list of objects to a json object
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <returns>Result of the conversion</returns>
        public static IJsonObject ConvertObject(IEnumerable<object> value)
        {
            var result = new JsonList();

            foreach (var item in value)
            {
                result.List.Add(ConvertObject(item));
            }

            return result;
        }

        /// <summary>
        /// Converts a dictionary of objects to a json object
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <returns>Result of the conversion</returns>
        public static IJsonObject ConvertObject(Dictionary<string, object> value)
        {
            var result = new JsonObject();

            foreach (var pair in value)
            {
                result.properties[pair.Key] = ConvertObject(pair.Value);
            }

            return result;
        }

        /// <summary>
        /// Converts the object to a jsonstring
        /// </summary>
        /// <returns>This object as a json string</returns>
        public override string ToString()
        {
            var result = new StringBuilder();

            result.Append('{');

            var komma = string.Empty;
            foreach (var pair in this.properties)
            {
                if (pair.Value == null)
                {
                    continue;
                }

                result.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "{0}\"{1}\": {2}",
                    komma,
                    pair.Key,
                    pair.Value.ToString());

                komma = ",";
            }

            result.Append('}');

            return result.ToString();
        }
    }
}
