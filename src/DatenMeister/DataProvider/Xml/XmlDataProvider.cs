using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DatenMeister.DataProvider.Xml
{
    /// <summary>
    /// Being used to import and export the xml file to drive
    /// </summary>
    public class XmlDataProvider : IDataProvider
    {
        public XmlExtent Load(string path, XmlSettings settings)
        {
            var loadedDocument = XDocument.Load(path);
            var extent = new XmlExtent(loadedDocument, path);

            return extent;
        }

        /// <summary>
        /// Saves the extent into database.
        /// </summary>
        /// <param name="extent">Extent to be stored</param>
        /// <param name="path">Path, where file shall be stored</param>
        /// <param name="settings">Settings being used</param>
        public void Save(XmlExtent extent, string path, XmlSettings settings)
        {
            // Stores the file into database
            extent.XmlDocument.Save(path);
        }
    }
}
