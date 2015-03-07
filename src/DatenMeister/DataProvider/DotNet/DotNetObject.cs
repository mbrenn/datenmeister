using BurnSystems.Logger;
using BurnSystems.Test;
using DatenMeister.DataProvider.Common;
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
        /// Stores the logger
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(DotNetObject));

        /// <summary>
        /// Stores the value itself which is abstracted
        /// </summary>
        private object value;

        /// <summary>
        /// Defines the extent being
        /// </summary>
        private DotNetExtent extent;

        /// <summary>
        /// Stores the id
        /// </summary>
        private string id;

        /// <summary>
        /// Stores the reflective sequence where the object is stored. 
        /// May be null, when object is not connected to any collection
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
        /// Gets the id of the dotnet object
        /// </summary>
        public string Id
        {
            get { return this.id; }
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

            Ensure.That(value != null, "No value is given");

            if (sequence != null)
            {
                this.sequence = sequence;
                this.extent = sequence.Extent as DotNetExtent;
            }

            this.value = value;

            if (!this.isSet("name") || ObjectConversion.IsNull(this.getAsSingle("name")))
            {
                this.id = Guid.NewGuid().ToString();
            }
            else
            {
                this.id = this.getAsSingle("name").ToString();
            }
        }

        public DotNetObject(IReflectiveSequence sequence, object value, string id)
            : this(sequence, value)
        {
            Ensure.That(id != null);
            Ensure.That(value != null);
            this.id = id;
        }

        /// <summary>
        /// Initializes a new instance of the DotNetObject and
        /// just assigns an object and an id
        /// </summary>
        /// <param name="value">Value to be added</param>
        /// <param name="id">Id of the object to set</param>
        private DotNetObject(object value, string id)
        {
            Ensure.That(id != null);
            Ensure.That(value != null);
            this.value = value;
            this.id = id;
        }

        public void SetMetaClassByMapping(DotNetExtent extent)
        {
            this.metaClass = null;
            if (this.value != null)
            {
                if (extent == null)
                {
                    // Find by general, perhabs global mapper
                    var mappings = Injection.Application.TryGet<IMapsMetaClassFromDotNet>();
                    if (mappings != null)
                    {
                        this.metaClass = mappings.GetMetaClass(this.value.GetType());
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
        public object get(string propertyName, RequestType requestType = RequestType.AsDefault)
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
            return this.ConvertIfNecessary(value, property, requestType);
        }

        /// <summary>
        /// Sets the property of a certain .Net object
        /// </summary>
        /// <param name="propertyName">Name of the property to be set</param>
        /// <param name="value">Value to be set</param>
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
                // For enums, an explicit converstion need to happen
                // It is necessary to convert, we do the default conversion
                var convertedValue = ObjectConversion.ConvertToEnum(value, targetType);
                method.Invoke(this.value, new object[] { convertedValue });
            }
            else
            {
                // It is necessary to convert, we do the default conversion
                // Tries to perform the conversion as good as possible
                var convertedValue = Convert.ChangeType(value, targetType);
                method.Invoke(this.value, new object[] { convertedValue });
            }
        }

        /// <summary>
        /// Gets all properties
        /// </summary>
        /// <returns>Enumeration of all items </returns>
        public IEnumerable<ObjectPropertyPair> getAll()
        {
            foreach (var property in this.value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = property.GetValue(this.value, null);

                yield return new ObjectPropertyPair(
                    property.Name,
                    this.ConvertIfNecessary(value, property, RequestType.AsDefault));
            }
        }

        /// <summary>
        /// Checks, if the property is set. 
        /// It will just check, that the property exists
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>true, if property exists</returns>
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
            logger.Message("unset should not work on DotNetObject");

            this.set(propertyName, null);
            return true;
        }

        public void delete()
        {
            if (this.sequence != null)
            {
                this.sequence.remove(this);
            }
            else
            {
                throw new InvalidOperationException("Not connected to a sequence");
            }
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
        private object ConvertIfNecessary(object checkObject, PropertyInfo propertyInfo, RequestType requestType)
        {
            var propertyName = propertyInfo.Name;

            if ((
                    ObjectConversion.IsNativeByType(propertyInfo.PropertyType) ||
                    ObjectConversion.IsEnumByType(propertyInfo.PropertyType) ||
                    propertyInfo.PropertyType == typeof(IObject))
                && (
                    requestType == RequestType.AsDefault ||
                    requestType == RequestType.AsSingle))
            {
                // A native type and it is set
                if (checkObject != null)
                {
                    return checkObject;
                }
                else
                {
                    return ObjectHelper.NotSet;
                }
            }
            else if (
                    (checkObject is IList<object>)
                && (
                    requestType != RequestType.AsDefault ||
                    requestType == RequestType.AsReflectiveCollection))
            {
                return new DotNetReflectiveSequence<object>(this.extent, checkObject as IList<object>);
            }
            else
            {
                // It might be a generic list. We also need to support the interface
                // to a generic list. Sad but true
                var listType = ObjectConversion.GetTypeOfListByType(propertyInfo.PropertyType);
                if (listType != null)
                {
                    checkObject = this.CreateListInstanceForPropertyInfo(propertyInfo);

                    // Sets to object 
                    this.set(propertyName, checkObject);

                    var typeReflectiveSequence = typeof(DotNetReflectiveSequence<>).MakeGenericType(listType);
                    var reflectiveSequence = typeReflectiveSequence
                            .GetMethod("CreateFromList")
                            .Invoke(null, new object[] { this.Extent, checkObject })
                        as IListWrapperReflectiveSequence;

                    return reflectiveSequence;
                }
            }

            if (checkObject == null)
            {
                return ObjectHelper.Null;
            }

            // Default type
            return new DotNetObject(
                null,
                checkObject,
                this.id + "/" + propertyName);
        }

        /// <summary>
        /// Creates a list instance for the given property. 
        /// If the property
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        private IList CreateListInstanceForPropertyInfo(PropertyInfo propertyInfo)
        {
            IList newList;
            // First... create instance, it might be that the property is an interface 
            if (propertyInfo.PropertyType.IsInterface)
            {
                // Find an appropriate interface 
                var propertyType = propertyInfo.PropertyType;
                Type interfaceType;
                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(IList<>))
                {
                    interfaceType = propertyType;
                }
                else
                {
                    interfaceType = propertyType.GetInterfaces()
                        .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IList<>))
                        .FirstOrDefault();
                }

                if (interfaceType == null)
                {
                    throw new NotImplementedException("The type is not of type IList<>, the type is of: " + propertyInfo.PropertyType.ToString());
                }

                // Got the interface, now create the associated List 
                var listElementType = interfaceType.GetGenericArguments().First();
                if (listElementType == typeof(IObject))
                {
                    // if the list element type is IObject, return objects, since the DotNetObject 
                    // will convert IObjects to objects 
                    listElementType = typeof(object);
                }

                var genericListType = typeof(List<>).MakeGenericType(listElementType);
                newList = Activator.CreateInstance(genericListType) as IList;
            }
            else
            {
                newList = Activator.CreateInstance(propertyInfo.PropertyType) as IList;
                if (newList == null)
                {
                    throw new NotImplementedException("The type is not of type IList, the type is of: " + propertyInfo.PropertyType.ToString());
                }
            }

            return newList;
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
            if (this.isSet("name") && !(ObjectConversion.IsNull(this.get("name"))))
            {
                return string.Format("\"{0}\" (DotNetObject)",
                    this.getAsSingle("name").ToString());
            }
            else
            {
                return string.Format("Id: {0}", this.id.ToString());
            }
        }

        /// <summary>
        /// Gets the extent type
        /// </summary>
        Type IKnowsExtentType.ExtentType
        {
            get { return typeof(DotNetExtent); }
        }
    }
}
