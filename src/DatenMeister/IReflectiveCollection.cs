using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister
{
    public interface IReflectiveCollection : ICollection<object>
    {
        /// <summary>
        /// Gets the extent
        /// </summary>
        IURIExtent Extent
        {
            get;
        }

        bool add(object value);

        bool addAll(IReflectiveSequence value);

        void clear();

        /// <summary>
        /// Return type is bool. 
        /// MOF specification states that it is object, but according to XMI file, this is bool
        /// </summary>
        /// <param name="value">Value to be removed</param>
        /// <returns>true, if value has been removed</returns>
        bool remove(object value);

        int size();
    }
}
