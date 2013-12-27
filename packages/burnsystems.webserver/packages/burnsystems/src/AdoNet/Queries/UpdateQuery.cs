//-----------------------------------------------------------------------
// <copyright file="UpdateQuery.cs" company="Martin Brenn">
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
    using System.Text;

    /// <summary>
    /// Creates a new Updatequery for database
    /// </summary>
    public class UpdateQuery : Query
    {
        /// <summary>
        /// Stores the name of the table
        /// </summary>
        private string tablename;

        /// <summary>
        /// Data to be updated
        /// </summary>
        private Dictionary<string, object> data;

        /// <summary>
        /// Where statement
        /// </summary>
        private Dictionary<string, object> where;

        /// <summary>
        /// Initializes a new instance of the UpdateQuery class.
        /// </summary>
        /// <param name="tablename">Name of the table to be updated</param>
        /// <param name="data">Data to be set</param>
        /// <param name="where">Constraint selecting the rows
        /// to be updated</param>
        public UpdateQuery(
            string tablename, 
            Dictionary<string, object> data, 
            Dictionary<string, object> where)
        {
            this.tablename = tablename;
            this.data = data;
            this.where = where;
        }

        /// <summary>
        /// Creates the query, which updated the rows
        /// </summary>
        /// <param name="connection">Connection to be used</param>
        /// <returns>Query, which updated the rows</returns>
        public override IDbCommand GetCommand(DbConnection connection)
        {
            var command = connection.CreateCommand();

            var statement = new StringBuilder();

            statement.AppendFormat(
                    CultureInfo.InvariantCulture, 
                    "UPDATE {0} SET ", 
                    this.tablename);

            // Check, if data is empty
            if (this.data.Count == 0)
            {
                throw new InvalidOperationException("Data is empty: this.data.Count");
            }

            bool isFirstLoop = true;
            foreach (var pair in this.data)
            {
                if (!isFirstLoop)
                {
                    statement.Append(',');
                }

                statement.AppendFormat(
                    CultureInfo.InvariantCulture,
                    "{0}.{1}=@set{1}",
                    this.tablename,
                    pair.Key);

                var parameter = command.CreateParameter();
                parameter.ParameterName = "@set" + pair.Key;
                parameter.Value = pair.Value ?? DBNull.Value;
                parameter.DbType = GetDbType(pair.Value);

                command.Parameters.Add(parameter);

                isFirstLoop = false;
            }

            // Append where statement if necessary
            if (this.where != null)
            {
                statement.AppendFormat(
                    CultureInfo.InvariantCulture,
                    " WHERE {0}",
                    MakeWhereStatement(command, this.where));
            }

            command.CommandText = statement.ToString();

            return command;
        }
    }
}
