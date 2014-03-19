using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DatenMeister.DataProvider.Xml
{
    /// <summary>
    /// Stores the mapping information for one type
    /// </summary>
    public class XmlTypeInformation
    {
        /// <summary>
        /// Defines the name of the node
        /// </summary>
        public string NodeName
        {
            get;
            set;
        }

        /// <summary>
        /// Defines the object type being associated to the node
        /// </summary>
        public IObject Type
        {
            get;
            set;
        }

        /// <summary>
        /// Defines the elem
        /// </summary>
        public XElement RootNode
        {
            get;
            set;
        }
    }
}
