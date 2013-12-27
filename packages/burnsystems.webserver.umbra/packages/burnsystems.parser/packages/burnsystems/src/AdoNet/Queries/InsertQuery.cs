//-----------------------------------------------------------------------
// <copyright file="InsertQuery.cs" company="Martin Brenn">
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
    /// Insert query, that stores a set of data into table
    /// </summary>
    public class InsertQuery : Query
    {
        /// <summary>
        /// Affected tablename
        /// </summary>
        private string tablename;

        /// <summary>
        /// Data to be inserted
        /// </summary>
        private IDictionary<string, object> data;

        /// <summary>
        /// Initializes a new instance of the InsertQuery class.
        /// </summary>
        /// <param name="tablename">Name of the table</param>
        /// <param name="data">Data to be added</param>
        public InsertQuery(string tablename, IDictionary<string, object> data)
        {
            this.tablename = tablename;
            this.data = data;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the REPLACE-Syntax
        /// shall be used
        /// </summary>
        public bool UseReplaceSyntax
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the query
        /// shall be executed delayed.
        /// </summary>
        public bool IsDelayed
        {
            get;
            set;
        }

        /// <summary>
        /// Creates the command, which inserts the data.
        /// </summary>
        /// <param name="connection">Connection to be used</param>
        /// <returns>Command, which can be executed</returns>
        public override IDbCommand GetCommand(DbConnection connection)
        {
            var leftPart = new StringBuilder();
            var rightPart = new StringBuilder();

            var command = connection.CreateCommand();

            // Makes left and right part
            bool first = true;
            foreach (var pair in this.data)
            {
                if (!first)
                {
                    leftPart.Append(',');
                    rightPart.Append(",");
                }

                rightPart.AppendFormat("@{0}", pair.Key);
                leftPart.Append(pair.Key);

                var parameter = command.CreateParameter();
                parameter.ParameterName = pair.Key;
                parameter.Value = pair.Value ?? DBNull.Value;
                parameter.DbType = Query.GetDbType(pair.Value);

                command.Parameters.Add(parameter);

                first = false;
            }

            // Makes query
            string queryText;
            var delayedText = this.IsDelayed ? " DELAYED " : string.Empty;
            
            if (!this.UseReplaceSyntax)
            {
                queryText = string.Format(
                    CultureInfo.InvariantCulture,
                    "INSERT {3} INTO {0} ({1}) VALUES ({2})",
                    this.tablename,
                    leftPart.ToString(),
                    rightPart.ToString(), 
                    delayedText);
            }
            else
            {
                queryText = string.Format(
                    CultureInfo.InvariantCulture,
                    "REPLACE {3} INTO {0} ({1}) VALUES ({2})",
                    this.tablename,
                    leftPart.ToString(),
                    rightPart.ToString(), 
                    delayedText);
            }

            command.CommandText = queryText;
            return command;
        }
    }
}