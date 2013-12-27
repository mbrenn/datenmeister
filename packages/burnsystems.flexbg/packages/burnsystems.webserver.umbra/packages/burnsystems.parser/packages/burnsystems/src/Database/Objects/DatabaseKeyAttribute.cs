//-----------------------------------------------------------------------
// <copyright file="DatabaseKeyAttribute.cs" company="Martin Brenn">
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
    /// This attribute indicates that the property shall be used as the primary key for database
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DatabaseKeyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the DatabaseKeyAttribute class.
        /// </summary>
        /// <param name="columnName">Name of the column associated to this property</param>
        public DatabaseKeyAttribute(string columnName)
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
