using DatenMeister.DataProvider.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic
{
    public class DatenMeisterPoolExtent : IURIExtent
    {
        /// <summary>
        /// Stores the pool
        /// </summary>
        private DatenMeisterPool pool;

        public DatenMeisterPoolExtent(DatenMeisterPool pool)
        {
            this.pool = pool;
        }

        /// <summary>
        /// Gets the context uri for this extent
        /// </summary>
        /// <returns></returns>
        public string ContextURI()
        {
            return "datenmeister:///pools";
        }

        /// <summary>
        /// Gets the elements as DotNetObject
        /// </summary>
        /// <returns>Enumeration of objects within the extent as dotnet-objects</returns>
        public IEnumerable<IObject> Elements()
        {
            return this.pool.Instances.Select(
                x => new DotNetObject(this, x.ToJson(), x.Extent.ContextURI()));
        }

        public IObject CreateObject(IObject type)
        {
            throw new NotImplementedException();
        }

        public void RemoveObject(IObject element)
        {
            throw new NotImplementedException();
        }
    }
}
