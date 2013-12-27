//-----------------------------------------------------------------------
// <copyright file="LongHelper.cs" company="Martin Brenn">
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
    using System.Collections.Generic;
    using System.Globalization;
    using BurnSystems.Interfaces;

    /// <summary>
    /// Hilfsklasse für lange Zahlen
    /// </summary>
    public class LongHelper : IParserObject
    {
        /// <summary>
        /// Encapsulated number
        /// </summary>
        private long number;

        /// <summary>
        /// Initializes a new instance of the LongHelper class.
        /// </summary>
        /// <param name="number">Number to be used</param>
        public LongHelper(long number)
        {
            this.number = number;
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
                case "NumberFormat":
                    return this.number.ToString("n0", CultureInfo.CurrentUICulture);
                case "ExplicitSign":
                    if (this.number > 0)
                    {
                        return "+" + this.number.ToString("n0", CultureInfo.CurrentUICulture);
                    }
                    else
                    {
                        return this.number.ToString("n0", CultureInfo.CurrentUICulture);
                    }

                default:
                    return null;
            }
        }

        /// <summary>
        /// Executes a function
        /// </summary>
        /// <param name="functionName">Name of Function</param>
        /// <param name="parameters">Parameter of function</param>
        /// <returns>Result of function</returns>
        public object ExecuteFunction(string functionName, IList<object> parameters)
        {            
            return null;
        }
    }
}
