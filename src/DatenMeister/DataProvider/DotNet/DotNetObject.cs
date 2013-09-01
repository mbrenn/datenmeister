using BurnSystems.Test;
using System;
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
        private Guid id = Guid.NewGuid();

        private object value;

        public DotNetObject(IURIExtent extent, object value)
            : this(value)
        {
            this.extent = extent;
        }

        public DotNetObject(object value)
        {
            Ensure.That(value != null);
            this.value = value;
        }
        
        public object Get(string propertyName)
        {
            var property = GetProperty(propertyName);

            var method = property.GetGetMethod();
            if (method == null)
            {
                throw new ArgumentException("Getter for " + propertyName + " not found");
            }

            return method.Invoke(this.value, null);
        }

        public void Set(string propertyName, object value)
        {
            var property = GetProperty(propertyName);

            var method = property.GetSetMethod();
            if (method == null)
            {
                throw new ArgumentException("Setter for " + propertyName + " not found");
            }

            method.Invoke(this.value, new object[] { value });
        }

        public IEnumerable<ObjectPropertyPair> GetAll()
        {
            foreach (var property in this.value.GetType().GetProperties())
            {
                yield return new ObjectPropertyPair(
                    property.Name,
                    property.GetValue(this.value));
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
            get { return this.id.ToString(); }
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
    }
}
