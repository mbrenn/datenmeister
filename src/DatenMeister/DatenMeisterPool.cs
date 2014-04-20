using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatenMeister.Logic;
using DatenMeister.Pool;

namespace DatenMeister
{
    /// <summary>
    /// Defines the datenmeister pool
    /// </summary>
    public class DatenMeisterPool : IPool
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
        public void Add(IURIExtent extent, string storagePath)
        {
            lock (this.syncObject)
            {
                this.CheckIfExtentAlreadyInAnyPool(extent);
                this.Add(extent, storagePath, null);
                extent.Pool = this;
            }
        }

        public void Add(IURIExtent extent, string storagePath, string name)
        {
            lock (this.syncObject)
            {
                this.CheckIfExtentAlreadyInAnyPool(extent);
                extent.Pool = this;
                this.extents.Add(
                    new ExtentInstance(extent, storagePath, name));
            }
        }

        public void Add(ExtentInstance instance)
        {
            lock (this.syncObject)
            {
                this.CheckIfExtentAlreadyInAnyPool(instance.Extent);

                // Check, if an extent with the same url already exists
                var number = 0;
                foreach (var extent in this.extents)
                {
                    if (extent.Extent.ContextURI() == instance.Extent.ContextURI())
                    {
                        this.extents.RemoveAt(number);
                        break;
                    }

                    number++;
                }

                // Adds the instance and assign it
                this.extents.Add(instance);
                instance.Extent.Pool = this;
            }
        }

        private void CheckIfExtentAlreadyInAnyPool(IURIExtent extent)
        {
            if (extent.Pool != null)
            {
                throw new InvalidOperationException("The extent is already assigned to a pool. An extent cannot be assigned to two pools at the same time");
            }
        }
    }
}
