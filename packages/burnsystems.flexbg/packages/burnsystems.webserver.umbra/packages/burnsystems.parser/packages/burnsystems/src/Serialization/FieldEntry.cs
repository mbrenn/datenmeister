//-----------------------------------------------------------------------
// <copyright file="FieldEntry.cs" company="Martin Brenn">
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
    using System.Reflection;

    /// <summary>
    /// This class defines the entry for the different fields
    /// </summary>
    public class FieldEntry
    {
        /// <summary>
        /// Gets or sets the id of the field
        /// </summary>
        public int FieldId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the field
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the methodinfo
        /// </summary>
        public FieldInfo FieldInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Converts this instance to string
        /// </summary>
        /// <returns>Name of field</returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
