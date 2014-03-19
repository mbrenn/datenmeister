using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider
{
    /// <summary>
    /// Just a generic object that is not aligned to any data provider or any other object. 
    /// The method is multithreading safe
    /// </summary>
    public class GenericObject : IObject
    {
        /// <summary>
        /// Stores the owner of the extent
        /// </summary>
        private IURIExtent owner;

        /// <summary>
        /// Stores the id
        /// </summary>
        private string id;

        public GenericObject(IURIExtent extent, string id)
        {
            this.owner = extent;
            this.id = id;
        }

        /// <summary>
        /// Stores the values
        /// </summary>
        private Dictionary<string, object> values = new Dictionary<string, object>();
        
        public object get(string propertyName)
        {
            lock (this.values)
            {
                return this.values[propertyName];
            }
        }

        public IEnumerable<ObjectPropertyPair> getAll()
        {
            lock (this.values)
            {
                return this.values.Select(x => new ObjectPropertyPair(x.Key, x.Value));
            }
        }

        public bool isSet(string propertyName)
        {
            lock (this.values)
            {
                return this.values.ContainsKey(propertyName);
            }
        }

        public void set(string propertyName, object value)
        {
            lock (this.values)
            {
                this.values[propertyName] = value;
            }
        }

        public void unset(string propertyName)
        {
            lock (this.values)
            {
                this.values.Remove(propertyName);
            }
        }

        public void delete()
        {
            this.owner.RemoveObject(this);
        }

        public string Id
        {
            get
            {
                return this.id;
            }
        }
    }
}
