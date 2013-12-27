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
    public class IndexProviderById<T> : IProviderById<T> where T : IHasId
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

            var id = (int) item.Id;
            if (id > 1000)
            {
                throw new InvalidOperationException("Ids with more than 1.000 are not supported");
            }

            while (this.items.Count <= id)
            {
                this.items.Add(default(T));
            }

            this.items[id] = item;
        }

        public T Get(long id)
        {
            if (this.items.Count <= id)
            {
                return default(T);
            }

            return this.items[(int) id];
        }

        public IEnumerable<T> GetAll()
        {
            return this.items.Where(x => x != null);
        }
    }
}
