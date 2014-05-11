using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider
{
    /// <summary>
    /// Defines a generic element, that can be used temporarily. 
    /// </summary>
    public class GenericElement : GenericObject, IElement
    {
        /// <summary>
        /// Gets or sets the type
        /// </summary>
        private IObject type
        {
            get;
            set;
        }

        private IObject containerInstance
        {
            get;
            set;
        }

        public GenericElement(IURIExtent extent = null, string id = null, IObject type = null, IObject container = null)
            : base(extent, id)
        {
            this.type = type;
            this.containerInstance = container;
        }

        public IObject getMetaClass()
        {
            return this.type;
        }

        public IObject container()
        {
            return this.containerInstance;
        }
    }
}
