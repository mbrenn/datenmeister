//-----------------------------------------------------------------------
// <copyright file="ObjectProperty.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using BurnSystems.Interfaces;

    /// <summary>
    /// This helper class stores the property information
    /// </summary>
    public class ObjectProperty : IParserObject
    {
        /// <summary>
        /// Gets or sets the name of the property
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value of the property
        /// </summary>
        public object Value
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value of the property
        /// </summary>
        public string ValueText
        {
            get;
            set;
        }

        /// <summary>
        /// This function returns a specific property, which is accessed by name
        /// </summary>
        /// <param name="name">Name of requested property</param>
        /// <returns>Property behind this object</returns>
        public object GetProperty(string name)
        {
            switch (name)
            {
                case "Name":
                    return this.Name;
                case "Value":
                    return this.Value;
                case "ValueText":
                    return this.ValueText;
                default:
                    return null;
            }
        }

        /// <summary>
        /// This function has to execute a function and to return an object
        /// </summary>
        /// <param name="functionName">Name of function</param>
        /// <param name="parameters">Parameters for the function</param>
        /// <returns>Return of function</returns>
        public object ExecuteFunction(string functionName, IList<object> parameters)
        {
            return null;
        }

        /// <summary>
        /// Converts to string
        /// </summary>
        /// <returns>Value of property</returns>
        public override string ToString()
        {
            return string.Format(
                "{0}: {1}",
                this.Name,
                this.ValueText);
        }
    }
}
