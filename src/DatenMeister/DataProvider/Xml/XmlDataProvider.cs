﻿using System;
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

            // Stores the file into database
            extent.XmlDocument.AddAnnotation(SaveOptions.OmitDuplicateNamespaces);
            extent.XmlDocument.Save(path);
        }
    }
}
