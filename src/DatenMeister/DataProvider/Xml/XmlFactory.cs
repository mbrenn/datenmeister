using BurnSystems.Test;
using DatenMeister.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider.Xml
{
    /// <summary>
    /// Used to implement the xmlfactory function for the xml file
    /// </summary>
    public class XmlFactory : Factory
    {
        public XmlFactory(IURIExtent extent)
            : base(extent)
        {
            Ensure.That(extent is XmlExtent, "extent != XmlExtent");
        }
    }
}
