//-----------------------------------------------------------------------
// <copyright file="DeleteQuery.cs" company="Martin Brenn">
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
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;

    /// <summary>
    /// Query, that deletes data from table. The rows that should
    /// be removed can be selected by where query
    /// </summary>
    public class DeleteQuery : Query
    {
        /// <summary>
        /// Affected table
        /// </summary>
        private string tablename;

        /// <summary>
        /// Where Statement
        /// </summary>
        private Dictionary<string, object> where;

        /// <summary>
        /// Initializes a new instance of the DeleteQuery class.
        /// </summary>
        /// <param name="tablename">Table whose rows shall be deleted</param>
        public DeleteQuery(string tablename)
            : this(tablename, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DeleteQuery class.
        /// </summary>
        /// <param name="tablename">Name of the table</param>
        /// <param name="where">Constraint of rows to be deleted</param>
        public DeleteQuery(string tablename, Dictionary<string, object> where)
        {
            this.tablename = tablename;
            this.where = where;
        }

        /// <summary>
        /// Gets command, which removes rows from table
        /// </summary>
        /// <param name="connection">Connection to be used</param>
        /// <returns>Command removing the rows</returns>
        public override IDbCommand GetCommand(DbConnection connection)
        {
            var command = connection.CreateCommand();

            command.CommandText = string.Format(
                CultureInfo.InvariantCulture,
                "DELETE FROM {0} WHERE {1}",
                this.tablename,
                MakeWhereStatement(command, this.where));

            return command;
        }
    }
}