using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Transformations.GroupBy
{
    public class GroupByDictionary
    {
        /// <summary>
        /// Stores the key-value association in a dictionary
        /// </summary>
        private Dictionary<object, List<object>> storage = new Dictionary<object, List<object>>();

        /// <summary>
        /// Gets the storage 
        /// </summary>
        public Dictionary<object, List<object>> Storage
        {
            get { return this.storage; }
        }

        /// <summary>
        /// Adds a value a certain key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddToGroup(object key, object value)
        {
            List<object> values;
            if (!this.storage.TryGetValue(key, out values))
            {
                values = new List<object>();
                this.storage[key] = values;
            }

            values.Add(value);
        }
    }
}
