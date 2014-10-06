using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Entities.FieldInfos
{
    public class MultiReferenceField : General
    {
        public MultiReferenceField(string name, string binding, string referenceUrl = null, string propertyValue = null)
            : base(name, binding)
        {
            this.referenceUrl = referenceUrl;
            this.propertyValue = propertyValue;
        }

        /// <summary>
        /// Stores the property value, which will be shown in the GUI
        /// </summary>
        public string propertyValue
        {
            get;
            set;
        }

        /// <summary>
        /// Defines the reference url being used to retrieve the items
        /// </summary>
        public string referenceUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the view definition for the table view. 
        /// </summary>
        public IObject tableViewInfo
        {
            get;
            set;
        }
    }
}
