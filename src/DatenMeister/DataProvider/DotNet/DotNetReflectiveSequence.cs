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

        public override T ConvertInstanceTo(object value)
        {
            // If the given object is already a DotNetObject, we do not encapsulate the instance
            var valueDotNetObject = value as DotNetObject;
            if (valueDotNetObject != null)
            {
                return (T) valueDotNetObject.Value;
            }

            return base.ConvertInstanceTo(value);
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
