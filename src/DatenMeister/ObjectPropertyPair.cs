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
}
