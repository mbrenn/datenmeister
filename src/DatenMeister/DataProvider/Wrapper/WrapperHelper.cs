using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.Wrapper
{
    /// <summary>
    /// Contains some helper methods
    /// </summary>
    public static class WrapperHelper
    {
        /// <summary>
        /// Checks, if the given item matches the type X. 
        /// If not, unwrap the extent if possible and redo the check
        /// </summary>
        /// <typeparam name="X">Extent, which needs to be found</typeparam>
        /// <param name="extent">Extent to be unwrapped</param>
        /// <returns>The found extent or null, if not found</returns>
        public static X FindWrappedExtent<X>(IURIExtent extent) where X : class, IWrapperExtent
        {
            while (extent != null)
            {
                var found = extent as X;
                if (found != null)
                {
                    return found;
                }

                var wrapped = extent as IWrapperExtent;
                if (wrapped != null)
                {
                    extent = wrapped.Unwrap();
                }
                else
                {
                    // Nothing found, we are out
                    extent = null;
                }
            }

            return null;
        }
    }
}
