using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.Database
{
    /// <summary>
    /// Contains some helper methods that are useful for database interaction
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Converts a certain type to a database type. 
        /// This is only possible for simple statements. Strings are also mapped to VARCHAR(255) and not to TEXT
        /// </summary>
        /// <param name="type">Type to be converted</param>
        /// <param name="databaseType">Type of the database</param>
        /// <returns>Text containing the database type</returns>
        public static string ConvertToDatabaseType(Type type, DatabaseType databaseType)
        {
            // Not nullables
            if (type == typeof(int))
            {
                return "INT NOT NULL";
            }

            if (type == typeof(string))
            {
                return "VARCHAR(255)";
            }

            if (type == typeof(DateTime))
            {
                return "DATETIME NOT NULL";
            }

            if (type == typeof(double))
            {
                if (databaseType == DatabaseType.MySql)
                {
                    return "DOUBLE NOT NULL";
                }
                else
                {
                    return "FLOAT NOT NULL";
                }
            }

            if (type == typeof(long))
            {
                return "BIGINT NOT NULL";
            }

            // Nullable
            if (type == typeof(int?))
            {
                return "INT";
            }

            if (type == typeof(DateTime?))
            {
                return "DATETIME";
            }

            if (type == typeof(double?))
            {
                if (databaseType == DatabaseType.MySql)
                {
                    return "DOUBLE";
                }
                else
                {
                    return "FLOAT";
                }
            }

            if (type == typeof(long?))
            {
                return "BIGINT";
            }

            throw new ArgumentException(
                string.Format(
                    LocalizationBS.UnknownTypeToDatabaseTypeConversion,
                    type.ToString()),
                "type");
        }
    }
}
