//-----------------------------------------------------------------------
// <copyright file="IParserObject.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// This interface has to be implemented by all objects, which offer methods
    /// or properties to the parser
    /// </summary>
    public interface IParserObject
    {
        /// <summary>
        /// This function returns a specific property, which is accessed by name
        /// </summary>
        /// <param name="name">Name of requested property</param>
        /// <returns>Property behind this object</returns>
        object GetProperty(string name);

        /// <summary>
        /// This function has to execute a function and to return an object
        /// </summary>
        /// <param name="functionName">Name of function</param>
        /// <param name="parameters">Parameters for the function</param>
        /// <returns>Return of function</returns>
        object ExecuteFunction(string functionName, IList<object> parameters);
    }
}
