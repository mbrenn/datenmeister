using DatenMeister.DataProvider.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Logic
{
    public class DatenMeisterPoolExtent :IURIExtent
    {
        /// <summary>
        /// Stores the pool
        /// </summary>
        private DatenMeisterPool pool;

        public DatenMeisterPoolExtent(DatenMeisterPool pool)
        {
            this.pool = pool;
        }

        public string ContextURI()
        {
            return "datenmeister:///pool";
        }

        public IEnumerable<IObject> Elements()
        {
            return this.pool.Extents.Select(
                x => new DotNetObject(x.ToJson()));
        }

        public IObject CreateObject()
        {
            throw new NotImplementedException();
        }

        public void RemoveObject(IObject element)
        {
            throw new NotImplementedException();
        }
    }
}
