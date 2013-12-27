//-----------------------------------------------------------------------
// <copyright file="AutoDictionary.cs" company="Martin Brenn">
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

    /// <summary>
    /// This interface has to be implemented by all objects, having a certain
    /// key-object and wants to be used the advantages of the autodictionary
    /// for example.
    /// </summary>
    public interface IHasId
    {
        /// <summary>
        /// Gets the name of the key of the instance
        /// </summary>
        long Id
        {
            get;
        }
    }
}
