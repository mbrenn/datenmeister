using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace BurnSystems.FlexBG.Modules.ConfigurationStorageM
{
    /// <summary>
    /// Defines the storage containing the configuration files
    /// </summary>
    public interface IConfigurationStorage
    {
        /// <summary>
        /// Adds a document to configuration storage
        /// </summary>
        /// <param name="document"></param>
        void Add(XDocument document);

        /// <summary>
        /// Gets the configuration documents
        /// </summary>
        IEnumerable<XDocument> Documents
        {
            get;
        }
    }
}
