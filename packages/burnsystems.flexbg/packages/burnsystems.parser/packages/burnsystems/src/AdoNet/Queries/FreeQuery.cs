//-----------------------------------------------------------------------
// <copyright file="FreeQuery.cs" company="Martin Brenn">
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
    using System.Data.Common;

    /// <summary>
    /// Creates a freequery with parameters
    /// </summary>
    public class FreeQuery : Query
    {
        /// <summary>
        /// Stores the parameters
        /// </summary>
        private Dictionary<string, object> parameters =
            new Dictionary<string, object>();

        /// <summary>
        /// Initializes a new instance of the FreeQuery class.
        /// </summary>
        /// <param name="queryText">Querytext to be used</param>
        public FreeQuery(string queryText)
        {
            this.QueryText = queryText;
        }

        /// <summary>
        /// Gets or sets the query text
        /// </summary>
        public string QueryText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets command with free query as string
        /// </summary>
        /// <param name="connection">Connection to be used</param>
        /// <returns>Command with filled parameters</returns>
        public override System.Data.IDbCommand GetCommand(DbConnection connection)
        {
            var command = connection.CreateCommand();
            command.CommandText = this.QueryText;

            foreach (var pair in this.parameters)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = pair.Key;
                parameter.Value = pair.Value;
                parameter.DbType = GetDbType(pair.Value);

                command.Parameters.Add(parameter);
            }

            return command;
        }

        /// <summary>
        /// Adds parameter to query
        /// </summary>
        /// <param name="parametername">Name of the parameter</param>
        /// <param name="value">Value to be added</param>
        public void AddParameter(string parametername, object value)
        {
            this.parameters[parametername] = value;
        }

        /// <summary>
        /// Returns the query text
        /// </summary>
        /// <returns>The query text</returns>
        public override string ToString()
        {
            return this.QueryText;
        }
    }
}
