using DatenMeister.DataProvider.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DatenMeister
{
    public abstract class BaseDatenMeisterSeetings : IDatenMeisterSettings
    {
        /// <summary>
        /// Gets or sets the datameister pool
        /// </summary>
        public DatenMeisterPool Pool
        {
            get;
            set;
        }

        public IURIExtent ProjectExtent
        {
            get;
            set;
        }

        public XmlSettings ExtentSettings
        {
            get;
            set;
        }

        public abstract XDocument CreateEmpty();
    }
}
