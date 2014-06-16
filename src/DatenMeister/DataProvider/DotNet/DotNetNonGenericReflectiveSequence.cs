using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.DotNet
{
    public class DotNetNonGenericReflectiveSequence : ListNonGenericWrapperReflectiveSequence
    {
        public DotNetNonGenericReflectiveSequence(IURIExtent extent, IList list)
            : base(extent, list)
        {
        }

        public override object ConvertInstanceTo(object value)
        {
            // If the given object is already a DotNetObject, we do not encapsulate the instance
            var valueDotNetObject = value as DotNetObject;
            if (valueDotNetObject != null)
            {
                return valueDotNetObject.Value;
            }

            return base.ConvertInstanceTo(value);
        }
    }
}
