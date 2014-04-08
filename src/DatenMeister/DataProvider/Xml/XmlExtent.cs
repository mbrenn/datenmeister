using BurnSystems.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DatenMeister.DataProvider.Xml
{
    public class XmlExtent: IURIExtent
    {
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
        public IEnumerable<IObject> Elements()
        {
            if (!this.Settings.SkipRootNode)
            {
                var rootElement = new XmlObject(this, this.XmlDocument.Root);
                yield return rootElement;
            }

            // Goes through the mapping table to find additional objects
            foreach (var mapInfo in this.Settings.Mapping.GetAll())
            {
                foreach (var xmlSubnode in mapInfo.RetrieveRootNode(this.XmlDocument).Elements())
                {
                    var result = new XmlObject(this, xmlSubnode, null);
                    yield return result;
                }
            }
        }

        /// <summary>
        /// Creates a new object, won't work completely, due to the required name of the xml node
        /// </summary>
        /// <returns>Exception will be returned</returns>
        public IObject CreateObject(IObject type)
        {
            var parentElement = this.XmlDocument.Root;

            // Checks, if we have a better element, where new node can be added
            var info = this.Settings.Mapping.FindByType(type);
            var nodeName = "element";
            if (info != null)
            {
                parentElement = info.RetrieveRootNode(this.XmlDocument);
                nodeName = info.NodeName;
            }

            // Adds a simple object 
            var newObject = new XElement(nodeName);
            newObject.Add(new XAttribute("id", Guid.NewGuid().ToString()));
            parentElement.Add(newObject);

            return new XmlObject(this, newObject);
        }

        /// <summary>
        /// Removes object from xml document
        /// </summary>
        /// <param name="element">Element to be removed</param>
        public void RemoveObject(IObject element)
        {
            throw new NotImplementedException();
        }
    }
}
