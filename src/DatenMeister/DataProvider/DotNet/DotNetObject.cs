﻿using BurnSystems.Test;
using DatenMeister.Logic;
using Ninject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.DotNet
{
    public class DotNetObject : IElement, IKnowsExtentType
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
        /// Stores the reflective sequence where the object is stored. May be null, when object is not connected
        /// </summary>
        private IReflectiveSequence sequence;

        /// <summary>
        /// Gets the raw value of the object
        /// </summary>
        public object Value
        {
            get { return this.value; }
        }

        /// <summary>
        /// Stores the metaclass of the object
        /// </summary>
        private IObject metaClass
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the dot net object. 
        /// The id is retrieved from the property 'name'. If the property is not existing, an exception will be thrown. 
        /// </summary>
        /// <param name="extent"></param>
        /// <param name="value"></param>
        public DotNetObject(IReflectiveSequence sequence, object value)
        {
            // TODO: Assign yourself to the reflective collection
            Ensure.That(!(value is DotNetObject), "DotNetObject may not be hosting another DotNetObject");
            Ensure.That(!(value is IObject), "DotNetObject may not be hosting another IObject");

            Ensure.That(value != null);

            if (sequence != null)
            {
                this.sequence = sequence;
                this.extent = sequence.Extent as DotNetExtent;
            }

            this.value = value;

            if (!this.isSet("name") || ObjectHelper.IsNull(this.get("name")))
            {
                this.id = Guid.NewGuid().ToString();
            }
            else
            {
                this.id = this.get("name").AsSingle().ToString();
            }
        }

        public DotNetObject(IReflectiveSequence sequence, object value, string id)
            : this(sequence, value)
        {
            Ensure.That(id != null);
            Ensure.That(value != null);
            this.value = value;
            this.id = id;
        }

        private DotNetObject(object value, string id)
        {
            Ensure.That(id != null);
            Ensure.That(value != null);
            this.value = value;
            this.id = id;
        }

        public void SetMetaClassByMapping(DotNetExtent extent)
        {
            if (this.value == null)
            {
                // No value given
                this.metaClass = null;
            }
            else
            {
                if (extent == null)
                {
                    // Find by general, perhabs global mapper
                    var mappings = Injection.Application.TryGet<IMapsMetaClassFromDotNet>();
                    if (mappings != null)
                    {
                        this.metaClass = mappings.GetMetaClass(this.value.GetType());
                    }
                    else
                    {
                        this.metaClass = null;
                    }
                }
                else
                {
                    // Find by the internal mapping
                    var result = extent.Mapping.FindByDotNetType(this.value.GetType());
                    if (result != null)
                    {
                        this.metaClass = result.Type;
                    }
                    else
                    {
                        this.metaClass = null;
                    }
                }
            }
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
            if (property == null)
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
                if (propertyName == "id")
                {
                    // Exception for id
                    return;
                }

                throw new ArgumentException("Property '" + propertyName + "' cannot be set, because it does not exist");
            }

            var method = property.GetSetMethod();
            if (method == null)
            {
                if (propertyName == "id")
                {
                    // Exception for id
                    return;
                }

                throw new ArgumentException("Setter for '" + propertyName + "' not found");
            }

            // Converts to target type, if necessary
            var targetType = property.PropertyType;
            if (targetType.IsAssignableFrom(value.GetType()))
            {
                // We can directly assign
                method.Invoke(this.value, new object[] { value });
            }
            else if (ObjectConversion.IsEnumByType(targetType))
            {
                // For enumerations, an explicit converstion need to happen
                // It is necessary to convert, we do the default conversion
                var convertedValue = ObjectConversion.ConvertToEnum(value, targetType);
                method.Invoke(this.value, new object[] { convertedValue });
            }
            else
            {                
                // It is necessary to convert, we do the default conversion
                var convertedValue = Convert.ChangeType(value, targetType);
                method.Invoke(this.value, new object[] { convertedValue });
            }
        }

        public IEnumerable<ObjectPropertyPair> getAll()
        {
            foreach (var property in this.value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
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
            this.sequence.remove(this);
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

            if (ObjectConversion.IsNative(checkObject))
            {
                return new DotNetUnspecified(this, propertyInfo, checkObject, PropertyValueType.Single);
            }
            else if (ObjectConversion.IsEnum(checkObject))
            {
                return new DotNetUnspecified(this, propertyInfo, checkObject, PropertyValueType.Single);
            }
            else if (checkObject is IList<object>)
            {
                var sequence = DotNetSequence.CreateFromList(this.extent, checkObject as IList<object>);
                return new DotNetUnspecified(this, propertyInfo, sequence, PropertyValueType.Enumeration);
            }
            else
            {
                // It might be a generic list. We also need to support the interface
                // to a generic list. Sad but true
                var listType = ObjectConversion.GetTypeOfListByType(checkObject.GetType());
                if (listType != null)
                {
                    var dotNetSequenceType = typeof(DotNetSequence<>).MakeGenericType(listType);
                    var sequence = dotNetSequenceType
                        .GetMethod("CreateFromList")
                        .Invoke(null, new object[] { this.Extent, checkObject });
                    return new DotNetUnspecified(this, propertyInfo, sequence, PropertyValueType.Enumeration);
                }
            }
            
            if (checkObject is IEnumerable)
            {
                var sequence = new DotNetSequence(this.extent);
                sequence.IsReadOnly = true;

                foreach (var value in (checkObject as IEnumerable))
                {
                    sequence.Add(value);
                }

                return new DotNetUnspecified(this, propertyInfo, sequence, PropertyValueType.Enumeration);
            }
            else if (checkObject is IObject)
            {
                return new DotNetUnspecified(this, propertyInfo, checkObject, PropertyValueType.Single);
            }
            else
            {
                // It is not an enumeration and it is not a simple type. 
                // It is a complex .Net-Type
                var elements = this.extent == null ? null : this.extent.Elements();
                return new DotNetUnspecified(this, propertyInfo, new DotNetObject(elements, checkObject, this.id + "/" + propertyName), PropertyValueType.Single);
            }
        }

        /// <summary>
        /// Gets the metaclass of the object. 
        /// The metaclasses need to be assigned to the mapping in DotNetTypeMapping
        /// </summary>
        /// <returns>The metaclass of the object</returns>
        public IObject getMetaClass()
        {
            if (this.metaClass != null)
            {
                return this.metaClass;
            }

            this.SetMetaClassByMapping(this.extent);
            return this.metaClass;
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
            if (this.isSet("name") && !ObjectHelper.IsNull(this.get("name")))
            {
                return string.Format("\"{0}\" (DotNetObject)",
                    this.get("name").AsSingle().ToString());
            }
            else
            {
                return string.Format("Id: {0}", this.id.ToString());
            }
        }

        Type IKnowsExtentType.ExtentType
        {
            get { return typeof(DotNetExtent); }
        }
    }
}
