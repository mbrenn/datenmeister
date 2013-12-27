using BurnSystems.ObjectActivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Umbra.Views.DetailView.Entities
{
    /// <summary>
    /// Defines one row in the entity view
    /// </summary>
    public abstract class EntityViewElement
    {
        /// <summary>
        /// Gets the name of the view element
        /// </summary>
        public abstract string Name
        {
            get;
        }

        /// <summary>
        /// Converts element to json
        /// </summary>
        /// <returns>Converts element to json</returns>
        public abstract object ToJson(IActivates container);

        /// <summary>
        /// Converts the object to a json element
        /// </summary>
        /// <param name="item">Value to be converted</param>
        /// <returns>Converted object that will be sent to browser</returns>
        public abstract object ObjectToJson(object item);

        /// <summary>
        /// Sets the property of the item
        /// </summary>
        /// <param name="item">Item to be set</param>
        /// <param name="value">Value of the item</param>
        public abstract void SetValue(object item, string value);
    }
}
