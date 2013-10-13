using BurnSystems.Test;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.DotNet
{
    public class DotNetObject : IObject
    {
        private IURIExtent extent;

        /// <summary>
        /// Stores the id
        /// </summary>
        private string id;

        private object value;

        public DotNetObject(IURIExtent extent, object value, string id)
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
        public object Get(string propertyName)
        {
            var property = GetProperty(propertyName);

            var method = property.GetGetMethod();
            if (method == null)
            {
                throw new ArgumentException("Getter for " + propertyName + " not found");
            }

            var value = method.Invoke(this.value, null);
            return this.ConvertIfNecessary(value, propertyName);
        }

        public void Set(string propertyName, object value)
        {
            var property = GetProperty(propertyName);

            var method = property.GetSetMethod();
            if (method == null)
            {
                throw new ArgumentException("Setter for " + propertyName + " not found");
            }

            // Converts to target type
            var targetType = property.PropertyType;
            var convertedValue = Convert.ChangeType(value, targetType);
            method.Invoke(this.value, new object[] { convertedValue });
        }

        public IEnumerable<ObjectPropertyPair> GetAll()
        {
            foreach (var property in this.value.GetType().GetProperties())
            {
                var value = property.GetValue(this.value);

                yield return new ObjectPropertyPair(
                    property.Name,
                    this.ConvertIfNecessary(value, property.Name));
            }
        }

        public bool IsSet(string propertyName)
        {
            var property = GetProperty(propertyName);

            var method = property.GetGetMethod();
            if (method == null)
            {
                return false;
            }

            return true;
        }

        public void Unset(string propertyName)
        {
            var property = GetProperty(propertyName);

            var method = property.GetSetMethod();
            if (method == null)
            {
                throw new ArgumentException("Setter for " + propertyName + " not found");
            }

            method.Invoke(this.value, null);
        }

        public void Delete()
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
        private System.Reflection.PropertyInfo GetProperty(string propertyName)
        {
            var property = this.value.GetType().GetProperty(propertyName);
            if (property == null)
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
    }
}
