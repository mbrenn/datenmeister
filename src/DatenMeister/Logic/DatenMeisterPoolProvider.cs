using System;
using System.Linq;
using DatenMeister.DataProvider.Xml;
using System.Xml.Linq;
using BurnSystems.Logging;
using System.IO;

namespace DatenMeister.Logic
{
    /// <summary>
    /// This provider is responsible to store and to retrieve 
    /// extents by storing the extent into file. 
    /// 
    /// An XmlFile will be storing the extent information
    /// </summary>
    public class DatenMeisterPoolProvider
    {
        private static ILog logger = new ClassLogger(typeof(DatenMeisterPoolProvider));

        public DatenMeisterPoolProvider()
        {
        }

        /// <summary>
        /// Loads the specified pool from d path.
        /// </summary>
        /// <param name='pool'>
        /// Pool to be loaded
        /// </param>
        /// <param name='path'>
        /// Path where pool is stored
        /// </param>
        public void Load(DatenMeisterPool pool, string path)
        {
            if (!File.Exists(path))
            {
                logger.Message("No pool file found: " + path);
                return;
            }

            var document = XDocument.Load(path);

            foreach (var xmlElement in document.Elements("extents").Elements("extent"))
            {
                var type = xmlElement.Attribute("type").Value;
                if (type != "xml")
                {

                    // Only Xml-Extents are supported yet
                    logger.Fail("Extent to be loaded is not of type 'xml'");
                    continue;
                }
                var uri = xmlElement.Attribute("uri").Value;

                // Loads the name
                var xmlName = xmlElement.Attribute("name");
                var extentName = string.Empty;
                if (xmlName != null)
                {
                    extentName = xmlName.Value;
                }

                // Loads the path
                var xmlPath = xmlElement.Attribute("path");

                if (xmlPath == null)
                {
                    // No path, cannot be loaded
                    logger.Fail("No path for extent: " + uri);
                    continue;
                }

                var extentPath = xmlPath.Value;

                var dataProvider = new XmlDataProvider();
                var extent = dataProvider.Load(extentPath, uri, null);

                // Store the new provider
                pool.Add(extent, extentPath, extentName);
            }
        }

        /// <summary>
        /// Saves the specified pool and path.
        /// </summary>
        /// <param name='pool'>
        /// Pool to be stored
        /// </param>
        /// <param name='path'>
        /// Path, where pool information will be stored
        /// </param>
        public void Save(DatenMeisterPool pool, string path)
        {
            var document = new XDocument();
            var element = new XElement("extents");
            document.Add(element);
            // First of all, create the pool information
            var instances = pool.Instances.ToList();
            foreach (var instance in instances)
            {
                // Only Xml Extents will be supported at all
                var xmlExtent = instance.Extent as XmlExtent;
                if (xmlExtent != null)
                {

                    var extentElement = new XElement("extent");
                    extentElement.Add(new XAttribute("type", "xml"));
                    extentElement.Add(new XAttribute("uri", instance.Extent.ContextURI()));
                    extentElement.Add(new XAttribute("name", instance.Name));

                    if (!string.IsNullOrEmpty(instance.StoragePath))
                    {
                        extentElement.Add(new XAttribute("path", instance.StoragePath));
                    }

                    element.Add(extentElement);
                }
                else
                {

                    logger.Message("Extent to be stored is not of type 'xml': " + instance.Extent.ContextURI());
                }
            }

            document.Save(path);
        }
    }
}