using BurnSystems.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Helper.ProviderByIdM
{
    /// <summary>
    /// Basic class which is capable to store items and implement IProviderById
    /// </summary>
    public class BaseProviderById<T> : IProviderById<T> where T : IHasId
    {
        /// <summary>
        /// Stores the items
        /// </summary>
        private List<T> items = new List<T>();

        public void Add(T item)
        {
            var result = this.Get(item.Id);
            if (result != null)
            {
                throw new InvalidOperationException(
                    string.Format(
                    "Item with id '{0} ({1})' already existing",
                    item.Id,
                    item.ToString()));
            }

            this.items.Add(item);
        }

        public T Get(long id)
        {
            return this.items.Where(x => x.Id == id).FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            return this.items;
        }
    }
}
