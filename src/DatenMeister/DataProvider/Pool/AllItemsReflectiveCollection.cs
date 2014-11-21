using DatenMeister.Logic;
using DatenMeister.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.DataProvider.Pool
{
    /// <summary>
    /// Returns all items in all extents within the given pool.
    /// Is a union of all
    /// </summary>
    public class AllItemsReflectiveCollection : UnionTransformation
    {
        /// <summary>
        /// Stores the associated pool
        /// </summary>
        private IPool pool;

        /// <summary>
        /// Initializes a new instnace of AllItemsReflectiveCollection
        /// </summary>
        /// <param name="pool"></param>
        public AllItemsReflectiveCollection(IPool pool)
            : base(pool.Extents.Select(x => x.Elements()))
        {
            this.pool = pool;
        }

        /// <summary>
        /// Initializes a new instnace of AllItemsReflectiveCollection
        /// </summary>
        /// <param name="pool"></param>
        public AllItemsReflectiveCollection(IPool pool, ExtentType extentType)
            : base(pool.ExtentMappings
                .Where (x=> x.ExtentInfo.ExtentType == extentType)
                .Select(x => x.Extent.Elements()))
        {
            this.pool = pool;
        }
    }
}
