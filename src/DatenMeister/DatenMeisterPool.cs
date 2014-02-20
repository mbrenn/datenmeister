using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatenMeister.Logic;

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
        private List<ExtentInstance> extents = new List<ExtentInstance>();

        /// <summary>
        /// Gets the extents as a read copty
        /// </summary>
        public IEnumerable<IURIExtent> Extents
        {
            get
            {
                lock (this.syncObject)
                {
                    return this.extents.Select(x => x.Extent).ToList();
                }
            }
        }

        public IEnumerable<ExtentInstance> Instances
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
        public void Add(IURIExtent extent, string path)
        {
            this.Add(extent, path, null);
        }

        public void Add(IURIExtent extent, string path, string name)
        {
            lock (this.syncObject)
            {
                this.extents.Add(
                    new ExtentInstance(extent, path, name));
            }
        }

        public void Add(ExtentInstance instance)
        {
            lock (this.syncObject)
            {
                this.extents.Add(instance);
            }
        }
    }
}
