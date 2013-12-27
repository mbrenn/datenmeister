using BurnSystems.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Helper.ProviderByIdM
{
    /// <summary>
    /// Provides objet by Id
    /// </summary>
    /// <typeparam name="T">Type of the retrieved items</typeparam>
    public interface IProviderById<T> where T : IHasId
    {
        /// <summary>
        /// Adds an item
        /// </summary>
        /// <param name="item">Item to be added</param>
        void Add(T item);

        /// <summary>
        /// Gets by id
        /// </summary>
        /// <param name="id">Id, whose data shall be retrieved</param>
        /// <returns>Found item or null</returns>
        T Get(long id);

        /// <summary>
        /// Gets all items
        /// </summary>
        /// <returns>Enumeration of items</returns>
        IEnumerable<T> GetAll();
    }
}
