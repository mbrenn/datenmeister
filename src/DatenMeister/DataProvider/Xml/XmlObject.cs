using BurnSystems.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DatenMeister.DataProvider.Xml
{
    /// <summary>
    /// Defines one xml object being used by Datenmeister
    /// </summary>
    public class XmlObject : IObject
    {
        /// <summary>
        /// Gets or sets the node
        /// </summary>
        public XElement Node
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the Xml Object
        /// </summary>
        /// <param name="node">Node being used</param>
        /// <param name="parent">The parent node, which is used for id derivation</param>
        public XmlObject(XElement node, XElement parent = null)
        {
            Ensure.That ( node != null);
            this.Node = node;

            // Check, if we have an id attribute
            // TODO: Check with RFC about xml.
            var idAttribute = this.Node.Attribute("id");
            if ( idAttribute == null)
            {
                // We got something with attribute null...
                // At the moment, this is not handled
                throw new InvalidOperationException("Xml node does not have a node");
            }
            else
            {
                this.Id = idAttribute.Value;
            }
        }

        public object Get(string propertyName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ObjectPropertyPair> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool IsSet(string propertyName)
        {
            throw new NotImplementedException();
        }

        public void Set(string propertyName, object value)
        {
            throw new NotImplementedException();
        }

        public void Unset(string propertyName)
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets or sets the id of the given xml object
        /// </summary>
        public string Id
        {
            get;
            set;
        }
    }
}
