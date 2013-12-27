//-----------------------------------------------------------------------
// <copyright file="ShowTablesQuery.cs" company="Martin Brenn">
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

    /// <summary>
    /// Gets a query, which returns all tables
    /// </summary>
    public class ShowTablesQuery : Query
    {
        /// <summary>
        /// Gets a new command, which shows all tables
        /// </summary>
        /// <param name="connection">Connection to be used</param>
        /// <returns>Command, which shows the tables</returns>
        public override System.Data.IDbCommand GetCommand(DbConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = "SHOW TABLES";

            return command;
        }
    }
}