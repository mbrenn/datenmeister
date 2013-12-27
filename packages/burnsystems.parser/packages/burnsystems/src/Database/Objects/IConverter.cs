using System.Collections.Generic;

namespace BurnSystems.Database.Objects
{
    /// <summary>
    /// Conversion interface between dictionary and real instance
    /// </summary>
    /// <typeparam name="T">Type to be converted</typeparam>
    public interface IConverter<T>
    {
        /// <summary>
        /// Gets the columnname of the primary key
        /// </summary>
        string PrimaryKeyName
        {
            get;
        }

        /// <summary>
        /// Converts the instance to a databaseobject
        /// </summary>
        /// <param name="item">Instance to be converted</param>
        /// <param name="includingPrimaryKey">true, if the primary key shall also be converted</param>
        /// <returns>Dictionary of properties of the item</returns>
        Dictionary<string, object> ConvertToDatabaseObject(T item, bool includingPrimaryKey);

        /// <summary>
        /// Converts the instance to a databaseobject
        /// </summary>
        /// <param name="item">Instance to be converted</param>
        /// <returns>Dictionary of properties of the item</returns>
        T ConvertToInstance(Dictionary<string, object> item);

        /// <summary>
        /// Gets the primary key id of the given object
        /// </summary>
        /// <param name="t">Object, whose primary key is required</param>
        /// <returns>Id of the object</returns>
        long GetId(T t);

        /// <summary>
        /// Sets the primary key of the given object
        /// </summary>
        /// <param name="t">Object, whose primary key shall be set</param>
        /// <param name="id">Id of the object</param>
        void SetId(T t, long id);
    }
}
