//-----------------------------------------------------------------------
// <copyright file="ShowColumnsQuery.cs" company="Martin Brenn">
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
    /// This class creates a query, which lets the user show all columns of a table
    /// </summary>
    public class ShowColumnsQuery : Query
    {
        /// <summary>
        /// Stores the name of the table
        /// </summary>
        private string tablename;

        /// <summary>
        /// Initializes a new instance of the ShowColumnsQuery class.
        /// </summary>
        /// <param name="tablename">Name of the table</param>
        public ShowColumnsQuery(string tablename)
        {
            this.tablename = tablename;
        }

        /// <summary>
        /// Gets the command, which queries the columns
        /// </summary>
        /// <param name="connection">Connection to be used</param>
        /// <returns>Command, which queries the columns</returns>
        public override System.Data.IDbCommand GetCommand(DbConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = string.Format(
                CultureInfo.InvariantCulture,
                "SHOW COLUMNS FROM {0}", 
                this.tablename);
            return command;
        }
    }
}