//-----------------------------------------------------------------------
// <copyright file="Pair.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using BurnSystems.Interfaces;

    /// <summary>
    /// Stores a pair of two object
    /// </summary>
    /// <typeparam name="TFirst">Datatype of first value</typeparam>
    /// <typeparam name="TSecond">Datatype of second value</typeparam>
    [Serializable]
    public class Pair<TFirst, TSecond> : IParserObject
    {
        /// <summary>
        /// Initializes a new instance of the Pair class.
        /// </summary>
        public Pair()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Pair class.
        /// </summary>
        /// <param name="first">First value to be stored</param>
        /// <param name="second">Second value to be stored</param>
        public Pair(TFirst first, TSecond second)
        {
            this.First = first;
            this.Second = second;
        }

        /// <summary>
        /// Gets or sets the first value
        /// </summary>
        public TFirst First
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the second value.
        /// </summary>
        public TSecond Second
        {
            get;
            set;
        }

        /// <summary>
        /// Converts the pair to a string
        /// </summary>
        /// <returns>The pair converted to a string</returns>
        public override string ToString()
        {
            return string.Format(
                CultureInfo.CurrentUICulture,
                "{0}, {1}", 
                this.First.ToString(), 
                this.Second.ToString());
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
                case "First":
                    return this.First;
                case "Second":
                    return this.Second;
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
        /// Checks, whether the instance is equal
        /// </summary>
        /// <param name="obj">Object to be checked</param>
        /// <returns>true, if it is equal</returns>
        public override bool Equals(object obj)
        {
            var pair = obj as Pair<TFirst, TSecond>;

            if (pair == null)
            {
                return false;
            }

            return pair.First.Equals(this.First)
                && pair.Second.Equals(this.Second);
        }

        /// <summary>
        /// Gets the hashcase
        /// </summary>
        /// <returns>Hashcade of the pair</returns>
        public override int GetHashCode()
        {
            return this.First.GetHashCode() ^ this.Second.GetHashCode();
        }
    }
}
