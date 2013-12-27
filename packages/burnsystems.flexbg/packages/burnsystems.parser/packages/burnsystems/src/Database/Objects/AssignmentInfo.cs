//-----------------------------------------------------------------------
// <copyright file="AssignmentInfo.cs" company="Martin Brenn">
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
    using System.Reflection;

    /// <summary>
    /// This class stores information which is used to map between the C#-Type and the databaseobject
    /// </summary>
    internal class AssignmentInfo
    {
        /// <summary>
        /// Gets or sets the name of the column
        /// </summary>
        public string ColumnName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Property Info of the associated item
        /// </summary>
        public PropertyInfo PropertyInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type
        /// </summary>
        public Type Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the conversion of an object retrieved from database to an object that shall be stores in the C#-instance
        /// </summary>
        public Func<object, object> ConvertToInstanceProperty
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the conversion of an object in C#-Instance to an object that shall be stored into database
        /// </summary>
        public Func<object, object> ConvertToDatabaseProperty
        {
            get;
            set;
        }
    }
}
