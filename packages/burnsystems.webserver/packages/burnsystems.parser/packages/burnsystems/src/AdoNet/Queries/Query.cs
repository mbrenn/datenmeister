//-----------------------------------------------------------------------
// <copyright file="Query.cs" company="Martin Brenn">
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
    using System.Text;

    /// <summary>
    /// This abstract class implements the interface of the concrete queries
    /// and offers a helper function to create where-statements.
    /// </summary>
    public abstract class Query
    {
        /// <summary>
        /// Makes where statement and adds parameter to command. It is important
        /// that the parameters are inserted in correct order
        /// </summary>
        /// <param name="command">Command to be filled</param>
        /// <param name="where">Where-Variables, which define the 
        /// constraint</param>
        /// <returns>Where statement</returns>
        public static string MakeWhereStatement(
            IDbCommand command,
            Dictionary<string, object> where)
        {
            // if no wherestatement, select everything
            if (where == null || where.Count == 0)
            {
                return "1=1";
            }

            // otherwise make where statement
            var statement = new StringBuilder();
            var isFirstLoop = true;

            foreach (var pair in where)
            {
                if (!isFirstLoop)
                {
                    statement.Append(" AND ");
                }

                statement.Append(pair.Key);
                statement.Append("=@");
                statement.Append(pair.Key);

                var parameter = command.CreateParameter();
                parameter.ParameterName = (string)pair.Key;
                parameter.Value = pair.Value;
                parameter.DbType = GetDbType(pair.Value);

                command.Parameters.Add(parameter);

                isFirstLoop = false;
            }

            return statement.ToString();
        }

        /// <summary>
        /// Gets object type of type of <c>oObject</c>
        /// </summary>
        /// <param name="value">Value to be used</param>
        /// <returns>DbType matching oObject</returns>
        public static System.Data.DbType GetDbType(object value)
        {
            if (value is int)
            {
                return DbType.Int32;
            }
            else if (value is long)
            {
                return DbType.Int64;
            }
            else if (value is string)
            {
                return DbType.String;
            }
            else if (value is DateTime)
            {
                return DbType.DateTime;
            }
            else if (value is bool)
            {
                return DbType.Boolean;
            }
            else if (value is double)
            {
                return DbType.Double;
            }
            else if (value is byte[])
            {
                return DbType.Binary;
            }
            else if (value == null)
            {
                return DbType.String;
            }
            else
            {
                throw new NotSupportedException(
                    "Query.GetDbType: DbType is not supperted: " + value.GetType().ToString());
            }
        }

        /// <summary>
        /// Gets command with data of query
        /// </summary>
        /// <param name="connection">Connection to be used</param>
        /// <returns>New command with information of query in it</returns>
        public abstract IDbCommand GetCommand(DbConnection connection);
    }
}