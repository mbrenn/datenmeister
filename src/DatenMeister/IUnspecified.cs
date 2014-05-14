using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister
{
    /// <summary>
    /// This interface needs to be implemented by all objects, that may
    /// be transformed from enumeration to single or vice versa. 
    /// </summary>
    public interface IUnspecified
    {
        /// <summary>
        /// Returns the object as a single object
        /// </summary>
        /// <returns>Object being returned</returns>
        object AsSingle();
    }
}
