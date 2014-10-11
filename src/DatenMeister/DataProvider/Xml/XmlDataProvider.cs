using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using DatenMeister.Logic;
using BurnSystems.Test;

namespace DatenMeister.DataProvider.Xml
{
    /// <summary>
    /// Being used to import and export the xml file to drive
    /// </summary>
    public class XmlDataProvider : IDataProvider
    {
        /// <summary>
        /// Loads the extent from a file and does not set the corresponding uri
        /// </summary>
        /// <param name="path">Path, where extent will get loaded from</param>
        /// <param name="uri">The URI which shall be associated</param>
        /// <param name="setttings">Settings being used to load the data</param>
        /// <returns></returns>
        public XmlExtent Load(string path, XmlSettings settings)
        {
            var loadedDocument = XDocument.Load(path);
            var extent = new XmlExtent(loadedDocument, path);
            extent.Settings = settings;

            return extent;
        }

        /// <summary>
        /// Loads the extent from a file and sets the corresponding uri
        /// </summary>
        /// <param name="path">Path, where extent will get loaded from</param>
        /// <param name="uri"></param>
        /// <param name="setttings"></param>
        /// <returns></returns>
        public XmlExtent Load(string path, string uri, XmlSettings settings)
        {
            var extent = this.Load(path, settings);
            extent.Uri = uri;

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
            Ensure.That(extent != null);

            // Stores the file into database
            extent.XmlDocument.AddAnnotation(SaveOptions.OmitDuplicateNamespaces);
            extent.XmlDocument.Save(path);
        }

        /// <summary>
        /// Creates an empty xmlextent and stores it at the given path
        /// </summary>
        /// <param name='path'>Path, where extent will be stored. 
        /// The path should also include filename and extension
        /// </param>
        /// <param name='url'>
        /// The url under which the xmlextent will be found
        /// </param>
        /// <param name='name'>
        /// The name of the extent. This name will be used for the name of the root node
        /// </paramm
        public ExtentInstance CreateEmpty(string path, string url, string name, ExtentType extentType)
        {
            // Create the node
            var document = new XDocument();
            document.Add(new XElement(name));

            // Store to file
            document.Save(path);

            // Add the extent			
            var extent = new XmlExtent(document, url);

            return new ExtentInstance(extent, path, name, extentType);
        }
    }
}
