using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Entities.FieldInfos
{
    /// <summary>
    /// Defines that the user gets a dropdown. The content is filled by 
    /// </summary>
    public class ReferenceByConstant : General
    {
        /// <summary>
        /// Initializes a new instance of the ReferenceByConstant class
        /// </summary>
        public ReferenceByConstant()
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the ReferenceByConstant class
        /// </summary>
        public ReferenceByConstant(string name, string binding)
            : base(name, binding)
        {
        }

        /// <summary>
        /// Gets or sets the list of values that shall be shown
        /// </summary>
        public IList<object> values
        {
            get;
            set;
        }
    }
}
