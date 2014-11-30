using BurnSystems.Logging;
using BurnSystems.Test;
using DatenMeister.Pool;
using Ninject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DatenMeister.DataProvider.Xml
{
    /// <summary>
    /// Retrieves the MOF-objects out of an Xml file
    /// </summary>
    public class XmlExtent : IURIExtent
    {
        /// <summary>
        /// Stores the logger
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(XmlExtent));

        /// <summary>
        /// Stores the xml-Extent-Settings
        /// </summary>
        private XmlSettings xmlSettings;

        /// <summary>
        /// Gets or sets the pool, where the object is stored
        /// </summary>
        public IPool Pool
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the xml document 
        /// </summary>
        public XDocument XmlDocument
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the uri
        /// </summary>
        public string Uri
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the settings for the Xml file
        /// </summary>
        public XmlSettings Settings
        {
            get { return this.xmlSettings; }
            set
            {
                Ensure.That(value != null, "Value cannot be null");
                this.xmlSettings = value;
            }
        }

        /// <summary>
        /// Gets or sets a flag whether the extent is currently dirty
        /// That means, it has unsaved changes
        /// </summary>
        public bool IsDirty
        {
            get;
            set;
        }

        private IKernel injection = new StandardKernel();
                
        /// <summary>
        /// Gets or sets the kernel for the injections
        /// </summary>
        public IKernel Injection
        {
            get { return this.injection; }
        }

        /// <summary>
        /// Initializes a new instance of the XmlExtent
        /// </summary>
        public XmlExtent(XDocument document, string uri, XmlSettings settings = null)
        {
            Ensure.That(document != null);
            Ensure.That(!string.IsNullOrEmpty(uri));

            this.XmlDocument = document;
            this.Uri = uri;            

            if (settings == null)
            {
                this.Settings = new XmlSettings();
            }
            else
            {
                this.Settings = settings;
            }
        }

        /// <summary>
        /// Gets the context uri
        /// </summary>
        /// <returns></returns>
        public string ContextURI()
        {
            return this.Uri;
        }

        /// <summary>
        /// Gets all elements
        /// </summary>
        /// <returns>Enumeration of all elements</returns>
        public IReflectiveSequence Elements()
        {
            if (this.Settings.UseRootNode)
            {
                return new XmlExtentRootReflectiveSequence(this);
            }
            else
            {
                return new XmlExtentSubnodeReflectiveSequence(this);
            }
        }

        /// <summary>
        /// Creates a new Xml Extent by specifying the root nodes
        /// </summary>
        /// <param name="settings">Settings to be used</param>
        /// <param name="rootNodeName">The root node name</param>
        /// <param name="uri">URI being associated to the extent</param>
        /// <returns>The created Xml Extent</returns>
        public static XmlExtent Create(XmlSettings settings, string rootNodeName, string uri = null)
        {
            Ensure.That(!string.IsNullOrEmpty(rootNodeName));

            return new XmlExtent(new XDocument(new XElement(rootNodeName)), uri, settings);
        }
    }
}
