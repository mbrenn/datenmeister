using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Logic
{
    /// <summary>
    /// Implements the IDictionary interface to make IObjects available as IObjects
    /// </summary>
    public class ObjectDictionary : IDictionary<string, object>
    {
        /// <summary>
        /// Stores the value
        /// </summary>
        private IObject value;

        /// <summary>
        /// Initializes a new instance of the ObjectDictionary class
        /// </summary>
        /// <param name="value">Value to be set</param>
        public ObjectDictionary(IObject value)
        {
            this.value = value;
        }

        public void Add(string key, object value)
        {
            this.value.set(key, value);
        }

        public bool ContainsKey(string key)
        {
            return value.isSet(key);
        }

        public ICollection<string> Keys
        {
            get { return this.value.getAll().Select(x => x.PropertyName).ToList(); }
        }

        public bool Remove(string key)
        {
            return this.value.unset(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            if (this.value.isSet(key))
            {
                value = this.value.get(key);
                return true;
            }

            value = null;
            return false;
        }

        public ICollection<object> Values
        {
            get { return this.value.getAll().Select(x => x.Value).ToList(); }
        }

        public object this[string key]
        {
            get
            {
                return this.value.get(key);
            }
            set
            {
                this.value.set(key, value);
            }
        }

        public void Add(KeyValuePair<string, object> item)
        {
            this.value.set(item.Key, item.Value);
        }

        public void Clear()
        {
            foreach (var key in this.Keys)
            {
                this.value.unset(key);
            }
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            if (!this.ContainsKey(item.Key))
            {
                return false;
            }

            var value = this[item.Key];
            if (value == item.Value)
            {
                return true;
            }

            return false;
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return this.value.getAll().Count(); }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            if (this.Contains(item))
            {
                this.value.unset(item.Key);
                return true;
            }

            return false;
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            foreach (var pair in this.value.getAll())
            {
                yield return new KeyValuePair<string, object>(pair.PropertyName, pair.Value);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            foreach (var pair in this.value.getAll())
            {
                yield return new KeyValuePair<string, object>(pair.PropertyName, pair.Value);
            }
        }
    }
}
