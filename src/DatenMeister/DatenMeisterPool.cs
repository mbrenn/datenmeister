using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister
{
    /// <summary>
    /// Defines the datenmeister pool
    /// </summary>
    public class DatenMeisterPool
    {
        /// <summary>
        /// Stores the syncronisation object
        /// </summary>
        private object syncObject = new object();

        /// <summary>
        /// Stores the extents itself
        /// </summary>
        private List<IURIExtent> extents = new List<IURIExtent>();

        /// <summary>
        /// Gets the extents as a read copty
        /// </summary>
        public IList<IURIExtent> Extents
        {
            get
            {
                lock (this.syncObject)
                {
                    return this.extents.ToList();
                }
            }
        }

        /// <summary>
        /// Adds the uri extent to datenmeister pool
        /// </summary>
        /// <param name="extent">Extent to be added</param>
        public void Add(IURIExtent extent)
        {
            lock (this.syncObject)
            {
                this.extents.Add(extent);
            }
        }
    }
}
