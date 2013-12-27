//-----------------------------------------------------------------------
// <copyright file="Mapper.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Database.Objects
{
    using System.Collections.Generic;
    using System.Data.Common;
    using BurnSystems.AdoNet.Queries;

    /// <summary>
    /// This class is responsible for the database access
    /// </summary>
    /// <typeparam name="T">Type of the object that shall be mapped to database</typeparam>
    public class Mapper<T> where T : new()
    {
        /// <summary>
        /// Used converter for the mapping
        /// </summary>
        private IConverter<T> converter;

        /// <summary>
        /// Initializes a new instance of the Mapper class.
        /// </summary>
        /// <param name="tableName">Name of the table to be used</param>
        /// <param name="connection">Databaseconnection to be used</param>
        public Mapper(string tableName, DbConnection connection)
        {
            this.TableName = tableName;
            this.Connection = connection;
            this.converter = new Converter<T>();
        }

        /// <summary>
        /// Initializes a new instance of the Mapper class.
        /// </summary>
        /// <param name="tableName">Name of the table to be used</param>
        /// <param name="connection">Databaseconnection to be used</param>
        /// <param name="converter">Converter used to convert instances</param>
        public Mapper(string tableName, DbConnection connection, IConverter<T> converter)
        {
            this.TableName = tableName;
            this.Connection = connection;
            this.converter = converter;
        }

        /// <summary>
        /// Gets or sets the tablename
        /// </summary>
        public string TableName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the database connection
        /// </summary>
        public DbConnection Connection
        {
            get;
            set;
        }

        /// <summary>
        /// Adds an item to database
        /// </summary>
        /// <param name="t">Item to be added</param>
        /// <returns>Id of the new item</returns>
        public long Add(T t)
        {
            var data = this.converter.ConvertToDatabaseObject(t, false);
            var insertQuery = new InsertQuery(this.TableName, data);
            this.Connection.ExecuteNonQuery(insertQuery);

            var freeQuery = new FreeQuery(string.Format("SELECT MAX({0}) FROM {1}", this.converter.PrimaryKeyName, this.TableName));
            var id = this.Connection.ExecuteScalar<long>(freeQuery);
            this.converter.SetId(t, id);

            return id;
        }

        /// <summary>
        /// Deletes the complete table
        /// </summary>
        public void Clear()
        {
            var deleteQuery = new DeleteQuery(this.TableName);
            this.Connection.ExecuteNonQuery(deleteQuery);
        }

        /// <summary>
        /// Removes an item from database
        /// </summary>
        /// <param name="id">Id of the item to be removed</param>
        public void Delete(long id)
        {
            var where = new Dictionary<string, object>();
            where[this.converter.PrimaryKeyName] = id;

            var deleteQuery = new DeleteQuery(this.TableName, where);
            this.Connection.ExecuteNonQuery(deleteQuery);
        }

        /// <summary>
        /// Removes an item from database
        /// </summary>
        /// <param name="t">Item to be removed</param>
        public void Delete(T t)
        {
            this.Delete(this.converter.GetId(t));
        }

        /// <summary>
        /// Deletes all items from table by dictionary
        /// </summary>
        /// <param name="where">Dictionary containing where constraint</param>
        public void Delete(Dictionary<string, object> where)
        {
            var deleteQuery = new DeleteQuery(this.TableName, where);
            this.Connection.ExecuteNonQuery(deleteQuery);
        }

        /// <summary>
        /// Writes changes of the item to database
        /// </summary>
        /// <param name="t">Item to be updated</param>
        public void Update(T t)
        {
            var where = new Dictionary<string, object>();
            where[this.converter.PrimaryKeyName] = this.converter.GetId(t);

            var data = this.converter.ConvertToDatabaseObject(t, false);

            var updateQuery = new UpdateQuery(this.TableName, data, where);
            this.Connection.ExecuteNonQuery(updateQuery);
        }

        /// <summary>
        /// Gets an item with specific id
        /// </summary>
        /// <param name="id">Id of the object to be retrieved</param>
        /// <returns>The object or default(T) if not found</returns>
        public T Get(long id)
        {
            var where = new Dictionary<string, object>();
            where[this.converter.PrimaryKeyName] = id;

            var selectQuery = new SelectQuery(this.TableName, where);

            foreach (var reader in this.Connection.ExecuteEnumeration(selectQuery))
            {
                var data = ConvertToDictionary(reader);

                return this.converter.ConvertToInstance(data);
            }

            return default(T);
        }

        /// <summary>
        /// Gets an array of all items
        /// </summary>
        /// <returns>Array of items</returns>
        public T[] GetAll()
        {
            var selectQuery = new SelectQuery(this.TableName);

            var result = new List<T>();

            foreach (var reader in this.Connection.ExecuteEnumeration(selectQuery))
            {
                var data = ConvertToDictionary(reader);

                result.Add(this.converter.ConvertToInstance(data));
            }

            return result.ToArray();
        }

        /// <summary>
        /// Gets all items whose properties match to the dictionary 
        /// </summary>
        /// <param name="where">Dictionary that will be converted to a where statement</param>
        /// <returns>Array of items to be returned</returns>
        public T[] Get(Dictionary<string, object> where)
        {
            var selectQuery = new SelectQuery(this.TableName, where);
            return Get(selectQuery);
        }

        /// <summary>
        /// Gets by simple key
        /// </summary>
        /// <param name="key">Column name</param>
        /// <param name="value">Value that is required for the column</param>
        /// <returns>Found elements</returns>
        public T[] Get(string key, object value)
        {
            var data = new Dictionary<string, object>();
            data[key] = value;
            return this.Get(data);
        }

        /// <summary>
        /// Gets all items matching to the query
        /// </summary>
        /// <param name="query">Query to be executed</param>
        /// <returns>Array of items to be returned</returns>
        public T[] Get(string query)
        {
            return this.Get(new FreeQuery(query));
        }

        /// <summary>
        /// Gets all items matchint to the query
        /// </summary>
        /// <param name="selectQuery">Query to be executed</param>
        /// <returns>Array of items to be returned</returns>
        public T[] Get(Query selectQuery)
        {
            var result = new List<T>();

            foreach (var reader in this.Connection.ExecuteEnumeration(selectQuery))
            {
                var data = ConvertToDictionary(reader);

                result.Add(this.converter.ConvertToInstance(data));
            }

            return result.ToArray();
        }

        /// <summary>
        /// Converts the reader to an object
        /// </summary>
        /// <param name="reader">Reader, which cursor is set to the current item</param>
        /// <returns>Converted instance</returns>
        public T ConvertToInstance(System.Data.IDataReader reader)
        {
            var data = ConvertToDictionary(reader);
            return this.converter.ConvertToInstance(data);
        }

        /// <summary>
        /// Just a minor helper methods, which converts the object at database cursor to a dictionary
        /// </summary>
        /// <param name="reader">Reader to be used</param>
        /// <returns>Dictionary containing the data of item at database cursor</returns>
        private static Dictionary<string, object> ConvertToDictionary(System.Data.IDataReader reader)
        {
            var data = new Dictionary<string, object>();
            for (var n = 0; n < reader.FieldCount; n++)
            {
                data[reader.GetName(n)] = reader.GetValue(n);
            }

            return data;
        }
    }
}
