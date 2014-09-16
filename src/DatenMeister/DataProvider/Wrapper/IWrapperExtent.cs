using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.Wrapper
{
    public interface IWrapperExtent : IURIExtent
    {
        /// <summary>
        /// Converts the given object to the most appropriate instance. 
        /// IObject, IReflectiveSequence and IUnspecified are wrapped
        /// </summary>
        /// <param name="value">Value to be used</param>
        /// <returns>Converted object</returns>
        object Convert(object value);

        /// <summary>
        /// Dies the unwrap
        /// </summary>
        /// <returns>Unwrapped extent</returns>
        IURIExtent Unwrap();
    }
}
