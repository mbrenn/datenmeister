//-----------------------------------------------------------------------
// <copyright file="ComplexHeader.cs" company="Martin Brenn">
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
    /// This header stores the data for a complex structure
    /// </summary>
    public class ComplexHeader
    {
        /// <summary>
        /// Gets or sets the id of the type
        /// </summary>
        public long TypeId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the object id
        /// </summary>
        public long ObjectId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the amount of properties
        /// </summary>
        public int FieldCount
        {
            get;
            set;
        }
    }
}
