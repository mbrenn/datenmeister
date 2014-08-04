using BurnSystems.Logging;
using BurnSystems.Test;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DatenMeister.DataProvider.Xml
{
    public class XmlExtent : IURIExtent
    {
        /// <summary>
        /// Stores the logger
        /// </summary>
        private static ILog logger = new ClassLogger(typeof(XmlExtent));

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
            get;
            set;
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

        /// <summary>
        /// Initializes a new instance of the XmlExtent
        /// </summary>
        public XmlExtent(XDocument document, string uri, XmlSettings settings = null)
        {
            Ensure.That(document != null);
            Ensure.That(!string.IsNullOrEmpty(uri));

            this.XmlDocument = document;
            this.Uri = uri;
            this.Settings = settings;

            if (this.Settings == null)
            {
                this.Settings = new XmlSettings();
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
            return new XmlExtentReflectiveSequence(this);
        }

        public static XmlExtent Create(XmlSettings settings, string rootNodeName, string uri = null)
        {
            Ensure.That(!string.IsNullOrEmpty(rootNodeName));

            return new XmlExtent(new XDocument(new XElement(rootNodeName)), uri, settings);
        }

        private class XmlExtentReflectiveSequence : BaseReflectiveSequence
        {
            private XmlExtent extent;

            public XmlExtentReflectiveSequence(XmlExtent extent)
                : base(extent)
            {
                Ensure.That(extent != null);
                this.extent = extent;
            }

            public override void add(int index, object value)
            {
                logger.LogEntry(new LogEntry("add(int, object) is not fully supported. Will be added to last position", LogLevel.Message));

                this.add(value);
            }

            public override object get(int index)
            {
                return this.getAll().ElementAt(index);
            }

            public override object remove(int index)
            {
                throw new NotImplementedException();
            }

            public override object set(int index, object value)
            {
                throw new NotImplementedException();
            }

            public override bool add(object value)
            {
                if (value is XmlObject)
                {
                    var valueAsXmlObject = value as XmlObject;
                    valueAsXmlObject.ContainerExtent = this.extent;
                    var parentElement = this.extent.XmlDocument.Root;

                    // Checks, if we have a better element, where new node can be added
                    var info = this.extent.Settings.Mapping.FindByType(valueAsXmlObject.getMetaClass());
                    if (info != null)
                    {
                        parentElement = info.RetrieveRootNode(this.extent.XmlDocument);
                    }

                    // Adds a simple object 
                    parentElement.Add(valueAsXmlObject.Node);

                    this.extent.IsDirty = true;

                    return true;
                }

                if (value == null)
                {
                    Debug.WriteLine("Nothing (null) has been added");
                    return false;
                }

                throw new InvalidOperationException("Only objects as IObject may be added");
            }

            public override void clear()
            {
                throw new NotImplementedException();
            }

            public override bool remove(object value)
            {
                Ensure.That(value != null);
                var valueAsObject = value as XmlObject;
                Ensure.That(valueAsObject != null, "Value is not XmlObject");
                
                valueAsObject.delete();

                return true;
            }

            public override int size()
            {
                return this.getAll().Count();
            }

            public override IEnumerable<object> getAll()
            {
                lock (this.extent.XmlDocument)
                {
                    var foundItems = new List<XElement>();

                    // Goes through the mapping table to find additional objects
                    foreach (var mapInfo in this.extent.Settings.Mapping.GetAll())
                    {
                        var rootNode = mapInfo.RetrieveRootNode(this.extent.XmlDocument);
                        if (rootNode != null)
                        {
                            foundItems.Add(rootNode);
                            foreach (var xmlSubnode in rootNode.Elements())
                            {
                                var result = new XmlObject(this.extent, xmlSubnode, null)
                                {
                                    ContainerExtent = this.extent
                                };

                                yield return result;
                            }
                        }
                    }

                    if (!this.extent.Settings.SkipRootNode)
                    {
                        foreach (var subNode in this.extent.XmlDocument.Root.Elements())
                        {
                            // Only for the items, that do not have a direct mapping via settings, the elements will be returned
                            if (!foundItems.Contains(subNode))
                            {
                                var subObject = new XmlObject(this.extent, subNode)
                                {
                                    ContainerExtent = this.extent
                                };

                                yield return subObject;
                            }
                        }
                    }
                }
            }
        }
    }
}
