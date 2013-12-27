//-----------------------------------------------------------------------
// <copyright file="StringHelper.cs" company="Martin Brenn">
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
    using System.Globalization;
    using System.Net;
    using System.Web;
    using BurnSystems.Interfaces;

    /// <summary>
    /// Diese Hilfsklasse kümmert sich um die allgemeine Verarbeitung von Strings
    /// </summary>
    public class StringHelper : IParserObject
    {
        /// <summary>
        /// Content to be encapsulated
        /// </summary>
        private string content;

        /// <summary>
        /// Initializes a new instance of the StringHelper class.
        /// </summary>
        /// <param name="content">Content to be set</param>
        public StringHelper(string content)
        {
            this.content = content;
        }

        /// <summary>
        /// Gets a property
        /// </summary>
        /// <param name="name">Name of property</param>
        /// <returns>Value of property</returns>
        public object GetProperty(string name)
        {
            switch (name)
            {
                case "Length":
                    return this.content.Length;
                case "HtmlEncoded":
                    return HttpUtility.HtmlEncode(this.content);
                case "UrlEncoded":
                    return HttpUtility.UrlEncode(this.content);
                case "Nl2Br":
                    return StringManipulation.Nl2Br(this.content);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Executes a function
        /// </summary>
        /// <param name="functionName">Name of function</param>
        /// <param name="parameters">Parameter of function</param>
        /// <returns>Result of function</returns>
        public object ExecuteFunction(string functionName, IList<object> parameters)
        {
            switch (functionName)
            {
                case "ToUpper":
                    return this.content.ToUpper(CultureInfo.CurrentUICulture);
                case "ToLower":
                    return this.content.ToLower(CultureInfo.CurrentUICulture);
                case "Trim":
                    return this.content.Trim();
                case "TrimEnd":
                    return this.content.TrimEnd();
                case "TrimStart":
                    return this.content.TrimStart();
                case "Substring":
                    return this.content.Substring(
                        Convert.ToInt32(parameters[0]),
                        Convert.ToInt32(parameters[1]));
                case "Shorten":
                    return StringManipulation.ShortenString(
                        this.content,
                        Convert.ToInt32(parameters[0]));
                case "Contains":
                    return this.content.Contains(parameters[0].ToString());
                case "AddSlashes":
                    return this.content.AddSlashes();
            }

            return null;
        }
    }
}
