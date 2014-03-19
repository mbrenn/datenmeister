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
        /// Initializes a new instance of the XmlExtent
        /// </summary>
        public XmlExtent(XDocument document, string uri)
        {
            Ensure.That(document != null);
            Ensure.That(!string.IsNullOrEmpty(uri));

            this.XmlDocument = document;
            this.Uri = uri;
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
            var rootElement = new XmlObject(this, this.XmlDocument.Root);
            yield return rootElement;
        }

        /// <summary>
        /// Creates a new object, won't work completely, due to the required name of the xml node
        /// </summary>
        /// <returns>Exception will be returned</returns>
        public IObject CreateObject()
        {
            // Adds a simple object 
            var newObject = new XElement("element");
            newObject.Add(new XAttribute("id", Guid.NewGuid().ToString()));
            this.XmlDocument.Root.Add(newObject);

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
