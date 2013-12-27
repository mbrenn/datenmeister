//-----------------------------------------------------------------------
// <copyright file="DropColumnQuery.cs" company="Martin Brenn">
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
    /// Drops a column from database
    /// </summary>
    public class DropColumnQuery : Query
    {
        /// <summary>
        /// Stores the name of the table to be modified
        /// </summary>
        private string tablename;

        /// <summary>
        /// Stores the name of the column to be dropped
        /// </summary>
        private string columnname;

        /// <summary>
        /// Initializes a new instance of the DropColumnQuery class.
        /// </summary>
        /// <param name="tablename">Name of the table to be modified</param>
        /// <param name="columnname">Name of the column to be modified</param>
        public DropColumnQuery(string tablename, string columnname)
        {
            this.columnname = columnname;
            this.tablename = tablename;
        }

        /// <summary>
        /// Gets command to drop a column defined above
        /// </summary>
        /// <param name="connection">Connection to be used</param>
        /// <returns>Command dropping the column</returns>
        public override System.Data.IDbCommand GetCommand(DbConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText =
                string.Format(
                    CultureInfo.InvariantCulture,
                    "ALTER TABLE {0} DROP COLUMN {1}", 
                    this.tablename,
                    this.columnname);
            return command;
        }
    }
}
