//-----------------------------------------------------------------------
// <copyright file="DatabasePropertyAttribute.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Database.Objects
{
    using System;

    /// <summary>
    /// This attributes indicates that the property shall be stored into database
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DatabasePropertyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the DatabasePropertyAttribute class.
        /// </summary>
        /// <param name="columnName">Name of the column associated to this property</param>
        public DatabasePropertyAttribute(string columnName)
        {
            this.ColumnName = columnName;
        }

        /// <summary>
        /// Gets or sets the column name, where the property shall be stored
        /// </summary>
        public string ColumnName
        {
            get;
            set;
        }
    }
}
