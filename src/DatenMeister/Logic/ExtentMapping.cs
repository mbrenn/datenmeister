using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic
{
    /// <summary>
    /// Maps the extent information and the extent itself together
    /// </summary>
    public class ExtentMapping
    {
        /// <summary>
        /// Gets or sets the information for the extent
        /// </summary>
        public ExtentInfoForPool ExtentInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the associated extent
        /// </summary>
        public IURIExtent Extent
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the ExtentMapping
        /// </summary>
        /// <param name="extentInfo">Information of the extent</param>
        /// <param name="extent">Extent being associated</param>
        public ExtentMapping(ExtentInfoForPool extentInfo, IURIExtent extent)
        {
            this.ExtentInfo = extentInfo;
            this.Extent = extent;
        }
    }
}
