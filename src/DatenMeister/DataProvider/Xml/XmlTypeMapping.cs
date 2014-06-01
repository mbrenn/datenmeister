using DatenMeister.Entities.AsObject.Uml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DatenMeister.DataProvider.Xml
{
    /// <summary>
    /// Stores the mapping between the types and 
    /// </summary>
    public class XmlTypeMapping
    {
        /// <summary>
        /// Stores the information
        /// </summary>
        private List<XmlTypeInformation> information = new List<XmlTypeInformation>();

        /// <summary>
        /// Returns all type information
        /// </summary>
        /// <returns></returns>
        public List<XmlTypeInformation> GetAll()
        {
            return this.information;
        }

        /// <summary>
        /// Adds a type to the extent
        /// </summary>
        /// <param name="type">Type to be added</param>
        public void Add(IObject type)
        {
            var name = NamedElement.getName(type);
            var pluralName = name + "s";

            var retrieveNode = new System.Func<XDocument, XElement>(
                xmlDocument =>
                {
                    var pluralElement = xmlDocument.Root
                       .Element(pluralName);

                    if (pluralElement == null)
                    {
                        pluralElement = new XElement(name);
                        xmlDocument.Root.Add(pluralElement);
                    }

                    return pluralElement;
                });


            this.Add(name, type, retrieveNode);
        }

        /// <summary>
        /// Adds a mapping between the node name and the type
        /// </summary>
        /// <param name="nodeName">Name of the Xml-Element to be used</param>
        /// <param name="type">Type to be used</param>
        /// <param name="retrieveRootNode">Function which retrieves the root node for a certain type</param>
        public void Add(string nodeName, IObject type, Func<XDocument, XElement> retrieveRootNode)
        {
            var info = new XmlTypeInformation()
            {
                NodeName = nodeName,
                Type = type,
                RetrieveRootNode = retrieveRootNode
            };

            this.information.Add(info);
        }

        /// <summary>
        /// Finds by the nodename
        /// </summary>
        /// <param name="nodeName">Name of the node</param>
        public XmlTypeInformation FindByNodeName(string nodeName)
        {
            return this.information.Where(x => x.NodeName == nodeName).FirstOrDefault();
        }

        /// <summary>
        /// Finds the node by type
        /// </summary>
        /// <param name="type"></param>
        public XmlTypeInformation FindByType(IObject type)
        {
            return this.information.Where(x => x.Type == type).FirstOrDefault();
        }
    }
}
