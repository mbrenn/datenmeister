//-----------------------------------------------------------------------
// <copyright file="SelectQuery.cs" company="Martin Brenn">
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
    using System.Text;

    /// <summary>
    /// This class creates a select query, which is used
    /// to retrieve information from the table
    /// </summary>
    public class SelectQuery : Query
    {
        /// <summary>
        /// Stores the name of the table to be selected
        /// </summary>
        private string tablename;

        /// <summary>
        /// Columns to be selected
        /// </summary>
        private string columns;

        /// <summary>
        /// Where statement
        /// </summary>
        private Dictionary<string, object> where = null;

        /// <summary>
        /// Limit from
        /// </summary>
        private int limitFrom = -1;

        /// <summary>
        /// Stores the number of rows to be selected
        /// </summary>
        private int limitCount = -1;

        /// <summary>
        /// Initializes a new instance of the SelectQuery class.
        /// </summary>
        /// <param name="tablename">Name of the table</param>
        public SelectQuery(string tablename)
        {
            this.tablename = tablename;
            this.columns = "*";
        }

        /// <summary>
        /// Initializes a new instance of the SelectQuery class.
        /// </summary>
        /// <param name="tablename">Name of the table</param>
        /// <param name="columns">Columns to be queried</param>
        public SelectQuery(string tablename, string columns)
        {
            this.tablename = tablename;
            this.columns = columns;
        }

        /// <summary>
        /// Initializes a new instance of the SelectQuery class.
        /// </summary>
        /// <param name="tablename">Name of the table</param>
        /// <param name="where">Constraint of the selection</param>
        public SelectQuery(string tablename, Dictionary<string, object> where)
        {
            this.tablename = tablename;
            this.where = where;
            this.columns = "*";
        }

        /// <summary>
        /// Initializes a new instance of the SelectQuery class.
        /// </summary>
        /// <param name="tablename">Name of the table</param>
        /// <param name="columns">Columns to be queried</param>
        /// <param name="where">Constraint of the selection</param>
        public SelectQuery(
            string tablename, 
            string columns, 
            Dictionary<string, object> where)
        {
            this.tablename = tablename;
            this.columns = columns;
            this.where = where;
        }

        /// <summary>
        /// Gets or sets the name of the column, which should be
        /// used for ordering
        /// </summary>
        public string OrderBy
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of row, which should be used
        /// as a starting point for query
        /// </summary>
        public int LimitFrom
        {
            get { return this.limitFrom; }
            set { this.limitFrom = value; }
        }

        /// <summary>
        /// Gets or sets the number of rows to be retrieved
        /// </summary>
        public int LimitCount
        {
            get { return this.limitCount; }
            set { this.limitCount = value; }
        }

        /// <summary>
        /// Gets command
        /// </summary>
        /// <param name="connection">Connection to be used</param>
        /// <returns>Command, which selects the rows</returns>
        public override IDbCommand GetCommand(DbConnection connection)
        {
            var command = connection.CreateCommand();

            var statement = new StringBuilder();

            // SELECT * FROM Tablename
            statement.AppendFormat(
                CultureInfo.InvariantCulture,
                "SELECT {0} FROM {1}",
                this.columns,
                this.tablename);

            // Where Statement
            if ((this.where != null) && (this.where.Count > 0))
            {
                statement.Append(" WHERE ");
                statement.Append(MakeWhereStatement(command, this.where));
            }

            // Order By Statement
            if (!string.IsNullOrEmpty(this.OrderBy))
            {
                statement.Append(" ORDER BY ");
                statement.Append(this.OrderBy);
            }

            // Limits
            if (this.LimitCount != -1)
            {
                statement.Append(" LIMIT ");

                if (this.LimitFrom != -1)
                {
                    statement.Append(this.LimitFrom);
                    statement.Append(',');
                }

                statement.Append(this.LimitCount);
            }

            command.CommandText = statement.ToString();

            return command;
        }
    }
}
