﻿using BurnSystems.Test;
using DatenMeister.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        /// Gets the raw value of the object
        /// </summary>
        public object Value
        {
            get { return this.value; }
        }

        /// <summary>
        /// Initializes a new instance of the dot net object. 
        /// The id is retrieved from the property 'name'. If the property is not existing, an exception will be thrown. 
        /// </summary>
        /// <param name="extent"></param>
        /// <param name="value"></param>
        public DotNetObject(DotNetExtent extent, object value)
        {
            Ensure.That(value != null);

            this.extent = extent;
            this.value = value;

            if (!this.isSet("name") || this.get("name") == null)
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
        /// Gets the extent
        /// </summary>
        public IURIExtent Extent
        {
            get { return this.extent; }
        }

        /// <summary>
        /// Gets the property of a certain .Net Object
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>Object that has been queried</returns>
        public object get(string propertyName)
        {
            var property = GetProperty(propertyName);
            if ( property == null )
            {
                return ObjectHelper.NotSet;
            }

            var method = property.GetGetMethod();
            if (method == null)
            {
                return ObjectHelper.NotSet;
            }

            var value = method.Invoke(this.value, null);
            return this.ConvertIfNecessary(value, property);
        }

        public void set(string propertyName, object value)
        {
            var property = GetProperty(propertyName);
            if (property == null)
            {
                throw new ArgumentException("Property " + propertyName + " cannot be set, because it does not exist");
            }

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
                    this.ConvertIfNecessary(value, property));
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
            if (property == null)
            {
                throw new InvalidOperationException("Property " + propertyName + " cannot be set, because it does not exist");
            }

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
            this.extent.Elements().remove(this);
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
                return null;
            }

            return property;
        }

        /// <summary>
        /// Converts the object to the required datatype as necessary
        /// </summary>
        /// <param name="checkObject">Object to be converted</param>
        /// <returns>Converted object</returns>
        private object ConvertIfNecessary(object checkObject, PropertyInfo propertyInfo)
        {
            var propertyName = propertyInfo.Name;

            if (Extensions.IsNative(checkObject))
            {
                return new DotNetUnspecified(this, propertyInfo, checkObject);
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

                return new DotNetUnspecified(this, propertyInfo, sequence);
            }
            else if (checkObject is IObject)
            {
                return new DotNetUnspecified(this, propertyInfo, checkObject);
            }
            else
            {
                return new DotNetUnspecified(this, propertyInfo, new DotNetObject(this.extent, checkObject, this.id + "/" + propertyName));
            }
        }

        /// <summary>
        /// Gets the metaclass of the object. 
        /// The metaclasses need to be assigned to the mapping in DotNetTypeMapping
        /// </summary>
        /// <returns>The metaclass of the object</returns>
        public IObject getMetaClass()
        {
            if (this.value == null || this.extent == null)
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

        public override string ToString()
        {
            if (this.isSet("name") && !string.IsNullOrEmpty(this.get("name").AsSingle().ToString()))
            {
                return string.Format("\"{0}\" (DotNetObject)",
                    this.get("name").AsSingle().ToString());
            }

            return base.ToString();
        }
    }
}
