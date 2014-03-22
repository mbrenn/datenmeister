using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider.DotNet
{
    /// <summary>
    /// Stores the mapping between DotNet Type
    /// </summary>
    public class DotNetTypeInformation
    {
        /// <summary>
        /// Defines the type, which has been mapped to <c>DotNetType</c>
        /// </summary>
        public IObject Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the .Net object, which shall be mapped to the DatenMeister type given in <c>Type</c>
        /// </summary>
        public Type DotNetType
        {
            get;
            set;
        }
    }
}
