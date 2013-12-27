//-----------------------------------------------------------------------
// <copyright file="QueryExtensions.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.AdoNet.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// This extension class adds several methods to the query 
    /// </summary>
    public static class QueryExtensions
    {
        /// <summary>
        /// Executes a query on the given database connection
        /// </summary>
        /// <param name="connection">Connection to be used</param>
        /// <param name="query">Query to be executed</param>
        /// <returns>Returns the number of affected lines</returns>
        public static int ExecuteNonQuery(this DbConnection connection, Query query)
        {
            using (var command = query.GetCommand(connection))
            {
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes a query on the given database connection
        /// </summary>
        /// <typeparam name="T">Type of the object that shall be converted between object and dictionary</typeparam>
        /// <param name="connection">Connection to be used</param>
        /// <param name="query">Query to be executed</param>
        /// <returns>Returns the object that has been queried</returns>
        public static T ExecuteScalar<T>(this DbConnection connection, Query query)
        {
            using (var command = query.GetCommand(connection))
            {
                var result = command.ExecuteScalar();

                if (result == null)
                {
                    return default(T);
                }

                return (T)Convert.ChangeType(
                    result,
                    typeof(T),
                    CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Executes a query on the given database connection
        /// </summary>
        /// <param name="connection">Connection to be used</param>
        /// <param name="query">Query to be executed</param>
        /// <returns>Datareader created by the SQL-statement</returns>
        public static IDataReader ExecuteReader(this DbConnection connection, Query query)
        {
            using (var command = query.GetCommand(connection))
            {
                return command.ExecuteReader();
            }
        }

        /// <summary>
        /// Executes a query on the given database connection and returns a reader
        /// object with cursor on correct position
        /// </summary>
        /// <param name="connection">Connection to be used</param>
        /// <param name="query">Query to be executed</param>
        /// <returns>Enumeration of datareader</returns>
        public static IEnumerable<IDataReader> ExecuteEnumeration(this DbConnection connection, Query query)
        {
            using (var command = query.GetCommand(connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return reader;
                    }
                }
            }
        }

        /// <summary>
        /// Returns true, if the given database connection is a mysql connection
        /// </summary>
        /// <param name="connection">Connection to be checked</param>
        /// <returns>True, if connection is a MySqlConnection</returns>
        public static bool IsMySqlConnection(this DbConnection connection)
        {
            return connection.GetType().FullName == "MySql.Data.MySqlClient.MySqlConnection";
        }

        /// <summary>
        /// Returns true, if the given database connection is an sqlserver connection
        /// </summary>
        /// <param name="connection">Connection to be checked</param>
        /// <returns>True, if connection is a SqlServer Connection</returns>
        public static bool IsSqlServerConnection(this DbConnection connection)
        {
            return connection is System.Data.SqlClient.SqlConnection;
        }

        /// <summary>
        /// Executes multiple queries, separated by a colon. 
        /// At the moment, this method does only a simple splitting and no check for quotes. 
        /// </summary>
        /// <param name="connection">Connection to be used</param>
        /// <param name="multipleLines">Lines separated by colons</param>
        public static void ExecuteMultipleQueries(this DbConnection connection, string multipleLines)
        {
            foreach (var queryText in multipleLines.Split(';')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x)))
            {
                // No empty queries and remove last ';'
                connection.ExecuteNonQuery(new FreeQuery(queryText));
            }
        }

        /// <summary>
        /// Gets the id of the 
        /// </summary>
        /// <returns></returns>
        public static long GetIdOfLastInsertedRow(this DbConnection connection)
        {
            if (connection.IsMySqlConnection())
            {
                var freeQuery = new FreeQuery("SELECT LAST_INSERT_ID()");
                return connection.ExecuteScalar<long>(freeQuery);
            }

            if (connection.IsSqlServerConnection())
            {
                var freeQuery = new FreeQuery("SELECT @@IDENTITY");
                return connection.ExecuteScalar<long>(freeQuery);
            }

            throw new NotImplementedException("Unknown database connection: " + connection.GetType().ToString());
        }
    }
}
