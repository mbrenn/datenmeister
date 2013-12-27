//-----------------------------------------------------------------------
// <copyright file="DropTableQuery.cs" company="Martin Brenn">
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
    using System.Data.Common;
    using System.Globalization;

    /// <summary>
    /// Creates an SQL-command, which drops a complete table
    /// </summary>
    public class DropTableQuery : Query
    {
        /// <summary>
        /// Stores the table to be dropped
        /// </summary>
        private string tablename;

        /// <summary>
        /// Initializes a new instance of the DropTableQuery class.
        /// </summary>
        /// <param name="tablename">Name of the table to be dropped</param>
        public DropTableQuery(string tablename)
        {
            this.tablename = tablename;
        }

        /// <summary>
        /// Gets the command, which drops a table
        /// </summary>
        /// <param name="connection">Used database connection</param>
        /// <returns>Command, which drops a table</returns>
        public override System.Data.IDbCommand GetCommand(DbConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = string.Format(
                CultureInfo.InvariantCulture,
                "DROP TABLE {0}",
                this.tablename);

            return command;
        }
    }
}
