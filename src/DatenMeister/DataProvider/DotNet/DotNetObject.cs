using BurnSystems.Test;
using DatenMeister.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.DotNet
{
    public class DotNetObject : IElement
    {
        /// <summary>
        /// Defines the extent being
        /// </summary>
        private DotNetExtent extent;

        /// <summary>
        /// Stores the id
        /// </summary>
        private string id;

        private object value;

        /// <summary>
        /// Initializes a new instance of the dot net object. 
        /// The id is retrieved from the property 'name'. If the property is not existing, an exception will be thrown. 
        /// </summary>
        /// <param name="extent"></param>
        /// <param name="value"></param>
        public DotNetObject(DotNetExtent extent, object value)
        {
            Ensure.That(extent != null);
            Ensure.That(value != null);

            this.extent = extent;
            this.value = value;

            if (!this.isSet("name"))
            {
                this.id = Guid.NewGuid().ToString();
            }
            else
            {
                this.id = this.get("name").ToString();
            }
        }

        public DotNetObject(DotNetExtent extent, object value, string id)
            : this(value, id)
        {
            Ensure.That(extent != null);

            this.extent = extent;
            this.id = id;
        }

        private DotNetObject(object value, string id)
        {
            Ensure.That(id != null);
            Ensure.That(value != null);
            this.value = value;
            this.id = id;
        }

        /// <summary>
        /// Gets the property of a certain .Net Object
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>Object that has been queried</returns>
        public object get(string propertyName)
        {
            var property = GetProperty(propertyName);

            var method = property.GetGetMethod();
            if (method == null)
            {
                return ObjectHelper.NotSet;
            }

            var value = method.Invoke(this.value, null);
            return this.ConvertIfNecessary(value, propertyName);
        }

        public void set(string propertyName, object value)
        {
            var property = GetProperty(propertyName);

            var method = property.GetSetMethod();
            if (method == null)
            {
                throw new ArgumentException("Setter for " + propertyName + " not found");
            }

            // Converts to target type, if necessary
            var targetType = property.PropertyType;
            if (targetType.IsAssignableFrom(value.GetType()))
            {
                // We can directly assign
                method.Invoke(this.value, new object[] { value });
            }
            else
            {
                // It is necessary to convert
                var convertedValue = Convert.ChangeType(value, targetType);
                method.Invoke(this.value, new object[] { convertedValue });
            }
        }

        public IEnumerable<ObjectPropertyPair> getAll()
        {
            foreach (var property in this.value.GetType().GetProperties())
            {
                var value = property.GetValue(this.value, null);

                yield return new ObjectPropertyPair(
                    property.Name,
                    this.ConvertIfNecessary(value, property.Name));
            }
        }

        public bool isSet(string propertyName)
        {
            var property = GetProperty(propertyName, false);
            if (property == null)
            {
                return false;
            }

            var method = property.GetGetMethod();
            if (method == null)
            {
                return false;
            }

            return true;
        }

        public bool unset(string propertyName)
        {
            var property = GetProperty(propertyName);

            var method = property.GetSetMethod();
            if (method == null)
            {
                throw new ArgumentException("Setter for " + propertyName + " not found");
            }

            method.Invoke(this.value, null);

            return true;
        }

        public void delete()
        {
            Ensure.That(extent != null, "No extent had been given");
            this.extent.RemoveObject(this);
        }

        /// <summary>
        /// Gets the id of the dotnet object
        /// </summary>
        public string Id
        {
            get { return this.id; }
        }

        /// <summary>
        /// Gets property of the current vale
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>Info of property or exception</returns>
        private System.Reflection.PropertyInfo GetProperty(string propertyName, bool throwException = true)
        {
            var property = this.value.GetType().GetProperty(propertyName);
            if (property == null && throwException)
            {
                throw new ArgumentException(propertyName + " not found");
            }

            return property;
        }

        /// <summary>
        /// Converts the object to the required datatype as necessary
        /// </summary>
        /// <param name="checkObject">Object to be converted</param>
        /// <returns>Converted object</returns>
        private object ConvertIfNecessary(object checkObject, string propertyName)
        {
            if (Extensions.IsNative(checkObject))
            {
                return checkObject;
            }
            else if (checkObject is IEnumerable)
            {
                var sequence = new DotNetSequence();
                var n = 0L;
                foreach (var value in (checkObject as IEnumerable))
                {
                    sequence.Add(new DotNetObject(this.extent, value, this.Id + "[" + n.ToString() + "]"));
                    n++;
                }
                
                return sequence;
            }
            else
            {
                return new DotNetObject(this.extent, checkObject, this.id + "/" + propertyName);
            }
        }

        /// <summary>
        /// Gets the metaclass of the object. 
        /// The metaclasses need to be assigned to the mapping in DotNetTypeMapping
        /// </summary>
        /// <returns>The metaclass of the object</returns>
        public IObject getMetaClass()
        {
            if (this.value == null)
            {
                return null;
            }

            var result = this.extent.Mapping.FindByDotNetType(this.value.GetType());
            if (result != null)
            {
                return result.Type;
            }

            return null;
        }

        /// <summary>
        /// Gets the container of the element. .Net objects are never in a container. 
        /// At least for now
        /// </summary>
        /// <returns>Usually null</returns>
        public IObject container()
        {
            return null;
        }
    }
}
