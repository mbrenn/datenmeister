//-----------------------------------------------------------------------
// <copyright file="Converter.cs" company="Martin Brenn">
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
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// This class maps the C# instance type to a database table and offers methods
    /// the insert, update, delete or get items. 
    /// </summary>
    /// <typeparam name="T">Type of the object that shall be converted between object and dictionary</typeparam>
    public class Converter<T> : IConverter<T> where T : new()
    {
        /// <summary>
        /// Stores the assignments 
        /// </summary>
        private static List<AssignmentInfo> assignments = new List<AssignmentInfo>();

        /// <summary>
        /// Stores the assignments  without key
        /// </summary>
        private static List<AssignmentInfo> assignmentsWithoutKey = new List<AssignmentInfo>();

        /// <summary>
        /// Stores the information about the primary key
        /// </summary>
        private static AssignmentInfo primaryKey = null;

        /// <summary>
        /// Initializes static members of the Converter class. 
        /// Static constructor which is used to update internal table about
        /// properties
        /// </summary>
        static Converter()
        {
            var type = typeof(T);

            // First check, if this type has an attribute DatabaseClassAttribute
            var databaseClassAttribute = type.GetCustomAttributes(typeof(DatabaseClassAttribute), false).FirstOrDefault();
            if (databaseClassAttribute == null)
            {
                throw new InvalidOperationException(LocalizationBS.Mapper_AttributeDatabaseClassNotSet);
            }

            // Create array of databaseproperties
            var properties = type.GetProperties(System.Reflection.BindingFlags.FlattenHierarchy | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (var property in properties)
            {
                var databaseKeyAttribute = property.GetCustomAttributes(typeof(DatabaseKeyAttribute), false).FirstOrDefault() as DatabaseKeyAttribute;
                var databasePropertyAttribute = property.GetCustomAttributes(typeof(DatabasePropertyAttribute), false).FirstOrDefault() as DatabasePropertyAttribute;

                if (databaseKeyAttribute != null && databasePropertyAttribute != null)
                {
                    throw new InvalidOperationException(LocalizationBS.Mapper_KeyAndPropertyAttributeSet);
                }

                // If Key of class is found
                if (databaseKeyAttribute != null)
                {
                    if (PrimaryKey != null)
                    {
                        throw new InvalidOperationException(LocalizationBS.Mapper_MultipleDatabaseKeyAttribute);
                    }

                    // Check, if we have a getter
                    if (property.GetGetMethod() == null)
                    {
                        throw new InvalidOperationException(string.Format(LocalizationBS.Mapper_NoPublicGetMethod, property.Name));
                    }

                    // Check, if we have a setter
                    if (property.GetSetMethod() == null)
                    {
                        throw new InvalidOperationException(string.Format(LocalizationBS.Mapper_NoPublicSetMethod, property.Name));
                    }
                    
                    PrimaryKey = ConvertPropertyInfo(databaseKeyAttribute.ColumnName, property);

                    // Check if databasekey is correct
                    if (PrimaryKey.Type != typeof(int) && PrimaryKey.Type != typeof(long))
                    {
                        throw new InvalidOperationException(LocalizationBS.Mapper_WrongTypeForDatabaseKeyAttribute);
                    }

                    assignments.Add(PrimaryKey);
                }

                // If this property shall be stored into database
                if (databasePropertyAttribute != null)
                {
                    // Check, if we have a getter
                    if (property.GetGetMethod() == null)
                    {
                        throw new InvalidOperationException(string.Format (LocalizationBS.Mapper_NoPublicGetMethod, property.Name));
                    }

                    // Check, if we have a setter
                    if (property.GetSetMethod() == null)
                    {
                        throw new InvalidOperationException(string.Format(LocalizationBS.Mapper_NoPublicSetMethod, property.Name));
                    }

                    var assignment = ConvertPropertyInfo(databasePropertyAttribute.ColumnName, property);
                    assignments.Add(assignment);
                    assignmentsWithoutKey.Add(assignment);
                }
            }

            if (PrimaryKey == null)
            {
                throw new InvalidOperationException(LocalizationBS.Mapper_NoDatabaseKeyAttribute);
            }
        }

        /// <summary>
        /// Gets the columnname of the primary key
        /// </summary>
        public string PrimaryKeyName
        {
            get { return primaryKey.ColumnName; }
        }

        /// <summary>
        /// Gets or sets the information about the primary key
        /// </summary>
        private static AssignmentInfo PrimaryKey
        {
            get { return primaryKey; }
            set { primaryKey = value; }
        }

        /// <summary>
        /// Converts the instance to a databaseobject
        /// </summary>
        /// <param name="item">Instance to be converted</param>
        /// <param name="includingPrimaryKey">true, if the primary key shall also be converted</param>
        /// <returns>Dictionary of properties of the item</returns>
        public Dictionary<string, object> ConvertToDatabaseObject(T item, bool includingPrimaryKey)
        {
            var result = new Dictionary<string, object>();
            
            var list = includingPrimaryKey ? assignments : assignmentsWithoutKey;

            foreach (var pair in list)
            {
                var rawValue = pair.PropertyInfo.GetGetMethod().Invoke(item, null);
                var databaseValue = pair.ConvertToDatabaseProperty(rawValue);
                result[pair.ColumnName] = databaseValue;
            }

            return result;
        }

        /// <summary>
        /// Converts the instance to a databaseobject
        /// </summary>
        /// <param name="item">Instance to be converted</param>
        /// <returns>Dictionary of properties of the item</returns>
        public T ConvertToInstance(Dictionary<string, object> item)
        {
            var result = new T();

            foreach (var pair in assignments)
            {
                var rawValue = item[pair.ColumnName];
                object instanceValue;

                if (rawValue == DBNull.Value)
                {
                    instanceValue = null;
                }
                else
                {
                    instanceValue = pair.ConvertToInstanceProperty(rawValue);
                }

                pair.PropertyInfo.GetSetMethod().Invoke(result, new object[] { instanceValue });
            }

            return result;
        }

        /// <summary>
        /// Gets the primary key id of the given object
        /// </summary>
        /// <param name="t">Object, whose primary key is required</param>
        /// <returns>Id of the object</returns>
        public long GetId(T t)
        {
            var rawValue = primaryKey.PropertyInfo.GetGetMethod().Invoke(t, null);
            var value = primaryKey.ConvertToDatabaseProperty(rawValue);
            return Convert.ToInt64(value);
        }

        /// <summary>
        /// Sets the primary key of the given object
        /// </summary>
        /// <param name="t">Object, whose primary key shall be set</param>
        /// <param name="id">Id of the object</param>
        public void SetId(T t, long id)
        {
            var value = primaryKey.ConvertToInstanceProperty(id);
            primaryKey.PropertyInfo.GetSetMethod().Invoke(t, new object[] { value });
        }

        /// <summary>
        /// Converts the PropertyInfo returned by reflection to an AssignmentInfo class
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        /// <param name="property">PropertyInfo to be converted</param>
        /// <returns>Converted PropertyInfo</returns>
        private static AssignmentInfo ConvertPropertyInfo(string columnName, System.Reflection.PropertyInfo property)
        {
            var info = new AssignmentInfo();
            info.ColumnName = columnName;
            info.PropertyInfo = property;
            info.Type = property.PropertyType;
            info.ConvertToDatabaseProperty = (x) => x;

            if (info.Type == typeof(string))
            {
                info.ConvertToInstanceProperty = (x) => x.ToString();
            }
            else if (info.Type == typeof(short))
            {
                info.ConvertToInstanceProperty = (x) => Convert.ToInt16(x);
            }
            else if (info.Type == typeof(int))
            {
                info.ConvertToInstanceProperty = (x) => Convert.ToInt32(x);
            }
            else if (info.Type == typeof(long))
            {
                info.ConvertToInstanceProperty = (x) => Convert.ToInt64(x);
            }
            else if (info.Type == typeof(float))
            {
                info.ConvertToInstanceProperty = (x) => Convert.ToSingle(x);
            }
            else if (info.Type == typeof(double))
            {
                info.ConvertToInstanceProperty = (x) => Convert.ToDouble(x);
            }
            else if (info.Type == typeof(decimal))
            {
                info.ConvertToInstanceProperty = (x) => Convert.ToDecimal(x);
            }
            else if (info.Type == typeof(System.DateTime))
            {
                info.ConvertToInstanceProperty = (x) => Convert.ToDateTime(x);
            }
            else if (info.Type == typeof(System.Boolean))
            {
                info.ConvertToInstanceProperty = (x) => Convert.ToBoolean(x);
            }
            else if (info.Type.IsEnum)
            {
                info.ConvertToDatabaseProperty = (x) => x.ToString();
                info.ConvertToInstanceProperty = (x) =>
                    {
                        return Enum.Parse(info.Type, x.ToString());
                    };
            }
            else
            {
                throw new InvalidOperationException(string.Format(LocalizationBS.Mapper_NotSupportedType, info.Type.ToString()));
            }

            return info;
        }
    }
}
