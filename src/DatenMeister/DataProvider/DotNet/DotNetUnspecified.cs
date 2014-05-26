using System;
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

        public DotNetUnspecified(IObject owner, PropertyInfo propertyInfo, object value)
            : base(owner, propertyInfo.Name, value)
        {
            this.propertyInfo = propertyInfo;
        }

        public override IReflectiveCollection AsReflectiveCollection()
        {
            return this.AsReflectiveSequence();
        }

        public override IReflectiveSequence AsReflectiveSequence()
        {
            throw new NotImplementedException();
        }
    }
}
