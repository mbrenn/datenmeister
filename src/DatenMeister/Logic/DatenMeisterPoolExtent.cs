using DatenMeister.DataProvider.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic
{
    public class DatenMeisterPoolExtent : DotNetExtent
    {
        /// <summary>
        /// Stores the pool
        /// </summary>
        private DatenMeisterPool pool;

        public DatenMeisterPoolExtent(DatenMeisterPool pool)
            : base("datenmeister:///pools")
        {
            this.pool = pool;
        }

        /// <summary>
        /// Gets the elements as DotNetObject
        /// </summary>
        /// <returns>Enumeration of objects within the extent as dotnet-objects</returns>
        public new IEnumerable<IObject> Elements()
        {
            return this.pool.Instances.Select(
                x => new DotNetObject(this, x.ToJson(), x.Extent.ContextURI()));
        }
    }
}
