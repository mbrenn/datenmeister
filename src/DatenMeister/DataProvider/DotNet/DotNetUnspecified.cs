using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.DotNet
{
    /// <summary>
    /// Implements the unspecified object for the DotNetExtents. 
    /// Will create a list, if used in AsReflectiveSequence
    /// </summary>
    public class DotNetUnspecified : BaseUnspecified
    {
        /// <summary>
        /// Stores the propertyInfo
        /// </summary>
        private PropertyInfo propertyInfo;

        public DotNetUnspecified(IObject owner, PropertyInfo propertyInfo, object value, PropertyValueType propertyValueType)
            : base(owner, propertyInfo.Name, value, propertyValueType)
        {
            this.propertyInfo = propertyInfo;
        }

        public override IReflectiveCollection AsReflectiveCollection()
        {
            return this.AsReflectiveSequence();
        }

        public override IReflectiveSequence AsReflectiveSequence()
        {
            var valueAsReflectiveSequence = this.Value as IReflectiveSequence;
            if (valueAsReflectiveSequence != null)
            {
                return valueAsReflectiveSequence;
            }

            // Check, if value list of object, if yes, add it
            // Just an exception to also support object. It might be necessary that additional types need to be supported
            var valueAsIListObject = this.Value as IList<object>;
            if (valueAsIListObject != null)
            {
                return new DotNetReflectiveSequence<object>(this.Owner.Extent, valueAsIListObject);
            }

            // Check, if value list, if yes, add it
            var valueAsIList = this.Value as IList;
            if (valueAsIList != null)
            {
                return new DotNetNonGenericReflectiveSequence(this.Owner.Extent, valueAsIList);
            }

            // Check, if value is null, if yes, we have to create an instance, which is quite timeconsuming.
            if (this.Value == null)
            {
                IList newList;
                // First... create instance, it might be that the property is an interface
                if (propertyInfo.PropertyType.IsInterface)
                {
                    // Find an appropriate interface
                    var propertyType = this.propertyInfo.PropertyType;
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
                        throw new NotImplementedException("The type is not of type IList<>, the type is of: " + this.propertyInfo.PropertyType.ToString());
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
                }

                // Sets to object
                this.Owner.set(propertyInfo.Name, newList);

                // Return the list, captured in a wrapper sequence
                return new DotNetNonGenericReflectiveSequence(this.Owner.Extent, newList);
            }

            if (this.propertyInfo.PropertyType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IList<>)))
            {
                throw new NotImplementedException(this.Value.ToString() + " cannot be converted as ReflectiveSequence due to missing implementation of IList<>-support. Is of type: " + this.Value.GetType().ToString());
            }

            throw new NotImplementedException(this.Value.ToString() + " cannot be converted as ReflectiveSequence. Is of type: " + this.Value.GetType().ToString());
        }
    }
}
