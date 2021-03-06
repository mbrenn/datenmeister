﻿using BurnSystems.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister
{
    /// <summary>
    /// Stores the object property pair, which associates a property to a value
    /// </summary>
    public class ObjectPropertyPair
    {
        /// <summary>
        /// Gets or sets the name of the property
        /// </summary>
        public string PropertyName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public object Value
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the ObjectPropertyPair
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">Value of the property</param>
        public ObjectPropertyPair(string propertyName, object value)
        {
            this.PropertyName = propertyName;
            this.Value = value;
       }

        public override string ToString()
        {
            if (this.Value == null)
            {
                return this.PropertyName + ": null";
            }
            else
            {
                return string.Format(
                    "{0}: {1}",
                    this.PropertyName,
                    this.Value.ToString());
            }
        }
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
        object get(string propertyName);

        /// <summary>
        /// Gets all properties as key value pairs
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
