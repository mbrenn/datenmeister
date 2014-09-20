using DatenMeister.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.DotNet
{
    public class DotNetReflectiveSequence<T> : ListWrapperReflectiveSequence<T>, IKnowsExtentType
    {
        public DotNetReflectiveSequence(IURIExtent extent, IList<T> list)
            : base(extent, list)
        {
        }

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

        public override object ConvertInternalToInstance(T item)
        {
            if (Extensions.IsNative(item))
            {
                return item;
            }

            if (item is DotNetObject)
            {
                return item;
            }

            return new DotNetObject(this.ExtentAsDotNetExtent, item);
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
