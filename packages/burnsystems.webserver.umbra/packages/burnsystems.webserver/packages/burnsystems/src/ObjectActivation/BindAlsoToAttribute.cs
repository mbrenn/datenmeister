using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.ObjectActivation
{
    /// <summary>
    /// This attribute is used to add a binding to a certain class, if 
    /// the class is bound to certain container
    /// </summary>
    public class BindAlsoToAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the type
        /// </summary>
        public Type Type
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the BindAlsoToAttribute class
        /// </summary>
        /// <param name="type">Type to be added</param>
        public BindAlsoToAttribute(Type type)
        {
            this.Type = type;
        }
    }
}
