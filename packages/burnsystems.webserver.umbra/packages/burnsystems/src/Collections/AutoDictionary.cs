//-----------------------------------------------------------------------
// <copyright file="AutoDictionary.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Collections
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This class stores object, whose classes implements the interface IHasKey.
    /// IHasKey offers an access to the name of the instance
    /// </summary>
    /// <typeparam name="T">Type of objects to be stored</typeparam>
    public class AutoDictionary<T> : IDictionary<string, T> where T : IHasKey
    {
        /// <summary>
        /// Object storing the instances
        /// </summary>
        private Dictionary<string, T> dictionary = new Dictionary<string, T>();

        /// <summary>
        /// Gets the number of entries
        /// </summary>
        public int Count
        {
            get { return this.dictionary.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether this dictionary is read only
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the collection of keys
        /// </summary>
        public ICollection<string> Keys
        {
            get { return this.dictionary.Keys; }
        }

        /// <summary>
        /// Gets a collection of values
        /// </summary>
        public ICollection<T> Values
        {
            get { return this.dictionary.Values; }
        }

        /// <summary>
        /// Returns an object with the given key
        /// </summary>
        /// <param name="key">Requested Key</param>
        /// <returns>Object with the key. If no key is found, an exception 
        /// will be thrown.</returns>
        public T this[string key]
        {
            get { return this.dictionary[key]; }
            set { this.Add(key, value); }
        }

        /// <summary>
        /// Adds a new object. If there is already an object with the same name, it
        /// will be overwritten
        /// </summary>
        /// <param name="item">Object to be added</param>
        public void Add(T item)
        {
            this.dictionary[item.Key] = item;
        }

        /// <summary>
        /// Removes the object from database
        /// </summary>
        /// <param name="key">Key of the object. </param>
        /// <returns>true, if the object was found and removed</returns>
        public bool Remove(string key)
        {
            return this.dictionary.Remove(key);
        }

        #region IDictionary<string,T> Member

        /// <summary>
        /// Adds a new entry
        /// </summary>
        /// <param name="key">Key of entry</param>
        /// <param name="value">Value of entry</param>
        public void Add(string key, T value)
        {
            if (value.Key != key)
            {
                throw new ArgumentException("key != value.key");
            }

            this.Add(value);
        }

        /// <summary>
        /// Checks, if key exists
        /// </summary>
        /// <param name="key">Key to checked</param>
        /// <returns>true, if key exists</returns>
        public bool ContainsKey(string key)
        {
            return this.dictionary.ContainsKey(key);
        }
    
        /// <summary>
        /// Tries to get a value
        /// </summary>
        /// <param name="key">Key of requested value</param>
        /// <param name="value">Output of value, if value exists</param>
        /// <returns>true, if value exists</returns>
        public bool TryGetValue(string key, out T value)
        {
            return this.dictionary.TryGetValue(key, out value);
        }
        
        #endregion

        #region ICollection<KeyValuePair<string,T>> Member

        /// <summary>
        /// Adds a new entry
        /// </summary>
        /// <param name="item">Entry to be added</param>
        public void Add(KeyValuePair<string, T> item)
        {
            this.Add(item.Key, item.Value);
        }

        /// <summary>
        /// Cleares dictionary
        /// </summary>
        public void Clear()
        {
            this.dictionary.Clear();
        }

        /// <summary>
        /// Checks, if a specific pair-value-item exists
        /// </summary>
        /// <param name="item">Item to be checked</param>
        /// <returns>true, if it exists</returns>
        public bool Contains(KeyValuePair<string, T> item)
        {
            return this.dictionary.ContainsKey(item.Key)
                && (item.Value.Key == item.Key);
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="array">The parameter is not used.</param>
        /// <param name="arrayIndex">The parameter is not used.</param>
        public void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes on etnry
        /// </summary>
        /// <param name="item">entry to be removed</param>
        /// <returns>true, if item is removed</returns>
        public bool Remove(KeyValuePair<string, T> item)
        {
            if (item.Key == item.Value.Key)
            {
                return this.Remove(item.Key);
            }

            return false;
        }

        #endregion

        #region IEnumerable<KeyValuePair<string,T>> Member

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>Enumerator of this collection</returns>
        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        #endregion

        #region IEnumerable Member

        /// <summary>
        /// Gets the enumerator
        /// </summary>
        /// <returns>Enumerator of this collection</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        #endregion
    }
}
