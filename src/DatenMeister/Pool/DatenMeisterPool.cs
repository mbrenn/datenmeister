using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatenMeister.Logic;
using DatenMeister.Pool;
using BurnSystems.ObjectActivation;
using DatenMeister.DataProvider;
using DatenMeister.Logic.TypeResolver;
using DatenMeister.Entities.DM;

namespace DatenMeister.Pool
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
        /// Stores the last id given to pool
        /// </summary>
        private static int lastId;

        /// <summary>
        /// Store the id of the pool.
        /// Used for debugging
        /// </summary>
        private int id;

        /// <summary>
        /// Stores the extents itself
        /// </summary>
        private List<ExtentContainer> extents = new List<ExtentContainer>();

        /// <summary>
        /// Gets a list of all extent Mappings, this list is thread-safe
        /// </summary>
        public IEnumerable<ExtentContainer> ExtentContainer
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
        /// Initializes a new instance of the DatenMeisterPool.
        /// Use Create to create a new instance
        /// </summary>
        private DatenMeisterPool()
        {
            lastId++;
            this.id = lastId;
        }

        /// <summary>
        /// Adds the uri extent to datenmeister pool
        /// </summary>
        /// <param name="extent">Extent to be added</param>
        public void Add(IURIExtent extent, string storagePath, ExtentType extentType)
        {
            lock (this.syncObject)
            {
                this.CheckIfExtentAlreadyInAnyPool(extent);
                this.Add(extent, storagePath, null, extentType);
            }
        }

        /// <summary>
        /// Adds the an extent to the datenmeister pool and defines
        /// the storage path and a name to the given pool
        /// </summary>
        /// <param name="extent">Extent to be added</param>
        /// <param name="storagePath">Path, where pool is stored</param>
        /// <param name="name">Name of the pool</param>
        public void Add(IURIExtent extent, string storagePath, string name, ExtentType extentType)
        {
            lock (this.syncObject)
            {
                this.CheckIfExtentAlreadyInAnyPool(extent);
                this.Add(
                    new ExtentInfo(storagePath, name, extentType, extent.ContextURI(), extent.GetType().ToString()), 
                    extent);
            }
        }

        public void Add(ExtentInfo info, IURIExtent extent)
        {
            lock (this.syncObject)
            {
                this.CheckIfExtentAlreadyInAnyPool(extent);

                // Check, if an extent with the same url already exists
                var number = 0;
                foreach (var innerExtent in this.extents)
                {
                    if (innerExtent.Extent.ContextURI() == extent.ContextURI())
                    {
                        this.extents.RemoveAt(number);
                        break;
                    }

                    number++;
                }

                // Adds the instance and assign it
                this.extents.Add(
                    new ExtentContainer(info, extent));
                extent.Pool = this;
            }
        }

        private void CheckIfExtentAlreadyInAnyPool(IURIExtent extent)
        {
            if (extent.Pool != null)
            {
                throw new InvalidOperationException("The extent is already assigned to a pool. An extent cannot be assigned to two pools at the same time");
            }
        }

        /// <summary>
        /// Gets or sets the pool being used for the application
        /// </summary>
        private static DatenMeisterPool ApplicationPool
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a default empty pool, where the DatenMeisterPoolExtent is already associated.
        /// The Pool will also be bound to the Global Application Binding
        /// </summary>
        /// <returns>The created pool</returns>
        public static DatenMeisterPool Create()
        {
            ApplicationPool = new DatenMeisterPool();

            Injection.Application.Rebind<IPool>().ToConstant(DatenMeisterPool.ApplicationPool);
            Injection.Application.Rebind<DatenMeisterPool>().ToConstant(ApplicationPool);

            // Adds the extent for the extents
            var poolExtent = new DatenMeisterPoolExtent(ApplicationPool);
            ApplicationPool.Add(poolExtent, null, DatenMeisterPoolExtent.DefaultName, ExtentType.Extents);

            return ApplicationPool;
        }

        /// <summary>
        /// Gets the meta extent type for a certain extenttype 
        /// </summary>
        /// <param name="extentType">Extenttype whose meta type is requested</param>
        public static ExtentType GetMetaExtentType(ExtentType extentType)
        {
            switch (extentType)
            {
                case ExtentType.Extents:
                    return ExtentType.MetaType;
                case ExtentType.MetaType:
                    return ExtentType.MetaType;
                case ExtentType.Type:
                    return ExtentType.MetaType;
                case ExtentType.View:
                    return ExtentType.MetaType;
                case ExtentType.Data:
                    return ExtentType.Type;
                case ExtentType.ApplicationData:
                    return ExtentType.MetaType;
                case ExtentType.Query:
                    return ExtentType.Type;
                default:
                    throw new NotImplementedException("Unknown Extenttype: " + extentType.ToString());
            }
        }
        public override string ToString()
        {
            return "DatenMeisterPool (#" + this.id.ToString() + ")";
        }
    }
}
