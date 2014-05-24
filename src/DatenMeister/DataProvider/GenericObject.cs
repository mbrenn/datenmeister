using DatenMeister.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider
{
    /// <summary>
    /// Just a generic object that is not aligned to any data provider or any other object. 
    /// The method is multithreading safe. Can be seen as a completely decoupled thing. 
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

        public GenericObject(IURIExtent extent = null, string id = null)
        {
            this.owner = extent;
            this.id = id;
        }

        public IURIExtent Extent
        {
            get { return this.Extent; }
        }

        /// <summary>
        /// Stores the values
        /// </summary>
        private Dictionary<string, object> values = new Dictionary<string, object>();
        
        public object get(string propertyName)
        {
            lock (this.values)
            {
                if (this.isSet(propertyName))
                {
                    return new GenericUnspecified(this, propertyName, this.values[propertyName]);
                }

                return new GenericUnspecified(this, propertyName, ObjectHelper.NotSet);
            }
        }

        public IEnumerable<ObjectPropertyPair> getAll()
        {
            lock (this.values)
            {
                return this.values.Select(x => 
                    new ObjectPropertyPair(x.Key,
                    new GenericUnspecified(this, x.Key, x.Value)));
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

        public bool unset(string propertyName)
        {
            lock (this.values)
            {
                return this.values.Remove(propertyName);
            }
        }

        public void delete()
        {
            this.owner.Elements().remove(this);
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
