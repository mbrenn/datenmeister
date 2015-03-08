using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.Generic
{
    /// <summary>
    /// Defines the factory 
    /// </summary>
    public class GenericFactory : IFactory
    {
        public IURIExtent Extent
        {
            get;
            set;
        }

        public GenericFactory(IURIExtent extent)
        {
            this.Extent = extent;
        }

        /// <summary>
        /// Creates the object
        /// </summary>
        /// <param name="type">Type of the object to be reated</param>
        /// <returns>Object being created</returns>
        public IObject create(IObject type)
        {
            var obj = new GenericElement(extent: this.Extent, type: type);
            return obj;
        }

        public IObject createFromString(IObject dataType, string value)
        {
            throw new NotImplementedException();
        }

        public string convertToString(IObject dataType, IObject value)
        {
            throw new NotImplementedException();
        }

        public static GenericFactory Generic = new GenericFactory(null);
    }
}
