using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister
{
    /// <summary>
    /// Defines the enumeration of possible request type which can be used to retrieve an object
    /// </summary>
    public enum RequestType
    {
        /// <summary>
        /// Uses the default method as defined by the type definition.
        /// If the type is not known, an exception will be thrown
        /// </summary>
        AsDefault,

        /// <summary>
        /// Returns the object as a single object. If multiple elements were found, 
        /// only the first element will be evaluated
        /// </summary>
        AsSingle,

        /// <summary>
        /// Returns the object as a reflective collection. If only one element is found,
        /// a nearly empty reflectivecollection will be created, if supported
        /// </summary>
        AsReflectiveCollection
    }

    /// <summary>
    /// Defines the interface for every object that has properties
    /// </summary>
    public interface IObject
    {
        /// <summary>
        /// Gets the property by propertyname. 
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>Retrieved object</returns>
        object get(string propertyName, RequestType requestType = RequestType.AsDefault);

        /// <summary>
        /// Gets all properties as key value pairs. 
        /// If it is unknown whether a single or multiple object will be returned, 
        /// a single object is returned
        /// </summary>
        /// <returns></returns>
        IEnumerable<ObjectPropertyPair> getAll();

        /// <summary>
        /// Checks, if a certain property is set
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>true, if set</returns>
        bool isSet(string propertyName);

        /// <summary>
        /// Sets the value of the property 
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">Value to be set</param>
        void set(string propertyName, object value);

        /// <summary>
        /// Unsets the property
        /// </summary>
        /// <param name="propertyName">Name of the property to be removed</param>
        bool unset(string propertyName);

        /// <summary>
        /// Deletes this object and all composed elements
        /// </summary>
        void delete();

        /// <summary>
        /// Gets the id of the object
        /// </summary>
        string Id
        {
            get;
        }

        IURIExtent Extent
        {
            get;
        }
    }
}
