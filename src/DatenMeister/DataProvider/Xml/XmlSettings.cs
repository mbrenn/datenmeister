using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.Xml
{
    /// <summary>
    /// Defines the settings for reading, manipulating and storing the xml file. 
    /// </summary>
    public class XmlSettings
    {
        /// <summary>
        /// Stores the type mapping between 
        /// </summary>
        public XmlTypeMapping Mapping
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the skips the root node by the enumeration of elements
        /// </summary>
        public bool SkipRootNode
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the XmlSettings
        /// </summary>
        public XmlSettings()
        {
            this.Mapping = new XmlTypeMapping();
        }
    }
}
