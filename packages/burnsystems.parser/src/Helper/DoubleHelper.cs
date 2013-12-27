//-----------------------------------------------------------------------
// <copyright file="DoubleHelper.cs" company="Martin Brenn">
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
    using BurnSystems.Interfaces;

    /// <summary>
    /// Hilfsklasse für Fließkommazahlen Zahlen
    /// </summary>
    public class DoubleHelper : IParserObject
    {
        /// <summary>
        /// Value to be encapsulated
        /// </summary>
        private double item;

        /// <summary>
        /// Initializes a new instance of the DoubleHelper class.
        /// </summary>
        /// <param name="number">Number to be parsed</param>
        public DoubleHelper(double number)
        {
            this.item = number;
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
                case "Negative":
                    return this.item * -1;
                case "NumberFormat":
                    return this.item.ToString("n0", CultureInfo.CurrentUICulture);
                case "ExplicitSign":
                    if (this.item > 0)
                    {
                        return "+" + this.item.ToString("n0", CultureInfo.CurrentUICulture);
                    }
                    else
                    {
                        return this.item.ToString("n0", CultureInfo.CurrentUICulture); 
                    }

                case "Ceiling":
                    return Math.Ceiling(this.item);
                case "Floor":
                    return Math.Floor(this.item);
                case "InvariantCulture":
                    return this.item.ToString(CultureInfo.InvariantCulture);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Executes a function
        /// </summary>
        /// <param name="functionName">Name of function</param>
        /// <param name="parameters">Parameters of function</param>
        /// <returns>Result of function</returns>
        public object ExecuteFunction(string functionName, IList<object> parameters)
        {
            switch (functionName)
            {                    
                case "Round":
                    if (parameters.Count == 0)
                    {
                        return Math.Round(this.item);
                    }

                    if (parameters.Count == 1)
                    {
                        double result = Math.Round(
                            this.item, 
                            Convert.ToInt32(parameters[0], CultureInfo.InvariantCulture));
                        return result;
                    }

                    break;
                default:
                    break;
            }

            return null;
        }
    }
}
