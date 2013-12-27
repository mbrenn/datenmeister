//-----------------------------------------------------------------------
// <copyright file="ReferenceHeader.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Stores the data of the reference
    /// </summary>
    public class ReferenceHeader
    {
        /// <summary>
        /// Gets or sets the object id
        /// </summary>
        public long ObjectId
        {
            get;
            set;
        }
    }
}
