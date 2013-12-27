//-----------------------------------------------------------------------
// <copyright file="IListHelper.cs" company="Martin Brenn">
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
    using System.Collections;
    using System.Collections.Generic;
    using BurnSystems.Interfaces;

    /// <summary>
    /// Defines a helper class for lists
    /// </summary>
    public class IListHelper : IParserObject
    {
        /// <summary>
        /// Item to be parsed
        /// </summary>
        private IList item;

        /// <summary>
        /// Initializes a new instance of the IListHelper class.
        /// </summary>
        /// <param name="item">Item to be parsed</param>
        public IListHelper(IList item)
        {
            this.item = item;
        }

        /// <summary>
        /// Gets a property of the list
        /// </summary>
        /// <param name="name">Name of property</param>
        /// <returns>Content of property</returns>
        public object GetProperty(string name)
        {
            switch (name)
            {
                case "Count":
                    return this.item.Count;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Executes a function on object
        /// </summary>
        /// <param name="functionname">Name of function</param>
        /// <param name="parameter">Parameters of function</param>
        /// <returns>Result of function</returns>
        public object ExecuteFunction(string functionname, IList<object> parameter)
        {
            return null;
        }
    }
}
