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

        /// <summary>
        /// Returns the object as a reflective collection
        /// </summary>
        /// <returns>The returned collection</returns>
        IReflectiveCollection AsReflectiveCollection();

        /// <summary>
        /// Returns the object as a reflective sequence
        /// </summary>
        /// <returns>The returned sequence</returns>
        IReflectiveSequence AsReflectiveSequence();
    }
}
