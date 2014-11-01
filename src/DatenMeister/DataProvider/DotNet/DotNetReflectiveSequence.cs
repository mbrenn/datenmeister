using DatenMeister.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.DotNet
{
    /// <summary>
    /// Encapsulates a list and provides it as a .NetSequence
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DotNetReflectiveSequence<T> : ListWrapperReflectiveSequence<T>, IKnowsExtentType
    {
        /// <summary>
        /// Initializes a new instance of DotNetReflectiveSequence
        /// </summary>
        /// <param name="extent">Extent to be used</param>
        /// <param name="list">List of elements being used</param>
        public DotNetReflectiveSequence(IURIExtent extent, IList<T> list)
            : base(extent, list)
        {
        }

        /// <summary>
        /// This method is called, when a conversion from the element to be added to the store format 
        /// is necessary. Per default, an explicit time conversion will be used. 
        /// This might also contain more complex function.
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <returns>The converted value</returns>
        public override T ConvertInstanceToInternal(object value)
        {
            // If the given object is already a DotNetObject, we do not encapsulate the instance
            var valueDotNetObject = value as DotNetObject;
            if (valueDotNetObject != null)
            {
                return (T) valueDotNetObject.Value;
            }

            var valueAsIObject = value as IObject;
            if (valueAsIObject != null)
            {
                // Ok, the object is not a DotNetObject, but another IObject, which needs
                // to be converted. 

                // Get the factory
                var factory = new DotNetFactory(this.ExtentAsDotNetExtent);

                // Get the DM-Type for the given list
                var objectType = this.ExtentAsDotNetExtent.Mapping.FindByDotNetType(typeof(T)).Type;

                // We have created the object
                var newValueAsIObject = factory.create(objectType);
                
                // Copy all the stuff from the IObject to my local object
                var copier = new ObjectCopier(this.Extent);

                // And return the result
                var returnObject = copier.CopyElement(valueAsIObject) as DotNetObject;
                return (T)returnObject.Value;
            }

            return base.ConvertInstanceToInternal(value);
        }

        /// <summary>
        /// This method is called, when an internal object is sent out to a caller. 
        /// Per default, the list content is sent out without any modification
        /// </summary>
        /// <param name="item">Item to be sent out</param>
        /// <returns>The sent out item</returns>
        public override object ConvertInternalToInstance(T item)
        {
            if (ObjectConversion.IsNative(item))
            {
                return item;
            }

            if (item is DotNetObject)
            {
                return item;
            }

            return new DotNetObject(this, item);
        }

        public DotNetExtent ExtentAsDotNetExtent
        {
            get { return (DotNetExtent)this.Extent; }
        }

        /// <summary>
        /// Knows the extent type
        /// </summary>
        public Type ExtentType
        {
            get { return typeof(DotNetExtent); }
        }
    }
}
