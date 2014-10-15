using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Entities.UML
{
    /// <summary>
    /// Defines the class for the object. 
    /// As defined in Uml Infrastructure definition v2.4.1 - Clause 10.2.1
    /// </summary>
    public class Class : Type
    {
        /// <summary>
        /// Gets or sets the value whether the class is abstract
        /// </summary>
        public bool isAbstract
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the owned attribute by this class
        /// </summary>
        public IList<IObject> ownedAttribute
        {
            get;
            set;
        }
    }
}
