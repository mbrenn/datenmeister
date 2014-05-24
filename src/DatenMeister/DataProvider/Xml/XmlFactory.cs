using BurnSystems.Test;
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
            Ensure.That(extent is XmlExtent, "extent != XmlExtent");
            this.extent = extent;
        }

        public override IObject create(IObject type)
        {
            // Checks, if we have a better element, where new node can be added
            var info = this.extent.Settings.Mapping.FindByType(type);
            var nodeName = "element";
            if (info != null)
            {
                nodeName = info.NodeName;
            }

            // Adds a simple object 
            var newObject = new XElement(nodeName);
            newObject.Add(new XAttribute("id", Guid.NewGuid().ToString()));

            return new XmlObject(this.extent, newObject);
        }
    }
}
