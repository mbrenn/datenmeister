using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister
{
    /// <summary>
    /// Defines whether the returned property shall be seen as a 
    /// single element or an enumeration
    /// </summary>
    public enum PropertyValueType
    {
        /// <summary>
        /// The property is returned as a single.
        /// </summary>
        Single,

        /// <summary>
        /// The property is returned as en enumeration
        /// </summary>
        Enumeration
    }

    /// <summary>
    /// This interface needs to be implemented by all objects, that may
    /// be transformed from enumeration to single or vice versa. 
    /// </summary>
    [Obsolete]
    public interface IUnspecified
    {
        /// <summary>
        /// Gets or sets the type of the property value
        /// </summary>
        PropertyValueType PropertyValueType
        {
            get;
            set;
        }

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
