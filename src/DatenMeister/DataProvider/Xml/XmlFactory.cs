using BurnSystems.Test;
using DatenMeister.Entities.AsObject.Uml;
using DatenMeister.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DatenMeister.DataProvider.Xml
{
    /// <summary>
    /// Used to implement the xmlfactory function for the xml file
    /// </summary>
    public class XmlFactory : Factory
    {
        private XmlExtent extent;

        public XmlFactory(XmlExtent extent)
        {
            this.extent = extent;
        }

        public override IObject create(IObject type)
        {
            var nodeName = "element";

            // Checks, if we have a better element, where new node can be added
            if (this.extent != null)
            {
                var info = this.extent.Settings.Mapping.FindByType(type);
                if (info != null)
                {
                    nodeName = info.NodeName;
                }
            }

            // Adds a simple object 
            var newNode = new XElement(nodeName);
            newNode.Add(new XAttribute("id", Guid.NewGuid().ToString()));

            // Check, if the given value is as an element, if yes, add the xmi:type
            if (type != null)
            {
                var name = NamedElement.getName(type);
                newNode.Add(new XAttribute(DatenMeister.Entities.AsObject.Uml.Types.XmiNamespace + "type", name));
            }

            return new XmlObject(this.extent, newNode);
        }
    }
}
