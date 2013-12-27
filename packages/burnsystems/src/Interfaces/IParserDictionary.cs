//-----------------------------------------------------------------------
// <copyright file="IParserDictionary.cs" company="Martin Brenn">
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
    /// <summary>
    /// This interface is used for objects, which want to offer an access
    /// by a field
    /// </summary>
    public interface IParserDictionary
    {
        /// <summary>
        /// This method is called, when the parser evaluates a field
        /// </summary>
        /// <param name="name">Name of object</param>
        /// <returns>Returned object</returns>
        object this[string name]
        {
            get;
        }
    }
}
