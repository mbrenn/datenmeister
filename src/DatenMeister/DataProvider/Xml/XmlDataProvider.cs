using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using DatenMeister.Logic;
using BurnSystems.Test;
using System.IO;
using BurnSystems;

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
        public XmlExtent Load(string path)
        {
            var loadedDocument = XDocument.Load(path);
            var extent = new XmlExtent(loadedDocument, path, new XmlSettings());

            return extent;
        }

        /// <summary>
        /// Loads the extent from a file and sets the corresponding uri
        /// </summary>
        /// <param name="path">Path, where extent will get loaded from</param>
        /// <param name="uri"></param>
        /// <param name="setttings"></param>
        /// <returns></returns>
        public XmlExtent Load(string path, string uri)
        {
            var extent = this.Load(path);
            extent.Uri = uri;

            return extent;
        }

        /// <summary>
        /// Saves the extent into database.
        /// </summary>
        /// <param name="extent">Extent to be stored</param>
        /// <param name="path">Path, where file shall be stored</param>
        /// <param name="settings">Settings being used</param>
        public void Save(XmlExtent extent, string path)
        {
            Ensure.That(extent != null);

            var rootNode = extent.XmlDocument.Root;
            if (rootNode.Attribute(DatenMeister.Entities.AsObject.Uml.Types.XmlNamespace + "xmi")
                == null)
            {
                extent.XmlDocument.Root.Add(
                    new XAttribute(
                        DatenMeister.Entities.AsObject.Uml.Types.XmlNamespace + "xmi",
                        DatenMeister.Entities.AsObject.Uml.Types.XmiNamespace.ToString()));
            }

            // Stores the file into database
            extent.XmlDocument.AddAnnotation(SaveOptions.OmitDuplicateNamespaces);

            // Creates the filename for a temporary path
            var directory = Path.GetDirectoryName(path);
            string randomFilename;
            string totalPath;

            do
            {
                randomFilename = StringManipulation.RandomString(16);
                totalPath = Path.Combine(directory, randomFilename) + ".xml";

                if (File.Exists(totalPath))
                {
                    continue;
                }
            }
            while (false);

            // Stores the file into the temporary path
            extent.XmlDocument.Save(totalPath);

            // And moves it to the final one
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.Move(totalPath, path);
        }
    }
}
